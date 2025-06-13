using Bean_Scene_Reservation.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bean_Scene_Reservation.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult ReservationAnalytics()
        {
            return View();
        }
        public async Task<IActionResult> TableUtilisation()
        {
            var applicationDbContext = _context.Tables
                .Include(t => t.Area)
                .Include(t => t.Reservations)
                .ThenInclude(r => r.Sitting)
                .ThenInclude(s => s.SittingType);

            ViewBag.SittingTypes = _context.SittingTypes.OrderBy(st => st.Id).ToList();
            ViewBag.Areas = _context.Areas.OrderBy(a => a.Id).ToList();

            return View(await applicationDbContext.ToListAsync());
        }
    }
}
