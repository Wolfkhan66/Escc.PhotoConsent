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
        #region Initilization and Index
        private IDatabaseService _databaseService = new DatabaseService();
        [CustomAuthorize]
        public ActionResult Index()
        {
            // In case this is the first time the application has been set up and the database is new.
            // Ensure the master admin form is present.
            var AdminForm = _databaseService.GetFormByID(1);
            if (AdminForm == null)
            {
                AdminForm = new ConsentFormModel();
                AdminForm.Notes = "[DO NOT DELETE]";
                AdminForm.ProjectName = "Master Form";
                AdminForm.CreatedBy = "Admin";
                AdminForm.GUID = Guid.Parse("00000000-0000-0000-0000-000000000000");
                AdminForm.Deleted = true;
                AdminForm.DateCreated = string.Format("{0} {1}:{2}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), DateTime.Now.Second);
                AdminForm.ConsentGiven = false;

                _databaseService.InsertConsentForm(AdminForm);
            }
            return View();
        }
        #endregion

        #region View ActionResults
        [CustomAuthorize]
        [Route("ViewForm/{ID}", Name = "ViewForm")]
        public ActionResult ViewForm(int ID)
        {
            var ViewModel = new FormViewModel();
            ViewModel.Form = _databaseService.GetFormByID(ID);
            ViewModel.Officers = _databaseService.GetOfficersByFormID(ID);
            ViewModel.Participants = _databaseService.GetParticipantsByFormID(ID);
            ViewModel.Photographers = _databaseService.GetPhotographersByFormID(ID);

            ViewModel.AllCommOfficers = _databaseService.GetOfficers().Where(x => x.FormID == 1).ToDictionary(x => x.OfficerID, x => x.Name);
            ViewModel.AllPhotographers = _databaseService.GetPhotographers().Where(x => x.FormID == 1).ToDictionary(x => x.PhotographerID, x => x.Name);

            var Photos = new List<PhotoModel>();
            foreach (var Particpant in ViewModel.Participants)
            {
                Photos.Add(_databaseService.GetPhotosByParticipantID(Particpant.ParticipantID).FirstOrDefault());
            }

            foreach (var Photo in Photos)
            {
                if (Photo != null)
                {
                    var Base64 = "data:image/png;base64," + Convert.ToBase64String(Photo.Image, 0, Photo.Image.Length);
                    ViewModel.Participants.Single(x => x.ParticipantID == Photo.ParticipantID).Base64Image = Base64;
                }
            }

            return View(ViewModel);
        }

        [CustomAuthorize]
        [Route("ViewPhotographer/{ID}", Name = "ViewPhotographer")]
        public ActionResult ViewPhotographer(int ID)
        {
            var model = new PhotographerViewModel();
            model.Photographer = _databaseService.GetPhotographerByID(ID);

            var PhotographerInstances = _databaseService.GetPhotographers().Where(x => x.Name == model.Photographer.Name && x.Email == model.Photographer.Email && x.FormID != 1 && x.ContactNumber == model.Photographer.ContactNumber);

            var Forms = new List<ConsentFormModel>();
            foreach (var Instance in PhotographerInstances)
            {
                Forms.Add(_databaseService.GetFormByID(Instance.FormID));
            }

            model.Forms.Table.Columns.Add("View", typeof(HtmlString));
            model.Forms.Table.Columns.Add("Project Name", typeof(string));
            model.Forms.Table.Columns.Add("Date Created", typeof(string));
            foreach (var Form in Forms)
            {
                var ViewLink = new HtmlString(string.Format("<a type=\"button\" class=\"btn btn-primary btn-sm\" href=\"../ViewForm/{0}\">View</a>", Form.FormID));
                model.Forms.Table.Rows.Add(ViewLink, Form.ProjectName, Form.DateCreated);
            }

            return View(model);
        }

        [CustomAuthorize]
        [Route("ViewCommOfficer/{ID}", Name = "ViewCommOfficer")]
        public ActionResult ViewCommOfficer(int ID)
        {
            var model = new CommOfficerViewModel();
            model.Officer = _databaseService.GetOfficerByID(ID);

            var OfficerInstances = _databaseService.GetOfficers().Where(x => x.Name == model.Officer.Name && x.Email == model.Officer.Email && x.FormID != 1 && x.ContactNumber == model.Officer.ContactNumber);

            var Forms = new List<ConsentFormModel>();
            foreach (var Instance in OfficerInstances)
            {
                Forms.Add(_databaseService.GetFormByID(Instance.FormID));
            }

            model.Forms.Table.Columns.Add("View", typeof(HtmlString));
            model.Forms.Table.Columns.Add("Project Name", typeof(string));
            model.Forms.Table.Columns.Add("Date Created", typeof(string));
            foreach (var Form in Forms)
            {
                var ViewLink = new HtmlString(string.Format("<a type=\"button\" class=\"btn btn-primary btn-sm\" href=\"../ViewForm/{0}\">View</a>", Form.FormID));
                model.Forms.Table.Rows.Add(ViewLink, Form.ProjectName, Form.DateCreated);
            }

            return View(model);
        }
        #endregion

        #region Management ActionResults
        [CustomAuthorize]
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

        [CustomAuthorize]
        [Route("ManagePhotographers", Name = "ManagePhotographers")]
        public ActionResult ManagePhotographers()
        {
            var model = new ManagePhotographerViewModel();
            var photographers = _databaseService.GetPhotographers().Where(x => x.FormID == 1).ToList();
            model.Photographers.Table = PreparePhotographersTable(photographers);
            return View("ManagePhotographers", model);
        }

        [CustomAuthorize]
        [Route("ManageCommOfficers", Name = "ManageCommOfficers")]
        public ActionResult ManageCommOfficers()
        {
            var model = new ManageCommOfficerViewModel();
            var officers = _databaseService.GetOfficers().Where(x => x.FormID == 1).ToList();
            model.Officers.Table = PrepareCommOfficersTable(officers);
            return View("ManageCommOfficers", model);
        }

        [CustomAuthorize]
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
        #endregion

        #region Prepare DataTable Methods
        public DataTable PrepareFormsTable(List<ConsentFormModel> Forms)
        {
            var Table = new DataTable();
            Table.Columns.Add("View", typeof(HtmlString));
            Table.Columns.Add("Date Created", typeof(string));
            Table.Columns.Add("Project Name", typeof(string));
            Table.Columns.Add("Consent", typeof(HtmlString));
            Table.Columns.Add("Paymo Number", typeof(string));
            Table.Columns.Add("Created By", typeof(string));

            foreach (var Form in Forms)
            {
                var Consent = Form.ConsentGiven ? new HtmlString("<span class=\"glyphicon glyphicon-ok text-success\" aria-hidden=\"true\"></span>") : new HtmlString("<span class=\"glyphicon glyphicon-remove text-danger\" aria-hidden=\"true\"></span></div>");
                var ViewLink = new HtmlString(string.Format("<a type=\"button\" class=\"btn btn-primary btn-sm\" href=\"ViewForm/{0}\">View</a>", Form.FormID));
                Table.Rows.Add(ViewLink, Form.DateCreated, Form.ProjectName, Consent, Form.PaymoNumber, Form.CreatedBy);
            }
            return Table;
        }

        public DataTable PreparePhotographersTable(List<PhotographerModel> Photographers)
        {
            var Table = new DataTable();
            Table.Columns.Add("View", typeof(HtmlString));
            Table.Columns.Add("Name", typeof(string));
            Table.Columns.Add("Email", typeof(string));
            Table.Columns.Add("Contact Number", typeof(string));

            foreach (var Photographer in Photographers)
            {
                var ViewLink = new HtmlString(string.Format("<a type=\"button\" class=\"btn btn-primary btn-sm\" href=\"ViewPhotographer/{0}\">View</a>", Photographer.PhotographerID));
                Table.Rows.Add(ViewLink, Photographer.Name, Photographer.Email, Photographer.ContactNumber);
            }
            return Table;
        }

        public DataTable PrepareCommOfficersTable(List<CommissioningOfficerModel> Officers)
        {
            var Table = new DataTable();
            Table.Columns.Add("View", typeof(HtmlString));
            Table.Columns.Add("Name", typeof(string));
            Table.Columns.Add("Email", typeof(string));
            Table.Columns.Add("Contact Number", typeof(string));

            foreach (var Officer in Officers)
            {
                var ViewLink = new HtmlString(string.Format("<a type=\"button\" class=\"btn btn-primary btn-sm\" href=\"ViewCommOfficer/{0}\">View</a>", Officer.OfficerID));
                Table.Rows.Add(ViewLink, Officer.Name, Officer.Email, Officer.ContactNumber);
            }
            return Table;
        }
        #endregion

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
            var FormId = model.FormID;
            _databaseService.InsertCommissioningOfficer(model);
            if (model.FormID == 1)
            {
                return RedirectToRoute("ManageCommOfficers");
            }
            else
            {
                model.FormID = 1;
                _databaseService.InsertCommissioningOfficer(model);
                return RedirectToRoute("ViewForm", new { ID = FormId });
            }
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
            var FormId = model.FormID;
            _databaseService.InsertPhotographer(model);
            if (model.FormID == 1)
            {
                return RedirectToRoute("ManagePhotographers");
            }
            else
            {
                model.FormID = 1;
                _databaseService.InsertPhotographer(model);
                return RedirectToRoute("ViewForm", new { ID = FormId });
            }
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
            if (photo.Count != 0)
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
            var OldModel = _databaseService.GetOfficerByID(model.OfficerID);
            var OfficerInstances = _databaseService.GetOfficers().Where(x => x.Name == OldModel.Name && x.Email == OldModel.Email && x.ContactNumber == OldModel.ContactNumber);
            
            foreach (var Instance in OfficerInstances)
            {
                model.OfficerID = Instance.OfficerID;
                _databaseService.UpdateCommissioningOfficer(model);
            }
            if (model.FormID == 1)
            {
                return RedirectToRoute("ManageCommOfficers");
            }
            else
            {
                return RedirectToRoute("ViewForm", new { ID = model.FormID });
            }
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
            var OldModel = _databaseService.GetPhotographerByID(model.PhotographerID);
            var PhotographerInstances = _databaseService.GetPhotographers().Where(x => x.Name == OldModel.Name && x.Email == OldModel.Email && x.ContactNumber == OldModel.ContactNumber);

            foreach (var Instance in PhotographerInstances)
            {
                model.PhotographerID = Instance.PhotographerID;
                _databaseService.UpdatePhotographer(model);
            }
            if (model.FormID == 1)
            {
                return RedirectToRoute("ManagePhotographers");
            }
            else
            {
                return RedirectToRoute("ViewForm", new { ID = model.FormID });
            }
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
            if (model.FormID == 1)
            {
                return RedirectToRoute("ManageCommOfficers");
            }
            else
            {
                return RedirectToRoute("ViewForm", new { ID = model.FormID });
            }
        }

        [HttpPost]
        public ActionResult DeleteParticipant(ParticipantModel model)
        {
            var photo = _databaseService.GetPhotosByParticipantID(model.ParticipantID).FirstOrDefault();
            if (photo != null)
            {
                _databaseService.DeletePhoto(model.ParticipantID);
            }
            _databaseService.DeleteParticipant(model);
            return RedirectToRoute("ViewForm", new { ID = model.FormID });
        }
        [HttpPost]
        public ActionResult DeletePhotographer(PhotographerModel model)
        {
            _databaseService.DeletePhotographer(model);
            if (model.FormID == 1)
            {
                return RedirectToRoute("ManagePhotographers");
            }
            else
            {
                return RedirectToRoute("ViewForm", new { ID = model.FormID });
            }
        }
        #endregion
    }
}