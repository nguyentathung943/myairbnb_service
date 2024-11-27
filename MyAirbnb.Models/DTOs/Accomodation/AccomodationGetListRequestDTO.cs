
using System.ComponentModel.DataAnnotations;

namespace MyAirbnb.Models.DTOs.Accomodation;

public class AccomodationGetListRequestDTO
{
    public string? SearchString { get; set; } = null;
    public DateTime? CheckInDate { get; set; } = null;
    public DateTime? CheckOutDate { get; set; } = null;

    [Range(1, 16, ErrorMessage = "Please provide valid number of Adults (1 - 16)")]
    public int Adults { get; set; } = 1;

    [Range(0, 16, ErrorMessage = "Please provide valid number of Children (0 - 16)")]
    public int Children { get; set; } = 0;

    [Range(0, 5, ErrorMessage = "Please provide valid number of Infants (0 - 5)")]
    public int Infants { get; set; } = 0;

    [Range(0, 5, ErrorMessage = "Please provide valid number of Pets (0 - 5)")]
    public int Pets { get; set; } = 0;
}
