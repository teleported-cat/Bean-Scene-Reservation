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
    public class SittingSchedulesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SittingSchedulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SittingSchedules
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SittingSchedules.Include(s => s.EndTime).Include(s => s.SittingType).Include(s => s.StartTime);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SittingSchedules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sittingSchedule = await _context.SittingSchedules
                .Include(s => s.EndTime)
                .Include(s => s.SittingType)
                .Include(s => s.StartTime)
                .Include(s => s.Sittings)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sittingSchedule == null)
            {
                return NotFound();
            }

            return View(sittingSchedule);
        }

        // GET: SittingSchedules/Create
        public IActionResult Create()
        {
            //ViewData["EndTimeId"] = new SelectList(_context.Timeslots, "Time", "Time");
            //ViewData["SittingTypeId"] = new SelectList(_context.SittingTypes, "Id", "Name");
            //ViewData["StartTimeId"] = new SelectList(_context.Timeslots, "Time", "Time");
            PopulateViewData();
            return View();
        }

        // POST: SittingSchedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Capacity,StartDate,EndDate,StartTimeId,EndTimeId,SittingTypeId,ForMonday,ForTuesday,ForWednesday,ForThursday,ForFriday,ForSaturday,ForSunday")] SittingSchedule sittingSchedule)
        {
            /*
            var sittingsContext = _context.Sittings
                .Include(s => s.EndTime)
                .Include(s => s.SittingType)
                .Include(s => s.StartTime);
            var sittings = await sittingsContext.ToListAsync();

            // Perform pre-check to see if there are any sittings of said type already within the date & weekday range
            foreach (var day in EachDay(sittingSchedule.StartDate, sittingSchedule.EndDate)) {
                // Check if the weekday is a part of the schedule, else skip this day
                string weekdayCurrent = day.DayOfWeek.ToString();
                if (weekdayCurrent == "Monday" && !sittingSchedule.ForMonday) { continue; }
                if (weekdayCurrent == "Tuesday" && !sittingSchedule.ForTuesday) { continue; }
                if (weekdayCurrent == "Wednesday" && !sittingSchedule.ForWednesday) { continue; }
                if (weekdayCurrent == "Thursday" && !sittingSchedule.ForThursday) { continue; }
                if (weekdayCurrent == "Friday" && !sittingSchedule.ForFriday) { continue; }
                if (weekdayCurrent == "Saturday" && !sittingSchedule.ForSaturday) { continue; }
                if (weekdayCurrent == "Sunday" && !sittingSchedule.ForSunday) { continue; }

                // Check if a sitting already exists for the date + type
                if (sittings.Any(s => s.Date == day && s.SittingTypeId == sittingSchedule.SittingTypeId))
                {
                    // Add to a list of dates to display to the user when 
                }


            }

            // If there are sittings, display and warn them

            // If user continues, generate sittings (skip pre-existing sittings)
            */

            if (ModelState.IsValid)
            {
                _context.Add(sittingSchedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["EndTimeId"] = new SelectList(_context.Timeslots, "Time", "Time", sittingSchedule.EndTimeId);
            //ViewData["SittingTypeId"] = new SelectList(_context.SittingTypes, "Id", "Name", sittingSchedule.SittingTypeId);
            //ViewData["StartTimeId"] = new SelectList(_context.Timeslots, "Time", "Time", sittingSchedule.StartTimeId);
            PopulateViewData(sittingSchedule);
            return View(sittingSchedule);
        }

        // GET: SittingSchedules/GenerateSittings/5
        [HttpGet]
        public async Task<IActionResult> GenerateSittings(int? id)
        {
            // After creating the schedule,
            // the user is redirected to this view to choose to generate the sittings,
            // delete the schedule, or do neither.

            if (id == null)
            {
                return NotFound();
            }

            var sittingSchedule = await _context.SittingSchedules
                .Include(s => s.EndTime)
                .Include(s => s.SittingType)
                .Include(s => s.StartTime)
                .Include(s => s.Sittings)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sittingSchedule == null)
            {
                return NotFound();
            }

            var sittings = _context.Sittings
                .Include(s => s.SittingType)
                .Where(s =>
                s.SittingTypeId == sittingSchedule.SittingTypeId 
                && s.Date >= sittingSchedule.StartDate 
                && s.Date <= sittingSchedule.EndDate
                );

            ViewData["Sittings"] = sittings.ToListAsync();

            return View(sittingSchedule);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateSittings(int id)
        {
            var sittingSchedule = await _context.SittingSchedules.FindAsync(id);
            if (sittingSchedule == null)
            {
                return NotFound();
            }

            var sittings = _context.Sittings
                .Include(s => s.SittingType)
                .Where(s =>
                s.SittingTypeId == sittingSchedule.SittingTypeId
                && s.Date >= sittingSchedule.StartDate
                && s.Date <= sittingSchedule.EndDate
                );

            // Loop every day within the start & end date
            foreach (var day in EachDay(sittingSchedule.StartDate, sittingSchedule.EndDate))
            {
                string weekdayCurrent = day.DayOfWeek.ToString();

                // Skip if it is a false weekday
                if (weekdayCurrent == "Monday" && !sittingSchedule.ForMonday) { continue; }
                if (weekdayCurrent == "Tuesday" && !sittingSchedule.ForTuesday) { continue; }
                if (weekdayCurrent == "Wednesday" && !sittingSchedule.ForWednesday) { continue; }
                if (weekdayCurrent == "Thursday" && !sittingSchedule.ForThursday) { continue; }
                if (weekdayCurrent == "Friday" && !sittingSchedule.ForFriday) { continue; }
                if (weekdayCurrent == "Saturday" && !sittingSchedule.ForSaturday) { continue; }
                if (weekdayCurrent == "Sunday" && !sittingSchedule.ForSunday) { continue; }

                // If a sitting already exists: skip
                if (sittings.Any(s => s.Date == day && s.SittingTypeId == sittingSchedule.SittingTypeId))
                {
                    continue;
                }

                // Else: create a new sitting
                var sitting = new Sitting {
                    Date = day,
                    SittingTypeId = sittingSchedule.SittingTypeId,
                    StartTimeId = sittingSchedule.StartTimeId,
                    EndTimeId = sittingSchedule.EndTimeId,
                    Status = (Sitting.SittingStatus)1,
                    Capacity = sittingSchedule.Capacity,
                    SittingScheduleId = sittingSchedule.Id,
                };

                _context.Add(sitting);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: SittingSchedules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sittingSchedule = await _context.SittingSchedules
                .Include(s => s.EndTime)
                .Include(s => s.SittingType)
                .Include(s => s.StartTime)
                .Include(s => s.Sittings)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sittingSchedule == null)
            {
                return NotFound();
            }

            return View(sittingSchedule);
        }

        // POST: SittingSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sittingSchedule = await _context.SittingSchedules.FindAsync(id);
            if (sittingSchedule != null)
            {
                // First: Delete all sittings with the schedule id
                var sittings = await _context.Sittings.Where(s => s.SittingScheduleId == id).ToListAsync();

                if (sittings.Count != 0)
                {
                    foreach (var sitting in sittings)
                    {
                        _context.Sittings.Remove(sitting);
                    }
                }

                // Final: Delete the schedule itself
                _context.SittingSchedules.Remove(sittingSchedule);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SittingScheduleExists(int id)
        {
            return _context.SittingSchedules.Any(e => e.Id == id);
        }

        #region PopulateViewDataOverloads
        /// <inheritdoc cref="PopulateViewData(int?, TimeOnly?, TimeOnly?)"/>
        private void PopulateViewData() {
            PopulateViewData(null, null, null);
        }

        /// <param name="sittingSchedule"></param>
        /// <inheritdoc cref="PopulateViewData(int?, TimeOnly?, TimeOnly?)"/>
        private void PopulateViewData(SittingSchedule sittingSchedule) {
            PopulateViewData(sittingSchedule.SittingTypeId, sittingSchedule.StartTimeId, sittingSchedule.EndTimeId);
        }

        /// <summary>
        /// Populates the ViewData object with data for the dropdown lists.
        /// </summary>
        /// <param name="sittingTypeId"></param>
        /// <param name="startTimeId"></param>
        /// <param name="endTimeId"></param>
        private void PopulateViewData(int? sittingTypeId = null, TimeOnly? startTimeId = null, TimeOnly? endTimeId = null) {
            ViewData["SittingTypeId"] = new SelectList(_context.SittingTypes.OrderBy(st => st.Id), "Id", "Name", sittingTypeId);
            ViewData["StartTimeId"] = new SelectList(_context.Timeslots, "Time", "Time", startTimeId);
            ViewData["EndTimeId"] = new SelectList(_context.Timeslots, "Time", "Time", endTimeId);
        }
        #endregion

        public IEnumerable<DateOnly> EachDay(DateOnly from, DateOnly thru) {
            for (var day = from; day <= thru; day = day.AddDays(1))
                yield return day;
        }

    }
}
