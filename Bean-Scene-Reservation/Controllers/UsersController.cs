using Bean_Scene_Reservation.Data;
using Bean_Scene_Reservation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security;

namespace Bean_Scene_Reservation.Controllers
{
    [Authorize(Roles = "Manager")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
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
            if (string.IsNullOrEmpty(id)) {
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
        public async Task<IActionResult> Create()
        {
            var userData = new CreateUserDto();

            // Get all available roles
            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            userData.AvailableRoles = roles;

            return View(userData);
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserDto userData)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = userData.Email,
                    Email = userData.Email,
                    PhoneNumber = userData.Phone,
                    FirstName = userData.FirstName,
                    LastName = userData.LastName,

                    // Normalised fields
                    NormalizedUserName = userData.Email.ToUpperInvariant(),
                    NormalizedEmail = userData.Email.ToUpperInvariant(),

                    // Randomly-generated stamp values
                    ConcurrencyStamp = Guid.NewGuid().ToString(), // Generate concurrency stamp (a random value that should change whenever a user is persisted to the store)
                    SecurityStamp = Guid.NewGuid().ToString(), // Generate security stamp (a random value that should change whenever a user's information is updated)

                    // Other auth data
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = false,
                    AccessFailedCount = 0,
                    LockoutEnabled = false,
                    LockoutEnd = null,
                    TwoFactorEnabled = false
                };

                var result = await _userManager.CreateAsync(user, userData.Password);

                if (result.Succeeded)
                {
                    // Add user to selected roles
                    if (userData.SelectedRoles != null && userData.SelectedRoles.Any())
                    {
                        await _userManager.AddToRolesAsync(user, userData.SelectedRoles);
                    }

                    //TempData["Success"] = "User created successfully!";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            // Reload available roles
            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            userData.AvailableRoles = roles;

            return View(userData);
        }

        // GET: Users/Edit/1ac0095d-c987-4dfc-a387-0ad330077fe1
        public async Task<IActionResult> Edit(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

            var userWithRoles = new EditUserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                SelectedRoles = roles.ToList(),
                AvailableRoles = allRoles,
                NewPassword = null
            };

            return View(userWithRoles);
        }

        // POST: Users/Edit/1ac0095d-c987-4dfc-a387-0ad330077fe1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditUserDto updatedUser)
        {
            if (id != updatedUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid) {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                // Update user properties
                user.UserName = updatedUser.Email;
                user.Email = updatedUser.Email;
                user.PhoneNumber = updatedUser.Phone;
                user.FirstName = updatedUser.FirstName;
                user.LastName = updatedUser.LastName;

                // Normalised fields
                user.NormalizedUserName = updatedUser.Email.ToUpperInvariant();
                user.NormalizedEmail = updatedUser.Email.ToUpperInvariant();

                user.SecurityStamp = Guid.NewGuid().ToString(); // Generate security stamp (a random value that should change whenever a user's information is updated)

                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    foreach (var error in updateResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    // Reload roles for redisplay
                    updatedUser.AvailableRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
                    return View(updatedUser);
                }

                // Update password if provided
                if (!string.IsNullOrEmpty(updatedUser.NewPassword))
                {
                    var removePasswordResult = await _userManager.RemovePasswordAsync(user);
                    if (removePasswordResult.Succeeded)
                    {
                        var addPasswordResult = await _userManager.AddPasswordAsync(user, updatedUser.NewPassword);
                        if (!addPasswordResult.Succeeded)
                        {
                            foreach (var error in addPasswordResult.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }

                            updatedUser.AvailableRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
                            return View(updatedUser);
                        }
                    }
                }

                // Update roles
                var currentRoles = await _userManager.GetRolesAsync(user);
                var rolesToRemove = currentRoles.Except(updatedUser.SelectedRoles ?? new List<string>()).ToList();
                var rolesToAdd = (updatedUser.SelectedRoles ?? new List<string>()).Except(currentRoles).ToList();

                if (rolesToRemove.Any())
                {
                    await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                }

                if (rolesToAdd.Any())
                {
                    await _userManager.AddToRolesAsync(user, rolesToAdd);
                }

                //TempData["Success"] = "User updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            // If we got this far, something failed, redisplay form
            updatedUser.AvailableRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return View(updatedUser);
        }

        // GET: Users/Delete/1ac0095d-c987-4dfc-a387-0ad330077fe1
        public async Task<IActionResult> Delete(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            var userWithRoles = new UserWithRolesDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Roles = roles.ToList()
            };

            return View(userWithRoles);
        }

        // POST: Users/Delete/1ac0095d-c987-4dfc-a387-0ad330077fe1
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

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

        public class CreateUserDto
        {
            [Required]
            [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 and at max 100 characters long.")]
            [DataType(DataType.Password)]
            [DisplayName("Password")]
            public string Password { get; set; }

            [DisplayName("First Name")]
            public string FirstName { get; set; }

            [DisplayName("Last Name")]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            [DisplayName("Email")]
            public string Email { get; set; }

            [Phone]
            public string? Phone { get; set; }

            [DisplayName("Roles")]
            public List<string> SelectedRoles { get; set; }

            public List<string> AvailableRoles { get; set; } = new List<string>();
        }
        public class EditUserDto
        {
            [Required]
            public string Id { get; set; }

            [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 and at max 100 characters long.")]
            [DataType(DataType.Password)]
            [DisplayName("Password (Leave blank to keep current)")]
            public string? NewPassword { get; set; } = null;

            [DisplayName("First Name")]
            public string FirstName { get; set; }

            [DisplayName("Last Name")]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            [DisplayName("Email")]
            public string Email { get; set; }

            [Phone]
            public string? Phone { get; set; }

            [DisplayName("Roles")]
            public List<string> SelectedRoles { get; set; }

            public List<string> AvailableRoles { get; set; } = new List<string>();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor.");
            }
        }
    }
}
