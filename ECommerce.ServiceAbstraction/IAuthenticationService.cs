using ECommerce.Shared.CommonResult;
using ECommerce.Shared.DTOS.IdentitDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServiceAbstraction
{
    public interface IAuthenticationService
    {
        Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO);
        Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO);

        Task<bool> CheckEmailAsync(string email);
        Task<Result<UserDTO>> GetUserByEmailAsync(string email);
    }
}
