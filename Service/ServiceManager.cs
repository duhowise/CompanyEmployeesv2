using Contracts;
using Service.Contracts;

namespace Service;

public interface IServiceManager
{
    ICompanyService CompanyService { get; }
    IEmployeeService EmployeeService { get; }
}

public class ServiceManager : IServiceManager
{
    private readonly Lazy<ICompanyService> _companyService;
    private readonly Lazy<IEmployeeService> _employeeService;
    private ILoggerManager _logger;

    public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger)
    {
        _logger = logger;
        _companyService = new Lazy<ICompanyService>(() => new CompanyService(repositoryManager, _logger));
        _employeeService = new Lazy<IEmployeeService>(()=>new EmployeeService(repositoryManager,_logger));
    }
    public ICompanyService CompanyService => _companyService.Value;
    public IEmployeeService EmployeeService => _employeeService.Value;
}