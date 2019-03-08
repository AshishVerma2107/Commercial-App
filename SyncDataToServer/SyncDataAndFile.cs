using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Util;
using ComtaxApp.Database;

namespace ComtaxApp.SyncDataToServer
{
    public class SyncDataAndFile
    {
        ServiceHelper service;
        Context mContext;
        DBHelper dba;
        InternetConnection con;
        Geolocation geo;

        public SyncDataAndFile(Context context)
        {
            this.mContext = context;
            service = new ServiceHelper();
            dba = new DBHelper();
            con = new InternetConnection();
            geo = new Geolocation();
        }


        public async Task UploadDataAsync(byte[] photobytes, string photoName)
        {
            try
            {
              
            }
            catch (Exception e)
            {
                Log.Error("Error", e.Message);
                
            }
            
        }
    }
}