using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RankMyFilmCore.Data;
using RankMyFilmCore.Models;

namespace RankMyFilmCore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FilmModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FilmModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FilmModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.filmModel.ToListAsync());
        }

        // GET: FilmModels/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filmModel = await _context.filmModel
                .SingleOrDefaultAsync(m => m.ID == id);
            if (filmModel == null)
            {
                return NotFound();
            }

            return View(filmModel);
        }

        // GET: FilmModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FilmModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("idFilm,title,poster,moyenne,nbRank,ID,UpdatedAt,DeletedAt,Deleted")] FilmModel filmModel)
        {
            if (ModelState.IsValid)
            {
                filmModel.ID = Guid.NewGuid();
                _context.Add(filmModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(filmModel);
        }

        // GET: FilmModels/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filmModel = await _context.filmModel.SingleOrDefaultAsync(m => m.ID == id);
            if (filmModel == null)
            {
                return NotFound();
            }
            return View(filmModel);
        }

        // POST: FilmModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("idFilm,title,poster,moyenne,nbRank,ID,UpdatedAt,DeletedAt,Deleted")] FilmModel filmModel)
        {
            if (id != filmModel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(filmModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmModelExists(filmModel.ID))
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
            return View(filmModel);
        }

        // GET: FilmModels/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filmModel = await _context.filmModel
                .SingleOrDefaultAsync(m => m.ID == id);
            if (filmModel == null)
            {
                return NotFound();
            }

            return View(filmModel);
        }

        // POST: FilmModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var filmModel = await _context.filmModel.SingleOrDefaultAsync(m => m.ID == id);
            _context.filmModel.Remove(filmModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmModelExists(Guid id)
        {
            return _context.filmModel.Any(e => e.ID == id);
        }
    }
}
