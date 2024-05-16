using AutoMapper;
using HotelMS.Application.Models.TodoItem;
using HotelMS.Core.Entities;

namespace HotelMS.Application.MappingProfiles;

public class RoomProfile : Profile
{
    public RoomProfile()
    {
        CreateMap<CreateRoomModel, Room>();
        CreateMap<UpdateRoomModel, Room>();
        CreateMap<Room, RoomResponseModel>();
        CreateMap<RoomResponseModel, Room>();

    }
}
