using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RankMyFilmCore;
using RankMyFilmCore.Data;

namespace RankMyFilmCore.WebApiRank
{
    [Produces("application/json")]
    [Route("api/RankModels")]
    public class RankModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RankModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/RankModels
        [HttpGet]
        public IEnumerable<RankModel> GetrankModel()
        {
            return _context.rankModel;
        }

        // GET: api/RankModels/5
        [HttpGet("{id}")]
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

        // PUT: api/RankModels/5
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

        // POST: api/RankModels
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

        // DELETE: api/RankModels/5
        [HttpDelete("{id}")]
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