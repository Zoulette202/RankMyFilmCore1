﻿using System;
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
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        
        private readonly ILogger _logger;


        public IConfiguration _configuration { get; set; }
        public ApplicationUsersAPIController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser>signManager, IConfiguration config, IEmailSender email )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signManager;
            _configuration = config;
            _emailSender = email;
            

        }

        // GET: api/user
        [HttpGet]
        public async Task<IActionResult> GetApplicationUser()
        {

            var applicationUsers =  _context.ApplicationUser.ToList();
            foreach (var a in applicationUsers)
            {

                var listFriendJesuit = await(from friend in _context.friendsModel
                                             where friend.idSuiveur == a.Id
                                             select friend).ToListAsync();

                var listFriendOnMeSuit = await(from friend in _context.friendsModel
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


        [HttpGet("create/{email}/{mdp}/{pseudo}")]
        public async Task<UtilitaireToken> createUser([FromRoute] string email, [FromRoute]string mdp , [FromRoute] string pseudo )
        {
             UtilitaireToken util = new UtilitaireToken();
           

                var user = new ApplicationUser { UserName = email, Email = email, pseudo = pseudo };
                var result = await _userManager.CreateAsync(user, mdp);
                if (result.Succeeded)
                {
                    

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                    await _emailSender.SendEmailConfirmationAsync(email, callbackUrl);

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    


                    var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
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
                    util.id = user.Id;
                    util.token = tok;
                    return util;     
                }
            

            return util;

        }


       [HttpGet("login/{email}/{mdp}")]
       public async Task<UtilitaireToken> GenerateJwtToken([FromRoute] string email, [FromRoute] string mdp)
        {
            UtilitaireToken util = new UtilitaireToken();
            
            var result = await _signInManager.PasswordSignInAsync(email, mdp, false, lockoutOnFailure: false);

            
            if (result.Succeeded)
            {
                var user = await (from u in _context.ApplicationUser
                                  where u.Email == email
                                  select u).FirstOrDefaultAsync(); 
                var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
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

             var tok=new JwtSecurityTokenHandler().WriteToken(token);
                util.id = user.Id;
                util.token = tok;
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
            } else
            {
                foreach(var f in friendModeliLMeSuis)
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

        public async void nbRankUser(ApplicationUser user)
        {
            var rankUser =  await (from rank in _context.rankModel
                                  where rank.idUser == user.Id
                                  select rank).CountAsync();

            user.nbRank = rankUser;
        }
    }
}