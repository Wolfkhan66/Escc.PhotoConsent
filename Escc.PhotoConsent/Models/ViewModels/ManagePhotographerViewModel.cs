using Escc.PhotoConsent.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.PhotoConsent.Models.ViewModels
{
    public class ManagePhotographerViewModel
    {
        public TableModel Photographers { get; set; }

        public ManagePhotographerViewModel()
        {
            Photographers = new TableModel("PhotographersTable");
        }
    }
}