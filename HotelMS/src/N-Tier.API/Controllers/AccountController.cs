using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HotelMS.Application.Models;
using HotelMS.Application.Models.TodoItem;
using HotelMS.Application.Models.User;
using HotelMS.Application.Services;
using HotelMS.Core.Entities.Identity;
using HotelMS.DataAccess.Persistence;
using System.Security.Principal;

namespace HotelMS.API.Controllers;

public class UsersController : ApiController
{
    private readonly IUserService _userService;
    private readonly SignInManager<ApplicationUser> _signInManager;
   
    private readonly UserManager<ApplicationUser> _userManager;

    public UsersController(IUserService userService, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        userManager.Options.SignIn.RequireConfirmedAccount = false;
        userManager.Options.SignIn.RequireConfirmedPhoneNumber = false;
        userManager.Options.SignIn.RequireConfirmedEmail = false;

        _userService = userService;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterAsync(CreateUserModel createUserModel)
    {
        return Ok(ApiResult<CreateUserResponseModel>.Success(await _userService.CreateAsync(createUserModel)));
    }


    [HttpPost("confirmEmail")]
    public async Task<IActionResult> ConfirmEmailAsync(ConfirmEmailModel confirmEmailModel)
    {
        return Ok(ApiResult<ConfirmEmailResponseModel>.Success(
            await _userService.ConfirmEmailAsync(confirmEmailModel)));
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginPost(LoginUserModel user)
    {
        var loggedInUser = await _userService.LoginAsync(user);

        return Ok(ApiResult<LoginResponseModel>.Success(loggedInUser));
    ;


    }
}
