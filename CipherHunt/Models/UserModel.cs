using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using CompareAttribute = System.ComponentModel.DataAnnotations.CompareAttribute;

namespace CipherHunt.Models
{
    public class UserModel
    {
    }
    public class LoginModel
    {
        [Required(ErrorMessage = "Enter email address")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Enter password")]
        public string Password { get; set; }
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
    public class LoginConfirmModel
    {
        [Required(ErrorMessage = "Enter your login password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        //[HiddenInput]
        public string ReturnUrl { get; set; }
    }
    public class OTPConfirmModel
    {
        [Required(ErrorMessage = "Enter verification code")]
        [MaxLength(6, ErrorMessage = "6 digit code is required")]
        public string VerificationCode { get; set; }
        [HiddenInput]
        public string pid { get; set; }
        public string Email { get; set; }
    }
    public class RegisterModel
    {
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Please enter email address")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Please retype password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "First name")]
        [Required(ErrorMessage = "Please enter first name")]
        public string FirstName { get; set; }
        [Display(Name = "Middle name")]
        public string Middlename { get; set; }
        [Display(Name = "Last name")]
        [Required(ErrorMessage = "Please enter last name")]
        public string LastName { get; set; }
        [Display(Name = "Mobile number")]
        [Required(ErrorMessage = "Please enter mobile number")]
        [MaxLength(10, ErrorMessage = "Cannot be more than 10 digit")]
        [RegularExpression("([4-9][0-9]*)", ErrorMessage = "Number must start with 4xxxxxx")]
        public string Mobile { get; set; }
        //[Required(ErrorMessage = "Please select gender")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Please accept terms and condition")]
        public bool AcceptTerms { get; set; }
        [Display(Name = "Invite Code")]
        public string InviteCode { get; set; }
        public string ISO2 { get; set; }
        public string TeleCode { get; set; }
    }
    public class ReferViewModel
    {
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Required(ErrorMessage = "Enter email address")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Enter your friend name")]
        public string FriendName { get; set; }
    }
    public class ChangePasswordModel
    {

        [Required(ErrorMessage = "Please fill out this field")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "Please fill out this field")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Please fill out this field")]
        [Compare("NewPassword", ErrorMessage = "Password not verified, Type again !")]
        public string ConfirmPassword { get; set; }
    }
    public class CompleteProfileModel
    {
        [Display(Name = "Nationality*")]
        [Required(ErrorMessage = "Please fill out this field")]
        public string Nationality { get; set; }
        [Display(Name = "Gender*")]
        [Required(ErrorMessage = "Please fill out this field")]
        public string Gender { get; set; }

        [Display(Name = "Date of birth*")]
        [Required(ErrorMessage = "Please fill out this field")]
        public string DOB { get; set; }
        [Display(Name = "Postal code*")]
        [Required(ErrorMessage = "Please fill out this field")]
        public string ZipCode { get; set; }
        [Display(Name = "Address*")]
        [Required(ErrorMessage = "Please fill out this field")]
        public string Address { get; set; }
        [Display(Name = "City*")]
        [Required(ErrorMessage = "Please fill out this field")]
        public string City { get; set; }
        [Display(Name = "Building*")]
        [Required(ErrorMessage = "Please fill out this field")]
        public string Building { get; set; }
        [Display(Name = "Street name*")]
        [Required(ErrorMessage = "Please fill out this field")]
        public string Street { get; set; }
        [Display(Name = "Mobile number*")]
        [Required(ErrorMessage = "Please fill out this field")]
        public string Mobile { get; set; }
        [Display(Name = "Telephone number")]
        public string Telephone { get; set; }
        [Display(Name = "Occupation")]
        [Required(ErrorMessage = "Please fill out this field")]
        public string Occupation { get; set; }
        [Display(Name = "Other occupation")]
        public string OtherOccupation { get; set; }
        [Display(Name = "Income source")]
        [Required(ErrorMessage = "Please fill out this field")]
        public string Income { get; set; }
        [Display(Name = "Other income source")]
        public string OtherIncome { get; set; }
    }
    public class ResetPasswordModel
    {
        [Display(Name = "Password*")]
        [Required(ErrorMessage = "Please fill out this field")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Please fill out this field")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Email*")]
        [Required(ErrorMessage = "Please fill out this field")]
        public string Email { get; set; }
        public string ProcessID { get; set; }
    }

    public class CommonDocModel
    {
        public string Flag { get; set; }
        public string UserName { get; set; }
        public string Ipaddress { get; set; }
        public string CustomerID { get; set; }
        public string Doc_type { get; set; }
        public string Customer_ID_Type { get; set; }
        public string Customer_ID_Number { get; set; }
    }
    public class SuccessModel
    {
        public string Message { get; set; }
        public string Title { get; set; }
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
    }
    public class CustomerDetailModel
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