using Escc.PhotoConsent.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.PhotoConsent.Models.ViewModels
{
    public class ManageCommOfficerViewModel
    {
        public TableModel Officers { get; set; }

        public ManageCommOfficerViewModel()
        {
            Officers = new TableModel("OfficersTable");
        }
    }
}