using System.ComponentModel.DataAnnotations;

namespace CSIRO.Models
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
