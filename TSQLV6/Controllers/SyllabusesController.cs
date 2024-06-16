using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TSQLV6.Models;

public class SyllabusesController : Controller
{
    private readonly UniversityDbContext _context;

    public SyllabusesController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: Syllabuses
    public async Task<IActionResult> Index()
    {
        var syllabuses = await _context.Syllabi
            .Include(s => s.Course)
            .Select(s => new SyllabusViewModel
            {
                SyllabusId = s.SyllabusId,
                CourseName = s.Course.CourseName,
                Description = s.Description,
                LearningObjectives = s.LearningObjectives,
                AssessmentMethods = s.AssessmentMethods,
                ReadingMaterials = s.ReadingMaterials
            })
            .ToListAsync();

        return View(syllabuses);
    }

    // GET: Syllabuses/Create
    public IActionResult Create()
    {
        ViewBag.Courses = new SelectList(_context.Courses, "CourseId", "CourseName");
        return View();
    }

    // POST: Syllabuses/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SyllabusCreateViewModel model)
    {
        if (ModelState.IsValid)
        {
            var syllabus = new Syllabus
            {
                CourseId = model.CourseId,
                Description = model.Description,
                LearningObjectives = model.LearningObjectives,
                AssessmentMethods = model.AssessmentMethods,
                ReadingMaterials = model.ReadingMaterials
            };
            _context.Add(syllabus);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Courses = new SelectList(_context.Courses, "CourseId", "CourseName", model.CourseId);
        return View(model);
    }

    // GET: Syllabuses/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var syllabus = await _context.Syllabi.FindAsync(id);
        if (syllabus == null)
        {
            return NotFound();
        }

        var model = new SyllabusEditViewModel
        {
            SyllabusId = syllabus.SyllabusId,
            CourseId = syllabus.CourseId,
            Description = syllabus.Description,
            LearningObjectives = syllabus.LearningObjectives,
            AssessmentMethods = syllabus.AssessmentMethods,
            ReadingMaterials = syllabus.ReadingMaterials
        };

        ViewBag.Courses = new SelectList(_context.Courses, "CourseId", "CourseName", syllabus.CourseId);
        return View(model);
    }

    // POST: Syllabuses/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, SyllabusEditViewModel model)
    {
        if (id != model.SyllabusId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var syllabus = await _context.Syllabi.FindAsync(id);
                if (syllabus == null)
                {
                    return NotFound();
                }

                syllabus.CourseId = model.CourseId;
                syllabus.Description = model.Description;
                syllabus.LearningObjectives = model.LearningObjectives;
                syllabus.AssessmentMethods = model.AssessmentMethods;
                syllabus.ReadingMaterials = model.ReadingMaterials;

                _context.Update(syllabus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SyllabusExists(model.SyllabusId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        ViewBag.Courses = new SelectList(_context.Courses, "CourseId", "CourseName", model.CourseId);
        return View(model);
    }

    // GET: Syllabuses/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var syllabus = await _context.Syllabi
            .Include(s => s.Course)
            .FirstOrDefaultAsync(m => m.SyllabusId == id);
        if (syllabus == null)
        {
            return NotFound();
        }

        return View(syllabus);
    }

    // GET: Syllabuses/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var syllabus = await _context.Syllabi
            .Include(s => s.Course)
            .FirstOrDefaultAsync(m => m.SyllabusId == id);
        if (syllabus == null)
        {
            return NotFound();
        }

        return View(syllabus);
    }

    // POST: Syllabuses/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var syllabus = await _context.Syllabi.FindAsync(id);
        if (syllabus == null)
        {
            return NotFound();
        }

        _context.Syllabi.Remove(syllabus);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool SyllabusExists(int id)
    {
        return _context.Syllabi.Any(e => e.SyllabusId == id);
    }
}
