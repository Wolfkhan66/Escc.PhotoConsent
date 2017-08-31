using Escc.PhotoConsent.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.PhotoConsent.Models.ViewModels
{
    public class FormViewModel
    {
        public ConsentFormModel Form { get; set; }
        public List<ParticipantModel> Participants { get; set; }
        public List<CommissioningOfficerModel> Officers { get; set; }
        public List<PhotographerModel> Photographers { get; set; }
        public List<HtmlString> ErrorMessage { get; set; }
        public Dictionary<int,string> AllCommOfficers { get; set; }
        public Dictionary<int,string> AllPhotographers { get; set; }

        public FormViewModel()
        {
            Participants = new List<ParticipantModel>();
            Officers = new List<CommissioningOfficerModel>();
            Photographers = new List<PhotographerModel>();
            ErrorMessage = new List<HtmlString>();

            AllCommOfficers = new Dictionary<int, string>();
            AllPhotographers = new Dictionary<int, string>();
        }
    }
}