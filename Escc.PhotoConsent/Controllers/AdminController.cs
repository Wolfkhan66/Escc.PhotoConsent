using Escc.PhotoConsent.Models.DataModels;
using Escc.PhotoConsent.Models.ViewModels;
using Escc.PhotoConsent.Services;
using Escc.PhotoConsent.Services.Interfaces;
using System;
using System.Data;
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
            var model = new ManageFormViewModel();
            var ConsentForms = _databaseService.GetConsentForms();

            model.Forms.Table = new DataTable();
            model.Forms.Table.Columns.Add("ID", typeof(string));
            model.Forms.Table.Columns.Add("Project Reference", typeof(string));
            model.Forms.Table.Columns.Add("Created By", typeof(string));
            model.Forms.Table.Columns.Add("Date Created", typeof(string));
            model.Forms.Table.Columns.Add("Consent", typeof(HtmlString));
            model.Forms.Table.Columns.Add("View", typeof(HtmlString));

            foreach (var Form in ConsentForms)
            {

                var Consent = Form.ConsentGiven ? new HtmlString("<span class=\"glyphicon glyphicon-ok text-success\" aria-hidden=\"true\"></span>") : new HtmlString("<span class=\"glyphicon glyphicon-remove text-danger\" aria-hidden=\"true\"></span></div>");
                var ViewLink = new HtmlString(string.Format("<a type=\"button\" class=\"btn btn-primary btn-sm\" href=\"ViewForm/{0}\">View</a>", Form.FormID));

                model.Forms.Table.Rows.Add(Form.FormID, Form.ProjectReference, Form.CreatedBy, Form.DateCreated, Consent, ViewLink);
            }
            

            return View(model);
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

        [HttpPost]
        public ActionResult CreateOfficer(CommissioningOfficerModel model)
        {
            _databaseService.InsertCommissioningOfficer(model);
            return RedirectToRoute("ViewForm", new { ID = model.FormID });
        }

        [HttpPost]
        public ActionResult CreateParticipant(ParticipantModel model)
        {
            _databaseService.InsertParticipant(model);
            return RedirectToRoute("ViewForm", new { ID = model.FormID });
        }
        [HttpPost]
        public ActionResult CreatePhotographer(PhotographerModel model)
        {
            _databaseService.InsertPhotographer(model);
            return RedirectToRoute("ViewForm", new { ID = model.FormID });
        }
    }
}