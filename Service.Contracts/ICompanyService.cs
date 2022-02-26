using Entities.DataTransferObjects;

namespace Service.Contracts;

public interface ICompanyService
{
    IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges);
    CompanyDto GetCompany(Guid id, bool trackChanges);
    Task<CompanyDto> GetCompanyAsync(Guid id, bool trackChanges);
    Task<CompanyDto> CreateCompany(CompanyForCreationDto? company);
    Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompany(
        IEnumerable<CompanyForCreationDto> companyCollection);
    Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid>? ids, bool trackChanges);
    Task DeleteCompany(Guid company);
}