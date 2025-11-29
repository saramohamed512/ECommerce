using ECommerce.ServiceAbstraction;
using ECommerce.Shared.DTOS.IdentitDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
    public class AuthenticationController : ApiBaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        [HttpPut("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var Result = await _authenticationService.LoginAsync(loginDTO);
            return HandleResult(Result);
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            var result = await _authenticationService.RegisterAsync(registerDTO);
            return HandleResult(result);
        }
        [Authorize]
        [HttpGet("emailExist")]
        public async Task<ActionResult<bool>> CheckEmail(string email)
        {
            var result = await _authenticationService.CheckEmailAsync(email);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("CurrentUser")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser(string email)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var result = await _authenticationService.GetUserByEmailAsync(Email!);
            return HandleResult(result);
        }
    }
}
