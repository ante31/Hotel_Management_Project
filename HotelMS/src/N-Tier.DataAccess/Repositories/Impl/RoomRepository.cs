using Microsoft.EntityFrameworkCore;
using HotelMS.Core.Entities;
using HotelMS.DataAccess.Persistence;

namespace HotelMS.DataAccess.Repositories.Impl;

public class RoomRepository : BaseRepository<Room>, IRoomRepository
{
    private readonly DatabaseContext _context;

    public RoomRepository(DatabaseContext context) : base(context) {
        _context = context;
    }

    public async Task<List<Room>> GetFreeRoomsByRange(DateTime dateFrom, DateTime dateTo, Guid? reservationId)
    {

        // Get reservations that overlap with the specified range
        var overlappingReservations = await _context.Reservation
            .Where(r => ((r.From <= dateFrom && r.To > dateFrom) ||  // New reservation starts inside existing reservation
                        (r.From >= dateFrom && r.To <= dateTo) ||    // Existing reservation is completely inside new reservation
                        (r.From < dateTo && r.To >= dateTo))         // New reservation ends inside existing reservation
                        && r.Approval && !r.Deleted && r.Id != reservationId)
            .Select(r => r.Room.Id)

            .ToListAsync();


        // Get rooms that are not reserved during the specified range
        var freeRooms = await _context.Room
            .Where(room => !overlappingReservations.Contains(room.Id) && !room.Deleted && room.AvailabilityStatus)
            .ToListAsync();

        return freeRooms;
    }
}
