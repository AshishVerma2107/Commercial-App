using ComtaxApp.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ComtaxApp.Database
{
    public class DBHelper
    {
        SQLiteConnection db;
        public DBHelper()
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ComTaxUP.db3");
            db = new SQLiteConnection(dbPath);
            try
            {
                db.CreateTable<GSTINModel>();
                db.CreateTable<GSTINVerFiles1>();
                db.CreateTable<BusinessModel>();

            }
            catch (Exception e)
            {
                Debug.WriteLine("Table error: " + e.Message);
            }

        }


        public int insertGSTINData(string geoLocation, string gstin, string wentNotVerified, string appDateTime, 
            string addressStatus, string businessStatus, string registrationAccording, string other, string compounding, 
            string premises, string manufacturer, string turnOver, string versionName, string uploadStatus)
        {
            int insertId = 0;
            try
            {
                GSTINModel tbl = new GSTINModel();
                tbl.GeoLocation = geoLocation;
                tbl.GSTINID = gstin;
                tbl.WentNotVerified = wentNotVerified;
                tbl.AppDateTime = appDateTime;
                tbl.AddressStatus = addressStatus;
                tbl.BusinessStatus = businessStatus;
                tbl.RegistrationAccording = registrationAccording;
                tbl.Other = other;
                tbl.CompoundingEligible = compounding;
                tbl.PremisesArea = premises;
                tbl.ManufacturerOrTrader = manufacturer;
                tbl.TurnOver = turnOver;
                tbl.versionName = versionName;
                tbl.UploadStatus = uploadStatus;
                int i = db.Insert(tbl);
                var data1 = db.Query<GSTINModel>("SELECT * from [GSTINModel]");
                if (data1.Count > 0)
                {
                    foreach (var val in data1)
                    {
                        insertId = val.Id;
                    }
                }
                return insertId;
            }
            catch (Exception ex)
            { return 0; }
        }

        public async Task<int> insertImageDetail(int insertedId, string fileName, string geoLocation)
        {
            int i = 0;
            try
            {
                GSTINVerFiles1 tbl = new GSTINVerFiles1();
                tbl.InsertedId = insertedId;
                tbl.VerFileName = fileName;
                tbl.GeoLocation = geoLocation;
                i = db.Insert(tbl);
                return i;
            }
            catch (Exception ex)
            { return 0; }
        }

        public int insertBusinessTypeData(List<BusinessModel> list)
        {
            int i = 0;
            for (int j = 0; j < list.Count; j++)
            {
                try
                {
                    var data1 = db.Query<BusinessModel>("SELECT * from [BusinessModel] where [BusinessTypeID]=" + list[j].BusinessTypeID);
                    if (data1.Count == 0)
                    {
                        BusinessModel tbl = new BusinessModel();
                        tbl.BusinessTypeID = list[j].BusinessTypeID;
                        tbl.BusinessType = list[j].BusinessType;
                        tbl.BusinessSubType = list[j].BusinessSubType;
                        i = db.Insert(tbl);
                    }
                }
                catch (Exception ex)
                { return 0; }
            }
            return i;
        }


        public List<BusinessModel> getBusinessTypeData()
        {
            try
            {
                List<BusinessModel> data1 = db.Query<BusinessModel>("SELECT * from [BusinessModel] order by BusinessType");
                return data1;
            }
            catch (Exception ex)
            {
                return null;
            }


        }


        public List<GSTINModel> getGSTINSurveyDetail(string status)
        {
            try
            {
               List<GSTINModel> data1 = db.Query<GSTINModel>("SELECT * from [GSTINModel] where [UploadStatus]='"+status+"'");
                return data1;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

       

        public List<GSTINModel> getGSTINAllSurvey()
        {
            try
            {
                List<GSTINModel> data1 = db.Query<GSTINModel>("SELECT * from [GSTINModel]");
                return data1;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<GSTINVerFiles1> getGSTINImages(int insertedId)
        {
            
            try
            {
                List<GSTINVerFiles1> data1 = db.Query<GSTINVerFiles1>("SELECT * from [GSTINVerFiles1] where [InsertedId] = " + insertedId);
                return data1;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public long updateGSTINStatus(int id)
        {
            try
            {

                var data = db.Table<GSTINModel>();
                int idvalue = Convert.ToInt32(id);

                var data1 = (from values in data
                             where values.Id == idvalue
                             select values).FirstOrDefault();
                data1.UploadStatus = "yes";

                long i = db.Update(data1);
                return i;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

    }
}