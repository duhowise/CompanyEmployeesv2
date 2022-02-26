using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasOne(x => x.Company).WithMany(x => x.Employees)
            .HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Cascade).IsRequired();
        builder.HasData
        (
            new Employee(id: new Guid("80abbca8-664d-4b20-b5de-024705497d4a"), name: "Sam Raiden", age: 26,
                position: "Software developer", companyId: new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870")),
            new Employee(id: new Guid("86dba8c0-d178-41e7-938c-ed49778fb52a"), name: "Jana McLeaf", age: 30,
                position: "Software developer", companyId: new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870")),
            new Employee
            (
                id: new Guid("021ca3c1-0deb-4afd-ae94-2159a8479811"),
                name: "Kane Miller",
                age: 35,
                position: "Administrator",
                companyId: new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3")
            )
        );
    }
}