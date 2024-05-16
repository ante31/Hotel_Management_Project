using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelMS.Application.Models.TodoItem;
using HotelMS.Application.Services;
using HotelMS.Core.Entities;
using HotelMS.Core.Entities.Identity;
using HotelMS.DataAccess.Persistence;
using HotelMS.DataAccess.Persistence.Migrations;
using HotelMS.Frontend.Pages.Users;
using HotelMS.Core.Enums;

namespace HotelMS.Frontend.Pages.Reservations
{
    [IgnoreAntiforgeryToken]
    public class EditModel : PageModel
    {
        private readonly IReservationService _reservationService;
        private readonly IRoomService _roomService;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditModel(IReservationService reservationService, IRoomService roomService, UserManager<ApplicationUser> userManager)
        {
            _reservationService = reservationService;
            _roomService = roomService;
            _userManager = userManager;

        }

        [BindProperty]
        public UpdateReservationModel Reservation { get; set; } = default!;

        public IEnumerable<RoomResponseModel> Rooms { get; private set; } = new List<RoomResponseModel>();

        private static Guid _reservationId;

        public static Guid ReservationId
        {
            get { return _reservationId; }
            set { _reservationId = value; }
        }


        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            ReservationId = id;

            var reservation = await _reservationService.GetById(id);
            if (reservation == null)
            {
                return base.BadRequest($"Unable to load reservation with ID '{id}'.");
            }

            ViewData["createdBy"] = reservation.CreatedBy;

            Reservation = new UpdateReservationModel
            {
                RoomId = reservation.Room.Id,
                From = reservation.From,
                To = reservation.To,
                Approval = reservation.Approval,
                Price = reservation.Price
            };


            Rooms = await _roomService.GetRangeAsync(reservation.From, reservation.To, id);


            return Page();
        }

        public async Task<IActionResult> OnPostGetRange(DateTime dateFrom, DateTime dateTo)
        {
            var reservation = await _reservationService.GetById(ReservationId);
            Rooms = await _roomService.GetRangeAsync(dateFrom, dateTo, ReservationId);

            foreach (var room in Rooms)
            {
                room.Helper = Enum.GetName(typeof(RoomTypeEnum), room.RoomType);
            }

            return new JsonResult(Rooms);
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            var reservation = await _reservationService.GetById(id);
            ViewData["createdBy"] = reservation.CreatedBy;

            var user = await _userManager.GetUserAsync(User);
            var userId = new Guid(user.Id);

            if (reservation == null)
            {
                return base.BadRequest($"Unable to load reservation with ID '{id}'.");
            }

            if (reservation.CreatedBy == userId)
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
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var room = await _roomService.GetById(Reservation.RoomId);
                TimeSpan difference = Reservation.To - Reservation.From;
                Reservation.Price = (int)difference.TotalDays * room.Price;
                await _reservationService.UpdateAsync(id, Reservation);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            return Redirect("~/Reservations/Index");
        }
    }
}
