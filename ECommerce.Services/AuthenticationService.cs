using ECommerce.Domain.Entities.IdentityModule;
using ECommerce.ServiceAbstraction;
using ECommerce.Shared.CommonResult;
using ECommerce.Shared.DTOS.IdentitDTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthenticationService(UserManager<ApplicationUser>userManager)
        {
            _userManager= userManager;
        }
        public async Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null)
            {
                return Error.InvalidCredentials("Invalid email or password.");
            }
            var IsPasswordValid = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
            if (!IsPasswordValid)
            {
                return Error.InvalidCredentials("Password Invalid");
            }
            return new UserDTO(user.Email!, user.DisplayName, "Token");

        }

        public async Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO)
        {
            var User = new ApplicationUser()
            {
                Email = registerDTO.Email,
                DisplayName = registerDTO.DisplayName,
                PhoneNumber = registerDTO.PhoneNumber,
                UserName = registerDTO.UserName,

            };
            var IdentityResult= await _userManager.CreateAsync(User,registerDTO.Password);
            if (IdentityResult.Succeeded)
            {
                return new UserDTO(User.Email, User.DisplayName, "Token");
            }
            return IdentityResult.Errors.Select(E=>Error.Validation(E.Code,E.Description)).ToList();
        }
    }
}
