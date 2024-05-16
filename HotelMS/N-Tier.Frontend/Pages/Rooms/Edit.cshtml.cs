using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelMS.Application.Models.TodoItem;
using HotelMS.Application.Services;
using HotelMS.Core.Entities;
using HotelMS.DataAccess.Persistence;

namespace HotelMS.Frontend.Pages.Rooms
{
    [Authorize(Roles = ("Administrator"))]
    public class EditModel : PageModel
    {
        private readonly IRoomService _roomService;

        public EditModel(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [BindProperty]
        public UpdateRoomModel Room { get; set; } = default!;
        

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var room = await _roomService.GetById(id);
            if (room == null)
            {
                return base.BadRequest($"Unable to load room with ID '{id}'.");
            }

            Room = new UpdateRoomModel
            {
               RoomType = room.RoomType,
               AvailabilityStatus = room.AvailabilityStatus,
               Price = room.Price, 
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            var room = await _roomService.GetById(id);
            if (room == null)
            {
                return base.BadRequest($"Unable to load room with ID '{id}'.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _roomService.UpdateAsync(id, Room);

            return Redirect("~/Rooms/Index");
        }
    }
}
