using Bean_Scene_Reservation.Data;
using Bean_Scene_Reservation.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace Bean_Scene_Reservation.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var usersWithRoles = new List<UserWithRolesDto>();

            foreach (var user in users) {
                var roles = await _userManager.GetRolesAsync(user);
                usersWithRoles.Add(new UserWithRolesDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    Roles = roles.ToList()
                });
            }

            return View(usersWithRoles);
        }

        // GET: Users/Details/1ac0095d-c987-4dfc-a387-0ad330077fe1
        [HttpGet("Users/Details/{id}")]
        public async Task<IActionResult> Details (string? id)
        {
            if (id == null) {
                return NotFound();
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            var userWithRoles = new DetailedUserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Roles = roles.ToList(),
                EmailConfirmed = user.EmailConfirmed,
                PhoneConfirmed = user.PhoneNumberConfirmed,
                TwoFactorEnabled = user.TwoFactorEnabled
            };

            return View(userWithRoles);
        }

        // GET: Users/Create

        // POST: Users/Create

        // GET: Users/Edit/1ac0095d-c987-4dfc-a387-0ad330077fe1

        // POST: Users/Edit/1ac0095d-c987-4dfc-a387-0ad330077fe1

        // GET: Users/Delete/1ac0095d-c987-4dfc-a387-0ad330077fe1

        // POST: Users/Delete/1ac0095d-c987-4dfc-a387-0ad330077fe1


        public class UserWithRolesDto
        {
            public string Id { get; set; }
            [DisplayName("Full Name")]
            public string FullName { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public List<string> Roles { get; set; }
        }

        public class DetailedUserDto
        {
            public string Id { get; set; }
            [DisplayName("Full Name")]
            public string FullName { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public List<string> Roles { get; set; }
            [DisplayName("Email Confirmed")]
            public bool EmailConfirmed { get; set; }
            [DisplayName("Phone Confirmed")]
            public bool PhoneConfirmed { get; set; }
            [DisplayName("Two Factor Enabled")]
            public bool TwoFactorEnabled { get; set; }
        }

    }
}
