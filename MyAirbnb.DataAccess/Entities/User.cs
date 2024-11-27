
namespace MyAirbnb.DataAccess.Entities;

public class User
{
    /// <summary>
    /// User Identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// User Name
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// User Password (Hashed)
    /// </summary>
    public string Password { get; set; }
}
