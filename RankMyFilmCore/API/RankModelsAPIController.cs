using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RankMyFilmCore;
using RankMyFilmCore.Data;

namespace RankMyFilmCore.WebApiRank
{
    [Produces("application/json")]
    [Route("api/Rank")]
    [EnableCors("CorsPolicy")]
    //[Authorize] // Require authenticated requests.
    public class RankModelsAPIController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        public RankModelsAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Rank
        [HttpGet]
        public IEnumerable<RankModel> GetrankModel()
        {
            return _context.rankModel;

        }

        // GET: api/Rank/get/5
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetRankModelById([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rankModel = await _context.rankModel.SingleOrDefaultAsync(m => m.ID == id);

            if (rankModel == null)
            {
                return NotFound();
            }

            return Ok(rankModel);
        }


        // GET: api/Rank/get/5
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetRankModelByIdUser([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rankModel = await _context.rankModel.SingleOrDefaultAsync(m => m.idUser == id);

            if (rankModel == null)
            {
                return NotFound();
            }

            return Ok(rankModel);
        }

        // PUT: api/Rank/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRankModel([FromRoute] Guid id, [FromBody] RankModel rankModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rankModel.ID)
            {
                return BadRequest();
            }

            _context.Entry(rankModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RankModelExists(id))
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

        // POST: api/Rank
        [HttpPost]
        public async Task<IActionResult> PostRankModel([FromBody] RankModel rankModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.rankModel.Add(rankModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRankModel", new { id = rankModel.ID }, rankModel);
        }

        [HttpGet("createRank/{idUser}/{idFilms}/{vote}")]
        public async Task<IActionResult> PostRankModelByIdUserIdFilm(string idUser, string idFilms, int vote)
        {
            var rankModel = await _context.rankModel.SingleOrDefaultAsync(m => m.idUser == idUser && m.idFilm == idFilms);

            if (rankModel == null)
            {
                var rank = new RankModel { idUser = idUser, idFilm = idFilms, Vote = vote };
                _context.rankModel.Add(rank);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetRankModel", new { id = rank.ID }, rank);
            }
            else
            {
                rankModel.Vote = vote;
                _context.Entry(rankModel).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RankModelExists(rankModel.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return CreatedAtAction("GetRankModel", new { id = rankModel.ID }, rankModel);
        }

        [HttpGet("createRank/{idUser}/{idFilms}/{vote}/{poster}/{title}")]
        public async Task<IActionResult> PostRankModelByIdUserIdFilmPosterAndTitle(string idUser, string idFilms, int vote,string poster, string title)
        {


            var rankModel = await _context.rankModel.SingleOrDefaultAsync(m => m.idUser == idUser && m.idFilm == idFilms);

            if (rankModel == null)
            {
                var rank = new RankModel { idUser = idUser, idFilm = idFilms, Vote = vote, poster = poster, Title = title};
                _context.rankModel.Add(rank);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetRankModel", new { id = rank.ID }, rank);
            }
            else
            {
                rankModel.Vote = vote;
                _context.Entry(rankModel).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RankModelExists(rankModel.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return CreatedAtAction("GetRankModel", new { id = rankModel.ID }, rankModel);
        }




        [HttpGet("getRankFilmsByFriend/{idUsers}/{idFilms}")]
        public async Task<IActionResult> getRankFilmsByFriend(string idUsers, string idFilms)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ListFriend = await (from friend in _context.friendsModel
                                    where friend.idSuiveur == idUsers
                                    select friend).ToListAsync();


            List<RankModel> listRank = new List<RankModel>();
            foreach (var item in ListFriend)
            {

                var rankFriend = await (from rank in _context.rankModel
                                        where rank.idUser == item.idSuivi.ToString()
                                        && rank.idFilm == idFilms
                                        select rank).FirstOrDefaultAsync();

                listRank.Add(rankFriend);

            }
            return Ok(listRank);

        }





        [HttpGet("getAverageRankFilmsByFriend/{idUsers}/{idFilms}")]
        public async Task<IActionResult> getAverageRankFilmsByFriend(string idUsers, string idFilms)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ListFriend = await (from friend in _context.friendsModel
                                    where friend.idSuiveur == idUsers
                                    select friend).ToListAsync();


            List<int> moyenByFriend = new List<int>();
            foreach (var item in ListFriend)
            {

                var rankFriend = await (from rank in _context.rankModel
                                        where rank.idUser == item.idSuivi.ToString()
                                        && rank.idFilm == idFilms
                                        select rank).FirstOrDefaultAsync();

                moyenByFriend.Add(rankFriend.Vote);

            }

            var value = moyenByFriend.Average();

            return Ok(value);

        }



        [HttpGet("GetRankModelByUserAndFilms/{idUser}/{idFilms}")]
        public async Task<IActionResult> GetRankModelByUserAndFilms(string idUser, string idFilms)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var rankModel = await (from rankModels in _context.rankModel
                                   where rankModels.idUser == idUser && rankModels.idFilm == idFilms
                                   select rankModels).FirstOrDefaultAsync();
           
            var ListFriend = await (from friend in _context.friendsModel
                                    where friend.idSuiveur == idUser
                                    select friend).ToListAsync();

            var ListAllUser = await (from user in _context.ApplicationUser
                                     select user).ToListAsync();

            List<int> moyenByFriend = new List<int>();
            List<int> moyenByAllUser = new List<int>();
            foreach (var item in ListFriend)
            {
                var rankFriend = await (from rank in _context.rankModel
                                        where rank.idUser == item.idSuivi && rank.idFilm == idFilms
                                        select rank).FirstOrDefaultAsync();

                if (rankFriend != null)
                {
                    moyenByFriend.Add(rankFriend.Vote);
                }
               
            }

            foreach (var item in ListAllUser)
            {
                var rankAllUser = await (from rank in _context.rankModel
                                         where rank.idUser == item.Id && rank.idFilm == idFilms
                                         select rank).FirstOrDefaultAsync();
                if (rankAllUser != null)
                {
                    moyenByAllUser.Add(rankAllUser.Vote);
                }
                
            }
            var valueFriend = 0.0;
            var valueAllUser = 0.0;
            if (moyenByFriend.Count > 0)
            {
                valueFriend = moyenByFriend.Average();
            }

            if(moyenByAllUser.Count > 0)
            {
                valueAllUser = moyenByAllUser.Average();
            }
             

            if (rankModel == null)
            {
                var rankModelVide = new RankModel { moyenneByFriend = valueFriend, moyenneByAllUser = valueAllUser };
                return Ok(rankModelVide);
            }
            rankModel.moyenneByFriend = valueFriend;
            rankModel.moyenneByAllUser = valueAllUser;

            return Ok(rankModel);
        }


        // DELETE: api/Rank/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteRankModel([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rankModel = await _context.rankModel.SingleOrDefaultAsync(m => m.ID == id);
            if (rankModel == null)
            {
                return NotFound();
            }

            _context.rankModel.Remove(rankModel);
            await _context.SaveChangesAsync();

            return Ok(rankModel);
        }

        private bool RankModelExists(Guid id)
        {
            return _context.rankModel.Any(e => e.ID == id);
        }
    }
}