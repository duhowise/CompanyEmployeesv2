using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.Models;
using Entities.RequestParameters;
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

    public CompanyDto GetCompany(Guid id, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(id, trackChanges);
        if (company is null)
        {
            throw new CompanyNotFoundException(id);
        }
        var companyDto = _mapper.Map<CompanyDto>(company);
        return companyDto;
    }

    public async Task<CompanyDto> GetCompanyAsync(Guid id, bool trackChanges)
    {
        var company = await _repository.Company.GetCompanyAsync(id, trackChanges);
        var companyDto = _mapper.Map<CompanyDto>(company);
        return companyDto;
    }

    public void CreateCompany(Company companyEntity)
    {
       _repository.Company.CreateCompany(companyEntity);
    }

    public async Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
    {
        var companies= await _repository.Company.GetByIdsAsync(ids, trackChanges);
        return _mapper.Map<CompanyDto[]>(companies);
    }

    public void DeleteCompany(CompanyDto company)
    {
        var companyModel=_mapper.Map<Company>(company);
        _repository.Company.DeleteCompany(companyModel);
    }

   
}