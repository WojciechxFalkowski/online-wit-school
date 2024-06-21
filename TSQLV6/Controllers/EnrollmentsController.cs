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
            IQueryable<Enrollment> enrollmentsQuery = _context.Enrollments.Include(e => e.Course);

            if (User.IsInRole("student"))
            {
                var userEmail = User.Identity.Name;

                var userId = _context.Users
                    .Where(u => u.Email == userEmail)
                    .Select(u => u.UserId.ToString())
                    .FirstOrDefault();

                enrollmentsQuery = enrollmentsQuery.Where(e => e.StudentId == userId);
            }

            var enrollments = await enrollmentsQuery
                .Select(e => new EnrollmentViewModel
                {
                    EnrollmentId = e.EnrollmentId,
                    StudentName = _context.Users
                        .Where(u => u.UserId.ToString() == e.StudentId)
                        .Select(u => u.FirstName + " " + u.LastName)
                        .FirstOrDefault(),
                    CourseName = e.Course.CourseName,
                    Grade = e.Grade,
                    Points = e.Points,
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

            var student = await _context.Users
                .Where(u => u.UserId.ToString() == enrollment.StudentId)
                .Select(u => new { u.FirstName, u.LastName })
                .FirstOrDefaultAsync();

            if (student == null)
            {
                return NotFound();
            }

            var viewModel = new EnrollmentDetailsViewModel
            {
                EnrollmentId = enrollment.EnrollmentId,
                StudentName = student.FirstName + " " + student.LastName,
                CourseName = enrollment.Course.CourseName,
                Grade = enrollment.Grade,
                CompletionDate = enrollment.CompletionDate.ToString(),
                Points = enrollment.Points
            };

            return View(viewModel);
        }

        // GET: Enrollments/Create
        public IActionResult Create()
        {
            ViewData["Students"] = new SelectList(_context.Users.Where(u => u.UserType == "student").Select(s => new { s.UserId, StudentName = s.FirstName + " " + s.LastName }), "UserId", "StudentName");
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

            if (ModelState.IsValid)
            {
                // Sprawdzenie, czy ten kurs jest już przypisany do studenta
                var exists = await _context.Enrollments.AnyAsync(e => e.StudentId == model.StudentId && e.CourseId == model.CourseId);
                if (exists)
                {
                    ModelState.AddModelError(string.Empty, "The student is already enrolled in this course.");
                    ViewData["Students"] = new SelectList(_context.Users.Where(u => u.UserType == "student").Select(s => new { s.UserId, StudentName = s.FirstName + " " + s.LastName }), "UserId", "StudentName", model.StudentId);
                    ViewData["Courses"] = new SelectList(_context.Courses, "CourseId", "CourseName", model.CourseId);
                    return View(model);
                }

                var enrollment = new Enrollment
                {
                    EnrollmentId = model.EnrollmentId,
                    StudentId = model.StudentId,
                    CourseId = model.CourseId,
                    Grade = model.Grade,
                    CompletionDate = model.CompletionDate,
                    Points = model.Points,
                };
                _context.Add(enrollment);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Enrollment created successfully.");
                return RedirectToAction(nameof(Index));
            }

            ViewData["Students"] = new SelectList(_context.Users.Where(u => u.UserType == "student").Select(s => new { s.UserId, StudentName = s.FirstName + " " + s.LastName }), "UserId", "StudentName", model.StudentId);
            ViewData["Courses"] = new SelectList(_context.Courses, "CourseId", "CourseName", model.CourseId);
            return View(model);
        }

        // GET: Enrollments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogInformation("Enrollment edit v1");
                return NotFound();
            }

            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null)
            {
                _logger.LogInformation("Enrollment edit v2");
                return NotFound();
            }

            _logger.LogInformation("Enrollment edit v");
            var model = new EditEnrollmentViewModel
            {
                EnrollmentId = enrollment.EnrollmentId,
                StudentId = enrollment.StudentId,
                CourseId = enrollment.CourseId,
                Grade = enrollment.Grade,
                CompletionDate = enrollment.CompletionDate,
                Points = enrollment.Points
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
                _logger.LogInformation("Enrollment edit v1");
                _logger.LogInformation("Enrollment edit v1 {id} {model.EnrollmentId}", id, model.EnrollmentId);

                return NotFound();
            }

            _logger.LogInformation("Enrollment edit v2");

            if (ModelState.IsValid)
            {
                _logger.LogInformation("Enrollment edit v3");

                try
                {
                    _logger.LogInformation("Enrollment edit v4");

                    var enrollment = await _context.Enrollments.FindAsync(id);
                    if (enrollment == null)
                    {
                        return NotFound();
                    }

                    enrollment.StudentId = model.StudentId;
                    enrollment.CourseId = model.CourseId;
                    enrollment.Grade = model.Grade;
                    enrollment.CompletionDate = model.CompletionDate;
                    enrollment.Points = model.Points;

                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    _logger.LogInformation("Enrollment edit v5");

                    if (!EnrollmentExists(model.EnrollmentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                _logger.LogInformation("Enrollment edit v6");

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
