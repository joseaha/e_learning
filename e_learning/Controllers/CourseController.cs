using e_learning.Data;
using e_learning.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace e_learning.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly e_learningDbContext _context;

        public CourseController(e_learningDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Courses.Where(c => c.estado == true).ToListAsync());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            var course = _context.Courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }
            return PartialView("Details",course);
        }
        [HttpPost]
        public ActionResult Create(Course course)
        {
            course.estado = true;
            if (!ModelState.IsValid)
            {
                _context.Courses.Add(course);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(course);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var course = _context.Courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }
            return PartialView("Delete",course);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var course = _context.Courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }

            course.estado = false; 
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Listado()
        {
            return View(await _context.Courses.Where(c => c.estado == true).ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Course modelo)
        {
            if (id != modelo.Id)
            {
                return NotFound();
            }
            modelo.estado = true;
            if (!ModelState.IsValid)
            {
                try
                {

                    var courseExitente = await _context.Courses.FindAsync(id);
                    if (courseExitente == null)
                    {
                        return NotFound();
                    }

                    courseExitente.name = modelo.name;
                    courseExitente.price = modelo.price;
                    courseExitente.description = modelo.description;
                    courseExitente.imgUrl = modelo.imgUrl;
                    courseExitente.VideoUrl = modelo.VideoUrl;
                    courseExitente.estado = true;

                    await _context.SaveChangesAsync();

                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "No se pueden guardar los cambios. " +
                                "Inténtalo de nuevo y, si el problema persiste, " +
                                "consulta con el administrador del sistema.");

                }
            }
            return NotFound();
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var categoriaModel = _context.Courses.FirstOrDefault(c => c.Id == id);
            return PartialView("Edit", categoriaModel);
        }

    }
}
