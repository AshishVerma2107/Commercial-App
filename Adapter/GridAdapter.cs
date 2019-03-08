using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using ComtaxApp.Model;

namespace ComtaxApp.Adapter
{
    public class GridAdapter : BaseAdapter
    {
        public override int Count => myList.Count;

        private Context mContext;
        public static List<ImageListModel> myList;

        public GridAdapter(Context c, List<ImageListModel> mList)
        {
            mContext = c;
            myList = mList;


        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View grid;
            LayoutInflater inflater = (LayoutInflater)mContext
                    .GetSystemService(Context.LayoutInflaterService);

          
            grid = new View(mContext);
            grid = inflater.Inflate(Resource.Layout.grid_layout, null);
            TextView textView = (TextView)grid.FindViewById(Resource.Id.grid_text);
            ImageView imageView = (ImageView)grid.FindViewById(Resource.Id.grid_image);
          

        
                BitmapFactory.Options bmOptions = new BitmapFactory.Options();
                bmOptions.InJustDecodeBounds = false;
                bmOptions.InSampleSize = 4;
                bmOptions.InPurgeable = true;
                Bitmap bitmap = BitmapFactory.DecodeFile(myList[position].ImagePath, bmOptions);
                imageView.SetImageBitmap(bitmap);

         
            if (convertView != null)
            {
                if (myList[position].Status.Equals("1"))
                {
                    imageView.SetBackgroundColor(Android.Graphics.Color.AntiqueWhite);// this is a selected position so make it red
                }
                else
                {
                    imageView.SetBackgroundColor(Android.Graphics.Color.White); //default color
                }
            }
            
            return grid;

        }

        public void setNewSelection(int position)
        {
            myList[position].Status.Equals("1");
            NotifyDataSetChanged();
        }

       


        public void removeSelection(int position)
        {
            myList[position].Status.Equals("0");
            NotifyDataSetChanged();

        }

       
    }
}