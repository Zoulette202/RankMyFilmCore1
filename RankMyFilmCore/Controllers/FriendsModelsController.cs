﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RankMyFilmCore;
using RankMyFilmCore.Data;

namespace RankMyFilmCore.Controllers
{
    public class FriendsModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FriendsModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FriendsModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.friendsModel.ToListAsync());
        }

        // GET: FriendsModels/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var friendsModel = await _context.friendsModel
                .SingleOrDefaultAsync(m => m.ID == id);
            if (friendsModel == null)
            {
                return NotFound();
            }

            return View(friendsModel);
        }

        // GET: FriendsModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FriendsModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("idSuiveur,idSuivi,ID,UpdatedAt,DeletedAt,Deleted")] FriendsModel friendsModel)
        {
            if (ModelState.IsValid)
            {
                friendsModel.ID = Guid.NewGuid();
                _context.Add(friendsModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(friendsModel);
        }

        // GET: FriendsModels/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var friendsModel = await _context.friendsModel.SingleOrDefaultAsync(m => m.ID == id);
            if (friendsModel == null)
            {
                return NotFound();
            }
            return View(friendsModel);
        }

        // POST: FriendsModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("idSuiveur,idSuivi,ID,UpdatedAt,DeletedAt,Deleted")] FriendsModel friendsModel)
        {
            if (id != friendsModel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(friendsModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FriendsModelExists(friendsModel.ID))
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
            return View(friendsModel);
        }

        // GET: FriendsModels/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var friendsModel = await _context.friendsModel
                .SingleOrDefaultAsync(m => m.ID == id);
            if (friendsModel == null)
            {
                return NotFound();
            }

            return View(friendsModel);
        }

        // POST: FriendsModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var friendsModel = await _context.friendsModel.SingleOrDefaultAsync(m => m.ID == id);
            _context.friendsModel.Remove(friendsModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FriendsModelExists(Guid id)
        {
            return _context.friendsModel.Any(e => e.ID == id);
        }
    }
}
