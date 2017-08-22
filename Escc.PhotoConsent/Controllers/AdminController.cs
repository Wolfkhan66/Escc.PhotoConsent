using Escc.PhotoConsent.Models.DataModels;
using Escc.PhotoConsent.Services;
using Escc.PhotoConsent.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Escc.PhotoConsent.Controllers
{
    public class AdminController : Controller
    {
        private IDatabaseService _databaseService = new DatabaseService();
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
        public ActionResult CreateForm(ConsentFormModel model)
        {
            model.DateCreated = DateTime.Now;
            model.ConsentGiven = false;
            _databaseService.InsertConsentForm(model);
            return View("ViewForms", model);
        }
    }
}