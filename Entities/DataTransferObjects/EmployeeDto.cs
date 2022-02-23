namespace Entities.DataTransferObjects;

public class EmployeeDto
{
    public EmployeeDto(Guid id, string name, int age, string position)
    {
        Id = id;
        Name = name;
        Age = age;
        Position = position;
    }
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Position { get; set; }
}