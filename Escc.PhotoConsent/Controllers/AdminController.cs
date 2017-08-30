using Escc.PhotoConsent.Models.DataModels;
using Escc.PhotoConsent.Models.ViewModels;
using Escc.PhotoConsent.Services;
using Escc.PhotoConsent.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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

            ViewModel.AllCommOfficers = _databaseService.GetOfficers().GroupBy(x => x.Name).Select(x => x.First()).ToDictionary(x => x.OfficerID, x => x.Name);
            ViewModel.AllPhotographers = _databaseService.GetPhotographers().GroupBy(x => x.Name).Select(x => x.First()).ToDictionary(x => x.PhotographerID, x => x.Name);

            var Photos = new List<PhotoModel>();
            foreach (var Particpant in ViewModel.Participants)
            {
                Photos.Add(_databaseService.GetPhotosByParticipantID(Particpant.ParticipantID).FirstOrDefault());
            }

            foreach (var Photo in Photos)
            {
                if(Photo != null)
                {
                    var Base64 = "data:image/png;base64," + Convert.ToBase64String(Photo.Image, 0, Photo.Image.Length);
                    ViewModel.Participants.Single(x => x.ParticipantID == Photo.ParticipantID).Base64Image = Base64;
                }
            }

            return View(ViewModel);
        }

        [Route("ManageForms", Name = "ManageForms")]
        public ActionResult ManageForms(List<ConsentFormModel> Forms)
        {
            var model = new ManageFormViewModel();
            if (Forms == null)
            {
                Forms = _databaseService.GetConsentForms().Where(x => x.Deleted == false).ToList();
            }
            model.Forms.Table = PrepareFormsTable(Forms);

            return View("ManageForms", model);
        }

        public DataTable PrepareFormsTable(List<ConsentFormModel> Forms)
        {
            var Table = new DataTable();
            Table.Columns.Add("ID", typeof(string));
            Table.Columns.Add("Project Name", typeof(string));
            Table.Columns.Add("Created By", typeof(string));
            Table.Columns.Add("Paymo Number", typeof(string));
            Table.Columns.Add("Date Created", typeof(string));
            Table.Columns.Add("Consent", typeof(HtmlString));
            Table.Columns.Add("View", typeof(HtmlString));

            foreach (var Form in Forms)
            {
                var Consent = Form.ConsentGiven ? new HtmlString("<span class=\"glyphicon glyphicon-ok text-success\" aria-hidden=\"true\"></span>") : new HtmlString("<span class=\"glyphicon glyphicon-remove text-danger\" aria-hidden=\"true\"></span></div>");
                var ViewLink = new HtmlString(string.Format("<a type=\"button\" class=\"btn btn-primary btn-sm\" href=\"ViewForm/{0}\">View</a>", Form.FormID));
                Table.Rows.Add(Form.FormID, Form.ProjectName, Form.CreatedBy, Form.PaymoNumber, Form.DateCreated, Consent, ViewLink);
            }
            return Table;
        }

        [HttpPost]
        [Route("SearchForms", Name = "SearchForms")]
        public ActionResult SearchForms(string ProjectName, string CreatedBy, string DateCreated, string Consent, string PaymoNumber)
        {
            var ConsentForms = _databaseService.GetConsentForms().Where(x => x.Deleted == false).ToList();

            if (ProjectName != "")
            {
                ConsentForms = ConsentForms.Where(x => x.ProjectName.ToLower() == ProjectName.ToLower()).ToList();
            }
            if (CreatedBy != "")
            {
                ConsentForms = ConsentForms.Where(x => x.CreatedBy.ToLower() == CreatedBy.ToLower()).ToList();
            }
            if (DateCreated != "")
            {
                ConsentForms = ConsentForms.Where(x => x.DateCreated.ToString().Split(' ')[0] == DateCreated).ToList();
            }
            if (PaymoNumber != "")
            {
                ConsentForms = ConsentForms.Where(x => x.PaymoNumber.ToLower() == PaymoNumber.ToLower()).ToList();
            }
            if (Consent != "" && Consent != "Both")
            {
                bool consentGiven = Consent.ToLower() == "true" ? true : false;
                ConsentForms = ConsentForms.Where(x => x.ConsentGiven == consentGiven).ToList();
            }
           return ManageForms(ConsentForms);
        }

        #region CreateMethods
        [HttpPost]
        public ActionResult CreateForm(ConsentFormModel model)
        {
            model.DateCreated = string.Format("{0} {1}:{2}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), DateTime.Now.Second);
            model.ConsentGiven = false;
            model.GUID = Guid.NewGuid();
            model.Deleted = false;
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
        public ActionResult CreateExistingOfficer(int FormID, int OfficerID)
        {
            var model = _databaseService.GetOfficerByID(OfficerID);
            model.FormID = FormID;
            _databaseService.InsertCommissioningOfficer(model);
            return RedirectToRoute("ViewForm", new { ID = FormID });
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

        [HttpPost]
        public ActionResult CreateExistingPhotographer(int FormID, int PhotographerID)
        {
            var model = _databaseService.GetPhotographerByID(PhotographerID);
            model.FormID = FormID;
            _databaseService.InsertPhotographer(model);
            return RedirectToRoute("ViewForm", new { ID = FormID });
        }

        [HttpPost]
        public ActionResult UploadPhoto(HttpPostedFileBase Image, int ParticipantID, int FormID)
        {
            var model = new PhotoModel();
            model.ParticipantID = ParticipantID;
            byte[] imageByte = null;
            BinaryReader rdr = new BinaryReader(Image.InputStream);
            imageByte = rdr.ReadBytes((int)Image.ContentLength);
            model.Image = imageByte;

            var photo = _databaseService.GetPhotosByParticipantID(ParticipantID);
            if(photo.Count != 0)
            {
                _databaseService.DeletePhoto(ParticipantID);
            }

            _databaseService.InsertPhoto(model);
            return RedirectToRoute("ViewForm", new { ID = FormID });
        }
        #endregion

        #region EditMethods
        [HttpPost]
        public ActionResult EditForm(ConsentFormModel model)
        {
            _databaseService.UpdateConsentForm(model);
            return RedirectToRoute("ViewForm", new { ID = model.FormID });
        }

        [HttpPost]
        public ActionResult EditOfficer(CommissioningOfficerModel model)
        {
            _databaseService.UpdateCommissioningOfficer(model);
            return RedirectToRoute("ViewForm", new { ID = model.FormID });
        }

        [HttpPost]
        public ActionResult EditParticipant(ParticipantModel model)
        {
            _databaseService.UpdateParticipant(model);
            return RedirectToRoute("ViewForm", new { ID = model.FormID });
        }
        [HttpPost]
        public ActionResult EditPhotographer(PhotographerModel model)
        {
            _databaseService.UpdatePhotographer(model);
            return RedirectToRoute("ViewForm", new { ID = model.FormID });
        }
        #endregion

        #region DeleteMethods
        [HttpPost]
        public ActionResult DeleteForm(ConsentFormModel model)
        {
            model.Deleted = true;
            _databaseService.DeleteConsentForm(model);
            return RedirectToRoute("ManageForms");
        }

        [HttpPost]
        public ActionResult DeleteOfficer(CommissioningOfficerModel model)
        {
            _databaseService.DeleteCommissioningOfficer(model);
            return RedirectToRoute("ViewForm", new { ID = model.FormID });
        }

        [HttpPost]
        public ActionResult DeleteParticipant(ParticipantModel model)
        {
            _databaseService.DeleteParticipant(model);
            return RedirectToRoute("ViewForm", new { ID = model.FormID });
        }
        [HttpPost]
        public ActionResult DeletePhotographer(PhotographerModel model)
        {
            _databaseService.DeletePhotographer(model);
            return RedirectToRoute("ViewForm", new { ID = model.FormID });
        }
        #endregion
    }
}