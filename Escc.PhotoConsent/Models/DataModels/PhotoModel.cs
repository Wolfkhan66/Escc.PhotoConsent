using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.PhotoConsent.Models.DataModels
{
    public class PhotoModel
    {
        public int PhotoID { get; set; }
        public int ParticipantID { get; set; }
        public byte[] Image { get; set; }
    }
}