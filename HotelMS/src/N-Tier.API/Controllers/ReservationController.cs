using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelMS.Application.Models;
using HotelMS.Application.Models.TodoItem;
using HotelMS.Application.Models.TodoList;
using HotelMS.Application.Services;
using HotelMS.Application.Services.Impl;

namespace HotelMS.API.Controllers;

public class ReservationController : ApiController
{
    private readonly IReservationService _reservationService;

    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(ApiResult<IEnumerable<ReservationResponseModel>>.Success(await _reservationService.GetAllAsync()));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(ApiResult<ReservationResponseModel>.Success(await _reservationService.GetById(id)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return Ok(ApiResult<bool>.Success(await _reservationService.DeleteAsync(id)));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateReservationModel createReservationModel)
    {
        return Ok(ApiResult<CreateReservationResponseModel>.Success(
            await _reservationService.CreateAsync(createReservationModel)));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateReservationModel updateReservationModel)
    {
        return Ok(ApiResult<UpdateReservationResponseModel>.Success(
            await _reservationService.UpdateAsync(id, updateReservationModel)));
    }
}
