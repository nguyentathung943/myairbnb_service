using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyAirbnb.DataAccess.Entities;

namespace MyAirbnb.DataAccess.Configurations;

public class ReservationConfigurations : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.CheckInDate)
            .IsRequired();

        builder.Property(r => r.CheckOutDate)
            .IsRequired();

        builder.Property(r => r.TotalPrice)
            .IsRequired();

        builder.Property(r => r.TotalAdults)
            .IsRequired();

        builder.Property(r => r.TotalChildren)
            .IsRequired();

        builder.Property(r => r.TotalInfants)
            .IsRequired();

        builder.Property(r => r.TotalPets)
            .IsRequired();

        builder.HasOne(r => r.Accommodation)
            .WithMany(a => a.Reservations)
            .HasForeignKey(r => r.AccommodationId);

        builder.HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId);
    }
}
