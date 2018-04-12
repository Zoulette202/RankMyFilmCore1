using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using RankMyFilmCore.Data;
using RankMyFilmCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RankMyFilmCore.Services
{
    public class CreateAdmin
    {
        private readonly ApplicationDbContext _context;

        public readonly UserManager<ApplicationUser> _userManager;
        public readonly RoleManager<IdentityRole> _roleManager;

        public CreateAdmin(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;

        }



        public async Task CreateRoles()
        {

            // In Startup iam creating first Admin Role and creating a default Admin User  
            bool roleExist = await _roleManager.RoleExistsAsync("Admin");
            if (!roleExist)
            {

                // first we create Admin rool   
                var role = new IdentityRole();
                role.Name = "Admin";
                await _roleManager.CreateAsync(role);

                //Here we create a Admin super user who will maintain the website                  

                var user = new ApplicationUser();
                user.UserName = "Steve";
                user.Email = "stevega@hotmail.fr";
                user.EmailConfirmed = true;
                user.pseudo = "SuperUser";
                string userPWD = "5Aout1995";

                bool userExist = _context.ApplicationUser.Any(e => e.Email == "stevega@hotmail.fr");
                if (!userExist)
                {
                    var chkUser = await _userManager.CreateAsync(user, userPWD);

                    //Add default User to Role Admin  
                    if (chkUser.Succeeded)
                    {
                        var result1 = _userManager.AddToRoleAsync(user, "Admin");

                    }
                }
            }
        }


        public async Task create()
        {


            var user = new ApplicationUser { UserName = "stevega@hotmail.fr", Email = "stevega@hotmail.fr", pseudo = "SuperUser", EmailConfirmed = true };
            var result = await _userManager.CreateAsync(user, "5Aout1995");
            bool x = await _roleManager.RoleExistsAsync("Admin");
            if (!x)
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                await _roleManager.CreateAsync(role);
                
                var result1 = await _userManager.AddToRoleAsync(user, "Admin");
                await _context.SaveChangesAsync();
            }
            else
            {
                await _userManager.AddToRoleAsync(user, "Admin");
                await _context.SaveChangesAsync();
            }

        }
    }
}
