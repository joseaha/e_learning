using e_learning.Areas.Identity.Data;
using e_learning.Data;
using e_learning.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using System.Diagnostics;

namespace e_learning.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        private readonly e_learningDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, e_learningDbContext context)
        {
            _logger = logger;
            this._userManager = userManager;
            _context = context;

        }
        public class EnrollmentChartData
        {
            public string CourseName { get; set; }
            public int EnrollmentCount { get; set; }
        }
        public IActionResult Index()
        {
            var enrollmentChartData = _context.Enrollments
                                 .Where(c => c.estado == true)
                                 .GroupBy(e => e.Course.name)
                                 .Select(group => new EnrollmentChartData
                                 {
                                     CourseName = group.Key,
                                     EnrollmentCount = group.Count()
                                 }).ToList();

            var labels = enrollmentChartData.Select(data => data.CourseName).ToList();
            var dataValues = enrollmentChartData.Select(data => data.EnrollmentCount).ToList();

            ViewBag.EnrollmentChartLabels = labels;
            ViewBag.EnrollmentChartData = dataValues;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ImprimirMatricula(string id)
        {
            var enrollments = _context.Enrollments.Where(c => c.UserId == id)
                .Include(e => e.Course)
                .Include(e => e.User)
                .FirstOrDefault();


            return new ViewAsPdf("ImprimirMatricula", enrollments)
            {
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageSize = Rotativa.AspNetCore.Options.Size.A4
            };
        }
    }
}