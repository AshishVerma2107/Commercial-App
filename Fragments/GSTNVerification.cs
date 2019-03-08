using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Preferences;
using Android.Provider;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using ComtaxApp.Adapter;
using ComtaxApp.Database;
using ComtaxApp.Model;
using Java.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Json;
using System.Threading.Tasks;

namespace ComtaxApp.Fragments
{
    public class GSTNVerification : Fragment
    {

        View rootView;
        TextInputEditText searchGST;
        Geolocation geo;
        TextView tradeName1, gstnIdText1, regDate1, mobileNumber1;
        LinearLayout  gridLayout, linear1, linear2, linear3;
        Button  submitDataVer;
        ImageButton cameraVer;
        GridView gridViewVer;
        CheckBox checkValidate;
        ProgressBar gstnProgress;
        InternetConnection con;
        ISharedPreferences prefs;
        String GSTIN;
        String phoneNumber;
        RadioGroup rg1, rg2, rg3;
        LinearLayout rg4;
        TextInputEditText otherText, areaBusiness, turnoverText;
        List<GSTINServerModel> result;
        ServiceHelper restService;
        Button businessSelectButton;
        RadioButton Yes1, No1, Yes2, No2, Yes3, No3; 
        CheckBox Manufacturer, Trader, ServiceDealer;
        string address = "", businessStatus = "", registration = "", manuf = "", trad = "", dealer = "", combine= "";

        string other = "";
        string compoundingEligible = "";
        string premisesArea = "";
        string turnOver = "";

        public static GridAdapter grid_adapter;

        public static File _file;
        string file_path;
        string geolocation;

        BlobFileUpload ic;

        DBHelper dba;


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            geo = new Geolocation();
            restService = new ServiceHelper();
            ic = new BlobFileUpload(Activity);
            dba = new DBHelper();
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            rootView = inflater.Inflate(Resource.Layout.gstn_verification, container, false);

            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());
            StrictMode.ThreadPolicy.Builder builder1 = new StrictMode.ThreadPolicy.Builder().PermitAll();
            StrictMode.SetThreadPolicy(builder1.Build());


            con = new InternetConnection();

            searchGST = (TextInputEditText)rootView.FindViewById(Resource.Id.searchGSTN);
            gstnProgress = (ProgressBar)rootView.FindViewById(Resource.Id.progressGST);
            tradeName1 = (TextView)rootView.FindViewById(Resource.Id.tradeName);
            gstnIdText1 = (TextView)rootView.FindViewById(Resource.Id.gstnId);
            regDate1 = (TextView)rootView.FindViewById(Resource.Id.regDate);
            mobileNumber1 = (TextView)rootView.FindViewById(Resource.Id.mobileNumberVer);
            linear1 = (LinearLayout)rootView.FindViewById(Resource.Id.linearQuestion1);
            linear2 = (LinearLayout)rootView.FindViewById(Resource.Id.linearQuestion2);
            linear3 = (LinearLayout)rootView.FindViewById(Resource.Id.linearQuestion3);
            gridLayout = (LinearLayout)rootView.FindViewById(Resource.Id.gridVer);
            submitDataVer = (Button)rootView.FindViewById(Resource.Id.submitVerification);
            cameraVer = (ImageButton)rootView.FindViewById(Resource.Id.cameraVer);
            gridViewVer = (GridView)rootView.FindViewById(Resource.Id.gridViewVer);
            checkValidate = (CheckBox)rootView.FindViewById(Resource.Id.checkVerification);
            rg1 = (RadioGroup)rootView.FindViewById(Resource.Id.myRadioGroup1);
            rg2 = (RadioGroup)rootView.FindViewById(Resource.Id.myRadioGroup2);
            rg3 = (RadioGroup)rootView.FindViewById(Resource.Id.myRadioGroup3);
            rg4 = (LinearLayout)rootView.FindViewById(Resource.Id.myRadioGroup4);
            Yes1 = (RadioButton)rootView.FindViewById(Resource.Id.yes1);
            Yes2 = (RadioButton)rootView.FindViewById(Resource.Id.yes2);
            Yes3 = (RadioButton)rootView.FindViewById(Resource.Id.yes3);
            No1 = (RadioButton)rootView.FindViewById(Resource.Id.no1);
            No2 = (RadioButton)rootView.FindViewById(Resource.Id.no2);
            No3 = (RadioButton)rootView.FindViewById(Resource.Id.no3);
            Manufacturer = (CheckBox)rootView.FindViewById(Resource.Id.manufacturer);
            Trader = (CheckBox)rootView.FindViewById(Resource.Id.trader);
            ServiceDealer = (CheckBox)rootView.FindViewById(Resource.Id.serviceDealer);
            turnoverText = (TextInputEditText)rootView.FindViewById(Resource.Id.turnOver);
            otherText = (TextInputEditText)rootView.FindViewById(Resource.Id.otherText1);
            areaBusiness = (TextInputEditText)rootView.FindViewById(Resource.Id.area1);
            businessSelectButton = (Button)rootView.FindViewById(Resource.Id.mySelection);

            grid_adapter = new GridAdapter(Activity, Utilities.imageList);
            gridViewVer.Adapter = grid_adapter;
            
            tradeName1.Visibility = ViewStates.Gone;
            gstnIdText1.Visibility = ViewStates.Gone;
            regDate1.Visibility = ViewStates.Gone;

            mobileNumber1.Visibility = ViewStates.Gone;
          
            gridLayout.Visibility = ViewStates.Gone;
            
            submitDataVer.Visibility = ViewStates.Gone;

            cameraVer.Visibility = ViewStates.Gone;
            gridViewVer.Visibility = ViewStates.Gone;
            checkValidate.Visibility = ViewStates.Gone;

            linear1.Visibility = ViewStates.Gone;
            linear2.Visibility = ViewStates.Gone;
            linear3.Visibility = ViewStates.Gone;

            otherText.Visibility = ViewStates.Gone;
            areaBusiness.Visibility = ViewStates.Gone;
            turnoverText.Visibility = ViewStates.Gone;
            rg4.Visibility = ViewStates.Gone;
            businessSelectButton.Visibility = ViewStates.Gone;


            Yes1.Click += RadioButtonClick;
            No1.Click += RadioButtonClick;
            Yes2.Click += RadioButtonClick;
            No2.Click += RadioButtonClick;
            Yes3.Click += RadioButtonClick;
            No3.Click += RadioButtonClick;

            businessSelectButton.Click += delegate
            {
                try
                {

                    Intent i = new Intent(Activity, typeof(BusinessTypeFragment));
                    Activity.StartActivity(i);

                    //FragmentTransaction ft = FragmentManager.BeginTransaction();
                    //BusinessTypeFragment f4 = new BusinessTypeFragment();
                    //ft.Replace(Resource.Id.container, f4);
                    //ft.AddToBackStack("BusinessTypeFragment");
                    //ft.Commit();
                   // FragmentManager.BeginTransaction().Add(Resource.Id.container, new BusinessTypeFragment()).Commit();
                }catch(Exception ex)
                {

                }
            };



            mobileNumber1.Click += delegate
            {
                AlertDialog.Builder alertDiag = new AlertDialog.Builder(Activity);
                alertDiag.SetTitle("Call Dialog");
                alertDiag.SetMessage("Do you want to call?");
                alertDiag.SetPositiveButton("Call", (senderAlert, args) => {
                    try
                    {
                        var uri = Android.Net.Uri.Parse("tel:" + phoneNumber);
                        var intent = new Intent(Intent.ActionDial, uri);
                        StartActivity(intent);
                    }catch(Exception e)
                    {

                    }
                });
                alertDiag.SetNegativeButton("Cancel", (senderAlert, args) => {
                    alertDiag.Dispose();
                });
                Android.App.Dialog diag = alertDiag.Create();
                diag.Show();
            };

            cameraVer.Click += delegate
            {
                CameraPic();
            };

            searchGST.TextChanged += delegate
            {
                if (searchGST.Text.ToString().Length >= 15)
                {
                    string searchText = searchGST.Text.ToString();
                    if (con.connectivity())
                    {
                        GSTNSearch(searchText).Wait();
                    }
                    else
                    {
                        Toast.MakeText(Activity, "Please connect to Internet.", ToastLength.Long).Show();
                    }
                }
                else
                {
                    tradeName1.Visibility = ViewStates.Gone;
                    gstnIdText1.Visibility = ViewStates.Gone;
                    regDate1.Visibility = ViewStates.Gone;
                    mobileNumber1.Visibility = ViewStates.Gone;
                    
                    gridLayout.Visibility = ViewStates.Gone;
                    checkValidate.Visibility = ViewStates.Gone;
                    submitDataVer.Visibility = ViewStates.Gone;

                    cameraVer.Visibility = ViewStates.Gone;
                    gridViewVer.Visibility = ViewStates.Gone;
                    linear1.Visibility = ViewStates.Gone;
                    linear2.Visibility = ViewStates.Gone;
                    linear3.Visibility = ViewStates.Gone;

                    otherText.Visibility = ViewStates.Gone;
                    areaBusiness.Visibility = ViewStates.Gone;
                    turnoverText.Visibility = ViewStates.Gone;
                    rg4.Visibility = ViewStates.Gone;
                    businessSelectButton.Visibility = ViewStates.Gone;
                }
            };

            submitDataVer.Click += delegate {

                

                other = otherText.Text.ToString();
                premisesArea = areaBusiness.Text.ToString();
                if (checkValidate.Selected) {
                    compoundingEligible = "Yes";
                }else{
                    compoundingEligible = "No";
                }

                //if (Manufacturer.Selected)
                //{
                //    manuf = "Manufacturer";
                //}
                //if (Trader.Selected)
                //{
                //    trad = "Trader";
                //}
                //if (ServiceDealer.Selected)
                //{
                //    dealer = "Service Dealer";
                //}

                turnOver = turnoverText.Text.ToString();
                //if (!manuf.Equals(""))
                //{
                //    if (!combine.Equals(""))
                //    {
                //        combine += "," + manuf;
                //    }
                //    else
                //    {
                //        combine += manuf;
                //    }
                //}
                //if (!trad.Equals(""))
                //{
                //    if (!combine.Equals(""))
                //    {
                //        combine += "," + trad;
                //    }
                //    else
                //    {
                //        combine += trad;
                //    }
                //}
                //if (!dealer.Equals(""))
                //{
                //    if (!combine.Equals(""))
                //    {
                //        combine += "," + dealer;
                //    }
                //    else
                //    {
                //        combine += dealer;
                //    }
                //}
                try
                {
                    combine = string.Join(",", Utilities.GlobalBusinessList);
                }catch(Exception ex)
                {
                    combine = "";
                }


                if (address.Equals(""))
                {
                    Toast.MakeText(Activity, "Please Select Address Status", ToastLength.Short).Show();
                    return;
                }
                if (businessStatus.Equals(""))
                {
                    Toast.MakeText(Activity, "Please Select Business Status", ToastLength.Short).Show();
                    return;
                }

                if (registration.Equals(""))
                {
                    Toast.MakeText(Activity, "Please Select Registration Status", ToastLength.Short).Show();
                    return;
                }

                if (Utilities.imageList.Count < 1)
                {
                    Toast.MakeText(Activity, "Please Capture Atleast One Photograph.", ToastLength.Short).Show();
                    return;
                }

             
                if (con.connectivity())
                {
                    submitDataVer.Enabled = false;
                    try
                    {
                        sendToServer();
                    }catch(Exception ex)
                    {
                        Toast.MakeText(Activity, "Something went wrong. Please try after sometime.", ToastLength.Long).Show();
                  
                        PackageManager manager = Activity.PackageManager;
                        PackageInfo info = manager.GetPackageInfo(Activity.PackageName, 0);
                        info.VersionName.ToString();
                        int i = dba.insertGSTINData(geo.GetGeoLocation(Activity), GSTIN, "", DateTime.Now.ToString("yyyy-MM-dd"), 
                            address, businessStatus, registration, other, compoundingEligible, premisesArea, 
                            combine, turnOver, info.VersionName.ToString(), "no");

                        for (int j = 0; j < Utilities.imageList.Count; j++) {
                            dba.insertImageDetail(i, Utilities.imageList[j].ImagePath, geo.GetGeoLocation(Activity));
                            }
                    }
                }
                else
                {
                    submitDataVer.Enabled = false;
                    PackageManager manager = Activity.PackageManager;
                    PackageInfo info = manager.GetPackageInfo(Activity.PackageName, 0);
                    info.VersionName.ToString();
                    int i = dba.insertGSTINData(geo.GetGeoLocation(Activity), GSTIN, "", DateTime.Now.ToString("yyyy-MM-dd"),
                        address, businessStatus, registration, other, compoundingEligible, premisesArea,
                        combine, turnOver, info.VersionName.ToString(), "no");

                    for (int j = 0; j < Utilities.imageList.Count; j++)
                    {
                        dba.insertImageDetail(i, Utilities.imageList[j].ImagePath, geo.GetGeoLocation(Activity));
                    }
                    Toast.MakeText(Activity, "Data Saved in Database.", ToastLength.Long).Show();
                    
                    combine = "";
                    GSTIN = "";
                    rg1.ClearCheck();
                    address = "";
                    rg2.ClearCheck();
                    businessStatus = "";
                    rg3.ClearCheck();
                    registration = "";
                    other = "";
                    otherText.Text = "";
                    checkValidate.Checked = false;
                    compoundingEligible = "";
                    premisesArea = "";
                    areaBusiness.Text = "";
                    turnoverText.Text = "";
                    Utilities.imageList.Clear();
                    turnOver = "";
                    Manufacturer.Checked = false;
                    Trader.Checked = false;
                    ServiceDealer.Checked = false;
                    submitDataVer.Enabled = true;
                    manuf = "";
                    trad = "";
                    dealer = "";
                    grid_adapter.NotifyDataSetChanged();
                    Utilities.GlobalBusinessList.Clear();

                }



            };

            return rootView;
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

        public async Task sendToServer()
        {

            List<GSTINVerFiles1> imageList = new List<GSTINVerFiles1>();
            for (int i = 0; i < Utilities.imageList.Count; i++)
            {

                byte[] img = GetStreamFromFile(Utilities.imageList[i].ImagePath);
                var url = await ic.UploadPhotoAsync(img, Utilities.imageList[i].ImagePath.Substring(Utilities.imageList[i].ImagePath.LastIndexOf("/") + 1));

                if (url != null)
                {
                    imageList.Add(new GSTINVerFiles1
                    {
                        VerFileName = url,
                        GeoLocation = geo.GetGeoLocation(Activity),
                    });
                }
            }

            GSTINModel model = new GSTINModel();
            model.GeoLocation = geo.GetGeoLocation(Activity);
            model.GSTINID = GSTIN;
            model.WentNotVerified = "";
            model.AppDateTime = DateTime.Now.ToString("yyyy-MM-dd");
            model.AddressStatus = address;
            model.BusinessStatus = businessStatus;
            model.RegistrationAccording = registration;
            model.Other = other;
            model.CompoundingEligible = compoundingEligible;
            model.PremisesArea = premisesArea;
            model.GSTINVerFiles = imageList;
            model.ManufacturerOrTrader = combine;
            model.TurnOver = turnOver;
            PackageManager manager = Activity.PackageManager;
            PackageInfo info = manager.GetPackageInfo(Activity.PackageName, 0);
            model.versionName = info.VersionName.ToString();

            string dt = JsonConvert.SerializeObject(model);
            string response = "";
            try
            {
                response = await restService.PostServiceMethod(Activity, "SetGSTINCertification", dt);
            }catch(Exception ex)
            {

            }
            if (response.Contains("Success"))
            {
                Toast.MakeText(Activity, "Data Submitted Successfully.", ToastLength.Long).Show();
                GSTIN = "";
                rg1.ClearCheck();
                address = "";
                rg2.ClearCheck();
                businessStatus = "";
                rg3.ClearCheck();
                registration = "";
                other = "";
                otherText.Text = "";
                checkValidate.Checked = false;
                compoundingEligible = "";
                premisesArea = "";
                areaBusiness.Text = "";
                turnoverText.Text = "";
                Utilities.imageList.Clear();
                combine = "";
                turnOver = "";
                submitDataVer.Enabled = true;
                Manufacturer.Checked = false;
                Trader.Checked = false;
                ServiceDealer.Checked = false;
                manuf = "";
                trad = "";
                dealer = "";
                grid_adapter.NotifyDataSetChanged();
                info.VersionName.ToString();
                Utilities.GlobalBusinessList.Clear();
                try
                {
                    int i = dba.insertGSTINData(geo.GetGeoLocation(Activity), GSTIN, "", DateTime.Now.ToString("yyyy-MM-dd"),
                        address, businessStatus, registration, other, compoundingEligible, premisesArea,
                        combine, turnOver, info.VersionName.ToString(), "yes");

                    for (int j = 0; j < imageList.Count; j++)
                    {
                        dba.insertImageDetail(i, imageList[j].VerFileName, geo.GetGeoLocation(Activity));
                    }
                }catch(Exception ex)
                {

                }
            }
            else
            {
                Toast.MakeText(Activity, "Something went wrong. Please try after sometime.", ToastLength.Long).Show();
                submitDataVer.Enabled = true;
                int i = dba.insertGSTINData(geo.GetGeoLocation(Activity), GSTIN, "", DateTime.Now.ToString("yyyy-MM-dd"),
                       address, businessStatus, registration, other, compoundingEligible, premisesArea,
                       combine, turnOver, info.VersionName.ToString(), "no");

                for (int j = 0; j < Utilities.imageList.Count; j++)
                {
                    dba.insertImageDetail(i, Utilities.imageList[j].ImagePath, geo.GetGeoLocation(Activity));
                }
            }
        }

        private void RadioButtonClick(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            int id = rb.Id;
           
            if(id == Resource.Id.yes1)
            {
                address = "Yes";
            }
            if (id == Resource.Id.no1)
            {
                address = "No";
            }
            if (id == Resource.Id.yes2)
            {
                businessStatus = "Yes";
            }
            if (id == Resource.Id.no2)
            {
                businessStatus = "No";
            }
            if (id == Resource.Id.yes3)
            {
                registration = "Yes";
            }
            if (id == Resource.Id.no3)
            {
                registration = "No";
            }
           
        }

        public void CameraPic()
        {
          TakeAPicture();
        }


        async private void TakeAPicture()
        {
            try
            {
                File _dir = new File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), "ComTax");
                string file_path = _dir.ToString();
                if (!_dir.Exists())
                {
                    _dir.Mkdirs();
                }
                try
                {
                    string FileName = Utilities.fileName();
                    Utilities.imageName1 = FileName;
                    Intent intent = new Intent(MediaStore.ActionImageCapture);
                    _file = new File(_dir, string.Format(FileName, Guid.NewGuid()));
                    Utilities.imageURL1 = _file.AbsolutePath;
                    intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(_file));
                    intent.AddFlags(ActivityFlags.GrantReadUriPermission);
                    StartActivityForResult(intent, 100);
                }
                catch (Exception e)
                {
                    Log.Error("Error In Capture", e.Message);
                }


            }
            catch (Exception e) { }

        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            if (requestCode == 100 && resultCode == (int)Android.App.Result.Ok)
            {
                 int height = Resources.DisplayMetrics.HeightPixels;
                int width = Resources.DisplayMetrics.WidthPixels;
               
                Utilities.imageList.Add(new ImageListModel
                {

                    GeoLocation = geolocation,
                    ImagePath = _file.AbsolutePath,
                    ImageName = Utilities.imageName1,
                    Status = "0",
                });
                grid_adapter.NotifyDataSetChanged();
            }
        }

       
        private async Task GSTNSearch(string search_string)
        {

            String errorMsg = "", panNo, stateCode, mobileNo = "", emailId, tradeName = "", regName = "", compInd, signInd, gstnId, assignedTo,
               stateJurs, CntJurs, RegStartDate = "", liabDate, fDate;
            prefs = PreferenceManager.GetDefaultSharedPreferences(Activity);
            string licenceid = prefs.GetString("LicenceId", "");
            string liceId = prefs.GetString("LicenceId", "");
            string desigId = prefs.GetString("DesignationId", "");
            //com.work121.ComTaxService service = new com.work121.ComTaxService();
            //com.work121.GSTINData[] result = null;
            dynamic value = new ExpandoObject();
            value.searchkey = search_string;
            string json = JsonConvert.SerializeObject(value);
            try
            {

                JsonValue item = await restService.GetServiceMethod(Activity, "GetGSTINData", json).ConfigureAwait(false);
                result = JsonConvert.DeserializeObject<List<GSTINServerModel>>(item);
                Activity.RunOnUiThread(() => {
                    if (result != null)
                    {
                        for (int i = 0; i < result.Count; i++)
                        {
                            errorMsg = result[i].ErrorMessage;
                            panNo = result[i].PANCardNo;
                            stateCode = result[i].StateCode;
                            mobileNo = result[i].MobileNo;
                            emailId = result[i].EmailID;
                            tradeName = result[i].TradeName;
                            regName = result[i].RegName;
                            compInd = result[i].CompInd;
                            signInd = result[i].SignInd;
                            gstnId = result[i].GSTINID;
                            assignedTo = result[i].AssignedTo;
                            stateJurs = result[i].StateJursdCd;
                            CntJurs = result[i].CntrJursdCd;
                            RegStartDate = result[i].RegStartDate;
                            liabDate = result[i].LiabDate;
                            fDate = result[i].FDate;
                            GSTIN = gstnId;
                            phoneNumber = mobileNo;
                        }

                        if (result != null)
                        {
                            if (!errorMsg.Contains("ER"))
                            {
                                try
                                {
                                    tradeName1.Visibility = ViewStates.Visible;
                                    gstnIdText1.Visibility = ViewStates.Visible;
                                    regDate1.Visibility = ViewStates.Visible;
                                    mobileNumber1.Visibility = ViewStates.Visible;
                                    
                                    gridLayout.Visibility = ViewStates.Visible;
                                    
                                    submitDataVer.Visibility = ViewStates.Visible;
                                    checkValidate.Visibility = ViewStates.Visible;
                                    cameraVer.Visibility = ViewStates.Visible;
                                    gridViewVer.Visibility = ViewStates.Visible;

                                    linear1.Visibility = ViewStates.Visible;
                                    linear2.Visibility = ViewStates.Visible;
                                    linear3.Visibility = ViewStates.Visible;

                                    otherText.Visibility = ViewStates.Visible;
                                    areaBusiness.Visibility = ViewStates.Visible;
                                    turnoverText.Visibility = ViewStates.Visible;
                                    businessSelectButton.Visibility = ViewStates.Visible;

                                    tradeName1.Text = "Trade Name - " + tradeName;
                                    gstnIdText1.Text = "Reg. Name - " + regName;
                                    regDate1.Text = "Registration Date - " + RegStartDate;
                                    mobileNumber1.Text = "Mobile Number - " + mobileNo;
                                    rg4.Visibility = ViewStates.Gone;

                                }
                                catch (Exception e)
                                {

                                }
                            }
                            else
                            {
                                tradeName1.Visibility = ViewStates.Visible;
                                tradeName1.Text = "Enter a valid GSTN";
                                gstnIdText1.Visibility = ViewStates.Gone;
                                regDate1.Visibility = ViewStates.Gone;
                                mobileNumber1.Visibility = ViewStates.Gone;
                                checkValidate.Visibility = ViewStates.Gone;
                                gridLayout.Visibility = ViewStates.Gone;
                                
                                submitDataVer.Visibility = ViewStates.Gone;

                                cameraVer.Visibility = ViewStates.Gone;
                                gridViewVer.Visibility = ViewStates.Gone;
                                linear1.Visibility = ViewStates.Gone;
                                linear2.Visibility = ViewStates.Gone;
                                linear3.Visibility = ViewStates.Gone;

                                otherText.Visibility = ViewStates.Gone;
                                areaBusiness.Visibility = ViewStates.Gone;
                                turnoverText.Visibility = ViewStates.Gone;
                                rg4.Visibility = ViewStates.Gone;
                                businessSelectButton.Visibility = ViewStates.Gone;
                            }
                        }
                        else
                        {
                            tradeName1.Visibility = ViewStates.Visible;
                            tradeName1.Text = "No data found.";
                            gstnIdText1.Visibility = ViewStates.Gone;
                            regDate1.Visibility = ViewStates.Gone;
                            mobileNumber1.Visibility = ViewStates.Gone;
                            
                            gridLayout.Visibility = ViewStates.Gone;
                            checkValidate.Visibility = ViewStates.Gone;
                            submitDataVer.Visibility = ViewStates.Gone;

                            cameraVer.Visibility = ViewStates.Gone;
                            gridViewVer.Visibility = ViewStates.Gone;
                            linear1.Visibility = ViewStates.Gone;
                            linear2.Visibility = ViewStates.Gone;
                            linear3.Visibility = ViewStates.Gone;

                            otherText.Visibility = ViewStates.Gone;
                            areaBusiness.Visibility = ViewStates.Gone;
                            turnoverText.Visibility = ViewStates.Gone;
                            rg4.Visibility = ViewStates.Gone;
                            businessSelectButton.Visibility = ViewStates.Gone;
                        }

                    }
                });
               

            }
            catch(Exception ex)
            {

            }
           
        }
    }
}