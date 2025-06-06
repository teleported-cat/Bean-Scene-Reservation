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
            var applicationDbContext = _context.Reservations.Include(r => r.Area).Include(r => r.EndTime).Include(r => r.Sitting).Include(r => r.StartTime);
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

        // GET: Reservations/Create
        public IActionResult Create()
        {
            ViewData["AreaId"] = new SelectList(_context.Areas, "Id", "Name");
            ViewData["EndTimeId"] = new SelectList(_context.Timeslots, "Time", "Time");
            ViewData["Date"] = new SelectList(_context.Sittings, "Date", "Date");
            ViewData["StartTimeId"] = new SelectList(_context.Timeslots, "Time", "Time");
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,SittingTypeId,StartTimeId,EndTimeId,AreaId,NumberOfGuests,FirstName,LastName,Email,Phone,Note,Status")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AreaId"] = new SelectList(_context.Areas, "Id", "Name", reservation.AreaId);
            ViewData["EndTimeId"] = new SelectList(_context.Timeslots, "Time", "Time", reservation.EndTimeId);
            ViewData["Date"] = new SelectList(_context.Sittings, "Date", "Date", reservation.Date);
            ViewData["StartTimeId"] = new SelectList(_context.Timeslots, "Time", "Time", reservation.StartTimeId);
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
            ViewData["Date"] = new SelectList(_context.Sittings, "Date", "Date", reservation.Date);
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
            ViewData["Date"] = new SelectList(_context.Sittings, "Date", "Date", reservation.Date);
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
    }
}
