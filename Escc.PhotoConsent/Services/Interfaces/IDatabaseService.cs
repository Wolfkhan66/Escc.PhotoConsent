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

        #region ConsentForm
        int GetFormIDAfterCreation(Guid GUID);
        ConsentFormModel GetFormByID(int FormID);
        List<ConsentFormModel> GetConsentForms();
        #endregion

        #region Participants
        List<ParticipantModel> GetParticipantsByFormID(int FormID);
        #endregion

        #region Commissioning Officer
        List<CommissioningOfficerModel> GetOfficersByFormID(int FormID);
        #endregion

        #region Photographers
        List<PhotographerModel> GetPhotographersByFormID(int FormID);
        #endregion


        #region Photos
        List<PhotoModel> GetPhotosByParticipantID(int ParticipantID);
        #endregion
    }
}