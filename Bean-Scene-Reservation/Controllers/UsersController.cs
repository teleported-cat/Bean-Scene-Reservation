using Bean_Scene_Reservation.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bean_Scene_Reservation.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Users;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Users/Details/1ac0095d-c987-4dfc-a387-0ad330077fe1


        // GET: Users/Create

        // POST: Users/Create

        // GET: Users/Edit/1ac0095d-c987-4dfc-a387-0ad330077fe1

        // POST: Users/Edit/1ac0095d-c987-4dfc-a387-0ad330077fe1

        // GET: Users/Delete/1ac0095d-c987-4dfc-a387-0ad330077fe1

        // POST: Users/Delete/1ac0095d-c987-4dfc-a387-0ad330077fe1


    }
}
