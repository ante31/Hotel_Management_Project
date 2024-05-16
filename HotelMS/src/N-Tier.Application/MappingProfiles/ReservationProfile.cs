using AutoMapper;
using HotelMS.Application.Models.TodoItem;
using HotelMS.Core.Entities;

namespace HotelMS.Application.MappingProfiles;

public class ReservationProfile : Profile
{
    public ReservationProfile()
    {
        CreateMap<CreateReservationModel, Reservation>();
        CreateMap<UpdateReservationModel, Reservation>();
        CreateMap<Reservation, ReservationResponseModel>();
    }
}
