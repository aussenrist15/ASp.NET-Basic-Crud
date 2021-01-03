using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using My_Blog.Models;

namespace My_Blog.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private UserHelper userHelper;

        public HomeController(ILogger<HomeController> logger) {
            _logger = logger;
            userHelper = new UserHelper();
        }

        public IActionResult Index() {
            
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(Users user) {
            if (userHelper.SignUpUser(user)) {
                HttpContext.Response.Cookies.Append("currentUser", user.Username);
                return RedirectToAction("Profile");
            }
            return View("Error");
        }

        
         public IActionResult Edit() {
            if (!HttpContext.Request.Cookies.ContainsKey("currentUser")) {
                return RedirectToAction("SignIn");
            }
            string username = HttpContext.Request.Cookies["currentUser"];
            Users authUser = userHelper.getUserByUsername(username);
            return View("EditProfile", authUser);
        }

        [HttpPost]
         public IActionResult Edit(Users user) {
            if (!HttpContext.Request.Cookies.ContainsKey("currentUser") || user == null) {
                return RedirectToAction("SignIn");
            }
            Users updatedUser = userHelper.UpdateUserProfile(user);
            return RedirectToAction("Profile");
        }
        public IActionResult SignIn() {
            
            return View("SignIn");
        }
        [HttpPost]
        public IActionResult SignIn(Users user) {
            if(userHelper.AuthenticateUser(user.Username, user.Password)!= null) {
                HttpContext.Response.Cookies.Append("currentUser", user.Username);
                return RedirectToAction("Profile");
            }
            return View("Error");
        }
         public IActionResult Profile() {
            if (!HttpContext.Request.Cookies.ContainsKey("currentUser")) {
                return RedirectToAction("SignIn");
            }
            string username = HttpContext.Request.Cookies["currentUser"];
            Users authUser = userHelper.getUserByUsername(username);
            return View("Profile", authUser);
        }
        public IActionResult Privacy() {
            return View();
        }
        public IActionResult Logout() {
            if (!HttpContext.Request.Cookies.ContainsKey("currentUser")) {
                return RedirectToAction("SignIn");
            }
            HttpContext.Response.Cookies.Delete("currentUser");
            return RedirectToAction("SignIn");
        }

    }
}
