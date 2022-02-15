using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects;

public class CompanyForCreationDto
{
    [Required(ErrorMessage = "company name is a required field.")]
    public string? Name { get; set; }
    [Required(ErrorMessage = "company address is a required field.")]
    public string? Address { get; set; }
    [Required(ErrorMessage = "Country name is a required field.")]
    public string? Country { get; set; }
    public IEnumerable<EmployeeForCreationDto>? Employees { get; set; }

}

