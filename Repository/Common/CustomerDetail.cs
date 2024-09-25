namespace Repository.Common
{
    public class CustomerDetail : CommonData
    {
        public string ID { get; set; }
        public string MEMBERID { get; set; }
        public string TOKEN { get; set; }
        public string USERNAME { get; set; }
        public string EMAIL { get; set; }
        public bool EMAILCONFIRMED { get; set; }
        public string FIRSTNAME { get; set; }
        public string MIDDLENAME { get; set; }
        public string LASTNAME { get; set; }
        public string FULLNAME { get; set; }
        public string PASSWORD { get; set; }
        public string PHONE { get; set; }
        public bool PHONENUMBERCONFIRMED { get; set; }
        public string ACCESSFAILEDCOUNT { get; set; }
        public string INVITE_CODE { get; set; }
        public bool ISENABLE { get; set; }
        public bool ISACTIVE { get; set; }
        public string CITY { get; set; }
        public string POSTALCODE { get; set; }
        public string FAX { get; set; }
        public string ADDRESS { get; set; }
        public string GENDER { get; set; }
        public string LASTLOGINPC { get; set; }
        public string LASTLOGINDATE { get; set; }
        public bool LOCKSTATUS { get; set; }
        public string LOCKDATE { get; set; }
        public string KYCSTATUS { get; set; }
        public string KYCAPPROVEBY { get; set; }
        public string KYCAPPROVEDATE { get; set; }
        public string REFERENCESOURCE { get; set; }
        public string REFERENCEID { get; set; }
        public string REFEREDBY { get; set; }
        public string DATEOFBIRTH { get; set; }
        public bool ENABLEPASSWORDCHANGE { get; set; }
        public string PROFILEIMAGE { get; set; }
        public string CREATETS { get; set; }
        public string CREATEBY { get; set; }
        public string UPDATETS { get; set; }
        public string UPDATEBY { get; set; }
    }
}
