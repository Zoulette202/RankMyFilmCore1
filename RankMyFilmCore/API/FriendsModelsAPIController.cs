using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RankMyFilmCore;
using RankMyFilmCore.Data;
using RankMyFilmCore.Models;

namespace RankMyFilmCore.API
{
    [Produces("application/json")]
    [Route("api/Friend")]
    [EnableCors("CorsPolicy")]
    public class FriendsModelsAPIController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FriendsModelsAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/FriendsModels
        [HttpGet]
        public async Task<IActionResult> GetfriendsModel()
        {
            foreach(var item in _context.friendsModel)
            {
                var pseudoSuiveur = await (from ApplicationUser in _context.ApplicationUser
                                           where ApplicationUser.Id == item.idSuiveur
                                           select ApplicationUser).FirstOrDefaultAsync();

                var pseudoSuivi = await (from ApplicationUser in _context.ApplicationUser
                                    where ApplicationUser.Id == item.idSuivi
                                    select ApplicationUser).FirstOrDefaultAsync();

                item.pseudoSuiveur = pseudoSuiveur.pseudo;
                item.pseudoSuivi = pseudoSuivi.pseudo;
            }
            return Ok(_context.friendsModel);
        }

        // GET: api/FriendsModels/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFriendsModel([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var friendsModel = await _context.friendsModel.SingleOrDefaultAsync(m => m.ID == id);

            if (friendsModel == null)
            {
                return NotFound();
            }

            return Ok(friendsModel);
        }


        /// <summary>
        /// idParamMoi représente la personne connecté en params 
        /// Elle te dit si tu suis la personne ou pas
        /// </summary>
        /// <param name="idMoi"></param>
        /// <param name="idQuelqun"></param>
        /// <returns></returns>
        [HttpGet("getMyFriend/{idMoi}/{idQuelqun}")]
        public async Task<IActionResult> getJeSuis([FromRoute] string idMoi, [FromRoute] string idQuelqun)
        {

            bool trouver = false;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ListfriendModel = await (from friend in _context.friendsModel
                                         where friend.idSuiveur.ToString() == idMoi && friend.idSuivi.ToString() == idQuelqun
                                         select friend).ToListAsync();

            if (ListfriendModel == null)
            {
                return NotFound();
            }
            if (ListfriendModel.Count > 0)
            {
                trouver = true;
            }

            return Ok(trouver);
        }


        /// <summary>
        /// Crée un lien d'amitié
        /// </summary>
        /// <param name="idMoi"></param>
        /// <param name="idQuelqun"></param>
        /// <returns></returns>
        [HttpGet("suivre/{idMoi}/{idQuelqun}")]
        public async Task<IActionResult> createFriends([FromRoute] string idMoi, [FromRoute] string idQuelqun)
        {
            if (ModelState.IsValid)
            {
                var friendsModel = new FriendsModel { ID = new Guid(), idSuiveur = idMoi, idSuivi = idQuelqun };
                var pseudoSuiveur = await (from ApplicationUser in _context.ApplicationUser
                                           where ApplicationUser.Id == friendsModel.idSuiveur
                                           select ApplicationUser).FirstOrDefaultAsync();

                var pseudoSuivi = await (from ApplicationUser in _context.ApplicationUser
                                         where ApplicationUser.Id == friendsModel.idSuivi
                                         select ApplicationUser).FirstOrDefaultAsync();

                friendsModel.pseudoSuiveur = pseudoSuiveur.pseudo;
                friendsModel.pseudoSuivi = pseudoSuivi.pseudo;
                _context.Add(friendsModel);
                await _context.SaveChangesAsync();

                return Ok(friendsModel);
            }else
            {
                return BadRequest(ModelState);
            }
        }



        /// <summary>        
        /// Elle te dit si la personne te suis ou pas
        /// </summary>
        /// <param name="idMoi"></param>
        /// <param name="idQuelqun"></param>
        /// <returns></returns>
        [HttpGet("getIlMeSuis/{idMoi}/{idQuelqun}")]
        public async Task<IActionResult> getIlMeSuis([FromRoute] string idMoi, [FromRoute] string idQuelqun)
        {

            bool trouver = false;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var listFriendsModel = await (from friend in _context.friendsModel
                                          where friend.idSuiveur.ToString() == idQuelqun && friend.idSuivi.ToString() == idMoi
                                          select friend).ToListAsync();

            if (listFriendsModel == null)
            {
                return NotFound();
            }
            if (listFriendsModel.Count > 0)
            {
                trouver = true;

            }

            return Ok(trouver);
        }

        /// <summary>
        /// Renvoie la liste d'amie de l'utilisateur      
        /// </summary>
        /// <param name="idMoi"></param>
        /// <returns></returns>
        [HttpGet("getMyListFriend/{idMoi}")]
        public async Task<IActionResult> getMyListFriend([FromRoute] string idMoi)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ListfriendModel = await (from friend in _context.friendsModel
                                         where friend.idSuiveur.ToString() == idMoi
                                         select friend).ToListAsync();

            if (ListfriendModel == null)
            {
                return NotFound();
            }
            List<ApplicationUser> listMyFriend = new List<ApplicationUser>();
            if (ListfriendModel.Count > 0)
            {

                foreach (var item in ListfriendModel)
                {
                    var applicationUser = await (from ApplicationUser in _context.ApplicationUser
                                                 where ApplicationUser.Id == item.idSuivi.ToString()
                                                 select ApplicationUser).FirstOrDefaultAsync();

                    if (applicationUser == null)
                    {
                        return NotFound();
                    }
                    listMyFriend.Add(applicationUser);

                }
            }

            return Ok(listMyFriend);
        }


        [HttpGet("getMyListFollow/{idMoi}")]
        public async Task<IActionResult> getMyListFollow([FromRoute] string idMoi)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ListfriendModel = await (from friend in _context.friendsModel
                                         where friend.idSuivi.ToString() == idMoi
                                         select friend).ToListAsync();

            if (ListfriendModel == null)
            {
                return NotFound();
            }
            List<ApplicationUser> listMyFollow = new List<ApplicationUser>();
            if (ListfriendModel.Count > 0)
            {

                foreach (var item in ListfriendModel)
                {
                    var applicationUser = await (from ApplicationUser in _context.ApplicationUser
                                                 where ApplicationUser.Id == item.idSuiveur.ToString()
                                                 select ApplicationUser).FirstOrDefaultAsync();

                    if (applicationUser == null)
                    {
                        return NotFound();
                    }
                    listMyFollow.Add(applicationUser);

                }
            }

            return Ok(listMyFollow);
        }


        // PUT: api/FriendsModels/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFriendsModel([FromRoute] Guid id, [FromBody] FriendsModel friendsModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != friendsModel.ID)
            {
                return BadRequest();
            }

            _context.Entry(friendsModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FriendsModelExists(id))
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

        // POST: api/FriendsModels
        [HttpPost]
        public async Task<IActionResult> PostFriendsModel([FromBody] FriendsModel friendsModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.friendsModel.Add(friendsModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFriendsModel", new { id = friendsModel.ID }, friendsModel);
        }

        // DELETE: api/FriendsModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFriendsModel([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var friendsModel = await _context.friendsModel.SingleOrDefaultAsync(m => m.ID == id);
            if (friendsModel == null)
            {
                return NotFound();
            }

            _context.friendsModel.Remove(friendsModel);
            await _context.SaveChangesAsync();

            return Ok(friendsModel);
        }

        private bool FriendsModelExists(Guid id)
        {
            return _context.friendsModel.Any(e => e.ID == id);
        }
    }
}