using System;
using System.Collections.Generic;
using System.Text;
using InstantMessenger.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace InstantMessenger.API.Controllers
{
    [ApiController]
    [Route($"api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService userService;
        public UserController (UserService userService)
        {
            this.userService = userService;
        }
        [HttpGet("{nick}")]
        public async Task<IActionResult> AddUser(string nick)
        {
            await userService.AddUserAsync(nick);
            return Ok();
        }
    }
}
