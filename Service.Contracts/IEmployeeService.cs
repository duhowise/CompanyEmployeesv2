using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestParameters;

namespace Service.Contracts;

public interface IEmployeeService
{
    Task<PagedList<EmployeeDto>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges);
    Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges);
    void CreateEmployeeForCompany(Guid companyId, Employee employeeEntity);
    void DeleteEmployee(Employee employeeForCompany);
}   