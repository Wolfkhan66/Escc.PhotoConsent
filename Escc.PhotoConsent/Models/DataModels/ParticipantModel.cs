using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Escc.PhotoConsent.Models.DataModels
{
    public class ParticipantModel
    {
        public int ParticipantID { get; set; }
        public int FormID { get; set; }
        public Guid FormGUID { get; set; }

        [Required(ErrorMessage = "Please enter a name")]
        public string Name { get; set; }
        public string Email { get; set; }

        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }

        public string Base64Image { get; set; }
    }
}