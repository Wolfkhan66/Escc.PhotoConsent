using Escc.PhotoConsent.Models.DataModels;
using Escc.PhotoConsent.Models.ViewModels;
using Escc.PhotoConsent.Services;
using Escc.PhotoConsent.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
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
        public ActionResult ManageForms(List<ConsentFormModel> Forms)
        {
            var model = new ManageFormViewModel();
            if (Forms == null)
            {
                Forms = _databaseService.GetConsentForms();
            }
            model.Forms.Table = PrepareFormsTable(Forms);

            return View("ManageForms", model);
        }

        public DataTable PrepareFormsTable(List<ConsentFormModel> Forms)
        {
            var Table = new DataTable();
            Table.Columns.Add("ID", typeof(string));
            Table.Columns.Add("Project Reference", typeof(string));
            Table.Columns.Add("Created By", typeof(string));
            Table.Columns.Add("Date Created", typeof(string));
            Table.Columns.Add("Consent", typeof(HtmlString));
            Table.Columns.Add("View", typeof(HtmlString));

            foreach (var Form in Forms)
            {
                var Consent = Form.ConsentGiven ? new HtmlString("<span class=\"glyphicon glyphicon-ok text-success\" aria-hidden=\"true\"></span>") : new HtmlString("<span class=\"glyphicon glyphicon-remove text-danger\" aria-hidden=\"true\"></span></div>");
                var ViewLink = new HtmlString(string.Format("<a type=\"button\" class=\"btn btn-primary btn-sm\" href=\"ViewForm/{0}\">View</a>", Form.FormID));
                Table.Rows.Add(Form.FormID, Form.ProjectReference, Form.CreatedBy, Form.DateCreated, Consent, ViewLink);
            }
            return Table;
        }

        public ActionResult SearchForms(string ProjectReference, string CreatedBy, string DateCreated, string Consent)
        {
            var ConsentForms = _databaseService.GetConsentForms();

            if (ProjectReference != "")
            {
                ConsentForms = ConsentForms.Where(x => x.ProjectReference.ToLower() == ProjectReference.ToLower()).ToList();
            }
            if (CreatedBy != "")
            {
                ConsentForms = ConsentForms.Where(x => x.CreatedBy.ToLower() == CreatedBy.ToLower()).ToList();
            }
            if (DateCreated != "")
            {
                ConsentForms = ConsentForms.Where(x => x.DateCreated.ToString().Split(' ')[0] == DateCreated).ToList();
            }
            if (Consent != "" || Consent != "Both")
            {
                bool consentGiven = Consent.ToLower() == "true" ? true : false;
                ConsentForms = ConsentForms.Where(x => x.ConsentGiven == consentGiven).ToList();
            }
           return ManageForms(ConsentForms);
        }

        [HttpPost]
        public ActionResult CreateForm(ConsentFormModel model)
        {
            model.DateCreated = string.Format("{0} {1}:{2}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), DateTime.Now.Second);
            model.ConsentGiven = false;
            model.GUID = Guid.NewGuid();
            _databaseService.InsertConsentForm(model);
            var FormID = _databaseService.GetFormIDByGuid(model.GUID);
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