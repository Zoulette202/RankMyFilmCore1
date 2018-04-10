using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RankMyFilmCore.Data;
using RankMyFilmCore.Models;
using RankMyFilmCore.Models.AccountViewModels;
using RankMyFilmCore.Services;
using RankMyFilmCore.Utilitaire;

namespace RankMyFilmCore.API
{
    [Produces("application/json")]
    [Route("api/User")]
    [EnableCors("CorsPolicy")]
    
    public class ApplicationUsersAPIController : Controller
    {
        private readonly ApplicationDbContext _context;

        public readonly UserManager<ApplicationUser> _userManager;
        public readonly SignInManager<ApplicationUser> _signInManager;
        public readonly IEmailSender _emailSender;
        public readonly RoleManager<IdentityRole> _roleManager;


        public readonly IConfiguration _configuration;
        public ApplicationUsersAPIController(ApplicationDbContext context,
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

        // GET: api/user
        [HttpGet]
        public async Task<IActionResult> GetApplicationUser()
        {

            var applicationUsers = _context.ApplicationUser.ToList();
            foreach (var a in applicationUsers)
            {

                var listFriendJesuit = await (from friend in _context.friendsModel
                                              where friend.idSuiveur == a.Id
                                              select friend).ToListAsync();

                var listFriendOnMeSuit = await (from friend in _context.friendsModel
                                                where friend.idSuivi == a.Id
                                                select friend).ToListAsync();

                if (listFriendJesuit.Count == 0)
                {
                    a.nbJesuis = 0;
                }
                else
                {
                    foreach (var f in listFriendJesuit)
                    {
                        a.nbJesuis += 1;
                    }
                }


                if (listFriendOnMeSuit.Count == 0)
                {
                    a.nbOnMeSuis = 0;
                }
                else
                {
                    foreach (var f in listFriendOnMeSuit)
                    {
                        a.nbOnMeSuis += 1;
                    }
                }
                nbRankUser(a);
            }

            return Ok(applicationUsers);
        }

        // GET: api/user/getByName/xxVeustyxx
        [HttpGet("getByName/{pseudo}")]
        public async Task<IActionResult> GetApplicationUserByName([FromRoute] string pseudo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var applicationUser = await (from ApplicationUser in _context.ApplicationUser
                                         where ApplicationUser.pseudo.StartsWith(pseudo)
                                         select ApplicationUser).ToListAsync();

            if (applicationUser.Count == 0)
            {
                return NotFound();
            }

            foreach (var a in applicationUser)
            {

                var listFriendJesuit = await (from friend in _context.friendsModel
                                              where friend.idSuiveur == a.Id
                                              select friend).ToListAsync();

                var listFriendOnMeSuit = await (from friend in _context.friendsModel
                                                where friend.idSuivi == a.Id
                                                select friend).ToListAsync();

                if (listFriendJesuit.Count == 0)
                {
                    a.nbJesuis = 0;
                }
                else
                {
                    foreach (var f in listFriendJesuit)
                    {
                        a.nbJesuis += 1;
                    }
                }


                if (listFriendOnMeSuit.Count == 0)
                {
                    a.nbOnMeSuis = 0;
                }
                else
                {
                    foreach (var f in listFriendOnMeSuit)
                    {
                        a.nbOnMeSuis += 1;
                    }
                }
                nbRankUser(a);
            }


            return Ok(applicationUser);
        }


        [HttpPost("create")]
        public async Task<Utilitaire.Utilitaire> CreateUser([FromBody] ModelCreateUser userParam)
        {
            Utilitaire.Utilitaire util = new Utilitaire.Utilitaire();
            if (userParam.userEmail == null && userParam.userPassword == null && userParam.userPseudo == null)
            {
                return util;
            }

            var user = new ApplicationUser { UserName = userParam.userEmail, Email = userParam.userEmail, pseudo = userParam.userPseudo, EmailConfirmed=true};
            var result = await _userManager.CreateAsync(user, userParam.userPassword);
            bool x = await _roleManager.RoleExistsAsync("User");
            if (!x)
            {
                var role = new IdentityRole();
                role.Name = "User";
                await _roleManager.CreateAsync(role);
                var result1 = await _userManager.AddToRoleAsync(user, "User");

            }else
            {
               await _userManager.AddToRoleAsync(user, "User");
            }

            

            if (result.Succeeded)
            {


                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                await _emailSender.SendEmailConfirmationAsync(userParam.userEmail, callbackUrl);

                await _signInManager.SignInAsync(user, isPersistent: false);



                var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userParam.userEmail),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"]));

                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Issuer"],
                    claims,
                    expires: expires,
                    signingCredentials: creds
                );

                var tok = new JwtSecurityTokenHandler().WriteToken(token);
                util.idUser = user.Id;
                util.tokenGenerate = tok;
                return util;
            }


            return util;

        }


        [HttpPost("login")]
        public async Task<Utilitaire.Utilitaire> Login([FromBody] ModelAuthentification userParam)
        {

            
            Utilitaire.Utilitaire util = new Utilitaire.Utilitaire();
            if (userParam.userEmail == null && userParam.userPassword == null)
            {
                return util;
            }
            var result = await _signInManager.PasswordSignInAsync(userParam.userEmail, userParam.userPassword, false, lockoutOnFailure: false);


            if (result.Succeeded)
            {
                var user = await (from u in _context.ApplicationUser
                                  where u.Email == userParam.userEmail
                                  select u).FirstOrDefaultAsync();
                var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userParam.userEmail),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"]));

                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Issuer"],
                    claims,
                    expires: expires,
                    signingCredentials: creds
                );

                var tok = new JwtSecurityTokenHandler().WriteToken(token);
                util.idUser = user.Id;
                util.tokenGenerate = tok;
                return util;
            }
            else
                return util;
        }


        [HttpGet("getByMailPassword/{email}/{password}")]
        public async Task<IActionResult> GetApplicationUserByMailPassword([FromRoute] string email, [FromRoute] string password)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var applicationUser = await (from ApplicationUser in _context.ApplicationUser
                                         where ApplicationUser.Email == email && ApplicationUser.PasswordHash == password // il faut hashé le password recu dans l'url
                                         select ApplicationUser).FirstOrDefaultAsync();

            if (applicationUser == null)
            {
                return NotFound();
            }
            nbRankUser(applicationUser);
            return Ok(applicationUser);
        }


        // GET: api/ApplicationUsersAPI/5
        [HttpGet("get/{id}/{idUser}")]
        public async Task<IActionResult> GetApplicationUserWithFollow([FromRoute] string id, [FromRoute] string idUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == idUser);
            var friendModeliLMeSuis = await (from friend in _context.friendsModel
                                             where friend.idSuiveur.ToString() == idUser && friend.idSuivi.ToString() == id
                                             select friend).ToListAsync();

            var friendModelJeLeSuis = await (from friend in _context.friendsModel
                                             where friend.idSuiveur.ToString() == id && friend.idSuivi.ToString() == idUser
                                             select friend).ToListAsync();

            if (applicationUser == null)
            {
                return NotFound();
            }

            if (friendModeliLMeSuis.Count == 0)
            {
                applicationUser.teSuis = false;
            }
            else
            {
                foreach (var f in friendModeliLMeSuis)
                {
                    applicationUser.nbOnMeSuis += 1;
                }
                applicationUser.teSuis = true;
            }


            if (friendModelJeLeSuis.Count == 0)
            {
                applicationUser.jeLeSuis = false;
            }
            else
            {
                foreach (var f in friendModelJeLeSuis)
                {
                    applicationUser.nbJesuis += 1;
                }
                applicationUser.jeLeSuis = true;
            }

            nbRankUser(applicationUser);

            return Ok(applicationUser);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetApplicationUser([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);

            var listFriendJesuit = await (from friend in _context.friendsModel
                                          where friend.idSuiveur == id
                                          select friend).ToListAsync();

            var listFriendOnMeSuit = await (from friend in _context.friendsModel
                                            where friend.idSuivi == id
                                            select friend).ToListAsync();

            if (listFriendJesuit.Count == 0)
            {
                applicationUser.nbJesuis = 0;
            }
            else
            {
                foreach (var f in listFriendJesuit)
                {
                    applicationUser.nbJesuis += 1;
                }
            }


            if (listFriendOnMeSuit.Count == 0)
            {
                applicationUser.nbOnMeSuis = 0;
            }
            else
            {
                foreach (var f in listFriendOnMeSuit)
                {
                    applicationUser.nbOnMeSuis += 1;
                }
            }


            if (applicationUser == null)
            {
                return NotFound();
            }

            nbRankUser(applicationUser);
            return Ok(applicationUser);
        }

        // PUT: api/ApplicationUsersAPI/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplicationUser([FromRoute] string id, [FromBody] ApplicationUser applicationUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != applicationUser.Id)
            {
                return BadRequest();
            }

            _context.Entry(applicationUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationUserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ApplicationUsersAPI
        [HttpPost]
        public async Task<IActionResult> PostApplicationUser([FromBody] ApplicationUser applicationUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ApplicationUser.Add(applicationUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetApplicationUser", new { id = applicationUser.Id }, applicationUser);
        }

        // DELETE: api/ApplicationUsersAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplicationUser([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            _context.ApplicationUser.Remove(applicationUser);
            await _context.SaveChangesAsync();

            return Ok(applicationUser);
        }

        private bool ApplicationUserExists(string id)
        {
            return _context.ApplicationUser.Any(e => e.Id == id);
        }

        private async void nbRankUser(ApplicationUser user)
        {
            var rankUser = await (from rank in _context.rankModel
                                  where rank.idUser == user.Id
                                  select rank).CountAsync();

            user.nbRank = rankUser;
        }





    }
}