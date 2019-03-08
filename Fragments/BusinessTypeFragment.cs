using Android.App;
using Android.OS;
using Android.Preferences;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using ComtaxApp.Adapter;
using ComtaxApp.Database;
using ComtaxApp.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Json;
using System.Threading.Tasks;

namespace ComtaxApp.Fragments
{
    [Activity(Label = "@string/app_name")]
    public class BusinessTypeFragment : AppCompatActivity
    {
        View rootView;
        ListView businessListView;
        ServiceHelper service;
        BusinessListAdapter adapter;
        List<BusinessModel> result;
        List<BusinessModel> values = new List<BusinessModel>();
        DBHelper dba;

      

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.business_type_fragment);
            service = new ServiceHelper();
            dba = new DBHelper();

            businessListView = (ListView)FindViewById(Resource.Id.businessList);

            BusinessListLoad().Wait();
        }


        private async Task BusinessListLoad()
        {
           
            string json = JsonConvert.SerializeObject("{}");
            try
            {

                JsonValue item = await service.GetServiceMethod(this, "GetBusinessTypeData", json).ConfigureAwait(false);
                result = JsonConvert.DeserializeObject<List<BusinessModel>>(item);
                if (result != null)
                {
                    dba.insertBusinessTypeData(result);
                    values = dba.getBusinessTypeData();
                    adapter = new BusinessListAdapter(this, values);
                    businessListView.SetAdapter(adapter);
                }
            }
            catch (Exception ex)
            {

            }

        }

      
    }
}