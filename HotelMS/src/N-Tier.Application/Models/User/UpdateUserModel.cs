using System.ComponentModel.DataAnnotations;

namespace HotelMS.Application.Models.User;

public class UpdateUserModel
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; }

    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    public bool ChangeRole { get; set; } // New property


}
