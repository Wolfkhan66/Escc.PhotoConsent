using Escc.PhotoConsent.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.PhotoConsent.Services.Interfaces
{
    public interface IDatabaseService
    {
        #region Insert
        void InsertConsentForm(ConsentFormModel model);
        void InsertParticipant(ParticipantModel model);
        void InsertCommissioningOfficer(CommissioningOfficerModel model);
        void InsertPhotographer(PhotographerModel model);
        void InsertPhoto(PhotoModel model);
        #endregion
    }
}