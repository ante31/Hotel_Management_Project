using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HotelMS.Application;
using HotelMS.Application.Models.TodoItem;
using HotelMS.Application.Services;
using HotelMS.Core.Entities;
using HotelMS.DataAccess.Persistence;

namespace HotelMS.Frontend.Pages.Rooms
{
    [Authorize(Roles = ("Administrator"))]
    public class IndexModel : PageModel
    {
        private readonly IRoomService _roomService;

        public IndexModel(IRoomService roomService)
        {
            _roomService = roomService;
        }

        public int pageIndex { get; set; } = 1;
        public bool HasPreviousPage { get; set; } = false;
        public bool HasNextPage { get; set; } = false;

        [BindProperty]
        public IEnumerable<RoomResponseModel> Rooms { get; set; } = default!;

        public async Task<PageResult> OnGetAsync(string searchString, int pageNumber, string sortString, int pageSize = 5, string filterString = "none")
        {
            ViewData["searchString"] = searchString;
            ViewData["pageSize"] = pageSize;
            ViewData["filterString"] = filterString;
            ViewData["sortString"] = sortString;

            pageNumber = pageNumber == 0 ? 1 : pageNumber;

            Rooms = await _roomService.GetAllAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                Rooms = Rooms.Where(item =>
                    item.RoomNumber.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                    item.RoomType.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                    item.Price.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            switch (filterString.ToLower())
            {
                case "available":
                    Rooms = Rooms.Where(item => item.AvailabilityStatus);
                    break;
                case "unavailable":
                    Rooms = Rooms.Where(item => !item.AvailabilityStatus);
                    break;
            }

            int roomsSize = Rooms.Count();

            Rooms = PaginatedList<RoomResponseModel>.Create(Rooms, pageNumber, pageSize);

            if(pageNumber > 1)
            {
                HasPreviousPage = true;
            }
            if ((pageNumber * pageSize) < roomsSize)
            {
                HasNextPage = true;
            }

            switch (sortString)
            {
                case "RoomNumberDesc":
                    Rooms = Rooms.OrderByDescending(item => item.RoomNumber).ToList();
                    break;
                case "RoomTypeAsc":
                    Rooms = Rooms.OrderBy(item => item.RoomType).ToList();
                    break;
                case "RoomTypeDesc":
                    Rooms = Rooms.OrderByDescending(item => item.RoomType).ToList();
                    break;
                case "AvailabilityStatusAsc":
                    Rooms = Rooms.OrderBy(item => item.AvailabilityStatus).ToList();
                    break;
                case "AvailabilityStatusDesc":
                    Rooms = Rooms.OrderByDescending(item => item.AvailabilityStatus).ToList();
                    break;
                case "PriceAsc":
                    Rooms = Rooms.OrderBy(item => item.Price).ToList();
                    break;
                case "PriceDesc":
                    Rooms = Rooms.OrderByDescending(item => item.Price).ToList();
                    break;
                default:
                    Rooms = Rooms.OrderBy(item => item.RoomNumber).ToList(); // Default sorting by room number ascending
                    break;
            }


            pageIndex = pageNumber;

            return Page();
        }

    }
}
