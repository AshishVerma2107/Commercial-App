using System;
using System.IO;
using System.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Android.Content;
using Android.Preferences;
using Android.Util;
using ComtaxApp.Model;
using Newtonsoft.Json;

namespace ComtaxApp
{
    class ServiceHelper
    {

        HttpClient client;
        HttpWebRequest request;
        Cryptography cryptography;
        Geolocation geo;
        string licenceId, UserId, AppDateTime, geolocation;
        public ServiceHelper()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            cryptography = new Cryptography();
            geo = new Geolocation();

        }

        public async Task<JsonValue> LoginUser2(string userid, string password, string licenceid, string geolocation)
        {
            //Items = new LoginModel();
            JsonValue jsonDoc = null;
            string url = "http://mobileapi.work121.com/api/Login/Getloginuser?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121&licenceId=" + licenceid + "&UserId=" + userid + "&Password=" + password + "&geolocation=" + geolocation + "&licenceFor=ComtaxApp";

            request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
            request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";

            try
            {
                // Send the request to the server and wait for the response:
                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                {

                    // Get a stream representation of the HTTP web response:
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        string text = reader.ReadToEnd();
                        string decrypted_Text = await OnCryptoAsync2(text, "Decryption");
                        // Use this stream to build a JSON document object:
                        jsonDoc = await Task.Run(() => JsonObject.Parse(decrypted_Text));
                        Log.Error("Response: {0}", jsonDoc.ToString());

                        // Return the JSON document:
                        // Items = JsonConvert.SerializeObject(jsonDoc);

                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("Error:", e.Message);
            }
            return jsonDoc;

        }



        public async Task<string> GetLicenceId(string geoLocation, string gcmid)
        {
            string licenceId = "";

            string url = "http://mobileapi.work121.com/api/Login/GetLicenceId?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121&licenceFor=ComtaxApp&geolocation=" + geoLocation + "&gcmid=" + gcmid;

            request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
            request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";

            try
            {
                // Send the request to the server and wait for the response:
                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                {

                    // Get a stream representation of the HTTP web response:
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        string text = reader.ReadToEnd();
                        string[] tokens = text.Split(',');
                        if (tokens[0].Contains("Success"))
                        {
                            licenceId = tokens[1].Substring(0, tokens[1].Length - 1);
                        }

                    }
                }
            }
            catch (Exception e)
            {

            }
            return licenceId;

        }

        public async Task<string> RegisterUser(string licenceid, string mobileNumber, string name, string geo_location, string npid, string userid)
        {
            string text = "";
            string url = "http://mobileapi.work121.com/api/Login/VerifylicenceId?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&" +
                "associatepwd=mA121&licenceId=" + licenceid + "&ProviderName=&ProvideId=&MobileNumber=" + mobileNumber + "&EmailID=&Name=" + name + "&" +
                "geolocation=" + geo_location + "&selfiePath=&NPID=" + npid + "&UserId=" + userid + "&licenceFor=ComtaxApp ";

            request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
            request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";

            try
            {
                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        text = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("Error:", e.Message);
            }
            return text;

        }

        ISharedPreferences prefs;

      
        public void init(Context context)
        {
            prefs = PreferenceManager.GetDefaultSharedPreferences(context);
            licenceId = prefs.GetString("LicenceId", "");
            UserId = prefs.GetString("DesignationId", "");
            AppDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");   
            geolocation = geo.GetGeoLocation(context);
        }

        public async Task<string> GetServiceMethod(Context context, string methodName, string jsonData)
        {

            prefs = PreferenceManager.GetDefaultSharedPreferences(context);
            init(context);

            if (jsonData.Equals(""))
            {
                jsonData = "w3ZUSfhMYXujjSK28/4kfw==";
            }

            string url = "http://mobileapi.work121.com/api/Comtax/GetMethod?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121&licenceId=" +
                licenceId + "&UserId=" + UserId + "&methodname=" + methodName + "&jsonData=" + jsonData + "&AppDateTime=" + AppDateTime;

            request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
            request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";
            try
            {
                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        string text = reader.ReadToEnd();
                        //  string decrypted_Text = await OnCryptoAsync(text, "Decryption");
                        ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(text);
                        if (responseModel.ResponseCode.Equals("Ok"))
                        {
                            string decrypted_Text = await OnCryptoAsync(responseModel.ResponseValue, "Decryption");
                            reader.Close();
                            stream.Close();
                            return decrypted_Text;
                        }
                        else
                        {
                            return responseModel.ResponseCode;
                        }

                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }


        public async Task<string> PostServiceMethod(Context context, string methodName, string jsonData)
        {

            prefs = PreferenceManager.GetDefaultSharedPreferences(context);
            init(context);

            if (jsonData.Equals(""))
            {
                jsonData = "w3ZUSfhMYXujjSK28/4kfw==";
            }

            string url = "http://mobileapi.work121.com/api/Comtax/PostMethod?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121&licenceId=" +
                licenceId + "&UserId=" + UserId + "&methodname=" + methodName + "&jsonData=" + jsonData + "&AppDateTime=" + AppDateTime+ "&GeoLocation=";

            request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
            request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";
            try
            {
                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        string text = reader.ReadToEnd();
                        //  string decrypted_Text = await OnCryptoAsync(text, "Decryption");
                        ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(text);
                        if (responseModel.ResponseCode.Equals("Ok"))
                        {
                            string decrypted_Text = await OnCryptoAsync(responseModel.ResponseValue, "Decryption");
                            reader.Close();
                            stream.Close();
                            return decrypted_Text;
                        }
                        else
                        {
                            return responseModel.ResponseCode;
                        }

                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public async Task<string> OnCryptoAsync(string textValue, string encDesc)
        {
            string text = "";
            if (encDesc.Equals("Encryption"))
            {
                text = textValue;
            }
            else
            {
                text = textValue.Substring(1, textValue.Length - 2);
            }
            string decryptedString = "";

            try
            {
                decryptedString = await cryptography.FunctionAsync(textValue, encDesc);
            }
            catch (Exception e)
            {
                // Debug.WriteLine("error", e.Message);
            }

            return decryptedString;
        }

        public async Task<string> OnCryptoAsync2(string textValue, string encDesc)
        {
            string text = "";
            if (encDesc.Equals("Encryption"))
            {
                text = textValue;
            }
            else
            {
                text = textValue.Substring(1, textValue.Length - 2);
            }
            string decryptedString = "";

            try
            {
                decryptedString = await cryptography.FunctionAsync(text, encDesc);
            }
            catch (Exception e)
            {
                // Debug.WriteLine("error", e.Message);
            }

            return decryptedString;
        }
     
    }
}