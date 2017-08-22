using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.PhotoConsent.Models.DataModels
{
    public class AdminCreateFormModel
    {
        public int FormID { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public string ProjectReference { get; set; }
        public string Notes { get; set; }

        public string[] Participant_Name { get; set; }
        public string[] Participant_Email { get; set; }
        public string[] Participant_ContactNumber { get; set; }

        public string[] CommissioningOfficer_Name { get; set; }
        public string[] CommissioningOfficer_Email { get; set; }
        public string[] CommissioningOfficer_ContactNumber { get; set; }

        public string[] Photographer_Name { get; set; }
        public string[] Photographer_Email { get; set; }
        public string[] Photographer_ContactNumber { get; set; }


        public AdminCreateFormModel()
        {
                
        }
    }
}