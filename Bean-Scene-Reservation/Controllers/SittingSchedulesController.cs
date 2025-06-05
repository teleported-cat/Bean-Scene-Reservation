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
            // Perform pre-check to see if there are any sittings of said type already within the date & weekday range
            foreach (var day in EachDay(sittingSchedule.StartDate, sittingSchedule.EndDate)) {
                // Check if the weekday is a part of the schedule
                string weekdayCurrent = day.DayOfWeek.ToString();
                if (weekdayCurrent == "Monday" && !sittingSchedule.ForMonday) { continue; }
                if (weekdayCurrent == "Tuesday" && !sittingSchedule.ForTuesday) { continue; }
                if (weekdayCurrent == "Wednesday" && !sittingSchedule.ForWednesday) { continue; }
                if (weekdayCurrent == "Thursday" && !sittingSchedule.ForThursday) { continue; }
                if (weekdayCurrent == "Friday" && !sittingSchedule.ForFriday) { continue; }
                if (weekdayCurrent == "Saturday" && !sittingSchedule.ForSaturday) { continue; }
                if (weekdayCurrent == "Sunday" && !sittingSchedule.ForSunday) { continue; }


            }

            // If there are sittings, display and warn them

            // If user continues, generate sittings (skip pre-existing sittings)


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

        //// GET: SittingSchedules/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var sittingSchedule = await _context.SittingSchedules.FindAsync(id);
        //    if (sittingSchedule == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["EndTimeId"] = new SelectList(_context.Timeslots, "Time", "Time", sittingSchedule.EndTimeId);
        //    ViewData["SittingTypeId"] = new SelectList(_context.SittingTypes, "Id", "Name", sittingSchedule.SittingTypeId);
        //    ViewData["StartTimeId"] = new SelectList(_context.Timeslots, "Time", "Time", sittingSchedule.StartTimeId);
        //    return View(sittingSchedule);
        //}

        //// POST: SittingSchedules/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Capacity,StartDate,EndDate,StartTimeId,EndTimeId,SittingTypeId,ForMonday,ForTuesday,ForWednesday,ForThursday,ForFriday,ForSaturday,ForSunday")] SittingSchedule sittingSchedule)
        //{
        //    if (id != sittingSchedule.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(sittingSchedule);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!SittingScheduleExists(sittingSchedule.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["EndTimeId"] = new SelectList(_context.Timeslots, "Time", "Time", sittingSchedule.EndTimeId);
        //    ViewData["SittingTypeId"] = new SelectList(_context.SittingTypes, "Id", "Name", sittingSchedule.SittingTypeId);
        //    ViewData["StartTimeId"] = new SelectList(_context.Timeslots, "Time", "Time", sittingSchedule.StartTimeId);
        //    return View(sittingSchedule);
        //}

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
