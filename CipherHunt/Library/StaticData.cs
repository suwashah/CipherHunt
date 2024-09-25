using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CipherHunt.Library
{
    public class StaticData
    {
        public static string GetUser()
        {
            var user = ReadSession("UserName", "");
            if (null == user)
            {
                HttpContext.Current.Response.Redirect("/Home");
            }
            if (ReadSession("ForcePwdChange", "") == "Y")
            {
                HttpContext.Current.Response.Redirect("/ChangeUserPassword");
            }
            return user;
        }
        public static string ReadSession(string key, string defVal)
        {
            Dictionary<string, string> responsemessage = new Dictionary<string, string>();
            responsemessage.Add("", "");
            try
            {
                var User = HttpContext.Current.Session[key] == null ? defVal : HttpContext.Current.Session[key].ToString();
                return User;

            }
            catch
            {
                return defVal;
            }
        }
        public static string GetIp()
        {
            string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ip;
        }
        public static string GetBrowser()
        {
            string browser = System.Web.HttpContext.Current.Request.UserAgent;
            return browser;
        }
        public static string Encrypt(string clearText)
        {
            //string EncryptionKey = GetUser();
            string EncryptionKey = HttpContext.Current.Session.SessionID;
            byte[] clearBytes = Encoding.UTF8.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public static string Decrypt(string cipherText)
        {
            //  string EncryptionKey = GetUser();
            try
            {
                string EncryptionKey = HttpContext.Current.Session.SessionID;
                cipherText = cipherText.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
            catch
            {
                cipherText = "";
            }
            return cipherText;
        }
        public static NameValueCollection GetQueryParameters(string encryptedstring)
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(WebUtility.HtmlDecode(Decrypt(encryptedstring)));
            return nvc;
        }

        public static List<string> MakeGrid<T>(List<T> obj, string ControlerName, Dictionary<string, string> column, string ReportName, bool EnableDataTableSort = false)
        {
            List<string> rtrStr = new List<string>();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<div class=\"row\">");
            sb.AppendLine("<div class=\"col-xs-12\">");
            sb.AppendLine("<div class=\"box box-primary\">");
            sb.AppendLine("<div class=\"box-header\">");
            sb.AppendLine("<h3 class=\"box-title\">");
            sb.AppendLine("<span id=\"ContentBodyPlaceHolder_tbl_header\">" + ReportName + "</span>");
            sb.AppendLine("</h3>");
            sb.AppendLine("</div>");
            sb.AppendLine("<div class=\"box-body\">");
            sb.AppendLine("<table id=\"" + ControlerName + "\" class=\"table table-bordered table-hover\">");
            sb.AppendLine("<thead>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<th>NO.</th>");
            foreach (var item in column)
            {
                sb.AppendLine("<th>" + item.Value + "</th>");

            }
            sb.AppendLine("</tr>");
            sb.AppendLine("</thead>");
            sb.AppendLine("<tbody>");
            int SN = 0;

            foreach (var item in obj)
            {
                SN++;
                int num = 0;
                sb.AppendLine("<tr>");
                sb.AppendLine("<td>" + SN + "</td>");
                Type t = item.GetType();
                foreach (var col in column)
                {
                    num++;
                    var value = item.GetType().GetProperty(col.Key).GetValue(item, null);
                    sb.AppendLine("<td>" + value + "</td>");
                }
                sb.AppendLine("</tr>");
            }

            sb.AppendLine("</tbody>");
            sb.AppendLine("</table>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");

            StringBuilder sb2 = new StringBuilder();
            if (EnableDataTableSort == true)
            {
                sb2.AppendLine("<script>");
                sb2.AppendLine("$(document).ready(function () {");
                sb2.AppendLine("$('#" + ControlerName + "').DataTable({");
                sb2.AppendLine("\"pageLength\": 100,");
                sb2.AppendLine("dom: 'lBfrtip',");
                sb2.AppendLine("\"scrollX\": true,");
                sb2.AppendLine("buttons: [");
                sb2.AppendLine("'copy',");
                sb2.AppendLine("{");
                sb2.AppendLine("extend: 'excelHtml5',");
                sb2.AppendLine("title: 'Merchant List'");
                sb2.AppendLine("},");
                sb2.AppendLine("{");
                sb2.AppendLine("extend: \"print\",");
                sb2.AppendLine("messageTop: \"<div style='width:100%;height:80px;position: relative;'><img src='../Images/nPay_small.png' style='width:120px;height:auto;position:absolute;top:9px;'><p style='display:inline-block;width:200px;position:absolute;left:110px;font-size: 12px;color: #8d8a8a;'>Net Payment Solutions Pvt. Ltd.<br>Harvest Moon Building, First Floor<br>Gairidhara, Kathmandu, Nepal<br>Tel: 977-01 - 4442319 / 4443318<br>Email: info@npay.com.np</p><h3 style='position: absolute;left: 45%;font-size: 16px;'>" + ReportName + "</h3></div><hr/>\",");
                sb2.AppendLine("customize: function (win) {");
                sb2.AppendLine("$(win.document.body)");
                sb2.AppendLine(".prepend(");
                sb2.AppendLine("'<img src=\"http://datatables.net/media/images/logo-fade.png\" style=\"position:absolute; top:0; left:0;\"'");
                sb2.AppendLine(");");
                sb2.AppendLine("");
                sb2.AppendLine("}");
                sb2.AppendLine("}");
                sb2.AppendLine("]");
                sb2.AppendLine("});");
                sb2.AppendLine("});");
                sb2.AppendLine("</script>");
            }

            rtrStr.Add(sb.ToString());
            rtrStr.Add(sb2.ToString());
            return rtrStr;
        }

        public static List<SelectListItem> SetDDLValue(Dictionary<string, string> dictionary, string selectedVal, string defLabel = "", bool isTextAsValue = false)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            if (!string.IsNullOrWhiteSpace(defLabel))
            {
                items.Add(new SelectListItem { Text = defLabel, Value = "" });
            }
            if (dictionary.Count > 0)
            {
                foreach (var item in dictionary)
                {
                    string Value = item.Key;
                    string Name = item.Value;
                    if (isTextAsValue)
                        Value = Name;

                    if (Value.ToUpper() == selectedVal)
                    {
                        items.Add(new SelectListItem { Text = Name, Value = Value, Selected = true });
                    }
                    else
                    {
                        items.Add(new SelectListItem { Text = Name, Value = Value });
                    }
                }
            }
            return items;
        }

        public static dynamic SetDDLValue2(Dictionary<string, string> dictionary, string selectedVal, string defLabel = "", bool isTextAsValue = false)
        {
            var items = new List<SelectListItem>();
            if (!string.IsNullOrWhiteSpace(defLabel))
            {
                items.Add(new SelectListItem { Text = defLabel, Value = "" });
            }
            foreach (var item in dictionary)
            {
                string Value = item.Key;
                string Name = item.Value;
                items.Add(new SelectListItem()
                {
                    Text = Name,
                    Value = Value,
                    Selected = Value == selectedVal ? true : false
                });
            }
            return items;
        }
        public static T ModelToCommon<T, Y>(Y fromObject, T toObject)
        {
            Type toType = toObject.GetType();
            Type fromType = fromObject.GetType();
            var tofields = toType.GetProperties();
            var fromfields = fromType.GetProperties();
            foreach (var propTo in tofields)
            {
                try
                {
                    PropertyInfo propFrom = fromType.GetProperty(propTo.Name);
                    if (propFrom != null && propFrom.CanWrite)
                        propTo.SetValue(toObject, propFrom.GetValue(fromObject, null));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return (T)Convert.ChangeType(toObject, typeof(T));
        }
    }
}