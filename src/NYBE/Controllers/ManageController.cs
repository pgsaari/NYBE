using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NYBE.Models;
using NYBE.Models.ManageViewModels;
using NYBE.Services;
using NYBE.Data;

namespace NYBE.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;

        public ManageController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender,
        ISmsSender smsSender,
        ApplicationDbContext ctx,
        ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _logger = loggerFactory.CreateLogger<ManageController>();
            _context = ctx;
        }

        //
        // GET: /Manage/Index
        [HttpGet]
        public async Task<IActionResult> Index(ManageMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == ManageMessageId.ChangeNameSuccess ? "Your name has been changed."
                : message == ManageMessageId.ChangeEmailSuccess ? "Your email has been changed."
                : message == ManageMessageId.ChangeSchoolSuccess ? "Your school has been changed."
                : message == ManageMessageId.ChangePhoneSuccess ? "You phone number has changed."
                : message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var model = new IndexViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                School = _context.Schools.Where(a => a.ID == user.SchoolID).FirstOrDefault(),
                PhoneNumber = user.PhoneNumber,
                Password = "•••••••••",
                PreferredContact = user.PreferredContact
            };
            return View(model);
        }

        //
        // GET: /Manage/ChangeName
        [HttpGet]
        public async Task<IActionResult> ChangeName()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var model = new ChangeNameViewModel
            {
                OldFirstName = user.FirstName,
                OldLastName = user.LastName
            };
            return View(model);
        }

        //
        // POST: /Manage/ChangeName
        [HttpPost]
        public async Task<IActionResult> ChangeName(ChangeNameViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    await _context.SaveChangesAsync();
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.ChangeNameSuccess });
                }
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        //
        // GET: /Manage/ChangeEmail
        [HttpGet]
        public IActionResult ChangeEmail()
        {
            return View();
        }

        //
        // POST: /Manage/ChangeEmail
        [HttpPost]
        public async Task<IActionResult> ChangeEmail(ChangeEmailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.SetEmailAsync(user, model.NewEmail);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.ChangeEmailSuccess });
                }
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        [HttpGet]
        public IActionResult ChangePreferredContact()
        {
            return View("ChangePreferredContact");
        }

        [HttpPost]
        public async Task<IActionResult> ChangePreferredContact(int contactPref)
        {
            var user = await GetCurrentUserAsync();
            user.PreferredContact = contactPref;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await _context.SaveChangesAsync();
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction(nameof(Index), new { Message = ManageMessageId.ChangeSchoolSuccess });
            }

            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });

        }

        //
        // GET: /Manage/ChangeSchool
        [HttpGet]
        public async Task<IActionResult> ChangeSchool()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var model = new ChangeSchoolViewModel
            {
                OldSchool = user.SchoolID,
                Schools = _context.Schools.Where(a => a.Status == 1).ToList()
            };
            return View(model);
        }

        //
        // POST: /Manage/ChangeSchool
        [HttpPost]
        public async Task<IActionResult> ChangeSchool(ChangeSchoolViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetCurrentUserAsync();

            if (user != null)
            {
                user.School = _context.Schools.Where(a => a.ID == model.School).FirstOrDefault();
                user.SchoolID = model.School;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    await _context.SaveChangesAsync();
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.ChangeSchoolSuccess });
                }
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        //
        // GET: /Manage/ChangePhoneNumber
        public async Task<IActionResult> ChangePhoneNumber()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var model = new ChangePhoneNumberViewModel
            {
                OldPhoneNumber = user.PhoneNumber
            };
            return View(model);
        }

        //
        // POST: /Manage/ChangePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePhoneNumber(ChangePhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.ChangePhoneSuccess });
                }
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        //
        // GET: /Manage/ChangePassword
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation(3, "User changed their password successfully.");
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.ChangePasswordSuccess });
                }
                AddErrors(result);
                return View(model);
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        public enum ManageMessageId
        {
            ChangeNameSuccess,
            ChangeEmailSuccess,
            ChangeSchoolSuccess,
            ChangePhoneSuccess,
            ChangePasswordSuccess,
            ChangePreferredContactSuccess,
            Error
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        #endregion
    }
}
