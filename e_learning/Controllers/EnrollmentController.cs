using e_learning.Data;
using e_learning.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace e_learning.Controllers
{
    [Authorize]

    public class EnrollmentController : Controller

    {
        private readonly e_learningDbContext _context;

        public EnrollmentController(e_learningDbContext context)
        {
            _context = context;
        }

        // GET: Enrollment
        public async Task<IActionResult> Index()
        {
            var enrollments = await _context.Enrollments.Where(c => c.estado == true)
                .Include(e => e.Course)
                .Include(e => e.User)
                .ToListAsync();
            return View(enrollments);
        }
        [HttpGet]
        public IActionResult Create(int Id)
        {
            ViewData["CourseId"] = _context.Courses.Find(Id).Id;
            ViewData["CourseName"] = _context.Courses.Find(Id).name;
            ViewData["CoursePrice"] = _context.Courses.Find(Id).price;

            return PartialView("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Enrollment enrollment)
        {
            enrollment.estado = true;
            enrollment.EnrollmentDate = DateTime.Now;
            if (!ModelState.IsValid)
            {
                _context.Add(enrollment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(enrollment);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = _context.Enrollments
                .Include(e => e.User)
                .Include(e => e.Course)
                .FirstOrDefault(e => e.Id == id);
            return PartialView("Delete", enrollment);
        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var categoria = await _context.Enrollments.FindAsync(id);

                if (categoria == null)
                {
                    return NotFound();
                }

                categoria.estado = false;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = _context.Enrollments
                .Include(e => e.User)
                .Include(e => e.Course)
                .FirstOrDefault(e => e.Id == id);
            return PartialView("Details", enrollment);
        }

        private bool EnrollmentExists(int id)
        {
            return _context.Enrollments.Any(e => e.Id == id);
        }

        
     
    }
}
