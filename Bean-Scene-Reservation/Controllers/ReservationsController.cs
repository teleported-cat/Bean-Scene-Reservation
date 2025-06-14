using Bean_Scene_Reservation.Data;
using Bean_Scene_Reservation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

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
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Reservations
                .Include(r => r.Area)
                .Include(r => r.StartTime)
                .Include(r => r.EndTime)
                .Include(r => r.Sitting)
                .ThenInclude(s => s.SittingType)
                .Include(r => r.User)
                .OrderByDescending(r => r.Date);
            TempData["isStaff"] = false;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Reservations/Details/5
        [Authorize(Roles = "Manager")]
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
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            TempData["isStaff"] = false;
            return View(reservation);
        }

        // GET: Reservations/Create
        [Authorize(Roles = "Manager")]
        public IActionResult Create()
        {
            PopulateViewData();
            TempData["isCapture"] = false;
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create([Bind("Id,Date,SittingTypeId,StartTimeId,EndTimeId,AreaId,NumberOfGuests,FirstName,LastName,Email,Phone,Note,Status,Source,UserId")] Reservation reservation)
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
            TempData["isCapture"] = false;
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        [Authorize(Roles = "Manager")]
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
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,SittingTypeId,StartTimeId,EndTimeId,AreaId,NumberOfGuests,FirstName,LastName,Email,Phone,Note,Status,Source,UserId")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            CheckReservationErrors(reservation);
            
            if (ModelState.IsValid)
            {
                //var reservations = _context.Reservations;
                //var oldReservation = reservations.Where(r => r.Id == reservation.Id).First();
                //var oldAreaName = oldReservation.Area.Name;
                //oldTables.Clear();

                //var openTables = TablesLeftInSittingEdit(reservation);
                //ClearOldTables(reservation);
                //AssignTables(reservation, openTables);

                // FIXED: Load the existing reservation with its tables from the database
                var existingReservation = await _context.Reservations
                    .Include(r => r.Table)
                    .FirstOrDefaultAsync(r => r.Id == reservation.Id);

                if (existingReservation == null)
                {
                    return NotFound();
                }

                // Update the existing reservation's properties with the new values
                existingReservation.Date = reservation.Date;
                existingReservation.SittingTypeId = reservation.SittingTypeId;
                existingReservation.StartTimeId = reservation.StartTimeId;
                existingReservation.EndTimeId = reservation.EndTimeId;
                existingReservation.AreaId = reservation.AreaId;
                existingReservation.NumberOfGuests = reservation.NumberOfGuests;
                existingReservation.FirstName = reservation.FirstName;
                existingReservation.LastName = reservation.LastName;
                existingReservation.Email = reservation.Email;
                existingReservation.Phone = reservation.Phone;
                existingReservation.Note = reservation.Note;
                existingReservation.Status = reservation.Status;
                existingReservation.Source = reservation.Source;
                existingReservation.UserId = reservation.UserId;

                // Clear existing tables
                existingReservation.Table.Clear();

                // Get open tables and assign new ones
                var openTables = TablesLeftInSittingEdit(existingReservation);
                AssignTables(existingReservation, openTables);
            }
            

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(reservation);
                    //await _context.SaveChangesAsync();
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
        [Authorize(Roles = "Manager")]
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
            TempData["isStaff"] = false;
            return RedirectToAction(nameof(UpdateStatus), new { id = id });
        }

        // GET: Reservations/Delete/5
        [Authorize(Roles = "Manager")]
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
                .Include(r => r.User)
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
        [Authorize(Roles = "Manager")]
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
        [Authorize(Roles = "Manager")]
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
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reservation == null)
            {
                return NotFound();
            }

            TempData["isStaff"] = false;
            return View(reservation);
        }

        #region GuestViews
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
        public async Task<IActionResult> CustomerCreate([Bind("Id,Date,SittingTypeId,StartTimeId,EndTimeId,AreaId,NumberOfGuests,FirstName,LastName,Email,Phone,Note,UserId")] Reservation reservation)
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
        #endregion

        #region MemberViews
        // GET: Reservations/ReservationHistory
        [HttpGet("Reservations/ReservationHistory")]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> CustomerView()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return View();
            }

            var applicationDbContext = _context.Reservations
                .Include(r => r.Area)
                .Include(r => r.StartTime)
                .Include(r => r.EndTime)
                .Include(r => r.Sitting)
                .ThenInclude(s => s.SittingType)
                .Include(r => r.User)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.Date);
            return View(await applicationDbContext.ToListAsync());
        }
        #endregion

        #region StaffViews
        // GET: Reservations/CaptureReservation
        [HttpGet("Reservations/CaptureReservation")]
        [Authorize(Roles = "Staff, Manager")]
        public IActionResult CaptureReservation()
        {
            PopulateViewData();
            TempData["isCapture"] = true;
            return View(nameof(Create));
        }

        // POST: Reservations/CaptureReservation
        [HttpPost("Reservations/CaptureReservation")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Staff, Manager")]
        public async Task<IActionResult> CaptureReservation([Bind("Id,Date,SittingTypeId,StartTimeId,EndTimeId,AreaId,NumberOfGuests,FirstName,LastName,Email,Phone,Note,Status,Source,UserId")] Reservation reservation)
        {
            CheckReservationErrors(reservation);

            if (ModelState.IsValid)
            {
                var openTables = TablesLeftInSitting(reservation);
                AssignTables(reservation, openTables);
            }

            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), "Home");
            }

            PopulateViewData(reservation);
            TempData["isCapture"] = true;
            return View(nameof(Create), reservation);
        }

        // GET: Reservations/UpcomingReservations
        [HttpGet("Reservations/UpcomingReservations")]
        [Authorize(Roles = "Staff, Manager")]
        public async Task<IActionResult> UpcomingReservations()
        {
            var applicationDbContext = _context.Reservations
                .Include(r => r.Area)
                .Include(r => r.StartTime)
                .Include(r => r.EndTime)
                .Include(r => r.Sitting)
                .ThenInclude(s => s.SittingType)
                .Include(r => r.User)
                .OrderByDescending(r => r.Date);
            TempData["isStaff"] = true;
            return View(nameof(Index), await applicationDbContext.ToListAsync());
        }

        // GET: Reservations/UpcomingReservations/Details/5
        [HttpGet("Reservations/UpcomingReservations/Details/{id}")]
        [Authorize(Roles = "Staff, Manager")]
        public async Task<IActionResult> UpcomingDetails(int? id)
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
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            TempData["isStaff"] = true;
            return View(nameof(Details), reservation);
        }

        // GET: Reservations/UpcomingReservations/UpdateStatus/5
        [HttpGet("Reservations/UpcomingReservations/UpdateStatus/{id}")]
        [Authorize(Roles = "Staff, Manager")]
        public async Task<IActionResult> UpcomingUpdateStatus(int? id)
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
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reservation == null)
            {
                return NotFound();
            }

            TempData["isStaff"] = true;
            return View(nameof(UpdateStatus),reservation);
        }

        // GET: Reservations/Edit/5/Confirmed
        [HttpGet("Reservations/UpcomingReservations/Edit/{id}/{status}")]
        [Authorize(Roles = "Staff, Manager")]
        public async Task<IActionResult> EditUpcoming(int id, string status)
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
            TempData["isStaff"] = true;
            return RedirectToAction(nameof(UpcomingUpdateStatus), new { id = id });
        }
        #endregion

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
                );
        }
        private void PopulateViewData(
            int? areaId = null, 
            TimeOnly? startTimeId = null, 
            TimeOnly? endTimeId = null,
            int? sittingTypeId = null,
            DateOnly? date = null
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
        private List<Table> TablesLeftInSittingEdit(Reservation reservation)
        {
            // Get a list of all reservations that:
                // Are in the same sitting as this one
                // Is in the same area as this one
                // Isn't cancelled
                // Doesn't include unedited reservation
            var reservationsInSitting = _context.Reservations
                .Where(r => r.Date == reservation.Date && r.SittingTypeId == reservation.SittingTypeId)
                .Where(r => r.AreaId == reservation.AreaId)
                .Where(r => r.Status != Enum.Parse<Reservation.ReservationStatus>("Cancelled"))
                .Where(r => r.Id != reservation.Id);

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
