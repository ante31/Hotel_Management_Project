using AutoMapper;
using HotelMS.Application.Models.User;
using HotelMS.Core.Entities.Identity;

namespace HotelMS.Application.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserModel, ApplicationUser>();
        CreateMap<ApplicationUser, UserResponseModel>();
    }
}
