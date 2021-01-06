using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SchoolSystem.Models;

namespace SchoolSystem.Controllers.ControllersAPI
{
    [RoutePrefix("API/Groups")]

    public class GroupsController : ApiController
    {
        private ControlContext db = new ControlContext();

        //Capture grades
        [HttpGet]
        [Route("GateStudents/{groupId}")]
        public IHttpActionResult GateStudents(int groupId)
        {
            var students = db.GroupsDetails.Where(gd => gd.GroupId == groupId).ToList();
            var myStudents = new List<object>();
            foreach(var student in students)
            {
                var myStudent = db.Users.Find(student.UserId);
                myStudents.Add(new
                {
                    GroupsDetails = student.GroupsDetailsId,
                    Groups = student.GroupId,
                    Student = myStudent
                });
            }

            return Ok(myStudents);
        }

        //Get grades
        [HttpGet]
        [Route("GateGrades/{groupId}/{userId}")]
        public IHttpActionResult GateGrades(int groupId, int userId)
        {
            var finalGrade = 0.0;
            var grades = db.GroupsDetails.Where(gd => gd.GroupId == groupId && gd.UserId == userId).ToList();
            foreach(var grade in grades)
            {
                foreach(var grade2 in grade.Grades)
                {
                    finalGrade += grade2.Percentage + grade2.Grade;
                }
            }

            return Ok<object>(new { Grades = finalGrade });
        }

        //Method for grades
        [HttpPost]
        [Route("SaveGrades")]
        public IHttpActionResult SaveGrades(JObject form)
        {
            var myStudentResponse = JsonConvert.DeserializeObject < MyStudentResponse > (form.ToString());
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    foreach(var student in myStudentResponse.Student)
                    {
                        var grade = new Grades
                        {
                            GroupsDetailsId = student.GroupsDetailsId,
                            Percentage = (float)myStudentResponse.Percentage,
                            Grade = (float)student.Grade
                        };

                        db.Grades.Add(grade);
                    }

                    db.SaveChanges();
                    transaction.Commit();

                    return Ok(true);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }

            
        }

        //GET Personalized
        [Route("GetGroups/{userId}")]
        public IHttpActionResult GetGroups(int userId)
        {
            var groups = db.Groups.Where(g => g.UserId == userId).ToList();
            var objects = db.GroupsDetails.Where(gd => gd.UserId == userId).ToList();
            var subjects = new List<Groups>();

            foreach(var objecte in objects)
            {
                subjects.Add(db.Groups.Find(objecte.GroupId));
            }

            var answer = new
            {
                SubjectTeacher = groups,
                RegisteredIn = objects
            };

            return Ok(answer);
        }

            // GET: api/Groups
            public IQueryable<Groups> GetGroups()
        {
            return db.Groups;
        }

      

        // PUT: api/Groups/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutGroups(int id, Groups groups)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != groups.GroupId)
            {
                return BadRequest();
            }

            db.Entry(groups).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Groups
        [ResponseType(typeof(Groups))]
        public IHttpActionResult PostGroups(Groups groups)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Groups.Add(groups);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = groups.GroupId }, groups);
        }

        // DELETE: api/Groups/5
        [ResponseType(typeof(Groups))]
        public IHttpActionResult DeleteGroups(int id)
        {
            Groups groups = db.Groups.Find(id);
            if (groups == null)
            {
                return NotFound();
            }

            db.Groups.Remove(groups);
            db.SaveChanges();

            return Ok(groups);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GroupsExists(int id)
        {
            return db.Groups.Count(e => e.GroupId == id) > 0;
        }
    }
}