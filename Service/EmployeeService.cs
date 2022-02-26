using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.Models;
using Entities.RequestParameters;
using Microsoft.EntityFrameworkCore;
using Service.Contracts;

namespace Service;

public class EmployeeService : IEmployeeService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;

    public EmployeeService(IRepositoryManager repository, IMapper mapper, ILoggerManager logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedList<EmployeeDto>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters,
        bool trackChanges)
    {
        var company = await _repository.Company.GetCompanyAsync(companyId, false);
        if (company == null) throw new CompanyNotFoundException(companyId);
        var employeeList =
            await _repository.Employee.GetEmployeesAsync(companyId, employeeParameters, trackChanges);
        var employeeListDto = _mapper.Map<List<EmployeeDto>>(employeeList);
        var count = await _repository.Employee.FindByCondition(x => x.CompanyId.Equals(companyId), trackChanges)
            .CountAsync();

        return new PagedList<EmployeeDto>(employeeListDto, count, employeeParameters.PageNumber,
            employeeParameters.PageSize);
    }

    public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
    {
        var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
        if (company == null) throw new CompanyNotFoundException(companyId);
        var employee = await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges);

        return _mapper.Map<EmployeeDto>(employee);
    }

    public async Task<EmployeeDto> CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto? employee)
    {
       
        var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
        if (company == null) throw new CompanyNotFoundException(companyId);
        
        var employeeEntity = _mapper.Map<Employee>(employee);
        _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
        await _repository.SaveAsync();
        var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);
        return employeeToReturn;
    }

    public async Task DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges)
    {
        var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);

        var employeeForCompany =await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges);
        if (employeeForCompany is null)
            throw new EmployeeNotFoundException(id);
        _repository.Employee.DeleteEmployee(employeeForCompany);
        await _repository.SaveAsync();
    }

    public async Task UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate,
        bool compTrackChanges, bool empTrackChanges)
    {
        var company = await _repository.Company.GetCompanyAsync(companyId, compTrackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);

        var employeeEntity = await _repository.Employee.GetEmployeeAsync(companyId, id,
            empTrackChanges);

        if (employeeEntity is null)
            throw new EmployeeNotFoundException(id);
        _mapper.Map(employeeForUpdate, employeeEntity);
      await  _repository.SaveAsync();
    }
}