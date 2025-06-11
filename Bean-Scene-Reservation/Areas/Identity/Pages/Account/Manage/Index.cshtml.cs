// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Bean_Scene_Reservation.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bean_Scene_Reservation.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            // Adds support for first and last name
            /// <summary>
            ///     A custom input model for given/first names.
            /// </summary>
            [Required]
            [StringLength(50, MinimumLength = 2, ErrorMessage = "Must be between 2-50 characters!")]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }
            /// <summary>
            ///     A custom input model for family/last names.
            /// </summary>
            [Required]
            [StringLength(50, MinimumLength = 2, ErrorMessage = "Must be between 2-50 characters!")]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            string firstName = user.FirstName;
            string lastName = user.LastName;

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FirstName = firstName,
                LastName = lastName
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            // Get first name from database
            string firstName = user.FirstName;
            // If what the user typed is the same as the database, don't update it
            if (Input.FirstName != firstName)
            {
                // Update the value and get the status of the action
                user.FirstName = Input.FirstName;
                var setFirstNameResult = await _userManager.UpdateAsync(user);
                // If it failed, display an error and refresh
                if (!setFirstNameResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set first name.";
                    return RedirectToPage();
                }
            }
            // Get last name from database
            string lastName = user.LastName;
            // If what the user typed is the same as the database, don't update it
            if (Input.LastName != lastName)
            {
                // Update the value and get the status of the action
                user.LastName = Input.LastName;
                var setLastNameResult = await _userManager.UpdateAsync(user);
                // If it failed, display an error and refresh
                if (!setLastNameResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set last name.";
                    return RedirectToPage();
                }
            }
            // Get phone number from database
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            // If what the user typed is the same as the database, don't update it
            if (Input.PhoneNumber != phoneNumber)
            {
                // Update the value and get the status of the action
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                // If it failed, display an error and refresh
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
