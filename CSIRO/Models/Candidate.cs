using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSIRO.Models
{
    public class Candidate
    {
       
        public int CandidateID { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Email { get; set; }
        
        public string Phone { get; set; }

        public double GPA { get; set; }

        [Display(Name = "Cover Letter")]
        [DataType(DataType.MultilineText)]
        public string CoverLetter { get; set; }

        [Display(Name = "Course Title")]
        public int CourseID { get; set; }

        [Display(Name = "University")]
        public int UniversityID { get; set; }

        [NotMapped]
        public string CourseTitle { get; set; }
        [NotMapped]
        public string University { get; set; }
        [NotMapped]
        public string Name { get; set; }

        [NotMapped]
        public List<SelectListItem> courseList = new List<SelectListItem>();

        [NotMapped]
        public List<SelectListItem> uniList = new List<SelectListItem>();


    }

}
