using HotelMS.Core.Entities;
using System.Collections.Generic;

namespace HotelMS.DataAccess.Repositories;

public interface IReservationRepository : IBaseRepository<Reservation> {

    Task<IEnumerable<Reservation>> GetAll();
    Task<IEnumerable<Reservation>> GetAllActive();
    Task<IEnumerable<Reservation>> GetReservationsByRange(DateTime from, DateTime to, int RoomNumber);

    Task<IEnumerable<Reservation>> GetAllActivePending();

    Task<Reservation> GetById(Guid id);
    Task<IEnumerable<Reservation>> GetByUserId(Guid userId);
    Task<IEnumerable<Reservation>> GetByRoomId(Guid userId);
}
