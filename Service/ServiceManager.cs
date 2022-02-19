using AutoMapper;
using Contracts;
using Service.Contracts;

namespace Service;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<ICompanyService> _companyService;
    private readonly Lazy<IEmployeeService> _employeeService;
    private readonly ILoggerManager _logger;

    public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper,ILoggerManager logger)
    {
        _logger = logger;
        _companyService = new Lazy<ICompanyService>(() => new CompanyService(repositoryManager, mapper,_logger));
        _employeeService = new Lazy<IEmployeeService>(()=>new EmployeeService(repositoryManager,_logger));
    }
    public ICompanyService CompanyService => _companyService.Value;
    public IEmployeeService EmployeeService => _employeeService.Value;
}