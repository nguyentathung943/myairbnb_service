
using System.ComponentModel.DataAnnotations;

namespace MyAirbnb.Models.DTOs.Reservation;
public class AccomodationReservationRequestDTO
{
    /// <summary>
    /// Accommodation Identifier
    /// </summary>
    [Required]
    public int AccommodationId { get; set; }

    /// <summary>
    /// User Identifier
    /// </summary>
    [Required]
    public int UserId { get; set; }

    /// <summary>
    /// Check In Date
    /// </summary>
    [Required]
    public DateTime CheckInDate { get; set; }

    /// <summary>
    /// Check Out Date
    /// </summary>
    [Required]
    public DateTime CheckOutDate { get; set; }

    /// <summary>
    /// Total Price
    /// </summary>
    [Required]
    public double TotalPrice { get; set; }

    /// <summary>
    /// Total Adults
    /// </summary>

    [Range(1, 16, ErrorMessage = "Please provide valid number of Adults (1 - 16)")]
    [Required]
    public int TotalAdults { get; set; }

    /// <summary>
    /// Total Children
    /// </summary>
    [Range(0, 16, ErrorMessage = "Please provide valid number of Children (0 - 16)")]
    [Required]
    public int TotalChildren { get; set; }

    /// <summary>
    /// Total Infants
    /// </summary>
    [Range(0, 5, ErrorMessage = "Please provide valid number of Infants (0 - 5)")]
    [Required]
    public int TotalInfants { get; set; }

    /// <summary>
    /// Total Pets
    /// </summary>
    [Range(0, 5, ErrorMessage = "Please provide valid number of Pets (0 - 5)")]
    [Required]
    public int TotalPets { get; set; }
}
