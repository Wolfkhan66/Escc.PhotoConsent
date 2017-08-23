using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Escc.PhotoConsent.Controllers
{
    public class FormController : Controller
    {
        // GET: Form
        public ActionResult Index(int FormId = 0)
        {

            return View();
        }
    }
}