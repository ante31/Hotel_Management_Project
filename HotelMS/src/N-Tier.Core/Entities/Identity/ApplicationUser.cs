using Microsoft.AspNetCore.Identity;

namespace HotelMS.Core.Entities.Identity;

public class ApplicationUser : IdentityUser { 
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool deleted { get; set; }

}
