using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestParameters;

namespace Service.Contracts;

public interface ICompanyService
{
    IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges);
    CompanyDto GetCompany(Guid id, bool trackChanges);

    Task<CompanyDto> GetCompanyAsync(Guid id, bool trackChanges);
    void CreateCompany(Company companyEntity);
    Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
    void DeleteCompany(CompanyDto company);
   
}