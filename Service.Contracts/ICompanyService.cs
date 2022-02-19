using Entities.DataTransferObjects;
using Entities.Models;

namespace Service.Contracts;

public interface ICompanyService
{
    IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges);
}