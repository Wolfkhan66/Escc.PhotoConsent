using Escc.PhotoConsent.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Escc.PhotoConsent.Models.ViewModels
{
    public class CommOfficerViewModel
    {
        public CommissioningOfficerModel Officer { get; set; }
        public TableModel Forms { get; set; }

        public CommOfficerViewModel()
        {
            Forms = new TableModel("FormsTable");
            Forms.Table = new DataTable();
        }
    }
}