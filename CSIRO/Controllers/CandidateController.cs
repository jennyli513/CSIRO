using CSIRO.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
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

        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult Index(string sortOrder)
        {
            var can = from c in _db.candidate
                      select c;
            ViewBag.NameSortParm = "name_asc";
            ViewBag.GPASortParm = "GPA_desc";
            switch (sortOrder)
            {
                case "name_asc":
                    can = can.OrderBy(c => c.LastName);
                    break;
                case "GPA_desc":
                    can = can.OrderByDescending(c => c.GPA);
                    break;
                default:
                    can = can.OrderBy(c => c.LastName);
                    break;

            }
            return View(can.ToList());
        }

        [HttpGet]

        //this method is to add an candidate
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

        public IActionResult ShowCandidates(string sortOrder, string searchString)
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

            //sort by last name and GPA
            ViewBag.NameSortParm = "name_asc";
            ViewBag.GPASortParm = "GPA_desc";

            if (!String.IsNullOrEmpty(searchString))
            {
                can = can.Where(c => c.LastName.Contains(searchString)
                                       || c.FirstName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_asc":
                    can = can.OrderBy(c => c.LastName);
                    break;
                case "GPA_desc":
                    can = can.OrderByDescending(c => c.GPA);
                    break;
                default:
                    can = can.OrderBy(c => c.CandidateID);
                    break;

            }


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
            Candidate c = _db.candidate.Find(Id);
            Course c1 = _db.course.Find(c.CourseID);
            c.CourseTitle = c1.Title;
            University u = _db.university.Find(c.UniversityID);
            c.University = u.Name;
            return View(c);    
        }   


    }
}
