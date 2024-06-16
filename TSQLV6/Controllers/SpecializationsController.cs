using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TSQLV6.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TSQLV6.Controllers
{
    public class SpecializationsController : Controller
    {
        private readonly UniversityDbContext _context;

        public SpecializationsController(UniversityDbContext context)
        {
            _context = context;
        }

        // GET: Specializations
        public async Task<IActionResult> Index()
        {
            var specializations = await _context.Specializations.Include(s => s.Department).ToListAsync();
            return View(specializations);
        }

        // GET: Specializations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialization = await _context.Specializations
                .Include(s => s.Department)
                .FirstOrDefaultAsync(m => m.SpecializationId == id);
            if (specialization == null)
            {
                return NotFound();
            }

            return View(specialization);
        }

        // GET: Specializations/Create
        public IActionResult Create()
        {
            ViewBag.Departments = new SelectList(_context.Departments, "DepartmentId", "DepartmentName");
            return View();
        }

        // POST: Specializations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpecializationCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var specialization = new Specialization
                {
                    SpecializationName = model.SpecializationName,
                    DepartmentId = model.DepartmentId,
                    Description = model.Description
                };

                _context.Add(specialization);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Departments = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", model.DepartmentId);
            return View(model);
        }

        // GET: Specializations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialization = await _context.Specializations.FindAsync(id);
            if (specialization == null)
            {
                return NotFound();
            }

            var model = new SpecializationEditViewModel
            {
                SpecializationId = specialization.SpecializationId,
                SpecializationName = specialization.SpecializationName,
                DepartmentId = specialization.DepartmentId,
                Description = specialization.Description
            };

            ViewBag.Departments = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", model.DepartmentId);
            return View(model);
        }

        // POST: Specializations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SpecializationEditViewModel model)
        {
            if (id != model.SpecializationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var specialization = await _context.Specializations.FindAsync(id);
                    if (specialization == null)
                    {
                        return NotFound();
                    }

                    specialization.SpecializationName = model.SpecializationName;
                    specialization.DepartmentId = model.DepartmentId;
                    specialization.Description = model.Description;

                    _context.Update(specialization);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecializationExists(model.SpecializationId))
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

            ViewBag.Departments = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", model.DepartmentId);
            return View(model);
        }

        private bool SpecializationExists(int id)
        {
            return _context.Specializations.Any(e => e.SpecializationId == id);
        }

        // GET: Specializations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialization = await _context.Specializations
                .Include(s => s.Department)
                .FirstOrDefaultAsync(m => m.SpecializationId == id);
            if (specialization == null)
            {
                return NotFound();
            }

            return View(specialization);
        }

        // POST: Specializations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var specialization = await _context.Specializations.FindAsync(id);
            _context.Specializations.Remove(specialization);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
