using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TSQLV6.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

public class ThesesController : Controller
{
    private readonly UniversityDbContext _context;
    private readonly ILogger<ThesesController> _logger;

    public ThesesController(UniversityDbContext context, ILogger<ThesesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: Theses
    public async Task<IActionResult> Index()
    {
        IQueryable<Thesis> thesesQuery = _context.Theses.Include(t => t.Student);

        if (!User.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "administrator"))
        {
            var currentUserEmail = User.Identity.Name;
            thesesQuery = thesesQuery.Where(t => t.Student.Email == currentUserEmail);
        }

        var theses = await thesesQuery.ToListAsync();

        return View(theses);
    }

    // GET: Theses/Create
    [Authorize(Roles = "student")]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Theses/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ThesisCreateViewModel thesisCreateViewModel)
    {
        if (!User.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "student"))
        {
            ModelState.AddModelError("", "Current user is not a student.");
        }

        if (ModelState.IsValid)
        {
            try
            {
                var student = await _context.Users
                    .Where(u => u.UserType == "student" && u.Email == User.Identity.Name)
                    .FirstOrDefaultAsync();

                if (student == null)
                {
                    ModelState.AddModelError("", "Student not found.");
                }
                else
                {
                    var thesis = new Thesis
                    {
                        Title = thesisCreateViewModel.Title,
                        ThesisType = thesisCreateViewModel.ThesisType,
                        DocumentPath = thesisCreateViewModel.DocumentPath,
                        UploadDate = DateTime.Now,
                        StudentId = student.UserId
                    };

                    _context.Add(thesis);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the thesis.");
                ModelState.AddModelError("", "An error occurred while creating the thesis. Please try again.");
            }
        }

        return View(thesisCreateViewModel);
    }

    // GET: Theses/Edit/5
    [Authorize(Roles = "student,administrator")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var thesis = await _context.Theses
            .Include(t => t.Student) // Wczytanie powiązanych danych użytkownika
            .FirstOrDefaultAsync(t => t.ThesisId == id);
        if (thesis == null)
        {
            return NotFound();
        }

        if (!User.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "administrator"))
        {
            var currentUserEmail = User.Identity.Name;
            if (thesis.Student?.Email != currentUserEmail)
            {
                return Forbid();
            }
        }

        var thesisEditViewModel = new ThesisEditViewModel
        {
            ThesisId = thesis.ThesisId,
            Title = thesis.Title,
            ThesisType = thesis.ThesisType,
            DocumentPath = thesis.DocumentPath
        };

        return View(thesisEditViewModel);
    }

    // POST: Theses/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ThesisEditViewModel thesisEditViewModel)
    {
        if (id != thesisEditViewModel.ThesisId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var thesis = await _context.Theses
                    .Include(t => t.Student) // Wczytanie powiązanych danych użytkownika
                    .FirstOrDefaultAsync(t => t.ThesisId == id);
                if (thesis == null)
                {
                    return NotFound();
                }

                if (!User.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "administrator"))
                {
                    var currentUserEmail = User.Identity.Name;
                    if (thesis.Student?.Email != currentUserEmail)
                    {
                        return Forbid();
                    }
                }

                thesis.Title = thesisEditViewModel.Title;
                thesis.ThesisType = thesisEditViewModel.ThesisType;
                thesis.DocumentPath = thesisEditViewModel.DocumentPath;

                _context.Update(thesis);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ThesisExists(thesisEditViewModel.ThesisId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        return View(thesisEditViewModel);
    }

    // GET: Theses/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var thesis = await _context.Theses
            .Include(t => t.Student) // Wczytanie powiązanych danych użytkownika
            .FirstOrDefaultAsync(m => m.ThesisId == id);
        if (thesis == null)
        {
            return NotFound();
        }

        if (!User.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "administrator"))
        {
            var currentUserEmail = User.Identity.Name;
            if (thesis.Student?.Email != currentUserEmail)
            {
                return Forbid();
            }
        }

        return View(thesis);
    }

    // GET: Theses/Delete/5
    [Authorize(Roles = "student,administrator")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var thesis = await _context.Theses
            .Include(t => t.Student) // Wczytanie powiązanych danych użytkownika
            .FirstOrDefaultAsync(m => m.ThesisId == id);
        if (thesis == null)
        {
            return NotFound();
        }

        if (!User.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "administrator"))
        {
            var currentUserEmail = User.Identity.Name;
            if (thesis.Student?.Email != currentUserEmail)
            {
                return Forbid();
            }
        }

        return View(thesis);
    }

    // POST: Theses/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var thesis = await _context.Theses.FindAsync(id);
        if (thesis == null)
        {
            return NotFound();
        }

        _context.Theses.Remove(thesis);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ThesisExists(int id)
    {
        return _context.Theses.Any(e => e.ThesisId == id);
    }
}
