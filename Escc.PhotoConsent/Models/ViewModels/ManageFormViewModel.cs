using Escc.PhotoConsent.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.PhotoConsent.Models.ViewModels
{
    public class ManageFormViewModel
    {
        public TableModel Forms { get; set; }

        public ManageFormViewModel()
        {
            Forms = new TableModel("FormsTable");
        }
    }
}