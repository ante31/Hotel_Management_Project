using HotelMS.Application.Models;
using HotelMS.Application.Models.TodoItem;
namespace HotelMS.Application.Services;

public interface IRoomService
{
    Task<IEnumerable<RoomResponseModel>> GetAllAsync();

    Task<IEnumerable<RoomResponseModel>> GetRangeAsync(DateTime from, DateTime to, Guid? ReservationId = null);

    Task<RoomResponseModel> GetById(Guid id);

    Task<bool> IsRoomNumberTakenAsync(int roomNumber);


    Task<CreateRoomResponseModel> CreateAsync(CreateRoomModel createRoomModel,
        CancellationToken cancellationToken = default);

    Task<UpdateRoomResponseModel> UpdateAsync(Guid id, UpdateRoomModel updateRoomModel,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Guid id);
}
