using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SchoolSystem.Models;

namespace SchoolSystem.Controllers.ControllersMVC
{
    [Authorize(Roles = "Teacher")]
    public class GroupsController : Controller
    {
        private ControlContext db = new ControlContext();

        //Delete student from course
        public ActionResult DeleteStudent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var groupsDetails = db.GroupsDetails.Find(id);
            if (groupsDetails == null)
            {
                return HttpNotFound();
            }

            db.GroupsDetails.Remove(groupsDetails);
            db.SaveChanges();
            return RedirectToAction(string.Format("Details/{0}", groupsDetails.GroupId));
        }


        // Add student Post
        [HttpPost]
        public ActionResult AddStudent(GroupsDetails groupsDetails)
        {

            if (ModelState.IsValid)
            {
                var exist = db.GroupsDetails.Where(gd => gd.GroupId == groupsDetails.GroupId && gd.UserId == groupsDetails.UserId).FirstOrDefault();
                if (exist == null)
                {
                    db.GroupsDetails.Add(groupsDetails);
                    db.SaveChanges();
                    return RedirectToAction(string.Format("Details/{0}", groupsDetails.GroupId));
                }

                ModelState.AddModelError(string.Empty, "Student is registered already");
                
            }

            ViewBag.UserId = new SelectList(db.Users.Where(u => u.Student).OrderBy(u => u.Name).ThenBy(u => u.Surname), "UserId", "FullName", groupsDetails.UserId);

            return View(groupsDetails);
        }

        // Add student Get
        public ActionResult AddStudent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Groups groups = db.Groups.Find(id);
            if (groups == null)
            {
                return HttpNotFound();
            }

            var groupsDetails = new GroupsDetails
            {
                GroupId = id.Value,
            };

            ViewBag.UserId = new SelectList(db.Users.Where(u => u.Student).OrderBy(u => u.Name).ThenBy(u => u.Surname), "UserId", "FullName");

            return View(groupsDetails);
        }

        

        // GET: Groups
        public ActionResult Index()
        {
            var groups = db.Groups.Include(g => g.Teacher);
            return View(groups.ToList());
        }

        // GET: Groups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Groups groups = db.Groups.Find(id);
            if (groups == null)
            {
                return HttpNotFound();
            }
            return View(groups);
        }

        // GET: Groups/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users.Where(u => u.Teacher).OrderBy(u => u.Name).ThenBy(u => u.Surname), "UserId", "FullName");
            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GroupId,Description,UserId")] Groups groups)
        {
            if (ModelState.IsValid)
            {
                db.Groups.Add(groups);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users.Where(u => u.Teacher).OrderBy(u => u.Name).ThenBy(u => u.Surname), "UserId", "FullName");
            return View(groups);
        }

        // GET: Groups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Groups groups = db.Groups.Find(id);
            if (groups == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users.Where(u => u.Teacher).OrderBy(u => u.Name).ThenBy(u => u.Surname), "UserId", "FullName", groups.GroupId);

            return View(groups);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GroupId,Description,UserId")] Groups groups)
        {
            if (ModelState.IsValid)
            {
                db.Entry(groups).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users.Where(u => u.Teacher).OrderBy(u => u.Name).ThenBy(u => u.Surname), "UserId", "FullName", groups.GroupId);
            return View(groups);
        }

        // GET: Groups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Groups groups = db.Groups.Find(id);
            if (groups == null)
            {
                return HttpNotFound();
            }
            return View(groups);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Groups groups = db.Groups.Find(id);
            db.Groups.Remove(groups);
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
