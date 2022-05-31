using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CSIRO.Models
{
    public class ManageRole
    {
        public string userID { get; set; }
        public string roleID { get; set; }

        [Display(Name = "Users")]
        public List<SelectListItem> userList { get; set; }

        [Display(Name = "Roles")]
        public List<SelectListItem> roleList { get; set; }
    }
}
