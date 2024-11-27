using Microsoft.EntityFrameworkCore;
using MyAirbnb.DataAccess.Entities;

namespace MyAirbnb.DataAccess;

public class ApplicationDbContext: DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Accommodation> Accommodations { get; set; }
    public DbSet<Reservation> Reservations { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
