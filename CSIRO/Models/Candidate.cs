using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSIRO.Models
{
    public class Candidate
    {
       
        public long CandidateID { get; set; }

        [Required(ErrorMessage = "Enter first name")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Enter last name")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Enter email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter Phone")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Enter GPA greater than 3.0")]
        [Range(minimum:3.0, maximum:5.0)]
        public double GPA { get; set; }

        [Required(ErrorMessage = "Enter Your cover letter")]
        [Display(Name = "Cover Letter")]
        [DataType(DataType.MultilineText)]
        public string CoverLetter { get; set; }

     

        [Display(Name = "Course Title")]
        public long CourseID { get; set; }

        [Display(Name = "University")]
        public long UniversityID { get; set; }


        [NotMapped]
        [Display(Name = "Course Title")]
        public string CourseTitle { get; set; }
        [NotMapped]
        [Display(Name = "University")]
        public string University { get; set; }
        [NotMapped]
        public string Name;
       
        [NotMapped]
        public List<SelectListItem> courseList = new List<SelectListItem>();

        [NotMapped]
        public List<SelectListItem> uniList = new List<SelectListItem>();


    }

}
