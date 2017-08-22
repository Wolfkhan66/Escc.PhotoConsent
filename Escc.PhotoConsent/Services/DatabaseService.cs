using Escc.PhotoConsent.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetaPoco;
using Escc.PhotoConsent.Models.DataModels;

namespace Escc.PhotoConsent.Services
{
    public class DatabaseService : IDatabaseService
    {
        #region Initialization
        private readonly Database _db;

        public DatabaseService()
        {
            _db = new Database("PhotoConsentDB");
        }
        #endregion

        #region ConsentForms

        /// <summary>
        /// Run SP to input the consent form into the database
        /// </summary>
        /// <param name="model">ConsentFormModel - </param>
        public void InsertConsentForm(ConsentFormModel model)
        {
            _db.Execute("EXEC InsertConsentForm @DateCreated, @CreatedBy, @ProjectReference, @DateSubmitted, @ConsentGiven, @Notes", new { model.DateCreated, model.CreatedBy, model.ProjectReference, model.DateSubmitted, model.ConsentGiven, model.Notes });
        }

        //TO DO 
        //Get
        //Update

        #endregion

        #region Participants

        /// <summary>
        /// Run SP to input a participant into the database
        /// </summary>
        /// <param name="model">ParticipantModel - </param>
        public void InsertParticipant(ParticipantModel model)
        {
            _db.Execute("EXEC InsertParticipant @FormID, @Name, @Email, @ContactNumber", new { model.FormID, model.Name, model.Email, model.ContactNumber });
        }

        //TO DO 
        //Get
        //Update

        #endregion

        #region Commissioning Officer

        /// <summary>
        /// Run SP to input a Commissioning Officer into the database
        /// </summary>
        /// <param name="model">ConsentFormModel - </param>
        public void InsertCommissioningOfficer(CommissioningOfficerModel model)
        {
            //_db.Execute("EXEC InsertCommissioningOfficer @FormID, @Name, @Email, @ContactNumber", new { model.FormID, model.Name, model.Email, model.ContactNumber });        
        }

        //TO DO 
        //Get
        //Update

        #endregion

        #region Photographer

        /// <summary>
        /// Run SP to input a Photographer into the database
        /// </summary>
        /// <param name="model">ConsentFormModel - </param>
        public void InsertPhotographer(PhotographerModel model)
        {
            _db.Execute("EXEC InsertPhotographer @FormID, @Name, @Email, @ContactNumber", new { model.FormID, model.Name, model.Email, model.ContactNumber });        
        }

        //TO DO 
        //Get
        //Update

        #endregion

        #region Photos

        /// <summary>
        /// Run SP to input an image into the database
        /// </summary>
        /// <param name="model">ConsentFormModel - </param>
        public void InsertPhoto(PhotoModel model)
        {
            _db.Execute("EXEC InsertPhoto @ParticipantID, @Image", new { model.ParticipantID, model.Image });
        }

        //TO DO 
        //Get
        //Update

        #endregion

    }
}