using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelMS.Application.Models.TodoItem;
using HotelMS.Application.Models.User;
using HotelMS.Application.Services;
using HotelMS.Core.Entities;
using HotelMS.Core.Entities.Identity;
using HotelMS.DataAccess.Persistence;

namespace HotelMS.Frontend.Pages.Users
{
    [Authorize(Roles = ("Administrator"))]
    public class EditModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EditModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public UpdateUserModel User { get; set; } = default!;

        public List<IdentityRole> Roles { get; set; } = new List<IdentityRole>();

        public string RoleName;
        public string OldRoleName;


        public async Task<IActionResult> OnGetAsync(string id)
        {
            Roles = await _roleManager.Roles.ToListAsync();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return base.BadRequest($"Unable to load user with ID '{id}'.");
            }

            RoleName = (await _userManager.GetRolesAsync(user)).FirstOrDefault();


            User = new UpdateUserModel
            {
               Email = user.Email,
               FirstName = user.FirstName,
               LastName = user.LastName, 
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id, string rolename)
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

            user.Email = User.Email;
            user.FirstName = User.FirstName;
            user.LastName = User.LastName;

            await _userManager.UpdateAsync(user);

            if (rolename == "Administrator")
            {
                await _userManager.RemoveFromRolesAsync(user, new List<string> { "Guest" });
            }
            else if(rolename == "Guest")
            {
                await _userManager.RemoveFromRolesAsync(user, new List<string> { "Administrator" });
            }
            await _userManager.AddToRoleAsync(user, rolename);



            return Redirect("~/Users/Index");
        }
    }
}
