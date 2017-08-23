using Escc.PhotoConsent.Models.DataModels;
using Escc.PhotoConsent.Models.ViewModels;
using Escc.PhotoConsent.Services;
using Escc.PhotoConsent.Services.Interfaces;
using System;
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

        [Route("ViewForm/{ID}", Name = "ViewForm")]
        public ActionResult ViewForm(int ID)
        {
            var ViewModel = new FormViewModel();
            ViewModel.Form = _databaseService.GetFormByID(ID);
            ViewModel.Officers = _databaseService.GetOfficersByFormID(ID);
            ViewModel.Participants = _databaseService.GetParticipantsByFormID(ID);
            ViewModel.Photographers = _databaseService.GetPhotographersByFormID(ID);
            return View(ViewModel);
        }

        [Route("ManageForms", Name = "ManageForms")]
        public ActionResult ManageForms()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateForm(ConsentFormModel model)
        {
            model.DateCreated = string.Format("{0} {1}:{2}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), DateTime.Now.Second);
            model.ConsentGiven = false;
            _databaseService.InsertConsentForm(model);
            var FormID = _databaseService.GetFormIDAfterCreation(model.ProjectReference, model.DateCreated, model.CreatedBy);
            return RedirectToRoute("ViewForm", new { ID = FormID });
        }
    }
}