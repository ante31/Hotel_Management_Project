using HotelMS.Core.Enums;

namespace HotelMS.Application.Models.TodoItem;

public class RoomResponseModel : BaseResponseModel
{
    public RoomTypeEnum RoomType { get; set; }

    public bool AvailabilityStatus { get; set; }

    public int RoomNumber { get; set; }

    public int Price { get; set; }

    public string Helper { get; set; }
}
