using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using ComtaxApp.Model;

namespace ComtaxApp.Adapter
{
    class BusinessListAdapter : BaseAdapter<BusinessModel>
    {
        List<BusinessModel> businesses;
        Context context;
        Java.IO.File fileName1;
        ISharedPreferences prefs;
       

        public BusinessListAdapter(Context mContext, List<BusinessModel> businessList)
        {
            this.businesses = businessList;
            this.context = mContext;
        }

        public override BusinessModel this[int position]
        {
            get
            {
                return businesses[position];
            }
        }

        public override int Count
        {
            get
            {
                return businesses.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            ViewHolder holder;

            var local = new LocalOnClickListener();

            if (view == null)
            {
                holder = new ViewHolder();
                //view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.layout1, parent, false);
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.business_List_Child, null);
                holder.HeadingText = (TextView)view.FindViewById(Resource.Id.headingView);
                holder.BusinessType = (TextView)view.FindViewById(Resource.Id.selectOption);
                holder.BusinessMark = (CheckBox)view.FindViewById(Resource.Id.checkBox);
                //holder.HeadingText.Visibility = ViewStates.Gone;
                view.Tag = holder;
            }
            else
            {
                holder = (ViewHolder) view.Tag;
            }

             holder.BusinessType.Text = businesses[position].BusinessSubType;

            string currentText = businesses[position].BusinessType;
            string previousText = null;

            if(position > 0)
            {
                previousText = businesses[position - 1].BusinessType;
            }

            if(previousText==null || !previousText.Equals(currentText))
            {
                holder.HeadingText.Visibility = ViewStates.Visible;
                holder.HeadingText.Text = currentText;
            }
            else
            {
                holder.HeadingText.Visibility = ViewStates.Gone;
            }

            local.HandleOnClick = () =>
            {
                if (holder.BusinessMark.Checked)
                {
                    Utilities.GlobalBusinessList.Add(businesses[position].BusinessTypeID);
                }
                else
                {
                    var item = Utilities.GlobalBusinessList.FindIndex(x => x == businesses[position].BusinessTypeID);
                    if (item >= 0)
                    {   
                        Utilities.GlobalBusinessList.RemoveAt(item);
                    }
                }
            };

            holder.BusinessMark.SetOnClickListener(local);

            return view;
        }

    }
}