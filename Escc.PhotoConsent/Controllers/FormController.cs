using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Escc.PhotoConsent.Models.ViewModels;
using Escc.PhotoConsent.Services.Interfaces;
using Escc.PhotoConsent.Services;
using Escc.PhotoConsent.Models.DataModels;

namespace Escc.PhotoConsent.Controllers
{
    public class FormController : Controller
    {
        private IDatabaseService _databaseService = new DatabaseService();

        [Route("ConsentForm/{formGuid}", Name = "ConsentForm")]
        public ActionResult ConsentForm(string formGuid  = "")
        {
            var ViewModel = new FormViewModel();
            if(formGuid != "")
            {
                var formID = _databaseService.GetFormIDByGuid(Guid.Parse(formGuid));
                ViewModel.Form = _databaseService.GetFormByID(formID);
                ViewModel.Officers = _databaseService.GetOfficersByFormID(formID);
                ViewModel.Participants = _databaseService.GetParticipantsByFormID(formID);
                ViewModel.Photographers = _databaseService.GetPhotographersByFormID(formID);
            }
            return View(ViewModel);
        }

        #region Create Methods
        [HttpPost]
        public ActionResult CreateOfficer(CommissioningOfficerModel model)
        {
            _databaseService.InsertCommissioningOfficer(model);
            return RedirectToRoute("ConsentForm", new { formGuid = model.FormGUID });
        }

        [HttpPost]
        public ActionResult CreateParticipant(ParticipantModel model)
        {
            _databaseService.InsertParticipant(model);
            return RedirectToRoute("ConsentForm", new { formGuid = model.FormGUID });
        }
        [HttpPost]
        public ActionResult CreatePhotographer(PhotographerModel model)
        {
            _databaseService.InsertPhotographer(model);
            return RedirectToRoute("ConsentForm", new { formGuid = model.FormGUID });
        }
        #endregion

        #region EditMethods
        [HttpPost]
        public ActionResult EditOfficer(CommissioningOfficerModel model)
        {
            _databaseService.UpdateCommissioningOfficer(model);
            return RedirectToRoute("ConsentForm", new { formGuid = model.FormGUID });
        }

        [HttpPost]
        public ActionResult EditParticipant(ParticipantModel model)
        {
            _databaseService.UpdateParticipant(model);
            return RedirectToRoute("ConsentForm", new { formGuid = model.FormGUID });
        }
        [HttpPost]
        public ActionResult EditPhotographer(PhotographerModel model)
        {
            _databaseService.UpdatePhotographer(model);
            return RedirectToRoute("ConsentForm", new { formGuid = model.FormGUID });
        }
        #endregion

        #region DeleteMethods
        [HttpPost]
        public ActionResult DeleteOfficer(CommissioningOfficerModel model)
        {
            _databaseService.DeleteCommissioningOfficer(model);
            return RedirectToRoute("ConsentForm", new { formGuid = model.FormGUID });
        }

        [HttpPost]
        public ActionResult DeleteParticipant(ParticipantModel model)
        {
            _databaseService.DeleteParticipant(model);
            return RedirectToRoute("ConsentForm", new { formGuid = model.FormGUID });
        }
        [HttpPost]
        public ActionResult DeletePhotographer(PhotographerModel model)
        {
            _databaseService.DeletePhotographer(model);
            return RedirectToRoute("ConsentForm", new { formGuid = model.FormGUID });
        }
        #endregion
    }
}