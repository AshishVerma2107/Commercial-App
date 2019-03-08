

namespace ComtaxApp.Model
{
    public class LoginModel
    {
        public string OrganizationId { get; set; }
        public string Organization { get; set; }
        public string OfficeId { get; set; }
        public string OfficeName { get; set; }
        public string NaturalPersonId { get; set; }
        public string UserName { get; set; }
        public string NpToOrgRelationID { get; set; }
        public string DesignationId { get; set; }
        public string Designation { get; set; }
        public string MobileNumber { get; set; }
        public string Message { get; set; }
        public string ProjectArea { get; set; }
        public string Controller { get; set; }
        public string ControllerAction { get; set; }
        public string IsActive { get; set; }
        public string EmailAddress { get; set; }
    }
}