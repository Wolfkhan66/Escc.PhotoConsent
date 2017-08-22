using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.PhotoConsent.Models.DataModels
{
    public class ConsentFormModel
    {
        public int FormID { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public string ProjectReference { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public bool ConsentGiven { get; set; }
        public string Notes { get; set; }
    }
}