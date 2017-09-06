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
            _db.Execute("EXEC InsertConsentForm @DateCreated, @CreatedBy, @ProjectName, @DateSubmitted, @ConsentGiven, @Notes, @GUID, @Deleted, @PaymoNumber", new { model.DateCreated, model.CreatedBy, model.ProjectName, model.DateSubmitted, model.ConsentGiven, model.Notes, model.GUID, model.Deleted, model.PaymoNumber });
        }

        public void UpdateConsentForm(ConsentFormModel model)
        {
            _db.Execute("EXEC UpdateConsentForm @CreatedBy, @ProjectName, @Notes, @FormID, @ConsentGiven, @DateSubmitted, @PaymoNumber", new { model.CreatedBy, model.ProjectName, model.Notes, model.FormID, model.ConsentGiven, model.DateSubmitted, model.PaymoNumber});
        }

        public void DeleteConsentForm(ConsentFormModel model)
        {
            _db.Execute("EXEC DeleteConsentForm @FormID, @Deleted", new { model.FormID, model.Deleted });
        }

        /// <summary>
        /// Run query to get form ID by GUID
        /// </summary>
        /// <param name="GUID">Guid </param>
        public int GetFormIDByGuid(Guid GUID)
        {
            int FormID = 0;
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["PhotoConsentDB"].ToString()))
            {
                cn.Open();
                var sql = string.Format("SELECT FormID FROM ConsentForms WHERE [GUID] = '{0}'", GUID.ToString());
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
                    model.DateCreated = (string)reader["DateCreated"];
                    model.FormID = (int)reader["FormID"];
                    model.GUID = Guid.Parse(reader["GUID"].ToString());
                    model.Deleted = (bool)reader["Deleted"];

                    try
                    {
                        model.DateSubmitted = (string)reader["DateSubmitted"];
                    }
                    catch (Exception)
                    {
                        model.DateSubmitted = "";
                    }
                    try
                    {
                        model.Notes = (string)reader["Notes"];
                    }
                    catch (Exception)
                    {
                        model.Notes = "";
                    }
                    try
                    {
                        model.ProjectName = (string)reader["ProjectName"];
                    }
                    catch (Exception)
                    {
                        model.ProjectName = "";
                    }
                    try
                    {
                        model.PaymoNumber = (string)reader["PaymoNumber"];
                    }
                    catch (Exception)
                    {
                        model.PaymoNumber = "";
                    }

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
                    model.DateCreated = (string)reader["DateCreated"];
                    model.FormID = (int)reader["FormID"];
                    model.GUID = Guid.Parse(reader["GUID"].ToString());
                    model.Deleted = (bool)reader["Deleted"];

                    try
                    {
                        model.DateSubmitted = (string)reader["DateSubmitted"];
                    }
                    catch (Exception)
                    {
                        model.DateSubmitted = "";
                    }
                    try
                    {
                        model.Notes = (string)reader["Notes"];
                    }
                    catch (Exception)
                    {
                        model.Notes = "";
                    }
                    try
                    {
                        model.ProjectName = (string)reader["ProjectName"];
                    }
                    catch (Exception)
                    {
                        model.ProjectName = "";
                    }
                    try
                    {
                        model.PaymoNumber = (string)reader["PaymoNumber"];
                    }
                    catch (Exception)
                    {
                        model.PaymoNumber = "";
                    }

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

        public void UpdateParticipant(ParticipantModel model)
        {
            _db.Execute("EXEC UpdateParticipant @Name, @Email, @ContactNumber, @ParticipantID", new { model.Name, model.Email, model.ContactNumber, model.ParticipantID });
        }

        public void DeleteParticipant(ParticipantModel model)
        {
            _db.Execute("EXEC DeleteParticipant @ParticipantID", new { model.ParticipantID });
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
                var sql = string.Format("SELECT * FROM Participants WHERE FormID = {0}", FormID);
                SqlCommand sqlCommand = new SqlCommand(sql, cn);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    var model = new ParticipantModel();
                    model.FormID = (int)reader["FormID"];
                    model.Name = (string)reader["Name"];
                    model.ParticipantID = (int)reader["ParticipantID"];
                    try
                    {
                        model.ContactNumber = (string)reader["ContactNumber"];
                    }
                    catch (Exception)
                    {
                        model.ContactNumber = "";
                    }

                    try
                    {
                        model.Email = (string)reader["Email"];
                    }
                    catch (Exception)
                    {
                        model.Email = "";
                    }

                    Participants.Add(model);
                }
                cn.Close();
            }
            return Participants;
        }

        /// <summary>
        /// Run a query to get all participants
        /// </summary>
        public List<ParticipantModel> GetParticipants()
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
                    model.FormID = (int)reader["FormID"];
                    model.Name = (string)reader["Name"];
                    model.ParticipantID = (int)reader["ParticipantID"];

                    try
                    {
                        model.ContactNumber = (string)reader["ContactNumber"];
                    }
                    catch (Exception)
                    {
                        model.ContactNumber = "";
                    }

                    try
                    {
                        model.Email = (string)reader["Email"];
                    }
                    catch (Exception)
                    {
                        model.Email = "";
                    }

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
        /// <param name="model">CommissioningOfficerModel - </param>
        public void InsertCommissioningOfficer(CommissioningOfficerModel model)
        {
            _db.Execute("EXEC InsertCommissioningOfficer @FormID, @Name, @Email, @ContactNumber", new { model.FormID, model.Name, model.Email, model.ContactNumber });        
        }

        public void UpdateCommissioningOfficer(CommissioningOfficerModel model)
        {
            _db.Execute("EXEC UpdateCommissioningOfficer @Name, @Email, @ContactNumber, @OfficerID", new {model.Name, model.Email, model.ContactNumber, model.OfficerID });
        }

        public void DeleteCommissioningOfficer(CommissioningOfficerModel model)
        {
            _db.Execute("EXEC DeleteCommissioningOfficer @OfficerID", new { model.OfficerID });
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
                var sql = string.Format("SELECT * FROM CommissioningOfficer WHERE FormID = {0}", FormID);
                SqlCommand sqlCommand = new SqlCommand(sql, cn);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    var model = new CommissioningOfficerModel();
                    model.FormID = (int)reader["FormID"];
                    model.Name = (string)reader["Name"];
                    model.OfficerID = (int)reader["OfficerID"];

                    try
                    {
                        model.ContactNumber = (string)reader["ContactNumber"];
                    }
                    catch (Exception)
                    {
                        model.ContactNumber = "";
                    }

                    try
                    {
                        model.Email = (string)reader["Email"];
                    }
                    catch (Exception)
                    {
                        model.Email = "";
                    }

                    Officers.Add(model);
                }
                cn.Close();
            }
            return Officers;
        }

        /// <summary>
        /// Run a query to get all Officers
        /// </summary>
        public List<CommissioningOfficerModel> GetOfficers()
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
                    model.FormID = (int)reader["FormID"];
                    model.Name = (string)reader["Name"];
                    model.OfficerID = (int)reader["OfficerID"];

                    try
                    {
                        model.ContactNumber = (string)reader["ContactNumber"];
                    }
                    catch (Exception)
                    {
                        model.ContactNumber = "";
                    }

                    try
                    {
                        model.Email = (string)reader["Email"];
                    }
                    catch (Exception)
                    {
                        model.Email = "";
                    }

                    Officers.Add(model);
                }
                cn.Close();
            }
            return Officers;
        }

        /// <summary>
        /// Run a query to get a single Officer by id
        /// </summary>
        /// <param name="OfficerID"> int </param>
        public CommissioningOfficerModel GetOfficerByID(int OfficerID)
        {
            var Officer = new CommissioningOfficerModel();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["PhotoConsentDB"].ToString()))
            {
                cn.Open();
                var sql = string.Format("SELECT * FROM CommissioningOfficer Where OfficerID = {0}", OfficerID);
                SqlCommand sqlCommand = new SqlCommand(sql, cn);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    Officer.FormID = (int)reader["FormID"];
                    Officer.Name = (string)reader["Name"];
                    Officer.OfficerID = (int)reader["OfficerID"];

                    try
                    {
                        Officer.ContactNumber = (string)reader["ContactNumber"];
                    }
                    catch (Exception)
                    {
                        Officer.ContactNumber = "";
                    }

                    try
                    {
                        Officer.Email = (string)reader["Email"];
                    }
                    catch (Exception)
                    {
                        Officer.Email = "";
                    }
                }
                cn.Close();
            }
            return Officer;
        }
        #endregion

        #region Photographer
        /// <summary>
        /// Run SP to input a Photographer into the database
        /// </summary>
        /// <param name="model">PhotographerModel - </param>
        public void InsertPhotographer(PhotographerModel model)
        {
            _db.Execute("EXEC InsertPhotographer @FormID, @Name, @Email, @ContactNumber", new { model.FormID, model.Name, model.Email, model.ContactNumber });        
        }

        public void UpdatePhotographer(PhotographerModel model)
        {
            _db.Execute("EXEC UpdatePhotographer @Name, @Email, @ContactNumber, @PhotographerID", new { model.Name, model.Email, model.ContactNumber, model.PhotographerID });
        }
        public void DeletePhotographer(PhotographerModel model)
        {
            _db.Execute("EXEC DeletePhotographer @PhotographerID", new { model.PhotographerID });
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
                var sql = string.Format("SELECT * FROM Photographers WHERE FormID = {0}", FormID);
                SqlCommand sqlCommand = new SqlCommand(sql, cn);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    var model = new PhotographerModel();
                    model.FormID = (int)reader["FormID"];
                    model.Name = (string)reader["Name"];
                    model.PhotographerID = (int)reader["PhotographerID"];

                    try
                    {
                        model.ContactNumber = (string)reader["ContactNumber"];
                    }
                    catch (Exception)
                    {
                        model.ContactNumber = "";
                    }

                    try
                    {
                        model.Email = (string)reader["Email"];
                    }
                    catch (Exception)
                    {
                        model.Email = "";
                    }

                    Photographers.Add(model);
                }
                cn.Close();
            }
            return Photographers;
        }

        /// <summary>
        /// Run a query to get all Photographers
        /// </summary>
        public List<PhotographerModel> GetPhotographers()
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
                    model.FormID = (int)reader["FormID"];
                    model.Name = (string)reader["Name"];
                    model.PhotographerID = (int)reader["PhotographerID"];

                    try
                    {
                        model.ContactNumber = (string)reader["ContactNumber"];
                    }
                    catch (Exception)
                    {
                        model.ContactNumber = "";
                    }

                    try
                    {
                        model.Email = (string)reader["Email"];
                    }
                    catch (Exception)
                    {
                        model.Email = "";
                    }
                    Photographers.Add(model);
                }
                cn.Close();
            }
            return Photographers;
        }

        /// <summary>
        /// Run a query to get a Photographer by Id
        /// </summary>
        /// <param name="PhotographerID"> int </param>
        public PhotographerModel GetPhotographerByID(int PhotographerID)
        {
            var Photographer = new PhotographerModel();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["PhotoConsentDB"].ToString()))
            {
                cn.Open();
                var sql = string.Format("SELECT * FROM Photographers Where PhotographerID = {0}", PhotographerID);
                SqlCommand sqlCommand = new SqlCommand(sql, cn);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    Photographer.FormID = (int)reader["FormID"];
                    Photographer.Name = (string)reader["Name"];
                    Photographer.PhotographerID = (int)reader["PhotographerID"];
                    try
                    {
                        Photographer.ContactNumber = (string)reader["ContactNumber"];
                    }
                    catch (Exception)
                    {
                        Photographer.ContactNumber = "";
                    }

                    try
                    {
                        Photographer.Email = (string)reader["Email"];
                    }
                    catch (Exception)
                    {
                        Photographer.Email = "";
                    }
                }
                cn.Close();
            }
            return Photographer;
        }
        #endregion

        #region Photos
        /// <summary>
        /// Run SP to input an image into the database
        /// </summary>
        /// <param name="model">PhotoModel - </param>
        public void InsertPhoto(PhotoModel model)
        {
            _db.Execute("EXEC InsertPhoto @ParticipantID, @Image", new { model.ParticipantID, model.Image });
        }

        public void DeletePhoto(int ParticipantID)
        {
            _db.Execute("EXEC DeletePhoto @ParticipantID", new { ParticipantID});
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
                var sql = string.Format("SELECT * FROM Photos WHERE ParticipantID = {0}", ParticipantID);
                SqlCommand sqlCommand = new SqlCommand(sql, cn);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    var model = new PhotoModel();
                    model.Image = (byte[])reader["Image"];
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