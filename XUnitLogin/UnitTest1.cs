using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using RankMyFilmCore.API;
using RankMyFilmCore.Data;
using RankMyFilmCore.Models;
using RankMyFilmCore.Services;
using RankMyFilmCore.Utilitaire;
using System;
using Xunit;

namespace XUnitLogin
{
    public class UnitTest1
    {
        public readonly ApplicationDbContext _context;
        public readonly UserManager<ApplicationUser> _userManager;
        public readonly SignInManager<ApplicationUser> _signInManager;
        public readonly IEmailSender _emailSender;
        public readonly RoleManager<IdentityRole> _roleManager;
        public readonly IConfiguration _configuration;
        public UnitTest1(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signManager,
            IConfiguration config,
            IEmailSender email,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signManager;
            _configuration = config;
            _emailSender = email;
            _roleManager = roleManager;


        }

        [Fact]
        public void Test1()
        {
            ModelAuthentification model = new ModelAuthentification { userEmail = "stevega@hotmail.fr", userPassword = "5Aout1995" };
            ApplicationUsersAPIController api = new ApplicationUsersAPIController(_context, _userManager, _signInManager, _configuration, _emailSender, _roleManager);
            var result = api.Login(model).IsCompletedSuccessfully;

            Assert.False(result, "1 should not be prime");
        }
    }
    
}
