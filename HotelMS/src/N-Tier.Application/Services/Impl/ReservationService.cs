using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HotelMS.Application.Models;
using HotelMS.Application.Models.TodoItem;
using HotelMS.Application.Models.TodoList;
using HotelMS.Core.Entities;
using HotelMS.Core.Entities.Identity;
using HotelMS.DataAccess.Persistence.Migrations;
using HotelMS.DataAccess.Repositories;
using HotelMS.DataAccess.Repositories.Impl;

namespace HotelMS.Application.Services.Impl;

public class ReservationService : IReservationService
{
    private readonly IMapper _mapper;
    private readonly IReservationRepository _reservationRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public ReservationService(IReservationRepository reservationRepository,
        IRoomRepository roomRepository,
        UserManager<ApplicationUser> userManager,
        IMapper mapper)
    {
        _reservationRepository = reservationRepository;
        _roomRepository = roomRepository;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<bool> DeleteByUserIdAsync(string userId)
    {
        var reservations = await _reservationRepository.GetAllActive();

        foreach (var reservation in reservations)
        {
            if (reservation.CreatedBy == userId && !reservation.Payed)
            {
                reservation.Deleted = true;
                await _reservationRepository.UpdateAsync(reservation);
            }
        }

        return true;
    }

    public async Task<bool> DeleteByRoomIdAsync(Guid id)
    {
        var reservations = await _reservationRepository.GetAllActive();

        foreach (var reservation in reservations)
        {
            if (reservation.Room.Id == id && !reservation.Payed)
            {
                reservation.Deleted = true;
                await _reservationRepository.UpdateAsync(reservation);
            }
        }

        return true;
    }


    public async Task<bool> DeleteAsync(Guid id)
    {
        var reservation = await _reservationRepository.GetFirstAsync(x => x.Id == id);
        reservation.Deleted = true;

        await _reservationRepository.UpdateAsync(reservation);
        return true;
    }
    public async Task<bool> ApproveAsync(Guid id)
    {
        var reservation = await _reservationRepository.GetFirstAsync(x => x.Id == id);
        reservation.Approval = true;

        await _reservationRepository.UpdateAsync(reservation);
        return true;
    }


    public async Task<bool> PayAsync(Guid id)
    {
        var reservation = await _reservationRepository.GetFirstAsync(x => x.Id == id);
        reservation.Payed = true;

        await _reservationRepository.UpdateAsync(reservation);
        return true;
    }


    public async Task<IEnumerable<ReservationResponseModel>> GetAllAsync()
    {

        var reservations = await _reservationRepository.GetAll();

        return _mapper.Map<IEnumerable<ReservationResponseModel>>(reservations);
    }

    public async Task<IEnumerable<ReservationResponseModel>> GetAllActiveAsync(string filterString)
    {
        var reservations = await _reservationRepository.GetAllActive();

        switch (filterString.ToLower())
        {
            case "pending":
                reservations = reservations.Where(item => !item.Approval);
                break;
            case "approved":
                reservations = reservations.Where(item => item.Approval && !item.Payed);
                break;
            case "payed":
                reservations = reservations.Where(item => item.Payed);
                break;
        }

        return _mapper.Map<IEnumerable<ReservationResponseModel>>(reservations);
    }

    public async Task<IEnumerable<ReservationResponseModel>> GetRangeAsync(DateTime from, DateTime to, int RoomNumber)
    {
        var reservations = await _reservationRepository.GetReservationsByRange(from, to, RoomNumber);

        return _mapper.Map<IEnumerable<ReservationResponseModel>>(reservations);
    }

    public async Task<IEnumerable<ReservationResponseModel>> GetByRoomIdAsync(Guid RoomId)
    {
        var reservations = await _reservationRepository.GetByRoomId(RoomId);

        return _mapper.Map<IEnumerable<ReservationResponseModel>>(reservations);
    }

    public async Task<IEnumerable<ReservationResponseModel>> GetAllBySearchStringAsync(string searchString, string filterString)
    {
        var reservations = await _reservationRepository.GetAllActive();
        var rooms = await _roomRepository.GetAllAsync();
        var users = await _userManager.Users
                .Where(user => user.deleted == false)
                .ToListAsync();

        reservations = reservations.Where(item =>
            item.Price.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
            item.To.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
            item.From.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
            users.Any(user => user.Id == item.CreatedBy && user.Email.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)) ||
            rooms.Any(room => room.Id == item.Room.Id && room.RoomNumber.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)));

        switch (filterString.ToLower())
        {
            case "pending":
                reservations = reservations.Where(item => !item.Approval);
                break;
            case "approved":
                reservations = reservations.Where(item => item.Approval && !item.Payed);
                break;
            case "payed":
                reservations = reservations.Where(item => item.Payed);
                break;
        }

        return _mapper.Map<IEnumerable<ReservationResponseModel>>(reservations);
    }

    public async Task<IEnumerable<ReservationResponseModel>> GetAllPendingBySearchStringAsync(string searchString, string filterString)
    {
        var reservations = await _reservationRepository.GetAllActive();

        reservations = reservations.Where(item =>
            (item.Price.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
            item.To.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
            item.From.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)) && item.Approval == true);

        return _mapper.Map<IEnumerable<ReservationResponseModel>>(reservations);
    }

    public async Task<IEnumerable<ReservationResponseModel>> GetPendingAsync()
    {

        var reservations = await _reservationRepository.GetAllActivePending();
        
        return _mapper.Map<IEnumerable<ReservationResponseModel>>(reservations);
    }


    public async Task<IEnumerable<ReservationResponseModel>> GetUserReservationsAsync(Guid id)
    {

        var reservations = await _reservationRepository.GetByUserId(id);

        return _mapper.Map<IEnumerable<ReservationResponseModel>>(reservations);
    }

    public async Task<IEnumerable<ReservationResponseModel>> GetAllUserReservationsBySearchStringAsync(Guid userId, string searchString, string filterString)
    {
        var reservations = await _reservationRepository.GetByUserId(userId);

        if (!string.IsNullOrEmpty(searchString))
        {
            reservations = reservations.Where(item =>
            item.To.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
            item.From.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
            item.Price.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
            item.Room.RoomNumber.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase));
        }

        switch (filterString.ToLower())
        {
            case "pending":
                reservations = reservations.Where(item => !item.Approval);
                break;
            case "approved":
                reservations = reservations.Where(item => item.Approval && !item.Payed);
                break;
            case "payed":
                reservations = reservations.Where(item => item.Payed);
                break;
        }

        return _mapper.Map<IEnumerable<ReservationResponseModel>>(reservations);
    }


    public async Task<ReservationResponseModel> GetById(Guid id)
    {
        var reservation = await _reservationRepository.GetById(id);

        return _mapper.Map<ReservationResponseModel>(reservation);
    }


    public async Task<UpdateReservationResponseModel> UpdateAsync(Guid id, UpdateReservationModel updateReservationModel, CancellationToken cancellationToken = default)
    {
        var reservation = await _reservationRepository.GetFirstAsync(ti => ti.Id == id);

        var room = await _roomRepository.GetFirstAsync(x => x.Id == updateReservationModel.RoomId);

        // Update the properties of the existing reservation
        reservation.From = updateReservationModel.From;
        reservation.To = updateReservationModel.To;
        reservation.Room = room;
        reservation.Approval = updateReservationModel.Approval;
        reservation.Price = updateReservationModel.Price;

        // Save the changes to the existing reservation

        return new UpdateReservationResponseModel
        {
            Id = (await _reservationRepository.UpdateAsync(reservation)).Id
        };
    }

    public async Task<CreateReservationResponseModel> CreateAsync(CreateReservationModel createReservationModel, CancellationToken cancellationToken = default)
    {
        var room = await _roomRepository.GetFirstAsync(x => x.Id == createReservationModel.RoomId);

        var reservation = _mapper.Map<Reservation>(createReservationModel);
        reservation.Room = room;

        return new CreateReservationResponseModel
        {
            Id = (await _reservationRepository.AddAsync(reservation)).Id
        };
    }
}
