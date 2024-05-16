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

namespace HotelMS.Frontend.Pages.Reservations
{
    public class RefundModel : PageModel
    {
        private readonly IReservationService _reservationService;

        public RefundModel(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var reservation = await _reservationService.GetById(id);
            if (reservation == null)
            {
                return base.BadRequest($"Unable to find reservation with ID '{id}'.");
            }

            return Page();

        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            var reservation = await _reservationService.GetById(id);
            if (reservation == null)
            {
                return base.BadRequest($"Unable to find reservation with ID '{id}'.");
            }

            await _reservationService.DeleteAsync(id);

            return RedirectToPage("./Index");
        }
    }
}
