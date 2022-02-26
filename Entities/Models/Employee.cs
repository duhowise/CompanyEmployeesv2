using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public class Employee
{
    public Employee(Guid id, string name, int age, string position, Guid companyId, Company company)
    {
        Id = id;
        Name = name;
        Age = age;
        Position = position;
        CompanyId = companyId;
        Company = company;
    }

    private Employee()
    {
        
    }

    public Employee(Guid id, string name, int age, string position, Guid companyId)
    {
        Id = id;
        Name = name;
        Age = age;
        Position = position;
        CompanyId = companyId;
    }

    [Column("EmployeeId")]
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Employee name is a required field.")]
    [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Age is a required field.")]
    public int Age { get; set; }
    [Required(ErrorMessage = "Position is a required field.")]
    [MaxLength(20, ErrorMessage = "Maximum length for the Position is 20 characters.")]
    public string Position { get; set; }

    [ForeignKey(nameof(Company))]
    public Guid CompanyId { get; set; }
    public Company Company { get; set; }
}