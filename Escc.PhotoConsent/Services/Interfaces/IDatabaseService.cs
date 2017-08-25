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

        #region Get
        int GetFormIDByGuid(Guid GUID);
        ConsentFormModel GetFormByID(int FormID);
        List<ConsentFormModel> GetConsentForms();
        List<ParticipantModel> GetParticipantsByFormID(int FormID);
        List<CommissioningOfficerModel> GetOfficersByFormID(int FormID);
        List<PhotographerModel> GetPhotographersByFormID(int FormID);
        List<PhotoModel> GetPhotosByParticipantID(int ParticipantID);
        #endregion

        #region Update
        void UpdateConsentForm(ConsentFormModel model);
        void UpdateParticipant(ParticipantModel model);
        void UpdateCommissioningOfficer(CommissioningOfficerModel model);
        void UpdatePhotographer(PhotographerModel model);
        #endregion

        #region Delete
        void DeleteConsentForm(ConsentFormModel model);
        void DeleteParticipant(ParticipantModel model);
        void DeleteCommissioningOfficer(CommissioningOfficerModel model);
        void DeletePhotographer(PhotographerModel model);
        void DeletePhoto(int ParticipantID);
        #endregion
    }
}