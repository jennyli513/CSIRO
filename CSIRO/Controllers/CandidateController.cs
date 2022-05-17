using CSIRO.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
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
     
        [HttpGet]
        public IActionResult AddCandidate()
        {
            var course = from c in _db.course
                           select c;
            var uni = from u in _db.university
                      select u;
            Candidate can = new Candidate();
            fillArray(course, uni,can);
            return View(can);
        }

        [HttpPost]
        public IActionResult AddCandidate(Candidate can)
        {
            if(!ModelState.IsValid) return View(can);
            _db.candidate.Add(can);
            _db.SaveChanges();
        
            return View("SuccessApplication");
        }

        public IActionResult ShowCandidates()
        {
            var can = from c1 in _db.course
                      join c2 in _db.candidate
                      on c1.CourseID equals c2.CourseID
                      join u in _db.university
                      on c2.UniversityID equals u.UniversityID
                      select new
                      {
                          CandidateID = c2.CandidateID,
                          FirstName = c2.FirstName,
                          LastName = c2.LastName,
                          CourseTitle = c1.Title,
                          GPA = c2.GPA,
                          University = u.Name
                      };

            List<Candidate> canList = new List<Candidate>();
            foreach(var c in can)
            {
                canList.Add(new Candidate
                {
                    CandidateID = c.CandidateID,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Name = c.FirstName + " " + c.LastName,
                    CourseTitle = c.CourseTitle,
                    GPA = c.GPA,
                    University = c.University

                });
            }
            return View(canList);
        }
        private void fillArray(System.Linq.IQueryable<Course> c, System.Linq.IQueryable<University> u,Candidate can)
        {
            foreach (var course in c)
            {
                SelectListItem item = new SelectListItem();
                item.Text = course.Title;
                item.Value = course.CourseID.ToString();
                can.courseList.Add(item);
            }


            foreach (var uni in u)
            {
                SelectListItem item = new SelectListItem();
                item.Text = uni.Name;
                item.Value = uni.UniversityID.ToString();
                can.uniList.Add(item);
            }
        }

        [HttpGet]
        public IActionResult ShowOneCandidate(long Id)
        {
            var can = from c1 in _db.course
                      join c2 in _db.candidate
                      on c1.CourseID equals c2.CourseID
                      join u in _db.university
                      on c2.UniversityID equals u.UniversityID
                      where c2.CandidateID == Id
                      select new
                      {
                          CandidateID = c2.CandidateID,
                          FirstName = c2.FirstName,
                          LastName = c2.LastName,
                          Email = c2.Email,
                          Phone = c2.Phone,
                          CourseTitle = c1.Title,
                          GPA = c2.GPA,
                          University = u.Name,
                          CoverLetter = c2.CoverLetter
                      };

            Candidate c = (Candidate)can;
            return View(c);
           
        }   
    }
}
