using DomaciZadatakMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DomaciZadatakMVC.Controllers
{

    public class HomeController : Controller
    {
        GetDBEntities1 context = new GetDBEntities1();

        
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult GetUsers()
        {
            var users = context.Users.ToList();

            return View(users);
        }

        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult GetProjets()
        {
            var projects = context.Projects.ToList();

            AdminViewModel adminViewModel = new AdminViewModel();
            List<Project> projectsList = context.Projects.ToList();
            List<Task> taskList = context.Tasks.ToList();
           
            adminViewModel.Pro = projectsList;
            adminViewModel.TaskE = taskList;

            var average = context.Tasks.ToList()
                .GroupBy(t => t.ProjectId)
                .Select(a => a.Average(p => Convert.ToInt32(p.Progress))).ToList();


            adminViewModel.Avge = average;

            return View(adminViewModel);
        }


        [Authorize(Roles = "Project Manager, Developer")]
        public ActionResult GetTasks()
        {
            //var tasks = context.Tasks.ToList();
            //var userId = User.Identity.GetUserId();
            var tasks = context.Tasks.Where(t => t.Assignee == User.Identity.Name || t.Assignee == null).ToList();

            return View(tasks);
        }

        [Authorize(Roles = "Admin")] 
        public ActionResult GetTasksAdmin() 
        {
            var tasks = context.Tasks.ToList();

            return View(tasks);
        }

        
    }
}