using System.ComponentModel.DataAnnotations;

namespace HotelMS.Application.Models.TodoItem;

public class UpdateReservationModel
{
    public Guid RoomId { get; set; }
    [DataType(DataType.Date)]
    public DateTime From { get; set; }
    [DataType(DataType.Date)]
    public DateTime To { get; set; }

    public bool Approval { get; set; }
    public bool Payed { get; set; }

    public double Price { get; set; }
}

public class UpdateReservationResponseModel : BaseResponseModel { }
