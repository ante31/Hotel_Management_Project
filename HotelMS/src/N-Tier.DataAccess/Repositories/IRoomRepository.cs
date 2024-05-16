using Microsoft.EntityFrameworkCore;
using HotelMS.Core.Entities;

namespace HotelMS.DataAccess.Repositories;

public interface IRoomRepository : IBaseRepository<Room> {
    Task<List<Room>> GetFreeRoomsByRange(DateTime dateFrom, DateTime dateTo, Guid? ReservationId);
}
