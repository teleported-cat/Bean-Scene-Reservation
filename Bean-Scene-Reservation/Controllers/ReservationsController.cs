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
using Microsoft.IdentityModel.Tokens;

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
                .Include(s => s.Table)
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
            PopulateViewData();
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,SittingTypeId,StartTimeId,EndTimeId,AreaId,NumberOfGuests,FirstName,LastName,Email,Phone,Note,Status,Source")] Reservation reservation)
        {
            CheckReservationErrors(reservation);

            if (ModelState.IsValid) {
                var openTables = TablesLeftInSitting(reservation);
                AssignTables(reservation, openTables);
            }

            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

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

            var reservation = await _context.Reservations
                .FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            PopulateViewData(reservation);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,SittingTypeId,StartTimeId,EndTimeId,AreaId,NumberOfGuests,FirstName,LastName,Email,Phone,Note,Status,Source")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            CheckReservationErrors(reservation);

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
            PopulateViewData(reservation);
            return View(reservation);
        }

        // GET: Reservations/Edit/5/Confirmed
        [HttpGet("Reservations/Edit/{id}/{status}")]
        public async Task<IActionResult> Edit(int id, string status)
        {
            // Find a reservation
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
                return NotFound("Reservation not found.");

            // Validate status
            if (!Enum.TryParse(status, out Reservation.ReservationStatus statusEnum))
                return BadRequest("Invalid status.");

            // Change the status
            reservation.Status = statusEnum;

            // Update the database
            _context.Update(reservation);
            await _context.SaveChangesAsync();

            // Redirect to the Details view (pass through reservation ID)
            return RedirectToAction(nameof(UpdateStatus), new { id = id });
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

        // GET: Reservations/UpdateStatus/5
        public async Task<IActionResult> UpdateStatus(int? id)
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

        // GET: Reservations/NewReservation
        [HttpGet("Reservations/NewReservation")]
        public IActionResult CustomerCreate()
        {
            PopulateViewData();
            return View();
        }

        // POST: Reservations/NewReservation
        [HttpPost("Reservations/NewReservation")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CustomerCreate([Bind("Id,Date,SittingTypeId,StartTimeId,EndTimeId,AreaId,NumberOfGuests,FirstName,LastName,Email,Phone,Note")] Reservation reservation)
        {
            CheckReservationErrors(reservation);

            if (ModelState.IsValid)
            {
                var openTables = TablesLeftInSitting(reservation);
                AssignTables(reservation, openTables);
            }

            if (ModelState.IsValid)
            {
                // Add status and source automatically
                reservation.Status = Enum.Parse<Reservation.ReservationStatus>("Pending");
                reservation.Source = Enum.Parse<Reservation.ReservationSource>("Online");

                _context.Add(reservation);
                await _context.SaveChangesAsync();

                // For Success Message
                var sittingType = await _context.SittingTypes
                    .Where(s => s.Id == reservation.SittingTypeId).FirstAsync();
                string sittingName = sittingType.Name;
                
                var area = await _context.Areas.Where(a => a.Id == reservation.AreaId).FirstAsync();
                string areaName = area.Name;

                TempData["SuccessMessage"] = $"Your {sittingName} booking on {reservation.Date} has been placed.";
                TempData["SuccessDetails"] = $"Your reservation will be from {reservation.StartTimeId} to {reservation.EndTimeId}, in the {areaName} area.";

                return RedirectToAction(nameof(Index), "Home");
            }

            PopulateViewData(reservation);
            return View(nameof(CustomerCreate), reservation);
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

        #region ReservationChecking
        private void CheckReservationErrors(Reservation reservation)
        {
            var sittings = _context.Sittings;
            bool sittingExists = sittings
                .Any(s => s.Date == reservation.Date && s.SittingTypeId == reservation.SittingTypeId);
            var sitting = sittings
                .Where(s => s.Date == reservation.Date && s.SittingTypeId == reservation.SittingTypeId).First();

            // Check if the sitting actually exists (remember, a sitting is a date PLUS a type)
            // Without this check, we can select a non-existant type on an existing date, causing a SQL error
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
            if (reservation.StartTimeId < sitting.StartTimeId)
            {
                ModelState.AddModelError("StartTimeId", "Sorry, this start time is before the sitting begins.");
            }
            if (reservation.EndTimeId > sitting.EndTimeId)
            {
                ModelState.AddModelError("EndTimeId", "Sorry, this end time is after the sitting ends.");
            }

            // Check if the reservation exceeds sitting max capacity
            // (it should also count the number of reservations already in this sitting to prevent too many sittings)
            if (reservation.NumberOfGuests > sitting.Capacity)
            {
                ModelState.AddModelError("NumberOfGuests", $"The number of guests exceed the sitting capacity of {sitting.Capacity}.");
            }

            // Give an error if both email and phone is empty
            if (reservation.Email.IsNullOrEmpty() && reservation.Phone.IsNullOrEmpty())
            {
                ModelState.AddModelError("Email", "Both email & phone cannot be blank!");
            }

        }
        private List<Table> TablesLeftInSitting(Reservation reservation)
        {
            // Get a list of all reservations that:
                // Are in the same sitting as this one
                // Is in the same area as this one
                // Isn't cancelled
            var reservationsInSitting = _context.Reservations
                .Where(r => r.Date == reservation.Date && r.SittingTypeId == reservation.SittingTypeId)
                .Where(r => r.AreaId == reservation.AreaId)
                .Where(r => r.Status != Enum.Parse<Reservation.ReservationStatus>("Cancelled"));

            // Get a list of tables in the area chosen
            var tablesInArea = _context.Tables.Where(t => t.AreaId == reservation.AreaId).ToList();

            // Get a list of all tables from the reservation list above
            var tablesForReservations = reservationsInSitting
                .SelectMany(r => r.Table)
                .Distinct()
                .ToList();

            // There shouldn't be any duplicates as the table assigning process prevents it

            // Return a list of all tables NOT in use in this sitting + area
            var tablesWithoutReservations = tablesInArea.Except(tablesForReservations).ToList();

            return tablesWithoutReservations;
        }
        private void AssignTables(Reservation reservation, List<Table> openTables)
        {
            // First calculate the number of tables required
            // Operation: tr = ceil(numOfGuests / 4)
            int tablesRequired = (int)Math.Ceiling(reservation.NumberOfGuests / 4m);

            // Check if there are enough tables left
            bool isSpaceForReservation = tablesRequired <= openTables.Count;
            if (!isSpaceForReservation) {
                // ERROR! Not enough tables left!
                ModelState.AddModelError("NumberOfGuests", "Sorry, there are not enough tables to sit these guests (" + openTables.Count + " tables left in this sitting).");
                return;
            }

            // Assign some tables
            for (int i = 0; i < tablesRequired; i++)
            {
                reservation.Table.Add(openTables[i]);
            }
        }
        #endregion
    }
}
