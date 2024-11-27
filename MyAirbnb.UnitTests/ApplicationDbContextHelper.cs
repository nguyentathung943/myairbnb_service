using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MyAirbnb.DataAccess;
using MyAirbnb.DataAccess.Entities;

public static class ApplicationDbContextHelper
{
    public static ApplicationDbContext Create()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var context = new ApplicationDbContext(options);

        return context;
    }

    public static void SeedUser(ApplicationDbContext context, int userId)
    {
        context.Users.Add(new User { Id = userId, UserName = "testuser", Password = "password" });
        context.SaveChanges();
    }

    public static void SeedAccomodation(ApplicationDbContext context, int accomodationId)
    {
        context.Accommodations.Add(
            new Accommodation
            {
                Id = accomodationId,
                Name = "Beach House",
                LocationTagName = "Beach",
                Description = "A beautiful beach house with stunning ocean views.",
                PricePerNight = 200.00m,
                MaxAdultsAllowed = 4,
                MaxChildrenAllowed = 2,
                MaxInfantsAllowed = 1,
                MaxPetsAllowed = 1
            });

        context.SaveChanges();
    }

    public static void SeedReservation(ApplicationDbContext context, int userId, int accomodationId, int resevationId) {

        context.Users.Add(new User { Id = userId, UserName = "User 02", Password = "password" });
        context.Accommodations.Add(
            new Accommodation
            {
                Id = accomodationId,
                Name = "Beach House",
                LocationTagName = "Beach",
                Description = "A beautiful beach house with stunning ocean views.",
                PricePerNight = 200.00m,
                MaxAdultsAllowed = 4,
                MaxChildrenAllowed = 2,
                MaxInfantsAllowed = 1,
                MaxPetsAllowed = 1
            });
        context.Reservations.Add(
            new Reservation() 
            { 
                Id = resevationId,
                TotalPrice = 200,
                UserId = userId,
                AccommodationId = accomodationId,
                TotalAdults = 4,
                TotalChildren = 2,
                TotalInfants = 1,
                TotalPets = 1,
                CheckInDate = new DateTime(2024, 11, 29),
                CheckOutDate = new DateTime(2024, 11, 30), 
            });

        context.SaveChanges(); 
    }

    public static void Destroy(ApplicationDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}
