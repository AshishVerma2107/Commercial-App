using SQLite;
using System.Collections.Generic;

namespace ComtaxApp.Model
{
    
    public class GSTINModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string GeoLocation { get; set; }
        public string GSTINID { get; set; }
        public string WentNotVerified { get; set; }
        public string AppDateTime { get; set; }
        public string AddressStatus { get; set; }
        public string BusinessStatus { get; set; }
        public string RegistrationAccording { get; set; }
        public string Other { get; set; }
        public string CompoundingEligible { get; set; }
        public string PremisesArea { get; set; }
        public string ManufacturerOrTrader { get; set; }
        public string TurnOver { get; set; }
        public string versionName { get; set; }
        [Ignore]
        public List<GSTINVerFiles1> GSTINVerFiles { get; set; }
        public string UploadStatus { get; set; }
    }
}