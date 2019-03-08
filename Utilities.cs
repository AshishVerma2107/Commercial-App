using System;
using System.Collections.Generic;
using ComtaxApp.Model;
using Java.Text;
using Java.Util;

namespace ComtaxApp
{
    public static class Utilities
    {
        public static string imageURL1 = "";
        public static string imageName1 = "";
        public static int currentPosition = -1;
        public static string report_name = "";
        public static string type = "";
        public static int groupPosition = -1;
        public static List<string> GlobalBusinessList = new List<string>();

        public static List<ImageListModel> imageList = new List<ImageListModel>();

        public static String fileName()
        {
            try
            {
                String timeStamp = new SimpleDateFormat("yyyyMMdd_HHmmss").Format(new Date());
                String name = "IMG_" + timeStamp + ".jpg";
                return name;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public static byte[] GetStreamFromFile(string filePath)
        {
            try
            {
                Android.Net.Uri uri = Android.Net.Uri.FromFile(new Java.IO.File(filePath));
                byte[] byteArray = System.IO.File.ReadAllBytes(uri.Path);
                return byteArray;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}