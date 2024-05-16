using System;
using Microsoft.AspNetCore.Identity;
using HotelMS.Core.Entities.Identity;
using HotelMS.DataAccess.Persistence;

namespace HotelMS.API
{
    public static class ServiceExtensions
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddRoles<IdentityRole>();
        }
    }
}
