using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Data.DataSeed
{
    public class IdentityDataInitializer : IDataInitilizer
    {
       
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<IdentityDataInitializer> _logger;
        public IdentityDataInitializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole>roleManager, ILogger<IdentityDataInitializer>logger) 
        {
            _userManager= userManager;
            _roleManager= roleManager;
            _logger= logger;
        }
        public async Task InitilizeAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }
                if (!_userManager.Users.Any())
                {
                    var User01 = new ApplicationUser
                    {
                        DisplayName = "Sara Mo",
                        UserName = "SaraMo",
                        Email = "sara@gmail.com",
                        PhoneNumber = "01234567890",


                    };
                    var User02 = new ApplicationUser
                    {
                        DisplayName = "Mo Ali",
                        UserName = "MoAli",
                        Email = "mo@gmail.com",
                        PhoneNumber = "01234567890",
                    };
                    await _userManager.CreateAsync(User01, "Pa$$w0rd");
                    await _userManager.CreateAsync(User02, "Pa$$w0rd");
                    await _userManager.AddToRoleAsync(User01, "Admin");
                    await _userManager.AddToRoleAsync(User02, "SuperAdmin");
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"Error occured during Identity data seeding: {ex}");
            }
        }
    }
}
