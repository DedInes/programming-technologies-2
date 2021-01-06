using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SchoolSystem.Classes;
using SchoolSystem.Models;

namespace SchoolSystem.Controllers
{
    public class UsersController : Controller
    {
        private ControlContext db = new ControlContext();

        // GET: Users
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserView view)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(view.User);
                try
                {
                    
                    if(view.Photo != null)
                    {
                        var pic = Utilities.UploadPhoto(view.Photo);
                        if (!string.IsNullOrEmpty(pic))
                        {
                            view.User.Photo = string.Format("~/Content/Photos/{0}", pic);
                        }
                    }
                    db.SaveChanges();

                    Utilities.CreateUserASP(view.User.UserName);
                    if (view.User.Student)
                    {
                        Utilities.AddRoleToUser(view.User.UserName, "Student");
                    }

                    if (view.User.Teacher)
                    {
                        Utilities.AddRoleToUser(view.User.UserName, "Teacher");
                    }
                    
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(view);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var view = new UserView
            {
                User = user
            };

            return View(view);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserView view)
        {
            if (ModelState.IsValid)
            {
                var db2 = new ControlContext();
                var oldUser = db2.Users.Find(view.User.UserId);
                db2.Dispose();

                if (view.Photo != null)
                {
                    var pic = Utilities.UploadPhoto(view.Photo);
                    if (!string.IsNullOrEmpty(pic))
                    {
                        view.User.Photo = string.Format("~/Content/Photos/{0}", pic);
                    }
                }
                else
                {
                    view.User.Photo = oldUser.Photo;
                }

                

                db.Entry(view.User).State = EntityState.Modified;
                try
                {
                    if(oldUser != null && oldUser.UserName != view.User.UserName)
                    {
                        Utilities.ChangeEmailUserASP(oldUser.UserName, view.User.UserName);
                    }
                    db.SaveChanges();
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(view);
                }
                return RedirectToAction("Index");
            }
            return View(view.User);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
