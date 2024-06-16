using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TSQLV6.Models;

namespace TSQLV6.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly UniversityDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public UsersController(UniversityDbContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
            _logger = logger;
        }

        // GET: Users
        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("administrator"))
            {
                return View(await _context.Users.ToListAsync());
            }
            else
            {
                return RedirectToAction("Details", new { id = User.FindFirstValue(ClaimTypes.NameIdentifier) });
            }
        }

        // GET: Users/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("administrator") && user.UserId != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return Forbid();
            }

            user.Password = null;

            ViewBag.IsAdmin = User.IsInRole("administrator");

            return View(user);
        }

        // GET: Users/Login
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(new User());
        }

        // POST: Users/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            {
                ModelState.AddModelError("", "Nieprawidłowa nazwa użytkownika lub hasło.");
                return View();
            }

            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Użytkownik o podanym adresie email nie istnieje.");
                return View();
            }

            var passwordResult = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);

            if (passwordResult == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("", "Nieprawidłowe hasło.");
                return View();
            }

            // Generowanie tokenu i ustawianie pliku cookie
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.UserType ?? "")
            };

            var claimsIdentity = new ClaimsIdentity(claims, "MyCookieAuth");

            await HttpContext.SignInAsync("MyCookieAuth", new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            });

            return RedirectToAction(nameof(Index));
        }

        // GET: Users/Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction(nameof(Login));
        }

        // GET: Users/Register
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        // POST: Users/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("UserId,Email,Password,FirstName,LastName,DateOfBirth,UserType")] User user)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                // Hashowanie hasła przed zapisaniem do bazy
                user.Password = _passwordHasher.HashPassword(user, user.Password);

                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("administrator") && user.UserId != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return Forbid();
            }

            ViewBag.IsAdmin = User.IsInRole("administrator");
            ViewBag.IsCurrentUser = user.UserId == int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var userEditViewModel = new UserEditViewModel
            {
                UserId = user.UserId,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                UserType = user.UserType
            };

            return View(userEditViewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserEditViewModel userEditViewModel)
        {
            if (id != userEditViewModel.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = await _context.Users.FindAsync(id);
                    if (existingUser == null)
                    {
                        return NotFound();
                    }

                    if (!User.IsInRole("administrator") && existingUser.UserId != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))
                    {
                        return Forbid();
                    }

                    existingUser.Email = userEditViewModel.Email;
                    existingUser.FirstName = userEditViewModel.FirstName;
                    existingUser.LastName = userEditViewModel.LastName;
                    existingUser.DateOfBirth = userEditViewModel.DateOfBirth;

                    if (User.IsInRole("administrator") && existingUser.UserId != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))
                    {
                        existingUser.UserType = userEditViewModel.UserType;
                    }

                    _context.Update(existingUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(userEditViewModel.UserId))
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
            ViewBag.IsAdmin = User.IsInRole("administrator");
            ViewBag.IsCurrentUser = id == int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(userEditViewModel);
        }

        // GET: Users/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                if (user.UserType == "administrator")
                {
                    ModelState.AddModelError("", "Nie można usunąć konta administratora.");
                    return RedirectToAction(nameof(Index));
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
