using HotelMS.Core.Enums;

namespace HotelMS.Application.Models.TodoItem;

public class UpdateRoomModel
{
    public RoomTypeEnum RoomType { get; set; }

    public int Price { get; set; }

    public bool AvailabilityStatus { get; set; }

}

public class UpdateRoomResponseModel : BaseResponseModel { }
