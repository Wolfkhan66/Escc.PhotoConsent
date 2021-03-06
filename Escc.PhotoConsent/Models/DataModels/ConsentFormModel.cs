﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Escc.PhotoConsent.Models.DataModels
{
    public class ConsentFormModel
    {
        public int FormID { get; set; }
        public Guid GUID { get; set; }
        public string DateCreated { get; set; }

        [Display(Name = "Created By")]
        [Required(ErrorMessage = "Please enter your name")]
        public string CreatedBy { get; set; }
        [Display(Name = "Project Name")]
        [Required(ErrorMessage = "Please enter a project name")]
        public string ProjectName { get; set; }

        [Display(Name = "Paymo Number")]
        public string PaymoNumber { get; set; }
        public string DateSubmitted { get; set; }
        public bool ConsentGiven { get; set; }
        public string Notes { get; set; }
        public bool Deleted { get; set; }
       
    }
}