using CSIRO.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CSIRO.Controllers
{
    

    public class CandidateController : Controller
    {
        private readonly CandidateDataContext _db;

        public CandidateController(CandidateDataContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DisplayCourse()
        {
            Course[] courseArray = _db.course.ToArray<Course>();
            return View(courseArray);
        }

        public IActionResult DisplayUniversity()
        {
            University[] uniArray = _db.university.ToArray<University>();
            return View(uniArray);
        }
    }
}
