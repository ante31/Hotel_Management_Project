using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HotelMS.Application;
using HotelMS.Application.Models.TodoItem;
using HotelMS.Application.Services;
using HotelMS.Core.Entities;
using HotelMS.Core.Entities.Identity;
using HotelMS.DataAccess.Persistence;

namespace HotelMS.Frontend.Pages.Users
{
    [Authorize(Roles = "Administrator")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public int pageIndex { get; set; } = 1;
        public bool HasPreviousPage { get; set; } = false;
        public bool HasNextPage { get; set; } = false;

        public IEnumerable<ApplicationUser> Users { get; set; } = default!;

        public async Task<PageResult> OnGetAsync(string searchString, int pageNumber, string sortString, int pageSize = 5)
        {
            ViewData["searchString"] = searchString;
            ViewData["sortString"] = sortString;
            ViewData["pageSize"] = pageSize;

            pageNumber = pageNumber == 0 ? 1 : pageNumber;

            Users = await _userManager.Users
                .Where(user => user.deleted == false)
                .ToListAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                Users = Users.Where(user => (user.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                                    user.FirstName.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                                    user.LastName.Contains(searchString, StringComparison.OrdinalIgnoreCase)));
            }

            switch (sortString)
            {
                case "FirstNameDesc":
                    Users = Users.OrderByDescending(item => item.FirstName).ToList();
                    break;
                case "LastNameAsc":
                    Users = Users.OrderBy(item => item.LastName).ToList();
                    break;
                case "LastNameDesc":
                    Users = Users.OrderByDescending(item => item.LastName).ToList();
                    break;
                case "EmailAsc":
                    Users = Users.OrderBy(item => item.Email).ToList();
                    break;
                case "EmailDesc":
                    Users = Users.OrderByDescending(item => item.Email).ToList();
                    break;
                default:
                    Users = Users.OrderBy(item => item.FirstName).ToList(); // Default sorting by FirstNameAsc
                    break;
            }

            int usersSize = Users.Count();

            Users = PaginatedList<ApplicationUser>.Create(Users, pageNumber, pageSize);

            if (pageNumber > 1)
            {
                HasPreviousPage = true;
            }
            if ((pageNumber * pageSize) < usersSize)
            {
                HasNextPage = true;
            }

            pageIndex = pageNumber;

            return Page();
        }
    }
}

