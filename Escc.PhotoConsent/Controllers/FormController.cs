using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Escc.PhotoConsent.Controllers
{
    public class FormController : Controller
    {

        [Route("ConsentForm/{formGuid}", Name = "ConsentForm")]
        public ActionResult ConsentForm(string formGuid  = "")
        {

            return View();
        }
    }
}