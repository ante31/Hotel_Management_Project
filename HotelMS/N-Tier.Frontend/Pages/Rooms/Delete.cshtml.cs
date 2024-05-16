using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HotelMS.Application.Models.TodoItem;
using HotelMS.Application.Services;
using HotelMS.Application.Services.Impl;
using HotelMS.Core.Entities;
using HotelMS.DataAccess.Persistence;
using HotelMS.DataAccess.Persistence.Migrations;

namespace HotelMS.Frontend.Pages.Rooms
{
    [Authorize(Roles = ("Administrator"))]
    public class DeleteModel : PageModel
    {
        private readonly IRoomService _roomService;
        private readonly IReservationService _reservationService;

        public DeleteModel(IRoomService roomService, IReservationService reservationService)
        {
            _roomService = roomService;
            _reservationService = reservationService;
        }


        public IEnumerable<ReservationResponseModel> Reservations { get; set; } = default!;


        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var Room = await _roomService.GetById(id);
            if (Room == null)
            {
                return base.BadRequest($"Unable to find room with ID '{id}'.");
            }

            Reservations = await _reservationService.GetByRoomIdAsync(Room.Id);
            ViewData["paidCount"] = Reservations.Where(reservation => reservation.Payed).ToList().Count();
            ViewData["approvedCount"] = Reservations.Where(reservation => reservation.Approval && !reservation.Payed).ToList().Count();

            return Page();

        }


        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            var Room = await _roomService.GetById(id);
            if (Room == null)
            {
                return base.BadRequest($"Unable to find room with ID '{id}'.");
            }

            await _roomService.DeleteAsync(id);
            await _reservationService.DeleteByRoomIdAsync(id);


            return RedirectToPage("./Index");
        }
    }
}
