using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TSQLV6.Models;
using System.Threading.Tasks;

namespace TSQLV6.Controllers
{
    [Authorize(Roles = "administrator")]
    public class StudentsController : Controller
    {
        private readonly UniversityDbContext _context;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(UniversityDbContext context, ILogger<StudentsController> logger)
        {
            _logger = logger;
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            var students = await _context.Students
                .Include(s => s.Department)
                .Include(s => s.Specialization)
                .Include(s => s.StudentNavigation)
                .Select(s => new StudentViewModel
                {
                    StudentId = s.StudentId,
                    StudyMode = s.StudyMode,
                    SpecializationId = s.SpecializationId,
                    DepartmentId = s.DepartmentId,
                    StudentName = s.StudentNavigation.FirstName + " " + s.StudentNavigation.LastName,
                    SpecializationName = s.Specialization != null ? s.Specialization.SpecializationName : "None",
                    DepartmentName = s.Department != null ? s.Department.DepartmentName : "None"
                })
                .ToListAsync();

            return View(students);
        }


        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Department)
                .Include(s => s.Specialization)
                .Include(s => s.StudentNavigation)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            var studentViewModel = new StudentViewModel
            {
                StudentId = student.StudentId,
                StudyMode = student.StudyMode,
                SpecializationId = student.SpecializationId,
                DepartmentId = student.DepartmentId,
                StudentName = student.StudentNavigation.FirstName + " " + student.StudentNavigation.LastName,
                SpecializationName = student.Specialization?.SpecializationName,
                DepartmentName = student.Department?.DepartmentName
            };

            return View(studentViewModel);
        }


        // GET: Students/Create
        public IActionResult Create()
        {
            ViewBag.StudyModeOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "Part-time", Text = "Part-time" },
                new SelectListItem { Value = "Full-time", Text = "Full-time" },
                new SelectListItem { Value = "Evening", Text = "Evening" }
            };

            ViewBag.Departments = new SelectList(_context.Departments, "DepartmentId", "DepartmentName");
            ViewBag.Specializations = new SelectList(_context.Specializations, "SpecializationId", "SpecializationName");
            ViewBag.Users = new SelectList(_context.Users.Where(u => u.UserType == "student").Select(u => new { u.UserId, Name = u.FirstName + " " + u.LastName }), "UserId", "Name");

            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentCreateViewModel studentViewModel)
        {
            _logger.LogInformation("Received POST request to create a student with the following data: {@StudentCreateViewModel}", studentViewModel);

            // Check if a student with the same StudentId already exists
            var existingStudent = await _context.Students.FindAsync(studentViewModel.StudentId);
            if (existingStudent != null)
            {
                ModelState.AddModelError("", "A student with the same StudentId already exists.");
            }

            if (ModelState.IsValid)
            {
                var student = new Student
                {
                    StudyMode = studentViewModel.StudyMode,
                    SpecializationId = studentViewModel.SpecializationId == 0 ? null : studentViewModel.SpecializationId,
                    DepartmentId = studentViewModel.DepartmentId == 0 ? null : studentViewModel.DepartmentId,
                    StudentId = studentViewModel.StudentId
                };

                _context.Add(student);
                try
                {
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Student created successfully.");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while creating the student.");
                }
            }

            // Log detailed model state errors
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    _logger.LogError("Model state error in key '{Key}': {ErrorMessage}", state.Key, error.ErrorMessage);
                }
            }

            ViewBag.StudyModeOptions = new List<SelectListItem>
    {
        new SelectListItem { Value = "Part-time", Text = "Part-time" },
        new SelectListItem { Value = "Full-time", Text = "Full-time" },
        new SelectListItem { Value = "Evening", Text = "Evening" }
    };

            ViewBag.Departments = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", studentViewModel.DepartmentId);
            ViewBag.Specializations = new SelectList(_context.Specializations, "SpecializationId", "SpecializationName", studentViewModel.SpecializationId);
            ViewBag.Users = new SelectList(_context.Users.Where(u => u.UserType == "student").Select(u => new { u.UserId, Name = u.FirstName + " " + u.LastName }), "UserId", "Name", studentViewModel.StudentId);

            return View(studentViewModel);
        }


        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.StudentNavigation)
                .FirstOrDefaultAsync(s => s.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            var studentViewModel = new StudentEditViewModel
            {
                StudentId = student.StudentId,
                StudyMode = student.StudyMode,
                SpecializationId = student.SpecializationId,
                DepartmentId = student.DepartmentId,
                OriginalStudentId = student.StudentId
            };

            ViewBag.StudyModeOptions = new List<SelectListItem>
    {
        new SelectListItem { Value = "Part-time", Text = "Part-time" },
        new SelectListItem { Value = "Full-time", Text = "Full-time" },
        new SelectListItem { Value = "Evening", Text = "Evening" }
    };

            ViewBag.Departments = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", studentViewModel.DepartmentId);
            ViewBag.Specializations = new SelectList(_context.Specializations, "SpecializationId", "SpecializationName", studentViewModel.SpecializationId);
            ViewBag.Users = new SelectList(_context.Users.Where(u => u.UserType == "student").Select(u => new { u.UserId, Name = u.FirstName + " " + u.LastName }), "UserId", "Name", studentViewModel.StudentId);

            return View(studentViewModel);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StudentEditViewModel studentViewModel)
        {
            _logger.LogInformation("Received POST request to edit a student with the following data: {@StudentCreateViewModel}", studentViewModel);



            if (ModelState.IsValid)
            {
                try
                {
                    var existingStudent = await _context.Students
                        .FirstOrDefaultAsync(s => s.StudentId == studentViewModel.StudentId && s.StudentId != id);

                    if (existingStudent != null)
                    {
                        ModelState.AddModelError("StudentId", "A student with the selected user already exists.");

                        ViewBag.StudyModeOptions = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Part-time", Text = "Part-time" },
                    new SelectListItem { Value = "Full-time", Text = "Full-time" },
                    new SelectListItem { Value = "Evening", Text = "Evening" }
                };

                        ViewBag.Departments = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", studentViewModel.DepartmentId);
                        ViewBag.Specializations = new SelectList(_context.Specializations, "SpecializationId", "SpecializationName", studentViewModel.SpecializationId);
                        ViewBag.Users = new SelectList(_context.Users.Where(u => u.UserType == "student").Select(u => new { u.UserId, Name = u.FirstName + " " + u.LastName }), "UserId", "Name", studentViewModel.StudentId);

                        return View(studentViewModel);
                    }

                    var student = await _context.Students.FindAsync(id);
                    if (student == null)
                    {
                        return NotFound();
                    }

                    student.StudyMode = studentViewModel.StudyMode;
                    student.SpecializationId = studentViewModel.SpecializationId == 0 ? null : studentViewModel.SpecializationId;
                    student.DepartmentId = studentViewModel.DepartmentId == 0 ? null : studentViewModel.DepartmentId;
                    student.StudentNavigation = await _context.Users.FindAsync(studentViewModel.StudentId);

                    _context.Update(student);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(studentViewModel.StudentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewBag.StudyModeOptions = new List<SelectListItem>
    {
        new SelectListItem { Value = "Part-time", Text = "Part-time" },
        new SelectListItem { Value = "Full-time", Text = "Full-time" },
        new SelectListItem { Value = "Evening", Text = "Evening" }
    };

            ViewBag.Departments = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", studentViewModel.DepartmentId);
            ViewBag.Specializations = new SelectList(_context.Specializations, "SpecializationId", "SpecializationName", studentViewModel.SpecializationId);
            ViewBag.Users = new SelectList(_context.Users.Where(u => u.UserType == "student").Select(u => new { u.UserId, Name = u.FirstName + " " + u.LastName }), "UserId", "Name", studentViewModel.StudentId);

            return View(studentViewModel);
        }



        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Department)
                .Include(s => s.Specialization)
                .Include(s => s.StudentNavigation)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            var studentViewModel = new StudentDeleteViewModel
            {
                StudentId = student.StudentId,
                StudyMode = student.StudyMode,
                SpecializationName = student.Specialization?.SpecializationName,
                DepartmentName = student.Department?.DepartmentName,
                StudentName = student.StudentNavigation.FirstName + " " + student.StudentNavigation.LastName
            };

            return View(studentViewModel);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentId == id);
        }
    }
}
