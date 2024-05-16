using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelMS.Application.Models;
using HotelMS.Application.Models.TodoItem;
using HotelMS.Application.Models.TodoList;
using HotelMS.Application.Services;
using HotelMS.Application.Services.Impl;

namespace HotelMS.API.Controllers;

public class RoomController : ApiController
{
    private readonly IRoomService _roomService;

    public RoomController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(ApiResult<IEnumerable<RoomResponseModel>>.Success(await _roomService.GetAllAsync()));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(ApiResult<RoomResponseModel>.Success(await _roomService.GetById(id)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return Ok(ApiResult<bool>.Success(await _roomService.DeleteAsync(id)));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateRoomModel createRoomModel)
    {
        return Ok(ApiResult<CreateRoomResponseModel>.Success(
            await _roomService.CreateAsync(createRoomModel)));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateRoomModel updateRoomModel)
    {
        return Ok(ApiResult<UpdateRoomResponseModel>.Success(
            await _roomService.UpdateAsync(id, updateRoomModel)));
    }
}
