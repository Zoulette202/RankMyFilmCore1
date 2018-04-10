using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RankMyFilmCore;
using RankMyFilmCore.Data;
using RankMyFilmCore.Models;

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
        public async Task<IActionResult> GetrankModel()
        {
            List<RankModel> allRanks = new List<RankModel>();
            var ListRank = await (from rank in _context.rankModel
                                  select rank).ToListAsync();

            var ListAllUser = await (from user in _context.ApplicationUser
                                     select user).ToListAsync();
            foreach (var ranks in ListRank)
            {
                var film = _context.filmModel.Where(f => f.idFilm == ranks.idFilm).FirstOrDefault();
                if (!(film == null))
                {
                    ranks.moyenneByAllUser = film.moyenne;
                    ranks.poster = film.poster;
                    ranks.title = film.title;
                    ranks.moyenneByFriend = 0;
                    allRanks.Add(ranks);
                }
            }


            return Ok(allRanks);

        }

        // GET: api/Rank/get/5
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetRankModelByIdUser([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

          

            var rankModel = await (from rank in _context.rankModel
                                   where rank.idUser == id
                                   select rank).ToListAsync();
            
            if (rankModel.Count == 0)
            {
                return Ok(rankModel);
            }
            List<RankModel> ranks = new List<RankModel>();
            foreach (var item in rankModel)
            {
                var film = _context.filmModel.Where(f => f.idFilm == item.idFilm).FirstOrDefault();
                if (!(film == null))
                {
                    item.moyenneByAllUser = film.moyenne;
                    item.poster = film.poster;
                    item.title = film.title;
                    item.moyenneByFriend = 0;
                    ranks.Add(item);
                }
            }

            return Ok(ranks);
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
                if (!rankModelExists(id))
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


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("createRank/{idUser}/{idFilms}/{ranksParam}")]
        public async Task<IActionResult> PostRankModelByIdUserIdFilm(string idUser, string idFilms, int ranksParam)
        {
            return Ok("ca marche");
        }



        //******************************************
        [HttpGet("createRank/{idUser}/{idFilms}/{ranksParam}/{posterParam}/{titleParam}")]
        public async Task<IActionResult> PostRankModelByIdUserIdFilmPosterAndTitle(string idUser, string idFilms, int ranksParam, string posterParam, string titleParam)
    {
            var film = _context.filmModel.Where(f => f.idFilm == idFilms).FirstOrDefault();

            if (film == null)
            {
                var createFilm = new FilmModel { idFilm = idFilms, title = titleParam, poster = posterParam, moyenne = ranksParam, nbRank = 1 };
                _context.filmModel.Add(createFilm);
                await _context.SaveChangesAsync();
                var ListFriend = from friend in _context.friendsModel
                                 where friend.idSuiveur == idUser
                                 select friend;
                var notesAllUsers = _context.rankModel.Where(x => x.idFilm == idFilms).ToList();
                 if (notesAllUsers.Count > 0)
                {
                    var moyenneFreinds = (from r in notesAllUsers
                                          join f in ListFriend on r.idUser equals f.idSuivi
                                          select r).Average(x => x.Vote);
                    var createRank = new RankModel { idUser = idUser, idFilm = idFilms, Vote = ranksParam, dateCréation = DateTime.Now, moyenneByAllUser = createFilm.moyenne, moyenneByFriend = moyenneFreinds, poster = posterParam, title = titleParam };
                    _context.rankModel.Add(createRank);
                    await _context.SaveChangesAsync();
                    return Ok(createRank);
                }
                else
                {
                    var createRank = new RankModel { idUser = idUser, idFilm = idFilms, Vote = ranksParam, dateCréation = DateTime.Now, moyenneByAllUser = createFilm.moyenne, moyenneByFriend = 0, poster = posterParam, title = titleParam };
                    _context.rankModel.Add(createRank);
                    await _context.SaveChangesAsync();
                    return Ok(createRank);
                }
               

               
            } else
            {
                var rankModel = await _context.rankModel.SingleOrDefaultAsync(m => m.idUser == idUser && m.idFilm == idFilms);
                if (rankModel == null)
                {
                    var ListFriend = from friend in _context.friendsModel
                                     where friend.idSuiveur == idUser
                                     select friend;
                    if (ListFriend.Count() == 0)
                    {

                        film.moyenne = (film.moyenne + ranksParam) / (film.nbRank + 1);
                        film.nbRank += 1;
                        _context.filmModel.Update(film);

                        var createRank = new RankModel { idUser = idUser, idFilm = idFilms, Vote = ranksParam, dateCréation = DateTime.Now, moyenneByFriend = 0, moyenneByAllUser = film.moyenne, title = titleParam, poster = posterParam };
                        _context.rankModel.Add(createRank);
                        await _context.SaveChangesAsync();

                        return Ok(createRank);

                    } else
                    {
                        var notesAllUsers = _context.rankModel.Where(x => x.idFilm == idFilms);

                        var moyenneFreinds = (from r in notesAllUsers
                                              join f in ListFriend on r.idUser equals f.idSuivi
                                              select r).Average(x => x.Vote);



                        film.moyenne = (film.moyenne + ranksParam) / (film.nbRank + 1);
                        film.nbRank += 1;


                        _context.filmModel.Update(film);

                        var createRank = new RankModel { idUser = idUser, idFilm = idFilms, Vote = ranksParam, dateCréation = DateTime.Now, moyenneByFriend = moyenneFreinds, moyenneByAllUser = film.moyenne, title = titleParam, poster = posterParam };
                        _context.rankModel.Add(createRank);

                        await _context.SaveChangesAsync();

                        return Ok(createRank);
                    }
                    
                } else
                {
                    var ListFriend = from friend in _context.friendsModel
                                     where friend.idSuiveur == idUser
                                     select friend;
                    var notesAllUsers = _context.rankModel.Where(x => x.idFilm == idFilms);

                    var moyenneFreinds = (from r in notesAllUsers
                                          join f in ListFriend on r.idUser equals f.idSuivi
                                          select r).Average(x => x.Vote);
                    rankModel.moyenneByFriend = moyenneFreinds;
                    rankModel.Vote = ranksParam;

                    film.moyenne = (film.moyenne + ranksParam) / (film.nbRank + 1);
                    _context.filmModel.Update(film);
                    _context.Entry(rankModel).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return Ok(rankModel);
                }
            }
        }



        [HttpGet("getRankFilmsByFriend/{idUsers}/{idFilms}")]
        public async Task<IActionResult> GetRankFilmsByFriend(string idUsers, string idFilms)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ListFriend = (from friend in _context.friendsModel
                                    where friend.idSuiveur == idUsers
                                    select friend);


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
        public async Task<IActionResult> GetAverageRankFilmsByFriend(string idUsers, string idFilms)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ListFriend = from friend in _context.friendsModel
                             where friend.idSuiveur == idUsers
                             select friend;
            var notesAllUsers = _context.rankModel.Where(x => x.idFilm == idFilms);

            var moyenneFreinds = (from r in notesAllUsers
                                  join f in ListFriend on r.idUser equals f.idSuivi
                                  select r).Average(x => x.Vote);


            return Ok(moyenneFreinds);

        }


        [HttpGet("getRankByDate/{idUsers}/{limite}")]
        public async Task<IActionResult> GetRankByDate([FromRoute]string idUsers, [FromRoute]int limite)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rankUser =  (from rank in _context.rankModel
                                  where rank.idUser == idUsers
                                  select rank);
           
            if (rankUser.Count() == 0)
            {
                return Ok(rankUser);
            }

            rankUser = rankUser.OrderByDescending(r => r.dateCréation);
            List<RankModel> ranks = new List<RankModel>();
            foreach (var item in rankUser)
            {
                var film = _context.filmModel.Where(f => f.idFilm == item.idFilm).FirstOrDefault();
                item.moyenneByAllUser = film.moyenne;
                item.poster = film.poster;
                item.title = film.title;
                item.moyenneByFriend = 0;
                ranks.Add(item);
            }

            return Ok(ranks.Take(limite));
        }


        [HttpGet("getRankByTitle/{idUsers}/{title}")]
        public async Task<IActionResult> GetRankByTitle([FromRoute]string idUsers, [FromRoute]string title)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var films = _context.filmModel.Where(f => f.title.StartsWith(title));

            var rankUser = (from f in films
                            join r in _context.rankModel
                            on f.idFilm equals r.idFilm
                            select r);

            if(rankUser.Count() == 0)
            {
                return Ok(rankUser);
            }
            List<RankModel> ranks = new List<RankModel>();
            foreach (var item in rankUser)
            {
                var film = _context.filmModel.Where(f => f.idFilm == item.idFilm).FirstOrDefault();
                item.moyenneByAllUser = film.moyenne;
                item.poster = film.poster;
                item.title = film.title;
                item.moyenneByFriend = 0;
                ranks.Add(item);
            }
            return Ok(ranks);
        }

        
        [HttpGet("GetRankModelByUserAndFilms/{idUser}/{idFilms}")]
        public async Task<IActionResult> GetRankModelByUserAndFilms(string idUser, string idFilms)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rankModel = _context.rankModel.Where(r => r.idUser == idUser && r.idFilm == idFilms).FirstOrDefault();

           if (rankModel == null)
            {
                var rankModelVide = new RankModel { moyenneByFriend = 0, moyenneByAllUser = 0 };
                return Ok(rankModelVide);
            } 
            var film = _context.filmModel.Where(f => f.idFilm == rankModel.idFilm).FirstOrDefault();

            rankModel.moyenneByAllUser = film.moyenne;
            rankModel.poster = film.poster;
            rankModel.title = film.title;

            var ListFriend = from friend in _context.friendsModel
                             where friend.idSuiveur == idUser
                             select friend;

            var notesAllUsers = _context.rankModel.Where(x => x.idFilm == idFilms);

            var moyenneFriends = (from r in notesAllUsers
                                  join f in ListFriend on r.idUser equals f.idSuivi
                                  select r).Average(x => x.Vote);

            rankModel.moyenneByFriend = moyenneFriends;
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

        
        private bool rankModelExists(Guid id)
        {
            return _context.rankModel.Any(e => e.ID == id);
        }
    }
}