using Bean_Scene_Reservation.Data;
using Bean_Scene_Reservation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bean_Scene_Reservation.Controllers
{
    [Authorize(Roles = "Manager")]
    public class TablesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TablesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tables
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Tables.Include(t => t.Area).OrderBy(t => t.AreaId);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Tables/Details/M1
        [HttpGet("Tables/Details/{tableNumber}")]
        public async Task<IActionResult> Details(string? tableNumber)
        {
            if (tableNumber == null)
            {
                return NotFound("This table number doesn't exist.");
            }

            var table = await _context.Tables
                .Include(t => t.Area)
                .Include(t => t.Reservations)
                .ThenInclude(r => r.Sitting)
                .ThenInclude(s => s.SittingType)
                .FirstOrDefaultAsync(m => m.TableNumber == tableNumber);
            if (table == null)
            {
                return NotFound("Table doesn't exist.");
            }

            return View(table);
        }

        // GET: Tables/Create
        public IActionResult Create()
        {
            ViewData["AreaId"] = new SelectList(_context.Areas, "Id", "Name");
            return View();
        }

        // POST: Tables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TableNumber,AreaId")] Table table)
        {
            var area = await _context.Areas.FindAsync(table.AreaId);

            if (area == null) {
                return NotFound();
            }


            string pattern = @"^[A-Z]\d{1,2}$";
            var regexMatch = Regex.Match(table.TableNumber, pattern);

            if (!regexMatch.Success) {
                ModelState.AddModelError("TableNumber", "Table number does not follow the pattern.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(table);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AreaId"] = new SelectList(_context.Areas.OrderBy(a => a.Id), "Id", "Name", table.AreaId);
            return View(table);
        }

        // GET: Tables/Edit/M1
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var table = await _context.Tables.FindAsync(id);
            if (table == null)
            {
                return NotFound();
            }
            ViewData["AreaId"] = new SelectList(_context.Areas, "Id", "Name", table.AreaId);
            return View(table);
        }

        // POST: Tables/Edit/M1
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("TableNumber,AreaId")] Table table)
        {
            if (id != table.TableNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(table);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TableExists(table.TableNumber))
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
            ViewData["AreaId"] = new SelectList(_context.Areas, "Id", "Name", table.AreaId);
            return View(table);
        }

        // GET: Tables/Delete/M1
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var table = await _context.Tables
                .Include(t => t.Area)
                .FirstOrDefaultAsync(m => m.TableNumber == id);
            if (table == null)
            {
                return NotFound();
            }

            return View(table);
        }

        // POST: Tables/Delete/M1
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var table = await _context.Tables.FindAsync(id);
            if (table != null)
            {
                _context.Tables.Remove(table);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TableExists(string id)
        {
            return _context.Tables.Any(e => e.TableNumber == id);
        }
    }
}
