using Repository.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CipherHunt.Areas.Cpanel.Models
{
    public class CpanelLoginModel
    {
        [Required(ErrorMessage = "Enter user name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Enter password")]
        public string Password { get; set; }
    }
    public class AuthenticatorModel
    {
        public string BarcodeImageUrl { get; set; }
        public string SetupCode { get; set; }
        public string UserName { get; set; }
        public string UniqueID { get; set; }
        [Required(ErrorMessage = "Please enter verification code")]
        public string OTPCode { get; set; }
        public string UniqueKey { get; set; }
        public string AuthDateTime { get; set; }
        public string AuthKey { get; set; }
        public string AuthApproveBy { get; set; }
    }
    public class CPanelUserModel
    {
        public string ID { get; set; }
        [Required(ErrorMessage = "Enter user name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Enter staff name")]
        public string StaffName { get; set; }
        [Required(ErrorMessage = "Enter email address")]
        public string EmailAddress { get; set; }
        public string CreateBy { get; set; }
        public string Create_TS { get; set; }
        public string Updte_TS { get; set; }
        public string Update_By { get; set; }
        public string LoginAttempt { get; set; }
        public string Token { get; set; }
        public bool isEnable { get; set; }
        public bool Enable_Pwd_Change { get; set; }
        public dynamic Prof_Pic { get; set; }
        public bool Lock_Status { get; set; }
        public string Lock_Icon { get; set; }
        public string Lock_By { get; set; }
        public string Lock_Reason { get; set; }
        public string LastLoginDate { get; set; }
        public string Auth_key { get; set; }
        public string Auth_DateTime { get; set; }
        public bool Auth_Required { get; set; }
        public string Auth_Approved_By { get; set; }
        public string Auth_Approved_Ts { get; set; }
        public List<CPANEL_USER_REMARKS> UserRemarks { get; set; }
    }
}