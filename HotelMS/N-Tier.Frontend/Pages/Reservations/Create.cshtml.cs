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
using HotelMS.Application.Models.User;
using HotelMS.DataAccess.Persistence.Migrations;
using HotelMS.Core.Enums;

namespace HotelMS.Frontend.Pages.Reservations
{
    [IgnoreAntiforgeryToken]
    public class CreateModel : PageModel
    {
        private readonly IReservationService _reservationService;
        private readonly IRoomService _roomService;

        public IEnumerable<RoomResponseModel> Rooms { get; private set; } = new List<RoomResponseModel>();

        public CreateModel(IReservationService reservationService, IRoomService roomService)
        {
            _reservationService = reservationService;
            _roomService = roomService;
        }

        public async Task<IActionResult> OnGet()
        {
            Rooms = await _roomService.GetAllAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostGetRange(DateTime dateFrom, DateTime dateTo)
        {
            Rooms = await _roomService.GetRangeAsync(dateFrom, dateTo);

            foreach (var room in Rooms)
            {
                room.Helper = Enum.GetName(typeof(RoomTypeEnum), room.RoomType);
            }

            return new JsonResult(Rooms);
        }


        [BindProperty]
        public CreateReservationModel Reservation { get; set; } = default!;
        

        public async Task<IActionResult> OnPostAsync()
        {

            if (Reservation.From > Reservation.To)
            {
                ModelState.AddModelError("Reservation.To", "Dates are not chronological");
                return Page();
            }

            if (Reservation.From < DateTime.Today)
            {
                ModelState.AddModelError("Reservation.From", "Start date cannot be in the past");
                return Page();
            }

            if (Reservation.From == Reservation.To)
            {
                ModelState.AddModelError("Reservation.To", "Dates can not be identical");
                return Page();
            }

            try
            {
                var room = await _roomService.GetById(Reservation.RoomId); 
                var difference = Reservation.To - Reservation.From;
                Reservation.Price = (int)difference.TotalDays * room.Price;
                await _reservationService.CreateAsync(Reservation);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return RedirectToPage("./Index");
        }
    }
}
