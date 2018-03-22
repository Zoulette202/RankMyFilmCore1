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

namespace RankMyFilmCore.WebApiRank
{
    [Produces("application/json")]
    [Route("api/Rank")]
    [EnableCors("CorsPolicy")]
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
        public async Task<IActionResult> GetRankModel([FromRoute] Guid id)
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
        public async Task<IActionResult> PostRankModelByIdUserIdFilm(Guid idUser, string idFilms, int vote)
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




        [HttpGet("GetRankModelByUserAndFilms/{idUser}/{idFilms}")]
        public async Task<IActionResult> GetRankModelByUserAndFilms(Guid idUser, string idFilms)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var rankModel = from rankModels in _context.rankModel
                            where rankModels.idUser == idUser && rankModels.idFilm == idFilms
                            select rankModels;
            if (rankModel == null)
            {
                return NotFound();
            }
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