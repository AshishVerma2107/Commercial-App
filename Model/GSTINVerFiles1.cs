using SQLite;

namespace ComtaxApp.Model
{
    public class GSTINVerFiles1
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int InsertedId { get; set; }
        public string VerFileName { get; set; }
        public string GeoLocation { get; set; }
    }
}