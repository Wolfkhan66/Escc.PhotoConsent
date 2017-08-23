using Escc.PhotoConsent.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetaPoco;
using Escc.PhotoConsent.Models.DataModels;
using System.Data.SqlClient;
using System.Configuration;

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

        /// <summary>
        /// Run query to get form ID form by Project Reference, Date Created and Created By
        /// </summary>
        /// <param name="ProjectReference, DateCreated, CreatedBy">string, DateTime, string </param>
        public int GetFormIDAfterCreation(string ProjectReference, DateTime DateCreated, string CreatedBy)
        {
            int FormID = 0;
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["PhotoConsentDB"].ToString()))
            {
                cn.Open();
                var sql = string.Format("SELECT FormID FROM ConsentForms WHERE [DateCreated] = {0} AND [ProjectReference] = {1} And [CreatedBy] = {2}", DateCreated, ProjectReference, CreatedBy);
                SqlCommand sqlCommand = new SqlCommand(sql, cn);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    FormID = (int)reader["FormID"];
                }
                cn.Close();
            }
            return FormID;
        }

        /// <summary>
        /// Run a query to get a form by ID
        /// </summary>
        /// <param name="FormID">int - </param>
        public ConsentFormModel GetFormByID(int FormID)
        {
            var form = new ConsentFormModel();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["PhotoConsentDB"].ToString()))
            {
                cn.Open();
                var sql = string.Format("SELECT * FROM ConsentForms WHERE [FormID] = {0}", FormID);
                SqlCommand sqlCommand = new SqlCommand(sql, cn);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    var model = new ConsentFormModel();
                    model.ConsentGiven = (bool)reader["ConsentGiven"];
                    model.CreatedBy = (string)reader["CreatedBy"];
                    model.DateCreated = (DateTime)reader["DateCreated"];
                    model.DateSubmitted = (DateTime)reader["DateSubmitted"];
                    model.FormID = (int)reader["FormID"]; 
                    model.Notes = (string)reader["Notes"];
                    model.ProjectReference = (string)reader["ProjectReference"];

                    form = model;
                }
                cn.Close();
            }
            return form;
        }

        /// <summary>
        /// Run a query to get all consent forms.
        /// </summary>
        public List<ConsentFormModel> GetConsentForms()
        {
            var forms = new List<ConsentFormModel>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["PhotoConsentDB"].ToString()))
            {
                cn.Open();
                var sql = string.Format("SELECT * FROM ConsentForms");
                SqlCommand sqlCommand = new SqlCommand(sql, cn);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    var model = new ConsentFormModel();
                    model.ConsentGiven = (bool)reader["ConsentGiven"];
                    model.CreatedBy = (string)reader["CreatedBy"];
                    model.DateCreated = (DateTime)reader["DateCreated"];
                    model.DateSubmitted = (DateTime)reader["DateSubmitted"];
                    model.FormID = (int)reader["FormID"];
                    model.Notes = (string)reader["Notes"];
                    model.ProjectReference = (string)reader["ProjectReference"];

                    forms.Add(model);
                }
                cn.Close();
            }
            return forms;
        }
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

        /// <summary>
        /// Run a query to get all participants for a consent form
        /// </summary>
        /// <param name="FormID"> int </param>
        public List<ParticipantModel> GetParticipantsByFormID(int FormID)
        {
            var Participants = new List<ParticipantModel>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["PhotoConsentDB"].ToString()))
            {
                cn.Open();
                var sql = string.Format("SELECT * FROM Participants");
                SqlCommand sqlCommand = new SqlCommand(sql, cn);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    var model = new ParticipantModel();
                    model.ContactNumber = (string)reader["ContactNumber"];
                    model.Email = (string)reader["Email"];
                    model.FormID = (int)reader["FormID"];
                    model.Name = (string)reader["Name"];
                    model.ParticipantID = (int)reader["ParticipantID"];

                    Participants.Add(model);
                }
                cn.Close();
            }
            return Participants;
        }
        #endregion

        #region Commissioning Officer
        /// <summary>
        /// Run SP to input a Commissioning Officer into the database
        /// </summary>
        /// <param name="model">ConsentFormModel - </param>
        public void InsertCommissioningOfficer(CommissioningOfficerModel model)
        {
            _db.Execute("EXEC InsertCommissioningOfficer @FormID, @Name, @Email, @ContactNumber", new { model.FormID, model.Name, model.Email, model.ContactNumber });        
        }

        /// <summary>
        /// Run a query to get all Officers for a consent form
        /// </summary>
        /// <param name="FormID"> int </param>
        public List<CommissioningOfficerModel> GetOfficersByFormID(int FormID)
        {
            var Officers = new List<CommissioningOfficerModel>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["PhotoConsentDB"].ToString()))
            {
                cn.Open();
                var sql = string.Format("SELECT * FROM CommissioningOfficer");
                SqlCommand sqlCommand = new SqlCommand(sql, cn);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    var model = new CommissioningOfficerModel();
                    model.ContactNumber = (string)reader["ContactNumber"];
                    model.Email = (string)reader["Email"];
                    model.FormID = (int)reader["FormID"];
                    model.Name = (string)reader["Name"];
                    model.OfficerID = (int)reader["OfficerID"];

                    Officers.Add(model);
                }
                cn.Close();
            }
            return Officers;
        }
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

        /// <summary>
        /// Run a query to get all Photographers for a consent form
        /// </summary>
        /// <param name="FormID"> int </param>
        public List<PhotographerModel> GetPhotographersByFormID(int FormID)
        {
            var Photographers = new List<PhotographerModel>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["PhotoConsentDB"].ToString()))
            {
                cn.Open();
                var sql = string.Format("SELECT * FROM Photographers");
                SqlCommand sqlCommand = new SqlCommand(sql, cn);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    var model = new PhotographerModel();
                    model.ContactNumber = (string)reader["ContactNumber"];
                    model.Email = (string)reader["Email"];
                    model.FormID = (int)reader["FormID"];
                    model.Name = (string)reader["Name"];
                    model.PhotographerID = (int)reader["PhotographerID"];

                    Photographers.Add(model);
                }
                cn.Close();
            }
            return Photographers;
        }
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

        /// <summary>
        /// Run a query to get all Photos for a participant
        /// </summary>
        /// <param name="ParticipantID"> int </param>
        public List<PhotoModel> GetPhotosByParticipantID(int ParticipantID)
        {
            var Photos = new List<PhotoModel>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["PhotoConsentDB"].ToString()))
            {
                cn.Open();
                var sql = string.Format("SELECT * FROM Photos");
                SqlCommand sqlCommand = new SqlCommand(sql, cn);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    var model = new PhotoModel();
                    model.Image = (string)reader["Image"];
                    model.ParticipantID = (int)reader["ParticipantID"];
                    model.PhotoID = (int)reader["PhotoID"];

                    Photos.Add(model);
                }
                cn.Close();
            }
            return Photos;
        }
        #endregion

    }
}