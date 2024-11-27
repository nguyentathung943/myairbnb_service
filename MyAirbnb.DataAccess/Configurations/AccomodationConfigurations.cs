using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyAirbnb.DataAccess.Entities;

namespace MyAirbnb.DataAccess.Configurations;

public class AccommodationConfigurations : IEntityTypeConfiguration<Accommodation>
{
    public void Configure(EntityTypeBuilder<Accommodation> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
            .IsRequired();

        builder.Property(a => a.LocationTagName)
            .IsRequired();

        builder.Property(a => a.Description)
            .IsRequired();

        builder.Property(a => a.PricePerNight)
            .IsRequired();

        builder.Property(a => a.MaxAdultsAllowed)
            .IsRequired();

        builder.Property(a => a.MaxChildrenAllowed)
            .IsRequired();

        builder.Property(a => a.MaxInfantsAllowed)
            .IsRequired();

        builder.Property(a => a.MaxPetsAllowed)
            .IsRequired();

        builder.HasMany(a => a.Reservations)
            .WithOne(r => r.Accommodation)
            .HasForeignKey(r => r.AccommodationId);

        builder.HasData(
            new Accommodation
            {
                Id = 1,
                Name = "Beach House",
                LocationTagName = "Beach",
                Description = "A beautiful beach house with stunning ocean views.",
                PricePerNight = 200.00m,
                MaxAdultsAllowed = 4,
                MaxChildrenAllowed = 2,
                MaxInfantsAllowed = 1,
                MaxPetsAllowed = 1
            },
            new Accommodation
            {
                Id = 2,
                Name = "Mountain Cabin",
                LocationTagName = "Mountain",
                Description = "A cozy cabin in the mountains, perfect for a getaway.",
                PricePerNight = 150.00m,
                MaxAdultsAllowed = 2,
                MaxChildrenAllowed = 2,
                MaxInfantsAllowed = 1,
                MaxPetsAllowed = 0
            },
            new Accommodation
            {
                Id = 3,
                Name = "City Apartment",
                LocationTagName = "City",
                Description = "A modern apartment in the heart of the city.",
                PricePerNight = 100.00m,
                MaxAdultsAllowed = 2,
                MaxChildrenAllowed = 1,
                MaxInfantsAllowed = 1,
                MaxPetsAllowed = 0
            },
            new Accommodation
            {
                Id = 4,
                Name = "Country Cottage",
                LocationTagName = "Country",
                Description = "A charming cottage in the countryside.",
                PricePerNight = 120.00m,
                MaxAdultsAllowed = 3,
                MaxChildrenAllowed = 2,
                MaxInfantsAllowed = 1,
                MaxPetsAllowed = 1
            },
            new Accommodation
            {
                Id = 5,
                Name = "Lake House",
                LocationTagName = "Lake",
                Description = "A serene house by the lake.",
                PricePerNight = 180.00m,
                MaxAdultsAllowed = 4,
                MaxChildrenAllowed = 3,
                MaxInfantsAllowed = 2,
                MaxPetsAllowed = 1
            },
            new Accommodation
            {
                Id = 6,
                Name = "Desert Villa",
                LocationTagName = "Desert",
                Description = "A luxurious villa in the desert.",
                PricePerNight = 250.00m,
                MaxAdultsAllowed = 5,
                MaxChildrenAllowed = 3,
                MaxInfantsAllowed = 2,
                MaxPetsAllowed = 0
            },
            new Accommodation
            {
                Id = 7,
                Name = "Forest Lodge",
                LocationTagName = "Forest",
                Description = "A rustic lodge in the forest.",
                PricePerNight = 160.00m,
                MaxAdultsAllowed = 3,
                MaxChildrenAllowed = 2,
                MaxInfantsAllowed = 1,
                MaxPetsAllowed = 1
            },
            new Accommodation
            {
                Id = 8,
                Name = "Island Bungalow",
                LocationTagName = "Island",
                Description = "A private bungalow on a tropical island.",
                PricePerNight = 300.00m,
                MaxAdultsAllowed = 2,
                MaxChildrenAllowed = 1,
                MaxInfantsAllowed = 1,
                MaxPetsAllowed = 0
            },
            new Accommodation
            {
                Id = 9,
                Name = "Ski Chalet",
                LocationTagName = "Ski",
                Description = "A cozy chalet near the ski slopes.",
                PricePerNight = 220.00m,
                MaxAdultsAllowed = 4,
                MaxChildrenAllowed = 2,
                MaxInfantsAllowed = 1,
                MaxPetsAllowed = 0
            },
            new Accommodation
            {
                Id = 10,
                Name = "Jungle Retreat",
                LocationTagName = "Jungle",
                Description = "A secluded retreat in the jungle.",
                PricePerNight = 280.00m,
                MaxAdultsAllowed = 3,
                MaxChildrenAllowed = 2,
                MaxInfantsAllowed = 1,
                MaxPetsAllowed = 1
            }
        );
    }
}
