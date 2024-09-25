using System;

namespace Repository.Common
{
    public class CPanelDetail : CommonData
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public string StaffName { get; set; }
        public string Email { get; set; }
        public string Create_TS { get; set; }
        public string Create_By { get; set; }
        public string Updte_TS { get; set; }
        public string Update_By { get; set; }
        public bool isEnable { get; set; }
        public bool Enable_Pwd_Change { get; set; }
        public dynamic Prof_Pic { get; set; }
        public bool Lock_Status { get; set; }
        public string Lock_Icon { get; set; }
        public string Lock_By { get; set; }
        public string Lock_Reason { get; set; }
        public string LoginAttempt { get; set; }
        public string LastLoginDate { get; set; }
        public string Auth_key { get; set; }
        public string Auth_DateTime { get; set; }
        public bool Auth_Required { get; set; }
        public string Auth_Approved_By { get; set; }
        public string Auth_Approved_Ts { get; set; }
    }
    public class CPANEL_USER_REMARKS
    {
        public Int32 REMARK_ID { get; set; }
        public Int32 USER_ID { get; set; }
        public String REMARK { get; set; }
        public String UPDATE_BY { get; set; }
        public String UPDATE_TS { get; set; }
        public String REMARK_TYPE { get; set; }
    }

}
