using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HotelMS.Application.Models.TodoItem;
using HotelMS.Application.Services;
using HotelMS.Core.Entities;
using HotelMS.Core.Entities.Identity;
using HotelMS.DataAccess.Persistence;

namespace HotelMS.Frontend.Pages.Users
{
    [Authorize(Roles = ("Administrator"))]
    public class DeleteModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IReservationService _reservationService;

        public DeleteModel(UserManager<ApplicationUser> userManager, IReservationService reservationService)
        {
            _userManager = userManager;
            _reservationService = reservationService;
        }

        public IEnumerable<ReservationResponseModel> Reservations { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var userId = new Guid(id);
            if (user == null)
            {
                return base.BadRequest($"Unable to find user with ID '{id}'.");
            }

            Reservations = await _reservationService.GetUserReservationsAsync(userId);
            ViewData["paidCount"] = Reservations.Where(reservation => reservation.Payed).ToList().Count();
            ViewData["approvedCount"] = Reservations.Where(reservation => reservation.Approval && !reservation.Payed).ToList().Count();

            return Page();

        }


        public async Task<IActionResult> OnPostAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return base.BadRequest($"Unable to load user with ID '{id}'.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            user.deleted = true;

            await _userManager.UpdateAsync(user);
            await _reservationService.DeleteByUserIdAsync(user.Id);

            return Redirect("~/Users/Index");
        }
    }
}
