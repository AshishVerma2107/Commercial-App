using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.OS;
using Android.Preferences;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using ComtaxApp.Database;
using ComtaxApp.Fragments;
using ComtaxApp.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlertDialog = Android.Support.V7.App.AlertDialog;

namespace ComtaxApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {

        DBHelper dba;
        ISharedPreferences prefs;
        ServiceHelper service;
        InternetConnection con;
        Geolocation geo;
        BlobFileUpload ic;
        Boolean trick = true;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());
            StrictMode.ThreadPolicy.Builder builder1 = new StrictMode.ThreadPolicy.Builder().PermitAll();
            StrictMode.SetThreadPolicy(builder1.Build());
            
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            prefs = PreferenceManager.GetDefaultSharedPreferences(ApplicationContext);
            dba = new DBHelper();
            service = new ServiceHelper();
            con = new InternetConnection();
            geo = new Geolocation();
            ic = new BlobFileUpload(this);
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);

            View header = navigationView.GetHeaderView(0);

            string userName = prefs.GetString("UserName", "");
            string number = prefs.GetString("MobileNumber", "");

            TextView name1 = (TextView)header.FindViewById(Resource.Id.nameInst);
            TextView mobile1 = (TextView)header.FindViewById(Resource.Id.numberInst);
            name1.Text = userName;
            mobile1.Text = number;

            DisplayLocationSettingsRequest();

            SupportFragmentManager.BeginTransaction().Replace(Resource.Id.container, new GSTNVerification()).Commit();
        }

        private bool DisplayLocationSettingsRequest()
        {
            bool islocationOn = false;
            var googleApiClient = new GoogleApiClient.Builder(this).AddApi(LocationServices.API).Build();
            googleApiClient.Connect();

            var locationRequest = LocationRequest.Create();
            locationRequest.SetPriority(LocationRequest.PriorityHighAccuracy);
            locationRequest.SetInterval(10000);
            locationRequest.SetFastestInterval(10000 / 2);

            var builder = new LocationSettingsRequest.Builder().AddLocationRequest(locationRequest);
            builder.SetAlwaysShow(true);

            var result = LocationServices.SettingsApi.CheckLocationSettings(googleApiClient, builder.Build());
            result.SetResultCallback((LocationSettingsResult callback) =>
            {
                switch (callback.Status.StatusCode)
                {
                    case LocationSettingsStatusCodes.Success:
                        {
                            islocationOn = true;
                            //DoStuffWithLocation();
                            break;
                        }
                    case LocationSettingsStatusCodes.ResolutionRequired:
                        {
                            try
                            {
                                // Show the dialog by calling startResolutionForResult(), and check the result
                                // in onActivityResult().
                                callback.Status.StartResolutionForResult(this, 100);
                            }
                            catch (IntentSender.SendIntentException e)
                            {
                            }

                            break;
                        }
                    default:
                        {
                            // If all else fails, take the user to the android location settings
                            StartActivity(new Intent(Android.Provider.Settings.ActionLocationSourceSettings));
                            break;
                        }
                }
            });
            return islocationOn;
        }

     


        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if (drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Exit");
                    alert.SetMessage("Do you want to close App.");
                    alert.SetPositiveButton(("Yes"), (sender, args) =>
                    {
                        this.FinishAffinity();
                    });
                    alert.SetNegativeButton(("No"), (sender, args) =>
                    {

                    });
                    Dialog dialog = alert.Create();
                    dialog.Show();
               
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                if (trick)
                {
                    trick = false;
                    ManualSyncData();
                }
            }

            return base.OnOptionsItemSelected(item);
        }



        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            Android.Support.V4.App.Fragment fragment = null;

            //if (id == Resource.Id.nav_challan)
            //{
               
            //}
            //else if (id == Resource.Id.nav_purchase)
            //{

            //}
            //else if (id == Resource.Id.nav_sale)
            //{

            //}
            //else if (id == Resource.Id.nav_tax)
            //{

            //}
            if (id == Resource.Id.nav_verification)
            {
                fragment = new GSTNVerification();
            }

            if (fragment != null)
            {
                SupportFragmentManager.BeginTransaction().Replace(Resource.Id.container, fragment).Commit();
            }

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }

        public async Task ManualSyncData()
        {
            try
            {
                List<GSTINModel> dataList = dba.getGSTINSurveyDetail("no");
                if (dataList != null && dataList.Count > 0)
                {
                    foreach (var val in dataList)
                    {
                        List<GSTINVerFiles1> imageList = new List<GSTINVerFiles1>();
                        int Id = val.Id;
                        List<GSTINVerFiles1> gstinImages = dba.getGSTINImages(Id);
                        for (int i = 0; i < gstinImages.Count; i++)
                        {
                            byte[] img = GetStreamFromFile(gstinImages[i].VerFileName);
                            var url = await ic.UploadPhotoAsync(img, gstinImages[i].VerFileName.Substring(gstinImages[i].VerFileName.LastIndexOf("/") + 1));

                            if (url != null)
                            {
                                imageList.Add(new GSTINVerFiles1
                                {
                                    VerFileName = url,
                                    GeoLocation = geo.GetGeoLocation(this),
                                });
                            }
                        }

                        GSTINModel model = new GSTINModel();
                        model.GeoLocation = val.GeoLocation;
                        model.GSTINID = val.GSTINID;
                        model.WentNotVerified = val.WentNotVerified;
                        model.AppDateTime = val.AppDateTime;
                        model.AddressStatus = val.AddressStatus;
                        model.BusinessStatus = val.BusinessStatus;
                        model.RegistrationAccording = val.RegistrationAccording;
                        model.Other = val.Other;
                        model.CompoundingEligible = val.CompoundingEligible;
                        model.PremisesArea = val.PremisesArea;
                        model.GSTINVerFiles = imageList;
                        model.ManufacturerOrTrader = val.ManufacturerOrTrader;
                        model.TurnOver = val.TurnOver;
                        model.versionName = val.versionName;

                        string dt = JsonConvert.SerializeObject(model);
                        string response = "";
                        try
                        {
                            response = await service.PostServiceMethod(this, "SetGSTINCertification", dt);
                        }
                        catch (Exception ex)
                        {

                        }
                        if (response.Contains("Success"))
                        {
                            trick = true;
                            Toast.MakeText(this, "Data Send Successfully.", ToastLength.Long).Show();
                            dba.updateGSTINStatus(Id);
                        }
                     
                    }
                }
            }catch(Exception ex)
            {

            }
        }

        public byte[] GetStreamFromFile(string filePath)
        {
            try
            {
                Android.Net.Uri uri = Android.Net.Uri.FromFile(new Java.IO.File(filePath));
                byte[] byteArray = System.IO.File.ReadAllBytes(uri.Path);
                return byteArray;
            }
            catch (System.Exception e)
            {
                return null;
            }
        }


    }
}

