using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Newtonsoft.Json;
using Service.Contracts;

namespace CompanyEmployees.Presentation.Controllers
{
    //[ApiVersion("1.0")]
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        public CompaniesController(IServiceManager serviceManager, IMapper mapper, ILoggerManager logger)
        {
            _serviceManager = serviceManager;
            _mapper = mapper;
            _logger = logger;
        }


        [HttpGet]
        public IActionResult GetCompanies()
        {
            var companies =  _serviceManager.CompanyService.GetAllCompanies(trackChanges: false);
            return Ok(companies);
        }

       
    }
}