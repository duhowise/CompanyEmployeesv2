using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Exceptions;
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

        if (company == null)
        {
            throw new CompanyNotFoundException(id);
        }
        var companyDto = _mapper.Map<CompanyDto>(company);
        return companyDto;
    }

    public async Task<CompanyDto> CreateCompany(CompanyForCreationDto? company)
    {
        var companyEntity = _mapper.Map<Company>(company);
        _repository.Company.CreateCompany(companyEntity);
        await _repository.SaveAsync();
        var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);
        return companyToReturn;

    }
    
    
    public async Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompany(
        IEnumerable<CompanyForCreationDto> companyCollection)
    {
        if (companyCollection is null)
            throw new CompanyCollectionBadRequest();
        var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
        foreach (var company in companyEntities)
        {
            _repository.Company.CreateCompany(company);
        }

        await _repository.SaveAsync();
        var companyCollectionToReturn =
            _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
        var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));
      return  (companies: companyCollectionToReturn, ids);

    }

    public async Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid>? ids, bool trackChanges)
    {
        if (ids is null)
            throw new IdParametersBadRequestException();
        var companyEntities = await _repository.Company.GetByIdsAsync(ids, trackChanges);
        if (ids.Count() != companyEntities.Count())
            throw new CollectionByIdsBadRequestException();
        return _mapper.Map<CompanyDto[]>(companyEntities);

    }

    public async Task DeleteCompany(Guid companyId)
    {
        var company =await _repository.Company.GetCompanyAsync(companyId, false);
        if (company != null) throw new CompanyNotFoundException(companyId);
        _repository.Company.DeleteCompany(company);
        await _repository.SaveAsync();
        
    }

   
}