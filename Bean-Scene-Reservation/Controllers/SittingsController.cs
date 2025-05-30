using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bean_Scene_Reservation.Data;
using Bean_Scene_Reservation.Models;

namespace Bean_Scene_Reservation.Controllers
{
    public class SittingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SittingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sittings
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Sittings.Include(s => s.EndTime).Include(s => s.SittingType).Include(s => s.StartTime);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Sittings/Details/2000-01-01/1
        [HttpGet("Sittings/Details/{date}/{type}")]
        public async Task<IActionResult> Details(DateOnly? date, int type)
        {
            if (date == null)
            {
                return NotFound();
            }

            var sitting = await _context.Sittings
                .Include(s => s.EndTime)
                .Include(s => s.SittingType)
                .Include(s => s.StartTime)
                .FirstOrDefaultAsync(s => s.Date == date && s.SittingTypeId == type);
            if (sitting == null)
            {
                return NotFound();
            }

            return View(sitting);
        }

        // GET: Sittings/Create
        public IActionResult Create()
        {
            //ViewData["EndTimeId"] = new SelectList(_context.Timeslots, "Time", "Time");
            //ViewData["SittingTypeId"] = new SelectList(_context.SittingTypes, "Id", "Name");
            //ViewData["StartTimeId"] = new SelectList(_context.Timeslots, "Time", "Time");
            PopulateViewData();
            return View();
        }

        // POST: Sittings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Date,SittingTypeId,StartTimeId,EndTimeId,Status,Capacity")] Sitting sitting)
        {
            // Check for existing sitting (PK already exists: Date + Type)
            if (SittingExists(sitting.Date, sitting.SittingTypeId))
            {
                ModelState.AddModelError("Date", "Sitting with this date & type already exists!");
            }

            if (ModelState.IsValid)
            {
                _context.Add(sitting);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["EndTimeId"] = new SelectList(_context.Timeslots, "Time", "Time", sitting.EndTimeId);
            //ViewData["SittingTypeId"] = new SelectList(_context.SittingTypes, "Id", "Name", sitting.SittingTypeId);
            //ViewData["StartTimeId"] = new SelectList(_context.Timeslots, "Time", "Time", sitting.StartTimeId);
            PopulateViewData(sitting);
            return View(sitting);
        }

        // GET: Sittings/Edit/2000-01-01/1
        [HttpGet("Sittings/Edit/{date}/{type}")]
        public async Task<IActionResult> Edit(DateOnly? date, int type)
        {
            if (date == null)
            {
                return NotFound();
            }

            var sitting = await _context.Sittings.FindAsync(date, type);
            if (sitting == null)
            {
                return NotFound();
            }
            //ViewData["EndTimeId"] = new SelectList(_context.Timeslots, "Time", "Time", sitting.EndTimeId);
            //ViewData["SittingTypeId"] = new SelectList(_context.SittingTypes, "Id", "Name", sitting.SittingTypeId);
            //ViewData["StartTimeId"] = new SelectList(_context.Timeslots, "Time", "Time", sitting.StartTimeId);
            PopulateViewData(sitting);
            return View(sitting);
        }

        // POST: Sittings/Edit/2000-01-01/1
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Sittings/Edit/{date}/{type}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DateOnly? date, int type, [Bind("Date,SittingTypeId,StartTimeId,EndTimeId,Status,Capacity")] Sitting sitting)
        {
            sitting.SittingTypeId = type;

            if (date != sitting.Date)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sitting);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SittingExists(sitting.Date, sitting.SittingTypeId))
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
            //ViewData["EndTimeId"] = new SelectList(_context.Timeslots, "Time", "Time", sitting.EndTimeId);
            //ViewData["SittingTypeId"] = new SelectList(_context.SittingTypes, "Id", "Name", sitting.SittingTypeId);
            //ViewData["StartTimeId"] = new SelectList(_context.Timeslots, "Time", "Time", sitting.StartTimeId);
            PopulateViewData(sitting);
            return View(sitting);
        }

        // GET: Sittings/Delete/2000-01-01/1
        [HttpGet("Sittings/Delete/{date}/{type}")]
        public async Task<IActionResult> Delete(DateOnly? date, int type)
        {
            if (date == null)
            {
                return NotFound();
            }

            var sitting = await _context.Sittings
                .Include(s => s.EndTime)
                .Include(s => s.SittingType)
                .Include(s => s.StartTime)
                .FirstOrDefaultAsync(s => s.Date == date && s.SittingTypeId == type);
            if (sitting == null)
            {
                return NotFound();
            }

            return View(sitting);
        }

        // POST: Sittings/Delete/2000-01-01/1
        [HttpPost("Sittings/Delete/{date}/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(DateOnly date, int type)
        {
            var sitting = await _context.Sittings.FindAsync(date, type);
            if (sitting != null)
            {
                _context.Sittings.Remove(sitting);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SittingExists(DateOnly date, int type)
        {
            return _context.Sittings.Any(s => s.Date == date && s.SittingTypeId == type);
        }

        #region PopulateViewDataOverloads
        /// <inheritdoc cref="PopulateViewData(int?, TimeOnly?, TimeOnly?)"/>
        private void PopulateViewData()
        {
            PopulateViewData(null, null, null);
        }

        /// <param name="sitting"></param>
        /// <inheritdoc cref="PopulateViewData(int?, TimeOnly?, TimeOnly?)"/>
        private void PopulateViewData(Sitting sitting)
        {
            PopulateViewData(sitting.SittingTypeId, sitting.StartTimeId, sitting.EndTimeId);
        }

        /// <summary>
        /// Populates the ViewData object with data for the dropdown lists.
        /// </summary>
        /// <param name="sittingTypeId"></param>
        /// <param name="startTimeId"></param>
        /// <param name="endTimeId"></param>
        private void PopulateViewData(int? sittingTypeId = null, TimeOnly? startTimeId = null, TimeOnly? endTimeId = null)
        {
            ViewData["SittingTypeId"] = new SelectList(_context.SittingTypes.OrderBy(st => st.Id), "Id", "Name", sittingTypeId);
            ViewData["StartTimeId"] = new SelectList(_context.Timeslots, "Time", "Time", startTimeId);
            ViewData["EndTimeId"] = new SelectList(_context.Timeslots, "Time", "Time", endTimeId);
        }
        #endregion
    }
}
