using Escc.PhotoConsent.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Escc.PhotoConsent.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Route("ViewForms", Name = "ViewForms")]
        public ActionResult ViewForms()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateForm(AdminCreateFormModel model)
        {
            return View(model);
        }
    }
}