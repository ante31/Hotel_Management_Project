using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HotelMS.Core.Entities;
using HotelMS.DataAccess.Persistence;
using HotelMS.Application.Services;
using HotelMS.Application.Models.TodoItem;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace HotelMS.Frontend.Pages.Rooms
{
    [Authorize(Roles = ("Administrator"))]
    public class CreateModel : PageModel
    {
        private readonly IRoomService _roomService;

        public CreateModel(IRoomService roomService)
        {
            _roomService = roomService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public CreateRoomModel Room { get; set; } = default!;
        

        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid)
            {
                return Page();
            }

            // Check if the RoomNumber is already taken
            bool isRoomNumberTaken = await _roomService.IsRoomNumberTakenAsync(Room.RoomNumber);

            if (isRoomNumberTaken)
            {
                ModelState.AddModelError("Room.RoomNumber", "This room number is already taken.");
                return Page();
            }

            try
            {
                Room.AvailabilityStatus = true;
                await _roomService.CreateAsync(Room);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return RedirectToPage("./Index");
        }
    }
}
