using DomaciZadatakMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace DomaciZadatakMVC.Controllers
{
    public class TasksController : Controller
    {
        GetDBEntities1 context = new GetDBEntities1();

        // GET: Tasks
        public ActionResult Create() 
        {
            var status = new List<string> { "New", "In progress", "Finished" };
            ViewBag.Status = new SelectList(status);

            var user = context.Users.ToList();
            ViewBag.User = new SelectList(user, "Username", "Name");

            var dev = context.Users.Where(u => u.Role == "Developer").ToList();

            ViewBag.Dev = new SelectList(dev, "Username", "Name");

            return View();
        }

        [HttpPost]
        public ActionResult Create(Task task)
        {
            if(ModelState.IsValid)
            {
                var dev = context.Tasks.ToList()
                    .Join(context.Users, t => t.UserId, u => u.Id, (t, u) => new { t.UserId, u.Role})
                    .Where(u => u.Role == "Developer" )
                    .GroupBy(t => t.UserId)
                    .Select(x => x.Count()).ToList();


                var nikola = context.Tasks.ToList()
                    .Where(t => t.UserId == 2)
                    .GroupBy(t => t.UserId)
                    .Select(x => x.Count(t => Convert.ToBoolean(t.UserId)));

                var janko = context.Tasks.ToList()
                    .Where(t => t.UserId == 3)
                    .GroupBy(t => t.UserId)
                    .Select(x => x.Count(t => Convert.ToBoolean(t.UserId)));


                if (task.UserId == 2)
                {
                    foreach (var n in nikola)
                    {
                            if (n <= 2)
                                context.Tasks.Add(task);
                        else
                        {
                            InfoAndError();
                            return View();
                        }
                    }
                }
                else if(task.UserId == 3)
                {
                    foreach(var j in janko)
                    {
                        if(j <= 2)
                            context.Tasks.Add(task);
                        else
                        {
                            InfoAndError();
                            return View();
                        }
                    }
                }
                else if(task.UserId == 4 || task.UserId == 5 || task.UserId == 1)
                            context.Tasks.Add(task);
                else
                {
                            InfoAndError();
                            return View();
                }
                    
                


                //context.Tasks.Add(task);
                int i = context.SaveChanges();

                if(i > 0)
                {
                    TempData["msg"] = "Task successfuly inserted";
                    ModelState.Clear();

                    if (User.IsInRole("Admin"))
                        return RedirectToAction("GetTasksAdmin", "Home");

                    return RedirectToAction("GetTasks", "Home");
                }
                else
                {
                    ViewBag.msg = "Task not inserted";
                    return View();
                }
            }

            return View();  
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);


            var task = context.Tasks.Find(id);

            //Project manager assign task only to developers
            ProjectManViewModel projectMan = new ProjectManViewModel();
            projectMan.Task = task;
            
            var dev = context.Users.Where(u => u.Role == "Developer").ToList();
            
            ViewBag.UserList = new SelectList(dev, "Username", "Name");

            var user = context.Users.ToList();

            ViewBag.UserAdmin = new SelectList(user, "Username", "Name");

            var status = new List<string> { "New", "In progress", "Finished" };
            ViewBag.Status = new SelectList(status);

            if (task == null)
                return HttpNotFound();

            return View(task);
        }

        [HttpPost]
        public ActionResult Edit(Task task)
        {
            if (ModelState.IsValid)
            {
                
                context.Entry(task).State = EntityState.Modified;

                if (context.SaveChanges() > 0)
                {
                    TempData["editMsg"] = "Data updated";
                    if(User.IsInRole("Admin"))
                        return RedirectToAction("GetTasksAdmin", "Home");

                    return RedirectToAction("GetTasks", "Home");
                    
                }
                else
                    ViewBag.msg = "Data not updated";
            }
            return View();
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var task = context.Tasks.Find(id);

            if (task == null)
                return HttpNotFound();

            return View(task);
        }

        [HttpPost]
        public ActionResult Delete(Task task)
        {
            context.Entry(task).State = EntityState.Deleted;

            if (context.SaveChanges() > 0)
            {
                TempData["editMsg"] = "Data deleted!";
                return RedirectToAction("GetTasks", "Home");
            }

            return View();
        }

        public void InfoAndError()
        {
            ViewBag.TaskError = "Developer already have 3 tasks";
            var status = new List<string> { "New", "In progress", "Finished" };
            ViewBag.Status = new SelectList(status);

            var user = context.Users.ToList();
            ViewBag.User = new SelectList(user, "Username", "Name");

            var dev1 = context.Users.Where(u => u.Role == "Developer").ToList();

            ViewBag.Dev = new SelectList(dev1, "Username", "Name");
        }
    }
}