using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Service.Contracts;

namespace Service;

public class CompanyService:ICompanyService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;

    public CompanyService(IRepositoryManager repository,IMapper mapper,ILoggerManager logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
    {
       
            var companies = _repository.Company.GetAllCompanies(trackChanges);
            var companiesDto = _mapper.Map<CompanyDto[]>(companies);
            return companiesDto;
        
    }
}