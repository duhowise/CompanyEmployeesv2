using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestParameters;
using Service.Contracts;

namespace Service;

public class EmployeeService:IEmployeeService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;

    public EmployeeService(IRepositoryManager repository,IMapper mapper, ILoggerManager logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedList<EmployeeDto>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
    {
        var employeePagedList= await _repository.Employee.GetEmployeesAsync(companyId, employeeParameters, trackChanges);
        return _mapper.Map<PagedList<EmployeeDto>>(employeePagedList);
    }

    public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
    {
        var employee= await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges);
        return _mapper.Map<EmployeeDto>(employee);
    }

    public void CreateEmployeeForCompany(Guid companyId, Employee employeeEntity)
    {
        _repository.Employee.CreateEmployeeForCompany(companyId,employeeEntity);
    }

    public void DeleteEmployee(Employee employeeForCompany)
    {
        _repository.Employee.DeleteEmployee(employeeForCompany);
    }
    
}