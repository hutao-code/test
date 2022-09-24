using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            Console.log("hutao1");
            Console.log("hutao2");
            Console.log("hutao3");
            Console.log("hutao4");
            Console.log("hutao5");
            return View();
        }
    }
}