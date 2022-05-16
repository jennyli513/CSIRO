using CSIRO.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        [HttpGet]
        public IActionResult AddCandidate()
        {
            var course = from c in _db.course
                           select c;
            Candidate can = new Candidate();
            fillArray(course, can);
            return View(can);
        }

        private void fillArray(System.Linq.IQueryable<Course> c, Candidate can)
        {
            foreach (var course in c)
            {
                SelectListItem item = new SelectListItem();
                item.Text = course.Title;
                item.Value = course.CourseID.ToString();
                can.courseList.Add(item);
            }
        }
    }
}
