using CSIRO.Models;
using CSIRO.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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

        public IActionResult Index()
        {
           
            return View();
        }


        [Authorize(Roles = "Candidate")]
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

        //method to fill array
        private void fillArray(System.Linq.IQueryable<Course> c, System.Linq.IQueryable<University> u, Candidate can)
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

        [Authorize(Roles = "Candidate")]
        [HttpPost]
        public IActionResult AddCandidate(Candidate can)
        {
            if(!ModelState.IsValid) return View(can);
            _db.candidate.Add(can);
            _db.SaveChanges();
        
            return View("SuccessApplication");
        }

        [Authorize(Roles = "Admin")]
        //show all the candidates and sort by last name asc or GPA desc
        [HttpGet]
        public IActionResult ShowCandidates(string sortOrder)
        {

            List<Candidate> canList = GetCandidates();
            ViewBag.CurrentSort = sortOrder;
            //sort by last name and GPA
            ViewBag.NameSortParm = "name_asc";
            ViewBag.GPASortParm = "GPA_desc";
            switch (sortOrder)
            {
                case "name_asc":
                    canList = canList.OrderBy(c => c.LastName).ToList();
                    break;
                case "GPA_desc":
                    canList = canList.OrderByDescending(c => c.GPA).ToList();
                    break;
                default:
                    canList = canList.OrderBy(c => c.CandidateID).ToList();
                    break;

            }
            return View(canList);
        }

        //method to get all the candidates as a list
        private List<Candidate> GetCandidates()
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
                          CourseID = c2.CourseID,
                          CourseTitle = c1.Title,
                          GPA = c2.GPA,
                          University = u.Name,

                      };
            List<Candidate> canList = new List<Candidate>();
            foreach (var c in can)
            {
                canList.Add(new Candidate
                {
                    CandidateID = c.CandidateID,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Name = c.FirstName + " " + c.LastName,
                    CourseID = c.CourseID,
                    CourseTitle = c.CourseTitle,
                    GPA = c.GPA,
                    University = c.University

                });
            }
            return canList;

        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult SearchCandidates(string searchString, long searchCourse)
        {
            List<Candidate> can = GetCandidates();

            if (!String.IsNullOrEmpty(searchString))
            {
                can = can.Where(c => c.LastName.ToUpper().Contains(searchString.ToUpper())
                                     || c.FirstName.ToUpper().Contains(searchString.ToUpper())).ToList();
            }

            if (searchCourse != 0)
            {
                can = can.Where(c => c.CourseID == searchCourse).ToList();
            }
            //call function to bind dropdown list
            BindCourseDropDown();
            return View(can);
        }

        private void BindCourseDropDown()
        {
           // dropdown list (course)
            var course = from c in _db.course
                         select c;
            List<SelectListItem> courseList = new List<SelectListItem>();
            foreach (var c in course)
            {
                courseList.Add(new SelectListItem { Text = c.Title, Value = c.CourseID.ToString() });
            }
            TempData["Courses"] = courseList;
        }

        [Authorize(Roles = "Admin, Candidate")]
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


        [Authorize(Roles = "Candidate")]
        [HttpGet]
        public IActionResult EditCandidate(long Id)
        {
            Candidate c = _db.candidate.Find(Id);

            populateCourseDropDownList(c.CourseID);
            populateUniDropDownList(c.UniversityID);
            return View(c);
        }

        //function to populate course/university dropdown list
        private void populateCourseDropDownList(object selectedCourse = null)
        {
            var c = from v in _db.course
                     select v;
            ViewBag.CourseID = new SelectList(c, "CourseID", "Title", selectedCourse);
        } 
        private void populateUniDropDownList(object selectedUni = null)
        {
            var u = from v in _db.university
                    select v;
            ViewBag.UniversityID = new SelectList(u, "UniversityID", "Name", selectedUni);
        }

        [Authorize(Roles = "Candidate")]
        [HttpPost]
        public IActionResult EditCandidate(Candidate c)
        {
            _db.Entry(c).State = EntityState.Modified;

            _db.SaveChanges();
            return RedirectToAction("ShowCandidates");
        }


        public IActionResult SendInvitation(long Id, Models.Email e)
        {
             string strKey = "";
           // string strKey = _config.GetValue<string>("SendGridKey");
            Candidate c = _db.candidate.Find(Id);
            //string toEmail = "jennyli84513@gmail.com";
            string toEmail = c.Email;
            string message = "";
            message += "Dear " + c.FirstName + " " + c.LastName + ",";
            message += "You are invited to the interview";
            message += "Regards";
            message += "Jenny";
            EmailOp emailOp = new EmailOp(toEmail, e.fromEmail, e.title,message, strKey);
            return View("EmailSent");
        }
    }
}
