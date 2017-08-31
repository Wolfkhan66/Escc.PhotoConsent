using Escc.PhotoConsent.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Escc.PhotoConsent.Models.ViewModels
{
    public class PhotographerViewModel
    {
        public PhotographerModel Photographer { get; set; }
        public TableModel Forms { get; set; }

        public PhotographerViewModel()
        {
            Forms = new TableModel("FormsTable");
            Forms.Table = new DataTable();
        }
    }
}