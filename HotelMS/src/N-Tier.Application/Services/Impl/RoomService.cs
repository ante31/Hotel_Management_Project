using AutoMapper;
using Microsoft.EntityFrameworkCore;
using HotelMS.Application.Models;
using HotelMS.Application.Models.TodoItem;
using HotelMS.Application.Models.TodoList;
using HotelMS.Core.Entities;
using HotelMS.Core.Exceptions;
using HotelMS.DataAccess.Persistence.Migrations;
using HotelMS.DataAccess.Repositories;
using HotelMS.DataAccess.Repositories.Impl;

namespace HotelMS.Application.Services.Impl;

public class RoomService : IRoomService
{
    private readonly IMapper _mapper;
    private readonly IRoomRepository _roomRepository;

    public RoomService(IRoomRepository roomRepository,
        IMapper mapper)
    {
        _roomRepository = roomRepository;
        _mapper = mapper;
    }

    public async Task<CreateRoomResponseModel> CreateAsync(CreateRoomModel createRoomModel, CancellationToken cancellationToken = default)
    {
        var room = _mapper.Map<Room>(createRoomModel);

        return new CreateRoomResponseModel
        {
            Id = (await _roomRepository.AddAsync(room)).Id
        };
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var room = await _roomRepository.GetFirstAsync(x => x.Id == id);
        room.Deleted = true;

        var result = await _roomRepository.UpdateAsync(room);

        return result!=null;
    }

    public async Task<IEnumerable<RoomResponseModel>> GetAllAsync()
    {

        var rooms = await _roomRepository.GetAllAsync();
        rooms = rooms.Where(x => !x.Deleted).ToList();

        return _mapper.Map<IEnumerable<RoomResponseModel>>(rooms);
    }

    public async Task<RoomResponseModel> GetById(Guid id)
    {
        var room = await _roomRepository.GetFirstAsync(x => x.Id == id);

        return _mapper.Map<RoomResponseModel>(room);

    }

    public async Task<bool> IsRoomNumberTakenAsync(int roomNumber)
    {
        try
        {
            var existingRoom = await _roomRepository.GetFirstAsync(x => x.RoomNumber == roomNumber);
            return existingRoom != null;
        }
        catch (ResourceNotFoundException)
        {
            return false;
        }
    }


    public async Task<IEnumerable<RoomResponseModel>> GetRangeAsync(DateTime from, DateTime to, Guid? ReservationId)
    {
        var rooms = await _roomRepository.GetFreeRoomsByRange(from, to, ReservationId);

        return _mapper.Map<IEnumerable<RoomResponseModel>>(rooms);
    }

    public async Task<UpdateRoomResponseModel> UpdateAsync(Guid id, UpdateRoomModel updateRoomModel, CancellationToken cancellationToken = default)
    {
        var room = await _roomRepository.GetFirstAsync(ti => ti.Id == id);

        _mapper.Map(updateRoomModel, room);

        return new UpdateRoomResponseModel
        {
            Id = (await _roomRepository.UpdateAsync(room)).Id
        };
    }
}
