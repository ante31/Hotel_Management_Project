using HotelMS.Application.Models;
using HotelMS.Application.Models.TodoItem;
namespace HotelMS.Application.Services;

public interface IReservationService
{
    Task<IEnumerable<ReservationResponseModel>> GetPendingAsync();

    Task<IEnumerable<ReservationResponseModel>> GetAllAsync();

    Task<IEnumerable<ReservationResponseModel>> GetAllActiveAsync(string filterString);

    Task<IEnumerable<ReservationResponseModel>> GetRangeAsync(DateTime from, DateTime to, int roomNumber);

    Task<IEnumerable<ReservationResponseModel>> GetByRoomIdAsync(Guid RoomId);

    Task<IEnumerable<ReservationResponseModel>> GetAllBySearchStringAsync(string searchString, string filterString);

    Task<IEnumerable<ReservationResponseModel>> GetAllPendingBySearchStringAsync(string searchString, string filterString);

    Task<IEnumerable<ReservationResponseModel>> GetUserReservationsAsync(Guid userId);

    Task<IEnumerable<ReservationResponseModel>> GetAllUserReservationsBySearchStringAsync(Guid userId, string searchString, string filterString);

    Task<ReservationResponseModel> GetById(Guid id);

    Task<CreateReservationResponseModel> CreateAsync(CreateReservationModel createReservationModel,
        CancellationToken cancellationToken = default);

    Task<UpdateReservationResponseModel> UpdateAsync(Guid id, UpdateReservationModel updateReservationModel,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Guid id);

    Task<bool> DeleteByUserIdAsync(string id);

    Task<bool> DeleteByRoomIdAsync(Guid id);

    Task<bool> PayAsync(Guid id);

    Task<bool> ApproveAsync(Guid id);
}
