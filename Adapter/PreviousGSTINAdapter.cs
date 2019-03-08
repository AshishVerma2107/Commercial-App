using System;
using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using ComtaxApp.Database;
using ComtaxApp.Model;

namespace ComtaxApp.Adapter
{
    public class PreviousGSTINAdapter : BaseAdapter<GSTINModel>
    {
        List<GSTINModel> gstins;
        Context context;
        Java.IO.File fileName1;
        ISharedPreferences prefs;
        DBHelper db;

        public PreviousGSTINAdapter(Context mContext, List<GSTINModel> gstinsList)
        {
            this.gstins = gstinsList;
            this.context = mContext;
        }

        public override GSTINModel this[int position]
        {
            get
            {
                return gstins[position];
            }
        }

        public override int Count
        {
            get
            {
                return gstins.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
           
            if (view == null)
            {
                //view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.layout1, parent, false);
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.previous_gstin_child_layout, null);
                TextView gstinText = (TextView)view.FindViewById(Resource.Id.gstiNumber);
                TextView nameText = (TextView)view.FindViewById(Resource.Id.nameTrader);
                ImageView statusImage = (ImageView)view.FindViewById(Resource.Id.imageViewStatus);

                gstinText.Text = gstins[position].GSTINID;
                nameText.Text = gstins[position].AppDateTime;

                try
                {
                    if (gstins[position].UploadStatus.Equals("yes"))
                    {
                        statusImage.SetImageResource(Resource.Drawable.ok_icon);
                    }
                    else
                    {
                        statusImage.SetImageResource(Resource.Drawable.notok_icon);
                    }

                }catch(Exception ex)
                {

                }


            }
          
           
            return view;

        }

    }
}