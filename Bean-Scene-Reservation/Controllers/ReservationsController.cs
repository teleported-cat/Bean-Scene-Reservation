using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bean_Scene_Reservation.Data;
using Bean_Scene_Reservation.Models;
using System.Runtime.ExceptionServices;

namespace Bean_Scene_Reservation.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Reservations
                .Include(r => r.Area)
                .Include(r => r.StartTime)
                .Include(r => r.EndTime)
                .Include(r => r.Sitting)
                .ThenInclude(s => s.SittingType);
                //.Include(r => r.User)
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Area)
                .Include(r => r.StartTime)
                .Include(r => r.EndTime)
                .Include(r => r.Sitting)
                .ThenInclude(s => s.SittingType)
                //.Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            //ViewData["AreaId"] = new SelectList(_context.Areas, "Id", "Name");
            //ViewData["StartTimeId"] = new SelectList(_context.Timeslots, "Time", "Time");
            //ViewData["EndTimeId"] = new SelectList(_context.Timeslots, "Time", "Time");
            //ViewData["SittingId"] = new SelectList(_context.SittingTypes, "Id", "Name");
            //ViewData["Date"] = new SelectList(_context.Sittings, "Date", "Date");
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            PopulateViewData();
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,SittingTypeId,StartTimeId,EndTimeId,AreaId,NumberOfGuests,FirstName,LastName,Email,Phone,Note,Status")] Reservation reservation)
        {
            // Check if the sitting actually exists (remember, a sitting is a date PLUS a type)
            // Without this check, we can select a non-existant type on an existing date, causing a SQL error
            bool sittingExists = _context.Sittings
                .Any(s => s.Date == reservation.Date && s.SittingTypeId == reservation.SittingTypeId);
            if (!sittingExists)
            {
                ModelState.AddModelError("SittingTypeId", "Sorry, this sitting doesn't exist for this date.");
            }

            // Obviously check if the start time is after the end time
            // Comparing using the timeslot's id is hacky... but smart
            if (reservation.StartTimeId >= reservation.EndTimeId)
            {
                ModelState.AddModelError("StartTimeId", "The start time cannot be the same or after the end time.");
            }

            // Check if the start and end time are within the sitting's time frame


            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["AreaId"] = new SelectList(_context.Areas, "Id", "Name", reservation.AreaId);
            //ViewData["EndTimeId"] = new SelectList(_context.Timeslots, "Time", "Time", reservation.EndTimeId);
            //ViewData["SittingTypeId"] = new SelectList(_context.SittingTypes, "Id", "Name", reservation.SittingTypeId);
            //ViewData["StartTimeId"] = new SelectList(_context.Timeslots, "Time", "Time", reservation.StartTimeId);
            PopulateViewData(reservation);
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["AreaId"] = new SelectList(_context.Areas, "Id", "Name", reservation.AreaId);
            ViewData["EndTimeId"] = new SelectList(_context.Timeslots, "Time", "Time", reservation.EndTimeId);
            ViewData["SittingTypeId"] = new SelectList(_context.SittingTypes, "Id", "Name", reservation.SittingTypeId);
            ViewData["StartTimeId"] = new SelectList(_context.Timeslots, "Time", "Time", reservation.StartTimeId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,SittingTypeId,StartTimeId,EndTimeId,AreaId,NumberOfGuests,FirstName,LastName,Email,Phone,Note,Status")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
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
            ViewData["AreaId"] = new SelectList(_context.Areas, "Id", "Name", reservation.AreaId);
            ViewData["EndTimeId"] = new SelectList(_context.Timeslots, "Time", "Time", reservation.EndTimeId);
            ViewData["SittingTypeId"] = new SelectList(_context.SittingTypes, "Id", "Name", reservation.SittingTypeId);
            ViewData["StartTimeId"] = new SelectList(_context.Timeslots, "Time", "Time", reservation.StartTimeId);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Area)
                .Include(r => r.EndTime)
                .Include(r => r.Sitting)
                .Include(r => r.StartTime)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }

        #region PopulateViewDataOverloads
        private void PopulateViewData()
        {
            PopulateViewData(null, null, null, null, null); // Add one more null once users are added
        }
        private void PopulateViewData(Reservation reservation)
        {
            PopulateViewData(
                reservation.AreaId, 
                reservation.StartTimeId, 
                reservation.EndTimeId, 
                reservation.SittingTypeId, 
                reservation.Date
                //reservation.UserId
                );
        }
        private void PopulateViewData(
            int? areaId = null, 
            TimeOnly? startTimeId = null, 
            TimeOnly? endTimeId = null,
            int? sittingTypeId = null,
            DateOnly? date = null
            //int? userId = null
            )
        {
            // Only select dates in the future, open, and prevent duplicate values
            var dateGrouping = _context.Sittings
                .GroupBy(s => s.Date).Select(g => g.First()).AsEnumerable();
            var dateContext = dateGrouping
                .Where(s => s.Date >= DateOnly.FromDateTime(DateTime.Today) && s.Status == Enum.Parse<Sitting.SittingStatus>("Open"));
            ViewData["Date"] = new SelectList(dateContext, "Date", "Date", date);
            
            // Sorts Types
            var typeContext = _context.SittingTypes.OrderBy(st => st.Id);
            ViewData["SittingTypeId"] = new SelectList(typeContext, "Id", "Name", sittingTypeId);

            // We can't restrict the times to the sitting's timeframe here, we'll have to do it in the view
            ViewData["StartTimeId"] = new SelectList(_context.Timeslots, "Time", "Time", startTimeId);
            ViewData["EndTimeId"] = new SelectList(_context.Timeslots, "Time", "Time", endTimeId);

            // Sorts Areas
            var areaContext = _context.Areas.OrderBy(a => a.Id);
            ViewData["AreaId"] = new SelectList(areaContext, "Id", "Name", areaId);

            // Users
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
        }

        #endregion
    }
}
