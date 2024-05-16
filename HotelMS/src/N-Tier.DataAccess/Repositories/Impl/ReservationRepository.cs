using Microsoft.EntityFrameworkCore;
using HotelMS.Core.Entities;
using HotelMS.DataAccess.Persistence;
using System.Collections.Generic;
using HotelMS.DataAccess.Persistence.Migrations;

namespace HotelMS.DataAccess.Repositories.Impl;

public class ReservationRepository : BaseRepository<Reservation>, IReservationRepository
{
    public ReservationRepository(DatabaseContext context) : base(context) { }

    public async Task<IEnumerable<Reservation>> GetAll()
    {
        return await DbSet
           .Include(e => e.Room)
           .ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetAllActive()
    {
        return await DbSet
            .Include(e => e.Room)
            .Where(e => e.Deleted == false)
            .ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetAllActivePending()
    {
        return await DbSet
            .Include(e => e.Room)
            .Where(e => e.Approval == false && e.Deleted == false)
            .ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetReservationsByRange(DateTime from, DateTime to, int RoomNumber)
    {
        return await DbSet
            .Include(e => e.Room)
            .Where(r => ((r.From <= from && r.To > from) ||  // New reservation starts inside existing reservation
                        (r.From >= from && r.To <= to) ||    // Existing reservation is completely inside new reservation
                        (r.From < to && r.To >= to))         // New reservation ends inside existing reservation
                        && r.Approval && !r.Deleted && r.Room.RoomNumber == RoomNumber)
            .ToListAsync();
    }

    public async Task<Reservation> GetById(Guid id)
    {
        return await DbSet.Include(e => e.Room).Where(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Reservation>> GetByUserId(Guid userId)
    {
        return await DbSet
            .Include(e => e.Room)
            .Where(reservation => reservation.CreatedBy == userId.ToString() && reservation.Deleted == false)
            .ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetByRoomId(Guid roomId)
    {
        return await DbSet
            .Include(e => e.Room)
            .Where(reservation => reservation.Room.Id == roomId && reservation.Deleted == false)
            .ToListAsync();
    }
}
