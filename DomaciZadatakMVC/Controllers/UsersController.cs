using DomaciZadatakMVC.Models;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;

namespace DomaciZadatakMVC.Controllers
{
    public class UsersController : Controller
    {
        GetDBEntities1 context = new GetDBEntities1();

        // GET: Users
        public ActionResult Create()
        { 
            return View();
        }

        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {


                context.Users.Add(user);
                int i = context.SaveChanges();

                if (i > 0)
                {
                    TempData["msg"] = "User successfuly inserted"; 
                    ModelState.Clear();
                    return RedirectToAction("GetUsers","Home");
                }
                else
                {
                    ViewBag.msg = "User not inserted";
                    return View();
                }
            }
            return View();
        }

        public ActionResult Edit(int? id)
        {
            if(id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            

            var user = context.Users.Find(id);

            if(user == null)
                return HttpNotFound();

            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(User user)
        {
            if(ModelState.IsValid)
            {
                context.Entry(user).State = EntityState.Modified;
                if (context.SaveChanges() > 0)
                {
                    TempData["editMsg"] = "Data updated";
                    return RedirectToAction("GetUsers", "Home");
                }
                else
                    ViewBag.msg = "Data not updated";
            }

            return View(user);
        }

        public ActionResult Delete(int? id)
        {
            if(id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = context.Users.Find(id);

            if (user == null)
                return HttpNotFound();

            return View(user);
        }

        [HttpPost]
        public ActionResult Delete(User user)
        {
            context.Entry(user).State = EntityState.Deleted;

            if(context.SaveChanges() > 0)
            {
                TempData["editMsg"] = "Data deleted!";
                return RedirectToAction("GetUsers", "Home");
            }

            return View();
        }
    }
}