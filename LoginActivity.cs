using System;
using System.Collections.Generic;
using System.Json;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Preferences;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Util;
using Android.Widget;
using ComtaxApp.Model;

namespace ComtaxApp
{
    [Activity(Label = "ComTax-UP", MainLauncher = true)]
    public class LoginActivity : AppCompatActivity
    {
        EditText user, pass;
        Button log;
        ISharedPreferences prefs;
        LoginModel detail;
        ServiceHelper restService;
        string licenceid;
        string geolocation;
        InternetConnection ic;
        Geolocation geo;
        ProgressDialog progress;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());
            StrictMode.ThreadPolicy.Builder builder1 = new StrictMode.ThreadPolicy.Builder().PermitAll();
            StrictMode.SetThreadPolicy(builder1.Build());

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Login);

            ic = new InternetConnection();
            prefs = PreferenceManager.GetDefaultSharedPreferences(ApplicationContext);
            restService = new ServiceHelper();
            geo = new Geolocation();

            user = FindViewById<EditText>(Resource.Id.username);
            pass = FindViewById<EditText>(Resource.Id.pass);
            log = FindViewById<Button>(Resource.Id.login);

            log.Click += delegate {
                UserLogin();
            };

            main_method();

        }

        public async void main_method()
        {

            await GetPermissionAsync();
            try
            {
                licenceid = prefs.GetString("LicenceId", "");

                if (licenceid != null && licenceid != "")
                {
                    bool isRegistered = prefs.GetBoolean("IsRegistered", false);
                    if (isRegistered)
                    {
                        Intent intent = new Intent(this, typeof(MainActivity));
                        intent.AddFlags(ActivityFlags.NewTask);
                        StartActivity(intent);
                        Finish();

                    }
                    else
                    {

                    }
                }
                else
                {
                    await Get_Licence_Id();
                }
            }
            catch (Exception ex)
            {

            }

        }


        public void UserLogin()
        {
            Validate();

        }


        public async void Validate()
        {

            var errorMsg = "";
            if (user.Text.Length == 0 && pass.Text.Length == 0)
            {
                if (user.Text.Length == 0 || pass.Text.Length == 0)
                {
                    errorMsg = "Please enter User Name ";


                }
                if (pass.Text.Length == 0 || pass.Text.Length == 0)
                {
                    errorMsg = errorMsg + "Please enter Password";
                }

                Toast.MakeText(this, errorMsg, ToastLength.Long).Show();
                return;
            }
            else
            {
                Boolean result = ic.connectivity();
                if (result)
                {
                    progress = new ProgressDialog(this);
                    progress.Indeterminate = true;
                    progress.SetProgressStyle(ProgressDialogStyle.Spinner);
                    progress.SetCancelable(false);
                    progress.SetMessage("Please wait...");
                    progress.Show();
                    JsonValue login_value = null;
                    try
                    {
                        login_value = await nextActivity(user.Text, pass.Text);
                    }
                    catch (Exception e)
                    {

                    }
                    if (login_value != null) { 
                    await ParseAndDisplay(login_value, user.Text);
                }

                    //  loginId1 = user.Text;
                    // password1 = pass.Text;
                }
                else
                {

                    Toast.MakeText(this, "No Internet", ToastLength.Long).Show();
                }

            }
        }

        async Task ParseAndDisplay(JsonValue json, String login_Id)
        {
            if (json.Count > 0)
            {
                for (int i = 0; i < json.Count; i++)
                {
                    try
                    {
                        detail = new LoginModel
                        {
                            OrganizationId = json[i]["OrganizationId"],
                            Organization = json[i]["Organization"],
                            OfficeId = json[i]["OfficeId"],
                            OfficeName = json[i]["OfficeName"],
                            NaturalPersonId = json[i]["NaturalPersonId"],
                            UserName = json[i]["UserName"],
                            NpToOrgRelationID = json[i]["NpToOrgRelationID"],
                            DesignationId = json[i]["DesignationId"],
                            Designation = json[i]["Designation"],
                            MobileNumber = json[i]["MobileNumber"],
                            Message = json[i]["Message"],
                            ProjectArea = json[i]["ProjectArea"],
                            Controller = json[i]["Controller"],
                            ControllerAction = json[i]["ControllerAction"],
                            IsActive = json[i]["IsActive"].ToString(),
                            EmailAddress = json[i]["EmailAddress"]
                        };
                        //User_List.Add(detail);
                    }
                    catch (Exception e) { Log.Error("Error", e.Message); }

                }

                ISharedPreferencesEditor editor = prefs.Edit();
                editor.PutString("OrganizationId", detail.OrganizationId);
                editor.PutString("Organization", detail.Organization);
                editor.PutString("OfficeId", detail.OfficeId);
                editor.PutString("OfficeName", detail.OfficeName);
                editor.PutString("NaturalPersonId", detail.NaturalPersonId);
                editor.PutString("UserName", detail.UserName);
                editor.PutString("NpToOrgRelationID", detail.NpToOrgRelationID);
                editor.PutString("DesignationId", detail.DesignationId);
                editor.PutString("Designation", detail.Designation);
                editor.PutString("MobileNumber", detail.MobileNumber);
                editor.PutString("EmailAddress", detail.EmailAddress);

                editor.Apply();

                geolocation = geo.GetGeoLocation(ApplicationContext);

                if (detail.UserName != null && detail.UserName != "")
                {
                    try
                    {
                        string isRegistered = await restService.RegisterUser(licenceid, detail.MobileNumber, detail.UserName, geolocation,
                            detail.NaturalPersonId, detail.DesignationId).ConfigureAwait(false);

                        progress.Dismiss();

                        if (isRegistered.Contains("Success"))
                        {

                            editor.PutBoolean("IsRegistered", true);
                            editor.Commit();

                            Intent intent = new Intent(this, typeof(MainActivity));
                            intent.AddFlags(ActivityFlags.NewTask);
                            StartActivity(intent);
                            Finish();
                        }
                        else
                        {
                            progress.Dismiss();
                            Toast.MakeText(ApplicationContext, "Try after some time", ToastLength.Short).Show();
                        }
                    }
                    catch (Exception ex)
                    {
                        progress.Dismiss();
                        Toast.MakeText(ApplicationContext, "Try after some time", ToastLength.Short).Show();
                    }
                }
                else
                {
                    progress.Dismiss();
                    Toast.MakeText(ApplicationContext, "Invalid User name or Password", ToastLength.Short).Show();
                }
            }
            else
            {
                progress.Dismiss();
                Toast.MakeText(ApplicationContext, "Invalid User name or Password", ToastLength.Short).Show();
            }
        }

        async Task<JsonValue> nextActivity(string un, string p)
        {
            licenceid = prefs.GetString("LicenceId", "");
            if (licenceid == null || licenceid == "")
            {
                await Get_Licence_Id();
                licenceid = prefs.GetString("LicenceId", "");
            }

            geolocation = geo.GetGeoLocation(ApplicationContext);

            JsonValue item = await restService.LoginUser2(un, p, licenceid, geolocation).ConfigureAwait(false);

            return item;

        }

        public async Task<string> Get_Licence_Id()
        {
            string licenceId = "";
            Boolean connectivity = ic.connectivity();

            if (connectivity)
            {
                geolocation = geo.GetGeoLocation(ApplicationContext);

                licenceId = await restService.GetLicenceId(geolocation, "").ConfigureAwait(false);

                ISharedPreferencesEditor editor = prefs.Edit();
                editor.PutString("LicenceId", licenceId);
                editor.Commit();
            }
            else
            {
                Toast.MakeText(ApplicationContext, "No Internet, Try after sometime", ToastLength.Short).Show();

            }

            return licenceId;

        }



        private async Task GetPermissionAsync()
        {
            List<String> permissions = new List<String>();
            try
            {

                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) == Permission.Denied)
                {
                    permissions.Add(Manifest.Permission.AccessFineLocation);
                }

                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.Camera) == Permission.Denied)
                {
                    permissions.Add(Manifest.Permission.Camera);
                }

                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) == Permission.Denied)
                {
                    permissions.Add(Manifest.Permission.ReadExternalStorage);
                }

                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) == Permission.Denied)
                {
                    permissions.Add(Manifest.Permission.WriteExternalStorage);
                }

                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.CallPhone) == Permission.Denied)
                {
                    permissions.Add(Manifest.Permission.CallPhone);
                }

                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadPhoneState) == Permission.Denied)
                {
                    permissions.Add(Manifest.Permission.ReadPhoneState);
                }



                if (permissions.Count > 0)
                {
                    ActivityCompat.RequestPermissions(this, permissions.ToArray(), 100);
                }

            }
            catch (Exception e)
            {
                System.Console.WriteLine("Error", e.Message);
            }

        }
    }
}