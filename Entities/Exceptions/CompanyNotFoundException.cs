namespace Entities.Exceptions;

public class CompanyNotFoundException:NotFoundException
{
    public CompanyNotFoundException(Guid employeeId) : base($"The company with id: {employeeId} doesn't exist in the database.")
    {
    } 
    
    
   
}