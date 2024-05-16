using HotelMS.Core.Enums;

namespace HotelMS.Application.Models.TodoItem;

public class CreateRoomModel
{
    public RoomTypeEnum RoomType { get; set; }

    public int Price { get; set; }

    public int RoomNumber { get; set; }

    public bool AvailabilityStatus { get; set; } = true;

}

public class CreateRoomResponseModel : BaseResponseModel { }
