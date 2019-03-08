
using SQLite;

namespace ComtaxApp.Model
{
    public class BusinessModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string BusinessTypeID { get; set; }
        public string BusinessType { get; set; }
        public string BusinessSubType { get; set; }
    }
}