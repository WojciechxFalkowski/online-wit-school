using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TSQLV6.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace TSQLV6.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly UniversityDbContext _context;
        private readonly ILogger<EnrollmentsController> _logger;

        public EnrollmentsController(UniversityDbContext context, ILogger<EnrollmentsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Enrollments
        public async Task<IActionResult> Index()
        {
            var enrollments = await _context.Enrollments
                .Include(e => e.Course)
                .Select(e => new EnrollmentViewModel
                {
                    EnrollmentId = e.EnrollmentId,
                    StudentName = _context.Users
                        .Where(u => u.UserId.ToString() == e.StudentId)
                        .Select(u => u.FirstName + " " + u.LastName)
                        .FirstOrDefault(),
                    CourseName = e.Course.CourseName,
                    Grade = e.Grade,
                    CompletionDate = e.CompletionDate.HasValue ? e.CompletionDate.Value.ToString("yyyy-MM-dd") : null
                })
                .ToListAsync();

            return View(enrollments);
        }

        // GET: Enrollments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments
                .Include(e => e.Course)
                .FirstOrDefaultAsync(m => m.EnrollmentId == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // GET: Enrollments/Create
        public IActionResult Create()
        {
            ViewData["Students"] = new SelectList(_context.Students.Select(s => new { s.StudentId, StudentName = s.StudentNavigation.FirstName + " " + s.StudentNavigation.LastName }), "StudentId", "StudentName");
            ViewData["Courses"] = new SelectList(_context.Courses, "CourseId", "CourseName");
            return View();
        }

        // POST: Enrollments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEnrollmentViewModel model)
        {
            _logger.LogInformation("Received POST request to create an enrollment with the following data: {Enrollment}", model);
            _logger.LogInformation("ModelState.IsValid: {IsValid}", ModelState.IsValid);

            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        _logger.LogError("Model state error in key '{Key}': {ErrorMessage}", state.Key, error.ErrorMessage);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                var enrollment = new Enrollment
                {
                    EnrollmentId = model.EnrollmentId,
                    StudentId = model.StudentId,
                    CourseId = model.CourseId,
                    Grade = model.Grade,
                    CompletionDate = model.CompletionDate
                };
                _context.Add(enrollment);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Enrollment created successfully.");
                return RedirectToAction(nameof(Index));
            }

            ViewData["Students"] = new SelectList(_context.Students.Select(s => new { s.StudentId, StudentName = s.StudentNavigation.FirstName + " " + s.StudentNavigation.LastName }), "StudentId", "StudentName", model.StudentId);
            ViewData["Courses"] = new SelectList(_context.Courses, "CourseId", "CourseName", model.CourseId);
            return View(model);
        }

        // GET: Enrollments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }

            var model = new EditEnrollmentViewModel
            {
                EnrollmentId = enrollment.EnrollmentId,
                StudentId = enrollment.StudentId,
                CourseId = enrollment.CourseId,
                Grade = enrollment.Grade,
                CompletionDate = enrollment.CompletionDate
            };

            ViewData["Students"] = new SelectList(_context.Students.Select(s => new { s.StudentId, StudentName = s.StudentNavigation.FirstName + " " + s.StudentNavigation.LastName }), "StudentId", "StudentName", model.StudentId);
            ViewData["Courses"] = new SelectList(_context.Courses, "CourseId", "CourseName", model.CourseId);
            return View(model);
        }

        // POST: Enrollments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditEnrollmentViewModel model)
        {
            if (id != model.EnrollmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var enrollment = await _context.Enrollments.FindAsync(id);
                    if (enrollment == null)
                    {
                        return NotFound();
                    }

                    enrollment.StudentId = model.StudentId;
                    enrollment.CourseId = model.CourseId;
                    enrollment.Grade = model.Grade;
                    enrollment.CompletionDate = model.CompletionDate;

                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(model.EnrollmentId))
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

            ViewData["Students"] = new SelectList(_context.Students.Select(s => new { s.StudentId, StudentName = s.StudentNavigation.FirstName + " " + s.StudentNavigation.LastName }), "StudentId", "StudentName", model.StudentId);
            ViewData["Courses"] = new SelectList(_context.Courses, "CourseId", "CourseName", model.CourseId);
            return View(model);
        }

        // GET: Enrollments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments
                .Include(e => e.Course)
                .FirstOrDefaultAsync(m => m.EnrollmentId == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnrollmentExists(int id)
        {
            return _context.Enrollments.Any(e => e.EnrollmentId == id);
        }
    }
}
