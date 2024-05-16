using Microsoft.AspNetCore.Identity;
using HotelMS.Core.Entities;
using HotelMS.Core.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace HotelMS.Application.Models.TodoItem;

public class ReservationResponseModel : BaseResponseModel
{
    public Guid CreatedBy { get; set; }

    [DataType(DataType.Date)]
    public DateTime From { get; set; }

    [DataType(DataType.Date)]
    public DateTime To { get; set; }

    public bool Approval { get; set; }

    public bool Payed { get; set; }

    public Room Room { get; set; }

    public double Price { get; set; }
}
