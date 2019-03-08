using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using ComtaxApp.Adapter;
using ComtaxApp.Database;
using ComtaxApp.Model;
using System.Collections.Generic;

namespace ComtaxApp.Fragments
{
    public class PreviousVerifications : Fragment
    {

        View rootView;
        TextInputEditText searchPreviousGST;
        ListView previousList;
        DBHelper dba;
        List<GSTINModel> previousLists = new List<GSTINModel>();
        PreviousGSTINAdapter adapter;


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            rootView = inflater.Inflate(Resource.Layout.previous_gstin_layout, container, false);

            searchPreviousGST = (TextInputEditText)rootView.FindViewById(Resource.Id.searchPreviousGSTN);
            previousList = (ListView)rootView.FindViewById(Resource.Id.previousGSTNList);
            previousLists = dba.getGSTINAllSurvey();
            adapter = new PreviousGSTINAdapter(Activity, previousLists);
            previousList.SetAdapter(adapter);

            return rootView;
        }
    }
}