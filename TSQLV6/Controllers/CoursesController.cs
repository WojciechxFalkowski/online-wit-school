using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TSQLV6.Models;

public class CoursesController : Controller
{
    private readonly UniversityDbContext _context;
    private readonly ILogger<CoursesController> _logger;

    public CoursesController(UniversityDbContext context, ILogger<CoursesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: Courses
    public async Task<IActionResult> Index()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction(nameof(Index));
        }
        var courses = await _context.Courses.Include(c => c.Lecturer).ToListAsync();
        var lecturers = await _context.Users.Where(u => u.UserType == "lecturer").ToDictionaryAsync(u => u.UserId, u => u.FirstName + " " + u.LastName);

        ViewBag.Courses = courses;
        ViewBag.Lecturers = lecturers;

        return View();
    }


    // GET: Courses/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var course = await _context.Courses
            .Include(c => c.Lecturer)
            .FirstOrDefaultAsync(m => m.CourseId == id);
        if (course == null)
        {
            return NotFound();
        }

        var lecturerName = course.Lecturer != null ? $"{course.Lecturer.FirstName} {course.Lecturer.LastName}" : "Unknown";
        var lecturerEmail = course.Lecturer != null ? course.Lecturer.Email : "Unknown";

        var courseViewModel = new CourseViewModel
        {
            CourseId = course.CourseId,
            CourseCode = course.CourseCode,
            CourseName = course.CourseName,
            Description = course.Description,
            AcademicYear = course.AcademicYear,
            LecturerId = course.LecturerId,
            LecturerEmail = lecturerEmail
        };

        ViewBag.LecturerName = lecturerName;
        ViewBag.LecturerEmail = lecturerEmail;

        return View(courseViewModel);
    }



    // GET: Courses/Create
    public IActionResult Create()
    {
        ViewBag.Lecturers = new SelectList(
            _context.Users
                .Where(u => u.UserType == "lecturer")
                .Select(u => new { u.UserId, FullName = u.FirstName + " " + u.LastName })
                .ToList(),
            "UserId", "FullName");
        return View();
    }


    // POST: Courses/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CourseViewModel courseViewModel)
    {
        if (ModelState.IsValid)
        {
            var course = new Course
            {
                CourseCode = courseViewModel.CourseCode,
                CourseName = courseViewModel.CourseName,
                Description = courseViewModel.Description,
                AcademicYear = courseViewModel.AcademicYear,
                LecturerId = courseViewModel.LecturerId
            };

            _context.Add(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Lecturers = new SelectList(
            _context.Users
                .Where(u => u.UserType == "lecturer")
                .Select(u => new { u.UserId, FullName = u.FirstName + " " + u.LastName })
                .ToList(),
            "UserId", "FullName", courseViewModel.LecturerId);
        return View(courseViewModel);
    }

    // GET: Courses/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var course = await _context.Courses
            .Include(c => c.Lecturer)
            .FirstOrDefaultAsync(c => c.CourseId == id);

        if (course == null)
        {
            return NotFound();
        }

        var courseViewModel = new CourseViewModel
        {
            CourseId = course.CourseId,
            CourseCode = course.CourseCode,
            CourseName = course.CourseName,
            Description = course.Description,
            AcademicYear = course.AcademicYear,
            LecturerId = course.LecturerId,
            LecturerEmail = course.Lecturer != null ? course.Lecturer.Email : ""
        };

        ViewBag.Lecturers = new SelectList(
            await _context.Users
                .Where(u => u.UserType == "lecturer")
                .Select(u => new { u.UserId, FullName = u.FirstName + " " + u.LastName })
                .ToListAsync(),
            "UserId", "FullName", course.LecturerId);

        return View(courseViewModel);
    }


    // POST: Courses/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CourseViewModel courseViewModel)
    {
        if (id != courseViewModel.CourseId)
        {
            return NotFound();
        }

        _logger.LogInformation($"LecturerEmail: {courseViewModel.LecturerEmail}, LecturerId: {courseViewModel.LecturerId}");
        _logger.LogInformation($"ModelState.IsValid: {ModelState.IsValid}");
        if (courseViewModel.LecturerId == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var course = await _context.Courses.FindAsync(id);
                if (course == null)
                {
                    return NotFound();
                }

                course.CourseCode = courseViewModel.CourseCode;
                course.CourseName = courseViewModel.CourseName;
                course.Description = courseViewModel.Description;
                course.AcademicYear = courseViewModel.AcademicYear;
                course.LecturerId = courseViewModel.LecturerId;

                _context.Update(course);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(courseViewModel.CourseId))
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

        ViewBag.Lecturers = new SelectList(
            await _context.Users
                .Where(u => u.UserType == "lecturer")
                .Select(u => new { u.UserId, FullName = u.FirstName + " " + u.LastName })
                .ToListAsync(),
            "UserId", "FullName", courseViewModel.LecturerId);

        return View(courseViewModel);
    }


    // GET: Courses/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var course = await _context.Courses
            .Include(c => c.Lecturer)
            .FirstOrDefaultAsync(m => m.CourseId == id);
        if (course == null)
        {
            return NotFound();
        }

        var courseViewModel = new CourseViewModel
        {
            CourseId = course.CourseId,
            CourseCode = course.CourseCode,
            CourseName = course.CourseName,
            Description = course.Description,
            AcademicYear = course.AcademicYear,
            LecturerId = course.LecturerId,
            LecturerEmail = course.Lecturer != null ? course.Lecturer.Email : ""
        };

        return View(courseViewModel);
    }

    // POST: Courses/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course == null)
        {
            return NotFound();
        }

        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CourseExists(int id)
    {
        return _context.Courses.Any(e => e.CourseId == id);
    }
}
