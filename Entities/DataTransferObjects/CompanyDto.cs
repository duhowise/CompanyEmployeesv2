namespace Entities.DataTransferObjects;

public class CompanyDto
{
   

    public CompanyDto(Guid id, string name, string fullAddress)
    {
        Id = id;
        Name = name;
        FullAddress = fullAddress;

    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string FullAddress { get; set; }
}