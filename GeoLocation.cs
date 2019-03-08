using Android.Content;
using Android.Locations;

namespace ComtaxApp
{
    public class Geolocation
    {
        LocationManager _locationManager;
        public string GetGeoLocation(Context context)
        {
            string geoLocation = "";
            _locationManager = (LocationManager)context.GetSystemService(LocationManager.KeyLocationChanged);

            if (_locationManager.IsProviderEnabled(LocationManager.GpsProvider))
            {
                var criteria = new Criteria { PowerRequirement = Power.Medium };

                var bestProvider = _locationManager.GetBestProvider(criteria, true);
                var location = _locationManager.GetLastKnownLocation(bestProvider);

                if (location != null)
                {
                    geoLocation = location.Latitude.ToString() + "," + location.Longitude.ToString();

                }

            }
            return geoLocation;
        }
    }
}