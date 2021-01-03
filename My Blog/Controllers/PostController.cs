using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using My_Blog.Models;

namespace My_Blog.Controllers {
    public class PostController : Controller {
        
        public IActionResult Index() {
            return View();
        }
        public IActionResult All() {
            if (!HttpContext.Request.Cookies.ContainsKey("currentUser")) {
                return RedirectToAction("SignIn", "Home");
            }
            Posthelper posthelper = new Posthelper();
            List<Posts> list = posthelper.GetPosts();
            return View("AllPosts", list);
        }
        public IActionResult Users() {
            if (!HttpContext.Request.Cookies.ContainsKey("currentUser")) {
                return RedirectToAction("SignIn", "Home");
            }
            string username = HttpContext.Request.Cookies["currentUser"];
            Posthelper posthelper = new Posthelper();
            List<Posts> posts = posthelper.GetUserPosts(username);
            return View("UsersPosts", posts);
        }
        public IActionResult Create() {
            if (!HttpContext.Request.Cookies.ContainsKey("currentUser")) {
                return RedirectToAction("SignIn", "Home");
            }
            return View("Create");
        }
        [HttpPost]
        public IActionResult Create(Posts post) {
            string username = HttpContext.Request.Cookies["currentUser"];
            post.Owner = username;
            Posthelper posthelper = new Posthelper();
            if (posthelper.AddPost(post))
                return RedirectToAction("Users");
            else
                return View("Error");
        }
        public IActionResult Delete(string id) {
            string username = HttpContext.Request.Cookies["currentUser"];
            Posthelper posthelper = new Posthelper();
            if(posthelper.GetPostByID(id).Owner != username)  return View("Error");
            
            if (posthelper.DelPost(id))
                return RedirectToAction("Users");
            else
                return View("Error");
        }
        
        public IActionResult Edit(string id) {
            if (!HttpContext.Request.Cookies.ContainsKey("currentUser")) {
                return RedirectToAction("SignIn", "Home");
            }
            Posthelper posthelper = new Posthelper();
            Posts post = posthelper.GetPostByID(id);
            if (post == null) {
                return RedirectToAction("Profile", "Home");
            }
            return View("Edit", post);
        } 
        [HttpPost]
         public IActionResult Edit(Posts posts) {
            if (!HttpContext.Request.Cookies.ContainsKey("currentUser")) {
                return RedirectToAction("SignIn", "Home");
            }
            string username = HttpContext.Request.Cookies["currentUser"];
            if(username != posts.Owner) {
                return View("Error");
            }
            Posthelper posthelper = new Posthelper();
            if(!posthelper.EditPost(posts)) return View("Error");
            return RedirectToAction("Users");
        } 
        

    }
}
