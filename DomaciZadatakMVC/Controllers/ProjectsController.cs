using DomaciZadatakMVC.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace DomaciZadatakMVC.Controllers
{
    public class ProjectsController : Controller
    {
        GetDBEntities1 context = new GetDBEntities1();

        // GET: Projects
        public ActionResult Create()
        {
            var user = context.Users.Where(u => u.Role == "Project Manager");
            ViewBag.Manager = new SelectList(user, "Username", "Name");

            return View();
        }

        [HttpPost]
        public ActionResult Create(Project project)
        {
            if (ModelState.IsValid)
            {

                context.Projects.Add(project);
               

                int i = context.SaveChanges();

                if (i > 0)
                {
                    TempData["msg"] = "Project successfuly inserted";
                    ModelState.Clear();
                    return RedirectToAction("GetProjets", "Home");
                }
                else
                {
                    ViewBag.msg = "Project not inserted";
                    return View();
                }
            }
            return View();
        }

        //Projett Manager
        public List<Project> GetProjects()
        {
            return context.Projects.ToList();
        }

        public List<User> GetUsers()
        {
            return context.Users.ToList();
        }

        
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);


            var project = context.Projects.Find(id);

            var manager = context.Users.Where(u => u.Role == "Project Manager").ToList();

            ViewBag.Manager = new SelectList(manager, "Username", "Name");


            if (project == null)
                return HttpNotFound();

            return View(project);
        }

        [HttpPost]
        public ActionResult Edit(Project project)
        {
            if (ModelState.IsValid)
            {
                context.Entry(project).State = EntityState.Modified;
                if (context.SaveChanges() > 0)
                {
                    TempData["editMsg"] = "Data updated";
                    return RedirectToAction("GetProjets", "Home");
                }
                else
                    ViewBag.msg = "Data not updated";
            }

            return View(project);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var project = context.Projects.Find(id);

            if (project == null)
                return HttpNotFound();

            return View(project);
        }

        [HttpPost]
        public ActionResult Delete(Project project, Task task)
        {

            context.Entry(project).State = EntityState.Deleted;

            if (context.SaveChanges() > 0)
            {
                TempData["editMsg"] = "Data deleted!";
                return RedirectToAction("GetProjets", "Home");
            }

            //using (var context = new GetDBEntities1())
            //{
            //    var parentObject = context.Projects.Find(1);
            //    parentObject.Tasks.Clear();
            //    context.SaveChanges();
            //}


            return View();
        }
    }
}