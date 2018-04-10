using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RankMyFilmCore;
using RankMyFilmCore.Data;

namespace RankMyFilmCore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RankModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RankModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RankModels
        public async Task<IActionResult> Index()
        {
            var listeRank = await _context.rankModel.ToListAsync();
            return View(listeRank);
        }

        // GET: RankModels/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rankModel = await _context.rankModel
                .SingleOrDefaultAsync(m => m.ID == id);
            if (rankModel == null)
            {
                return NotFound();
            }

            return View(rankModel);
        }

        // GET: RankModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RankModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("idUser,idFilm,Vote,vu,Commentaire,ID,UpdatedAt,DeletedAt,Deleted")] RankModel rankModel)
        {
            if (ModelState.IsValid)
            {
                rankModel.ID = Guid.NewGuid();
                rankModel.dateCréation = DateTime.Now;
                _context.Add(rankModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rankModel);
        }

        // GET: RankModels/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rankModel = await _context.rankModel.SingleOrDefaultAsync(m => m.ID == id);
            if (rankModel == null)
            {
                return NotFound();
            }
            return View(rankModel);
        }

        // POST: RankModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("idUser,idFilm,Vote,vu,Commentaire,ID,UpdatedAt,DeletedAt,Deleted")] RankModel rankModel)
        {
            if (id != rankModel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rankModel);
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
                return RedirectToAction(nameof(Index));
            }
            return View(rankModel);
        }

        // GET: RankModels/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rankModel = await _context.rankModel
                .SingleOrDefaultAsync(m => m.ID == id);
            if (rankModel == null)
            {
                return NotFound();
            }

            return View(rankModel);
        }

        // POST: RankModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var rankModel = await _context.rankModel.SingleOrDefaultAsync(m => m.ID == id);
            _context.rankModel.Remove(rankModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RankModelExists(Guid id)
        {
            return _context.rankModel.Any(e => e.ID == id);
        }
    }
}
