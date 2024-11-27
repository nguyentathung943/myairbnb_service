
namespace MyAirbnb.Models.DTOs.Accomodation;

public class AccomodationDTO
{
    /// <summary>
    /// Identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Location Name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Location Tag Name
    /// </summary>
    public string LocationTagName { get; set; }

    /// <summary>
    /// Location Description
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Price Per Night
    /// </summary>
    public decimal PricePerNight { get; set; }

    /// <summary>
    /// Maximum Adults Allowed
    /// </summary>
    public int MaxAdultsAllowed { get; set; }

    /// <summary>
    /// Maximum Children Allowed
    /// </summary>
    public int MaxChildrenAllowed { get; set; }

    /// <summary>
    /// Maximum Infants Allowed
    /// </summary>
    public int MaxInfantsAllowed { get; set; }

    /// <summary>
    /// Maximum Pets Allowed
    /// </summary>
    public int MaxPetsAllowed { get; set; }

    /// <summary>
    /// Guest Capacity (Infant & Pets excluded)
    /// </summary>
    public int GuestCapacity { get => MaxAdultsAllowed + MaxChildrenAllowed; }
}
