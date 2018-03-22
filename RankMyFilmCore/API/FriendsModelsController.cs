using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RankMyFilmCore;
using RankMyFilmCore.Data;

namespace RankMyFilmCore.API
{
    [Produces("application/json")]
    [Route("api/Friend")]
    public class FriendsModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FriendsModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/FriendsModels
        [HttpGet]
        public IEnumerable<FriendsModel> GetfriendsModel()
        {
            return _context.friendsModel;
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