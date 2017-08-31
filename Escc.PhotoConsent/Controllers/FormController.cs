using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Escc.PhotoConsent.Models.ViewModels;
using Escc.PhotoConsent.Services.Interfaces;
using Escc.PhotoConsent.Services;
using Escc.PhotoConsent.Models.DataModels;
using System.IO;
using Escc.Services;
using System.Net.Mail;
using System.Configuration;

namespace Escc.PhotoConsent.Controllers
{
    public class FormController : Controller
    {
        private IDatabaseService _databaseService = new DatabaseService();

        [Route("ConsentForm/{formGuid}", Name = "ConsentForm")]
        public ActionResult ConsentForm(string formGuid = "", List<HtmlString> ErrorMessage = null)
        {
            var ViewModel = new FormViewModel();
            if (formGuid != "")
            {
                var formID = _databaseService.GetFormIDByGuid(Guid.Parse(formGuid));
                ViewModel.Form = _databaseService.GetFormByID(formID);
                ViewModel.Participants = _databaseService.GetParticipantsByFormID(formID);
                ViewModel.ErrorMessage = ErrorMessage == null ? new List<HtmlString>() : ErrorMessage;

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
            }

            if (ViewModel.Form.ConsentGiven)
            {
                return View("FormSubmission");
            }
            else
            {
                return View("ConsentForm", ViewModel);
            }
        }

        [HttpPost]
        public ActionResult FormSubmission(int FormID, bool ConsentGiven, string FormGuid)
        {
            var Form = _databaseService.GetFormByID(FormID);
            Form.ConsentGiven = ConsentGiven;
            var Participants = _databaseService.GetParticipantsByFormID(FormID);

            var ErrorMessage = new List<HtmlString>();
            if (ConsentGiven == false)
            {
                ErrorMessage.Add(new HtmlString("<b>You did not give consent!</b> Please tick the checkbox confirming your consent and try again."));
            }
            if (Participants.Count == 0)
            {
                ErrorMessage.Add(new HtmlString("<b>You did not add any participants!</b> Please add at least one and try again."));
            }
            foreach (var Particpant in Participants)
            {
                var photos = _databaseService.GetPhotosByParticipantID(Particpant.ParticipantID).FirstOrDefault();
                if (photos == null)
                {
                    ErrorMessage.Add(new HtmlString(string.Format("<b>The participant: \"{0}\" has no photo!</b> Please click on the Upload button below their empty image icon to choose a photo. ", Particpant.Name, Particpant.Name)));
                }
            }

            if (ErrorMessage.Count != 0)
            {
                return ConsentForm(FormGuid, ErrorMessage);
            }
            else
            {
                Form.DateSubmitted = string.Format("{0} {1}:{2}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), DateTime.Now.Second);
                _databaseService.UpdateConsentForm(Form);
                SendSuccessEmail(Participants);
                return View("FormSubmission");
            }
        }

        public void SendSuccessEmail(List<ParticipantModel> Participants)
        {
            foreach (var Participant in Participants)
            {
                if(Participant.Email != "")
                {
                    var From = ConfigurationManager.AppSettings["EmailFrom"];
                    var To = Participant.Email;
                    var Subject = "Escc PhotoConsent: Consent Form Successfully Submitted";
                    var Body = string.Format("This email is to confirm that '{0}' has successfully given consent.", Participant.Name);
                    var Mail = new MailMessage(From, To, Subject, Body);
                    Mail.IsBodyHtml = true;
                    Mail.BodyEncoding = System.Text.Encoding.UTF8;

                    var emailService = ServiceContainer.LoadService<IEmailSender>(new ConfigurationServiceRegistry(), null);
                    emailService.SendAsync(Mail);
                }
            }
        }

        #region Create Methods
        [HttpPost]
        public ActionResult CreateParticipant(ParticipantModel model)
        {
            _databaseService.InsertParticipant(model);
            return RedirectToRoute("ConsentForm", new { formGuid = model.FormGUID });
        }

        [HttpPost]
        public ActionResult UploadPhoto(HttpPostedFileBase Image, int ParticipantID, int FormID, string Formguid)
        {
            var extensionsList = new List<string> { ".jpg", ".png", ".gif", ".bmp", ".tif", ".tiff", ".jpeg", ".jif", ".jfif", ".pdf", ".pcd", ".jp2", ".jpx", ".j2k", ".j2c" };

            if (Image == null)
            {
                var ErrorMessage = new List<HtmlString>();
                ErrorMessage.Add(new HtmlString("<b>You did not choose an image!</b> Please try again."));
                return ConsentForm(Formguid, ErrorMessage);
            }
            else if (! extensionsList.Any(f => Image.FileName.Contains(f)))
            {
                var ErrorMessage = new List<HtmlString>();
                ErrorMessage.Add(new HtmlString("<b>Thats not an image!</b> Please try again."));
                return ConsentForm(Formguid, ErrorMessage);
            }
            else
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
                return RedirectToRoute("ConsentForm", new { formGuid = Formguid });
            }
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
        public ActionResult DeleteParticipant(ParticipantModel model)
        {
            var photo = _databaseService.GetPhotosByParticipantID(model.ParticipantID).FirstOrDefault();
            if (photo != null)
            {
                _databaseService.DeletePhoto(model.ParticipantID);
            }
            _databaseService.DeleteParticipant(model);
            return RedirectToRoute("ConsentForm", new { formGuid = model.FormGUID });
        }
        #endregion
    }
}