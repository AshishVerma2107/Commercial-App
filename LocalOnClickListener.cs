using Android.Views;

namespace ComtaxApp
{
    public class LocalOnClickListener : Java.Lang.Object, View.IOnClickListener
    {
        public void OnClick(View v)
        {
            HandleOnClick();
        }
        public System.Action HandleOnClick { get; set; }
    }
}