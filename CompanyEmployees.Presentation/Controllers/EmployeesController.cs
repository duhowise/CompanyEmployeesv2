﻿using AutoMapper;
using CompanyEmployees.Presentation.ActionFilters;
using CompanyEmployees.Presentation.Utility;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestParameters;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.Contracts;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IServiceManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly EmployeeLinks _employeeLinks;

        public EmployeesController(IServiceManager repository, ILoggerManager logger, IMapper mapper , EmployeeLinks employeeLinks)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _employeeLinks = employeeLinks;
        }

        [HttpGet]
        [HttpHead]
      [ServiceFilter(typeof(ValidateMediaTypeAttribute))]  
        public async Task<IActionResult> GetEmployeesForCompany(Guid companyId,[FromQuery] EmployeeParameters employeeParameters)
        {
            var company =await _repository.CompanyService.GetCompanyAsync(companyId, false);
            if (company == null)
            {
                _logger.LogInfo($"Company with Id: {companyId} does not exist");
                return NotFound();
            }

            var employeesFromDb =await _repository.EmployeeService.GetEmployeesAsync(companyId,employeeParameters, trackChanges: false);
            Response.Headers.Add("X-Pagination",JsonConvert.SerializeObject(employeesFromDb.MetaData));


            var employeesDto = _mapper.Map<EmployeeDto[]>(employeesFromDb);
            var links = _employeeLinks.TryGenerateLinks(employeesDto, employeeParameters.Fields, companyId, HttpContext);
            return links.HasLinks ? Ok(links.LinkedEntities) : Ok(links.ShapedEntities);
        }

        [HttpGet("{id:guid}", Name = "GetEmployeeForCompany")]
        public async Task<IActionResult> GetEmployeeForCompany(Guid companyId, Guid id)
        {
            var company =await _repository.CompanyService.GetCompanyAsync(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with Id: {companyId} does not exist");
                return NotFound();
            }

            var employee = _repository.EmployeeService.GetEmployeeAsync(companyId, id, trackChanges: false);
            var employeeDto = _mapper.Map<EmployeeDto>(employee);
            return Ok(employeeDto);
        }


        [HttpPost]
        public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto? employee)
        {
            if (employee == null)
            {
                _logger.LogError("EmployeeForCreationDto object sent from client is null.");
                return BadRequest("EmployeeForCreationDto object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the EmployeeForCreationDto object");
                return UnprocessableEntity(ModelState);
            }
            var company =await _repository.CompanyService.GetCompanyAsync(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }

            var employeeEntity = _mapper.Map<Employee>(employee);
            _repository.EmployeeService.CreateEmployeeForCompany(companyId, employeeEntity);
          await  _repository.SaveAsync();
            var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);
            return CreatedAtRoute("GetEmployeeForCompany", new
            {
                companyId,
                id =
                    employeeToReturn.Id
            }, employeeToReturn);
        }

        [ServiceFilter(typeof(ValidateEmployeeForCompanyExistsAttribute))]
        [HttpDelete("{id}")] public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId, Guid id)
        {
            
            var employeeForCompany =HttpContext.Items["employee"] as Employee;
           _repository.EmployeeService.DeleteEmployee(employeeForCompany);
          await  _repository.SaveAsync();
            return NoContent();
        }

        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateEmployeeForCompanyExistsAttribute))]
        [HttpPut("{id}")]
       public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id,
           [FromBody] EmployeeForUpdateDto? employee)
       {
           var employeeEntity=HttpContext.Items["employee"] as Employee;
           _mapper.Map(employee, employeeEntity);
          await _repository.SaveAsync();
           return NoContent();
       }
       [HttpPatch("{id}")]
        [ServiceFilter(typeof(ValidateEmployeeForCompanyExistsAttribute))]
       public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id,
           [FromBody] JsonPatchDocument<EmployeeForUpdateDto>? patchDoc)
       {
           if (patchDoc == null)
           {
               _logger.LogError("patchDoc object sent from client is null.");
               return BadRequest("patchDoc object is null");
           }


           var employeeEntity=HttpContext.Items["employee"] as Employee;
          
           var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);
           //patchDoc.ApplyTo(employeeToPatch,ModelState);
           TryValidateModel(employeeToPatch);
           if (!ModelState.IsValid)
           {
               _logger.LogError("Invalid model state for the patch document");
               return UnprocessableEntity(ModelState);
            }
            _mapper.Map(employeeToPatch, employeeEntity);
          await _repository.SaveAsync();
           return NoContent();
       }
    }
}