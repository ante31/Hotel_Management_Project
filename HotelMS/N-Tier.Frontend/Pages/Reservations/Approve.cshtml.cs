using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HotelMS.Application.Services;
using HotelMS.Core.Entities;
using HotelMS.DataAccess.Persistence;
using HotelMS.Application.Models.TodoItem;

namespace HotelMS.Frontend.Pages.Reservations
{
    public class ApproveModel : PageModel
    {
        private readonly IReservationService _reservationService;

        public ApproveModel(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        public IEnumerable<ReservationResponseModel> Reservations { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var reservation = await _reservationService.GetById(id);
            if (reservation == null)
            {
                return base.BadRequest($"Unable to find reservation with ID '{id}'.");
            }

            Reservations = await _reservationService.GetRangeAsync(reservation.From, reservation.To, reservation.Room.RoomNumber);
            ViewData["approvedCount"] = Reservations.Count();


            return Page();

        }


        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            var reservation = await _reservationService.GetById(id);
            if (reservation == null)
            {
                return base.BadRequest($"Unable to find reservation with ID '{id}'.");
            }

            await _reservationService.ApproveAsync(id);

            return RedirectToPage("./Index");
        }
    }
}
