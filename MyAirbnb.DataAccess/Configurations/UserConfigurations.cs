using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MyAirbnb.DataAccess.Entities;

namespace MyAirbnb.DataAccess.Configurations;

internal class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasData(
            new User
            {
                Id = 1,
                UserName = "admin",
                Password = "admin"
            }
        );
    }
}
