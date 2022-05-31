using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CSIRO.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace CSIRO.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private RoleManager<IdentityRole> roleManager { get; }
        private UserManager<IdentityUser> userManager { get; }
        List<string> userList;
        public AdminController(RoleManager<IdentityRole> _roleManager, UserManager<IdentityUser> _userManager)
        {
            this.roleManager = _roleManager;
            this.userManager = _userManager;
            userList = new List<string>();

            var users = userManager.Users;
            //Email as name
            foreach (var user in users) userList.Add(user.Email);

        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View(new CreateRoleViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel m)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole { Name = m.RoleName };
                IdentityResult result = await roleManager.CreateAsync(role);
                if (result.Succeeded) return View("Display", roleManager.Roles);
                foreach (IdentityError e in result.Errors)
                {
                    ModelState.AddModelError("", e.Description);
                }
            }
            return View("Display", roleManager.Roles);
        }

        public IActionResult Display()
        {
            if (HttpContext.Session.GetString("userId") == null) RedirectToAction("Login", "Account");

            return View("Display", roleManager.Roles);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            IdentityResult result = await roleManager.DeleteAsync(role);
            return View("Display", roleManager.Roles);
        }

        [HttpGet]
        public IActionResult ManageRole()
        {
            ManageRole mr = new ManageRole();
            FillArray(mr);
            return View(mr);
        }
        [HttpPost]
        public async Task<IActionResult> ManageRole(ManageRole mr)
        {
            var role = await roleManager.FindByIdAsync(mr.roleID);
            var user = await userManager.FindByIdAsync(mr.userID);

            if (role == null || user == null) return View("Error");
            if (!(await userManager.IsInRoleAsync(user, role.Name)))
            {
                var result = await userManager.AddToRoleAsync(user, role.Name);
            }

            return View("Display", roleManager.Roles);
        }

        private void FillArray(ManageRole mr)
        {
            var users = userManager.Users;
            mr.userList = new List<SelectListItem>();
            foreach (var user in users)
            {
                SelectListItem item = new SelectListItem();
                item.Text = user.Email;
                item.Value = user.Id;

                mr.userList.Add(item);
            }

            var roles = roleManager.Roles;
            mr.roleList = new List<SelectListItem>();
            foreach (var role in roles)
            {
                SelectListItem item = new SelectListItem();
                item.Text = role.Name;
                item.Value = role.Id;

                mr.roleList.Add(item);
            }
        }

        //find role by username=email
        [HttpGet]
        public async Task<IActionResult> GetRole()
        {
            foreach (var email in userList)
            {
                var u = await userManager.FindByEmailAsync(email);
                var r = await userManager.GetRolesAsync(u);
            }
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
