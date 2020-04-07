using DomaciZadatakMVC.Models;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace DomaciZadatakMVC.Controllers
{
    public class AccountController : Controller
    {
        GetDBEntities1 context = new GetDBEntities1();

        // GET: Account
        public ActionResult Singup() 
        {
            return View();
        }

        [HttpPost]
        public ActionResult Singup(User user)
        {
            if (ModelState.IsValid)
            {
                context.Users.Add(user);
                if (context.SaveChanges() > 0)
                    return RedirectToAction("Login");
            }

            return View();
        }

        public ActionResult Login()
        {


            return View();
        }

        [HttpPost]
        public ActionResult Login(User user, string ReturnUrl)
        {
            if(ModelState.IsValid)
            {
                var users = context.Users
                    .Where(x => x.Username == user.Username && x.Password == user.Password)
                    .FirstOrDefault();
                

                if(users != null)
                {
                    FormsAuthentication.SetAuthCookie(user.Username, false);
                    Session["uname"] = user.Username.ToString();

                    if (ReturnUrl != null)
                        return Redirect(ReturnUrl);
                    else
                        return Redirect("/Home/Index"); 
                }
                else
                {
                    ViewBag.ErrorMessage = "Username or Password is incorrect!";
                    return View(user);
                }
            }

            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session["uname"] = null;

            return Redirect("Login");
        }

    }
}