
namespace MyAirbnb.Models.DTOs.Accomodation;

public class AccomodationGetListResponseDTO
{
    /// <summary>
    /// Accomodations Return List
    /// </summary>
    public IEnumerable<AccomodationDTO> Accomodations { get; set; } = Enumerable.Empty<AccomodationDTO>();

    /// <summary>
    /// Total Count based on provided Filter
    /// </summary>
    public int TotalCount { get; set; } = 0;
}
