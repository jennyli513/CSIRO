using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSIRO.Models
{
    public class Course
    {
        public long CourseID { get; set; }
        public string Title { get; set; }

        [NotMapped]
        public List<SelectListItem> cList = new List<SelectListItem>();

    }

}
