using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Net;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Data;

namespace Repository.HelperFunction
{
    public class UtilityHelper : Converter, IUtilityHelper
    {
        public UtilityHelper()
        {

        }
        public string maskCardNumber(string cardNo, int prefixDisplayCount, int suffixDisplayCount, int cardLength = 16)
        {
            string maskedCard = string.Empty;
            char[] charArray = cardNo.ToCharArray();
            int maskedCardLenght;
            for (int i = 0; i < cardLength; i++)
            {
                maskedCardLenght = maskedCard.Length;
                if (maskedCardLenght >= prefixDisplayCount && maskedCardLenght < (cardLength - suffixDisplayCount))
                {
                    maskedCard = maskedCard + "*";
                }
                else
                {
                    maskedCard = maskedCard + charArray[i].ToString();
                }
            }
            return maskedCard;
        }
        public bool checkCardIdFormat(string id)
        {
            int idLength = id.Length;
            int currentDigit;
            int idSum = 0;
            int currentProcNum = 0; //the current process number (to calc odd/even proc)
            string firstDigit = string.Empty;
            firstDigit = id.Substring(0, 1);
            if (id.Length != 16)
                return false;
            if (firstDigit != "6")
                return false;
            for (int i = idLength - 1; i >= 0; i--)
            {
                //get the current rightmost digit from the string
                string idCurrentRightmostDigit = id.Substring(i, 1);
                //parse to int the current rightmost digit, if fail return false (not-valid id)
                if (!int.TryParse(idCurrentRightmostDigit, out currentDigit))
                    return false;
                //bouble value of every 2nd rightmost digit (odd)
                //if value 2 digits (can be 18 at the current case),
                //then sumarize the digits (made it easy the by remove 9)
                if (currentProcNum % 2 != 0)
                {
                    if ((currentDigit *= 2) > 9)
                        currentDigit -= 9;
                }
                currentProcNum++; //increase the proc number
                //summarize the processed digits
                idSum += currentDigit;
            }
            //if digits sum is exactly divisible by 10, return true (valid), else false (not-valid)
            return (idSum % 10 == 0);
        }
        public string CreateUserPaymentXml(string user_card_id, string card_exp_month, string card_exp_year)
        {
            string url = string.Empty;
            url = url + "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            url = url + "<UserPaymentData>";
            url = url + "<MethodName>" + "FCR" + "</MethodName>";
            url = url + "<CardNo>" + user_card_id + "</CardNo>";
            url = url + "<ExpiryDate>" + "<Month>" + card_exp_month + "</Month><Year>" + card_exp_year + "</Year></ExpiryDate>";
            url = url + "<PinNo>****</PinNo>";
            url = url + "</UserPaymentData>";

            return url;
        }
        public string GetClientIP()
        {
            string result = string.Empty;
            string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(ip))
            {
                string[] ipRange = ip.Split(',');
                int le = ipRange.Length - 1;
                result = ipRange[0];
            }
            else
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            return result;
        }
        public string GetClientMachineIP()
        {
            string ipaddress;
            ipaddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (ipaddress == "" || ipaddress == null)
                ipaddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            return ipaddress;
        }
        public string GetMAC()
        {
            string strHostName = "";
            strHostName = System.Net.Dns.GetHostName();

            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

            IPAddress[] addr = ipEntry.AddressList;

            return addr[addr.Length - 1].ToString();

        }
        public string GetSHA256(string text)
        {
            SHA256 sha = new SHA256Managed();
            byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(text));
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString();
        }
        public string GetIPAddressClient()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }
        public string generateCardID()
        {
            int[] static_sct_id = new int[6] { 6, 3, 9, 6, 1, 6 };
            int[] static_sct_id1 = new int[6] { 3, 3, 9, 6, 2, 6 };

            int sum1 = static_sct_id1[0] + static_sct_id1[1] + static_sct_id1[2] + static_sct_id1[3] + static_sct_id1[4] + static_sct_id1[5];

            string def = "0123456789";
            Random rnd = new Random();
            StringBuilder ret = new StringBuilder();

            for (int i = 0; i < 9; i++)
                ret.Append(def.Substring(rnd.Next(def.Length), 1));

            string randomDigits = ret.ToString();

            char[] char_randon_digits = randomDigits.ToCharArray();

            int[] int_random_digits = Array.ConvertAll(char_randon_digits, c => (int)Char.GetNumericValue(c));

            int ran0 = Math.Abs((int_random_digits[0] * 2));
            int ran1 = int_random_digits[1];
            int ran2 = Math.Abs((int_random_digits[2] * 2));
            int ran3 = int_random_digits[3];
            int ran4 = Math.Abs((int_random_digits[4] * 2));
            int ran5 = int_random_digits[5];
            int ran6 = Math.Abs((int_random_digits[6] * 2));
            int ran7 = int_random_digits[7];
            int ran8 = Math.Abs((int_random_digits[8] * 2));

            if (ran0 > 9)
            {
                char[] char_ran0 = ran0.ToString().ToCharArray();
                int new_ran0 = (int)Char.GetNumericValue(char_ran0[0]) + (int)Char.GetNumericValue(char_ran0[1]);
                ran0 = new_ran0;
            }

            if (ran2 > 9)
            {
                char[] char_ran2 = ran2.ToString().ToCharArray();
                int new_ran2 = (int)Char.GetNumericValue(char_ran2[0]) + (int)Char.GetNumericValue(char_ran2[1]);
                ran2 = new_ran2;
            }

            if (ran4 > 9)
            {
                char[] char_ran4 = ran4.ToString().ToCharArray();
                int new_ran4 = (int)Char.GetNumericValue(char_ran4[0]) + (int)Char.GetNumericValue(char_ran4[1]);
                ran4 = new_ran4;
            }

            if (ran6 > 9)
            {
                char[] char_ran6 = ran6.ToString().ToCharArray();
                int new_ran6 = (int)Char.GetNumericValue(char_ran6[0]) + (int)Char.GetNumericValue(char_ran6[1]);
                ran6 = new_ran6;
            }

            if (ran8 > 9)
            {
                char[] char_ran8 = ran8.ToString().ToCharArray();
                int new_ran8 = (int)Char.GetNumericValue(char_ran8[0]) + (int)Char.GetNumericValue(char_ran8[1]);
                ran8 = new_ran8;
            }

            int sum2 = ran0 + ran1 + ran2 + ran3 + ran4 + ran5 + ran6 + ran7 + ran8;
            int total_sum = sum1 + sum2;
            int multipleOf9 = total_sum * 9;

            char[] final_sum_array = multipleOf9.ToString().ToCharArray();

            string x = string.Empty;

            for (int i = 0; i < multipleOf9.ToString().Count(); i++)
            {
                x = final_sum_array[i].ToString();
            }

            string sct_code = string.Join(string.Empty, static_sct_id);
            string other_code = string.Join(string.Empty, int_random_digits);

            return sct_code + other_code + x;

        }
        public string GetIPAddress(string url)
        {
            string ip = string.Empty;

            IPAddress[] ips;

            ips = Dns.GetHostAddresses(url);

            for (int i = 0; i < ips.Length; i++)
            {
                ip = ip + ips[i].ToString();
            }
            return ip;
        }
        public string GetParam()
        {
            string request = string.Empty;
            using (Stream receiveStream = HttpContext.Current.Request.InputStream)
            {
                receiveStream.Position = 0;
                using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                {
                    request = readStream.ReadToEnd();
                }

            }
            return request;
        }
        public string ToXML(Object oObject)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlSerializer xmlSerializer = new XmlSerializer(oObject.GetType());
            using (MemoryStream xmlStream = new MemoryStream())
            {
                xmlSerializer.Serialize(xmlStream, oObject);
                xmlStream.Position = 0;
                xmlDoc.Load(xmlStream);
                return xmlDoc.InnerXml;
            }
        }
        public string ToXMLString(String oObject)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlSerializer xmlSerializer = new XmlSerializer(oObject.GetType());
            using (MemoryStream xmlStream = new MemoryStream())
            {
                xmlSerializer.Serialize(xmlStream, oObject);
                xmlStream.Position = 0;
                xmlDoc.Load(xmlStream);
                return xmlDoc.InnerXml;
            }
        }
        public string getXMlValue(XmlDocument xmlObj, string NodeName)
        {
            string nodeValue = string.Empty;
            XmlNodeList xmlNodeObj = xmlObj.GetElementsByTagName(NodeName);
            if (xmlNodeObj.Count > 0)
            {
                nodeValue = xmlObj.GetElementsByTagName(NodeName).Item(0).InnerText;
            }
            else
                nodeValue = "0";

            return nodeValue;
        }
        public string getXMlNodeAttributesValue(XmlDocument xmlObj, string NodeName, string InnerAttributes)
        {
            string nodeValue = string.Empty;
            XmlNodeList xmlNodeObj = xmlObj.GetElementsByTagName(NodeName);
            if (xmlNodeObj.Count > 0)
            {
                nodeValue = xmlObj.GetElementsByTagName(NodeName).Item(0).Attributes[InnerAttributes].Value;
            }
            else
                nodeValue = "123";
            return nodeValue;
        }
        public void SendHTTPRequest(string request_string)
        {
            HttpWebRequest delivery_request = (HttpWebRequest)WebRequest.Create(request_string);
        }
        public string requestForm(string var)
        {
            string retVal = HttpContext.Current.Request.Form[var];
            if (retVal == null || retVal == "")
            {
                retVal = HttpContext.Current.Request.QueryString[var];
            }
            StringBuilder correctStr = new StringBuilder(retVal);
            correctStr.Replace(";", "");
            correctStr.Replace("'", "");
            correctStr.Replace("--", "");
            correctStr.Replace("<", "");
            correctStr.Replace(">", "");

            return correctStr.ToString().Trim();
        }
        public string formateDate(DateTime inputDate)
        {
            string returnDate = string.Empty;

            returnDate = inputDate.ToString("u").Substring(0, inputDate.ToString("u").Length - 1);
            return returnDate;
        }
        public string PostRedirection(string URL, string msg, string code)
        {
            string returnData = string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.Append("<html>");
            sb.AppendFormat(@"<body onload='document.forms[""form""].submit()'>");
            sb.AppendFormat("<form name='form' action='{0}' method='post'>", URL);
            sb.AppendFormat("<input type='hidden' name='msg' value='{0}'>", msg);
            sb.AppendFormat("<input type='hidden' name='code' value='{0}'>", code);
            // Other params go here
            sb.Append("</form>");
            sb.Append("</body>");
            sb.Append("</html>");

            returnData = sb.ToString();
            return returnData;
        }
        public string Decrypt(string base64EncodedData, string salt)
        {
            string myValue = string.Empty;
            string encrypted_data = string.Empty;

            //Base64 to String
            try
            {
                var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
                encrypted_data = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch
            {
                return "INVALID BASE 64 Value";
            }


            //MAKE salt even if its odd with the last digit of salt length
            int saltLength = salt.Length;
            string valueToAppend = Convert.ToString(saltLength);
            if (salt.Length % 2 != 0)
            {
                if (saltLength > 9)
                    valueToAppend = Convert.ToString(saltLength).Substring(Convert.ToString(saltLength).Length - 1, 1);


                salt = salt + valueToAppend;
            }
            saltLength = salt.Length;

            //Match the salt value
            string prefix = encrypted_data.Substring(0, saltLength / 2);
            string suffix = encrypted_data.Substring(encrypted_data.Length - saltLength / 2, saltLength / 2);

            if (salt != prefix + suffix)
            {
                return "INVALID SALT";
            }

            //Get Data excluding salt value
            string encrypted_data_without_salt = string.Empty;
            encrypted_data_without_salt = encrypted_data.Substring(prefix.Length, (encrypted_data.Length - prefix.Length));
            encrypted_data_without_salt = encrypted_data_without_salt.Substring(0, (encrypted_data_without_salt.Length - suffix.Length));

            //Replace strings
            string strWithoutAlpha = encrypted_data_without_salt.Replace("A", "0").Replace("B", "1")
                                                .Replace("C", "2").Replace("D", "3")
                                                .Replace("E", "4").Replace("F", "5")
                                                .Replace("G", "6").Replace("H", "7")
                                                .Replace("I", "8").Replace("J", "9");

            //Get Plain value from encrypted Data
            int counter = Convert.ToInt16(valueToAppend);
            for (int j = 0; j < counter; j++)
            {
                string part1 = strWithoutAlpha.Substring(0, strWithoutAlpha.Length / 2);
                string part2 = strWithoutAlpha.Substring(strWithoutAlpha.Length - strWithoutAlpha.Length / 2, strWithoutAlpha.Length / 2);

                myValue = "";

                for (int i = 0; i < part1.Length; i++)
                {
                    myValue += part1.Substring(i, 1) + part2.Substring(i, 1);

                }
                strWithoutAlpha = myValue;
            }
            myValue = strWithoutAlpha;
            return myValue;
        }
        public String ConstructQueryString(NameValueCollection parameters)
        {
            List<String> items = new List<String>();

            foreach (String name in parameters)
                items.Add(String.Concat(name, "=", System.Web.HttpUtility.UrlEncode(parameters[name])));

            return String.Join("&", items.ToArray());
        }
        public bool emailIsValid(string email)
        {
            string expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, string.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool HasSpecialCharacter(string myString)
        {
            string specialCharacters = ";'@+~`^=";
            char[] specialCharactersArray = specialCharacters.ToCharArray();

            int index = myString.IndexOfAny(specialCharactersArray);
            //index == -1 no special characters
            if (index == -1)
                return false;
            else
                return true;

        }
        public string ConvertDate(string dateString)
        {
            // string UK_format = "dd/mm/yyyy";
            String date_format = ConfigurationManager.AppSettings["date_format"].ToString();
            DateTime dt = new DateTime();
            String[] date_array;
            int year;
            int month;
            int date;
            if (date_format == "dd-MM-yyyy")
            {
                int pos = dateString.IndexOf('-');
                if (pos > 0)
                {
                    date_array = dateString.Split('-');
                }
                else
                {
                    date_array = dateString.Split('/');
                }
                year = Convert.ToInt32(date_array[2]);
                month = Convert.ToInt32(date_array[1]);
                date = Convert.ToInt32(date_array[0]);
                dt = new DateTime(year, month, date, 0, 0, 0, 0);
            }
            else if (date_format == "dd/MM/yyyy")
            {
                date_array = dateString.Split('/');
                year = Convert.ToInt32(date_array[2]);
                month = Convert.ToInt32(date_array[1]);
                date = Convert.ToInt32(date_array[0]);
                dt = new DateTime(year, month, date, 0, 0, 0, 0);
            }
            else if (date_format == "MM/dd/yyyy")
            {
                date_array = dateString.Split('/');
                year = Convert.ToInt32(date_array[2]);
                month = Convert.ToInt32(date_array[0]);
                date = Convert.ToInt32(date_array[1]);
                dt = new DateTime(year, month, date, 0, 0, 0, 0);
            }
            else if (date_format == "yyyy-MM-dd")
            {
                date_array = dateString.Split('-');
                year = Convert.ToInt32(date_array[0]);
                month = Convert.ToInt32(date_array[1]);
                date = Convert.ToInt32(date_array[2]);
                dt = new DateTime(year, month, date, 0, 0, 0, 0);
            }
            else if (date_format == "yyyy/MM/dd")
            {
                date_array = dateString.Split('/');
                year = Convert.ToInt32(date_array[0]);
                month = Convert.ToInt32(date_array[1]);
                date = Convert.ToInt32(date_array[2]);
                dt = new DateTime(year, month, date, 0, 0, 0, 0);
            }
            string strNewDate = dt.ToString("yyyy-MM-dd"); // dateTime.ToString("yyyy-dd-mm");
            return strNewDate;
        }
        public string MD5Encode(string str_encode)
        {
            MD5 md5Hash = MD5.Create();
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(str_encode));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
        public string GetMaskedEmail(string emailaddress)
        {
            //Added by subash shahi on Oct 17,2017
            String maskEmail = String.Empty;
            int totalstringLength = emailaddress.Length;
            int index = totalstringLength;
            if (emailaddress.Contains('@'))
            {
                index = emailaddress.IndexOf('@');
            }
            maskEmail = emailaddress.Substring(0, 2) + "*****" + emailaddress.Substring(index, totalstringLength - index);
            return maskEmail;
        }
        public string changeFilename(string input)
        {
            input = input.Replace(" ", "");
            input = input.Replace("/", "");
            input = input.Replace(":", "");
            input = input.Replace(".", "");
            input = input.Replace("-", "_");
            return input;
        }
        public int decimal_round()
        {
            int decimalRound = 2;
            string d_round = ConfigurationManager.AppSettings["decimal_round"].ToString();
            if (!String.IsNullOrEmpty(d_round))
            {
                decimalRound = Convert.ToInt32(d_round);
            }
            return decimalRound;
        }
        public string ConfigPaymentType()
        {
            string ptype = ConfigurationManager.AppSettings["payment_type"].ToString();
            return ptype;
        }
        public string DecimalAmountFormatter(string amount)
        {
            //It returns the value of amount seperated by comma after 3 digits
            if (String.IsNullOrEmpty(amount))
            {
                amount = "0";
            }
            Decimal ConvertAmount = Convert.ToDecimal(amount);
            string ret = ConvertAmount.ToString("#,##0.00");
            return ret;
        }
        public string DecimalAmountFormatterCustomRound(string amount, int roundvalue = 0)
        {
            string format = "#,##0";
            string init = "0";
            for (int i = 1; i < roundvalue; i++)
            {
                init = init + "0";
            }
            if (roundvalue > 0)
            {
                format = format + "." + init;
            }
            //It returns the value of amount seperated by comma after 3 digits
            if (String.IsNullOrEmpty(amount))
            {
                amount = "0";
            }
            Decimal ConvertAmount = Convert.ToDecimal(amount);
            string ret = ConvertAmount.ToString(format);
            return ret;
        }
        public string AmountCommaRemover(string str)
        {
            //It returns the value of amount seperated by comma after 3 digits
            if (str == "" || str == null)
            {
                return "";
            }
            else
            {
                str = str.Replace(",", "");
                return str;
            }
        }
        public string StringToDecimalwithRetString(string input)
        {
            Decimal a = 0;
            if (String.IsNullOrEmpty(input) == false)
            {
                a = Convert.ToDecimal(input);
            }
            return a.ToString();
        }
        public string EncryptSession(string userid)
        {
            string _browserInfo = HttpContext.Current.Request.Browser.Browser + HttpContext.Current.Request.Browser.Version + HttpContext.Current.Request.UserAgent + "~" + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            string _sessionValue = userid + "^" + DateTime.Now.Ticks + "^" + _browserInfo + "^" + System.Guid.NewGuid();
            byte[] _encodedAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(_sessionValue);
            string _encryptedString = Convert.ToBase64String(_encodedAsBytes);
            return _encryptedString;
        }
        public void AcquireRequestState()
        {
            string _sessionIPAddress = string.Empty;
            string _sessionBrowserInfo = string.Empty;
            if (HttpContext.Current.Session != null && HttpContext.Current.Session["encryptedSession"] != null)
            {
                string _encryptedString = HttpContext.Current.Session["encryptedSession"].ToString();
                //_encryptedString = DefaultAuthenticationTypes.ApplicationCookie;
                byte[] _encodedAsBytes = System.Convert.FromBase64String(_encryptedString);
                string _decryptedString = System.Text.ASCIIEncoding.ASCII.GetString(_encodedAsBytes);
                char[] _seperator = new char[] { '^' };
                if (_decryptedString != String.Empty && !String.IsNullOrEmpty(_decryptedString))
                {
                    string[] _splitStrings = _decryptedString.Split(_seperator);
                    if (_splitStrings.Count() > 0)
                    {
                        if (_splitStrings[2].Count() > 0)
                        {
                            string[] _userBrowserInfo = _splitStrings[2].Split('~');
                            if (_userBrowserInfo.Count() > 0)
                            {
                                _sessionBrowserInfo = _userBrowserInfo[0];
                                _sessionIPAddress = _userBrowserInfo[1];
                            }
                        }
                    }
                }
                string _currentUserIpAddress;
                string _currentBrowserInfo;
                if (string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
                {
                    _currentUserIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                else
                {
                    _currentUserIpAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                }
                _currentBrowserInfo = HttpContext.Current.Request.Browser.Browser + HttpContext.Current.Request.Browser.Version + HttpContext.Current.Request.UserAgent;
                System.Net.IPAddress result;
                if (!System.Net.IPAddress.TryParse(_currentUserIpAddress, out result))
                {
                    result = System.Net.IPAddress.None;
                }
                if ((_sessionIPAddress != "" && _sessionIPAddress != string.Empty) || (_sessionBrowserInfo != "" && _sessionBrowserInfo != string.Empty))
                {
                    if (_sessionIPAddress != _currentUserIpAddress || _sessionBrowserInfo != _currentBrowserInfo)
                    {
                        HttpContext.Current.Session.RemoveAll();
                        HttpContext.Current.Session.Clear();
                        HttpContext.Current.Session.Abandon();
                        HttpContext.Current.Response.Cookies["AYAQQGU774F92C8ASPB89708B583F0VA"].Expires = DateTime.Now.AddSeconds(-30);
                        HttpContext.Current.Response.Cookies.Add(new HttpCookie("AYAQQGU774F92C8ASPB89708B583F0VA", ""));
                        HttpContext.Current.Response.Redirect("~/" + GetAppsettingValue("Root_Page"));
                    }
                }
            }
        }
        public string EncryptString(string clearText, string EncryptionKey)
        {

            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
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
        public string DecryptString(string cipherText, string EncryptionKey)
        {
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = null;
            try
            {
                cipherBytes = Convert.FromBase64String(cipherText);
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
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
            }
            catch
            {
                return "";
            }
            return cipherText;
        }
        public string DefaultEncryptionKey()
        {
            string ret = GetAppsettingValue("Default_Encryption_Key");
            return ret;
        }
        public string replaceByhyphen(string str)
        {
            str = str.Replace(" ", "-");
            return str;
        }
        public string CheckSession(string SessionName, bool isAgent = false)
        {
            string SessionValue = "";
            string RedirectURL = GetAppsettingValue("Root_Page");
            if (HttpContext.Current.Session[SessionName] != null)
            {
                SessionValue = HttpContext.Current.Session[SessionName].ToString();
            }
            else
            {
                HttpContext.Current.Session.RemoveAll();
                HttpContext.Current.Session.Abandon();
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Response.Cookies["AYAQQGU774F92C8ASPB89708B583F0VA"].Expires = DateTime.Now.AddSeconds(-30);
                HttpContext.Current.Response.Cookies.Add(new HttpCookie("AYAQQGU774F92C8ASPB89708B583F0VA", ""));
                if (isAgent == true)
                {
                    RedirectURL = "~/CPanel/Acc/Login";
                }
                HttpContext.Current.Response.Redirect(RedirectURL);
            }
            return SessionValue;
        }
        public string CheckClientSession(string SessionName)
        {
            string SessionValue = "";
            if (HttpContext.Current.Session[SessionName] != null)
            {
                SessionValue = HttpContext.Current.Session[SessionName].ToString();
            }
            return SessionValue;
        }
        public void setSecureProtocol(bool bSecure)
        {
            string redirectUrl = null;
            HttpContext context = HttpContext.Current;
            // if we want HTTPS and it is currently HTTP
            if (bSecure && !context.Request.IsSecureConnection)
                redirectUrl = context.Request.Url.ToString().Replace("http:", "https:");
            else
                // if we want HTTP and it is currently HTTPS
                if (!bSecure && context.Request.IsSecureConnection)
                redirectUrl = context.Request.Url.ToString().Replace("https:", "http:");
            //else
            // in all other cases we don't need to redirect
            // check if we need to redirect, and if so use redirectUrl to do the job
            if (redirectUrl != null)
                context.Response.Redirect(redirectUrl);
        }
        public object isNull(object input, string replace)
        {
            if (input == null)
            {
                return replace;
            }
            return input;
        }
        public object isNull(object input, int replace)
        {
            if (input == null)
            {
                return replace;
            }
            return input;
        }
        public string isNull(string input, string replacement)
        {
            if (String.IsNullOrEmpty(input) || input.Contains("()"))
            {
                return replacement;
            }
            else
            {
                return input;
            }
        }
        public string GetUnixTimeStamp()
        {
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return unixTimestamp.ToString();
        }
        public void setCookies(string cookiesName, string cookiesValue)
        {
            HttpContext.Current.Response.Cookies[cookiesName].Expires = DateTime.Now.AddDays(1);
            HttpContext.Current.Response.Cookies[cookiesName].Value = cookiesValue.Trim();
        }
        public void DestroyCookies(string cookiesName)
        {
            HttpContext.Current.Response.Cookies[cookiesName].Expires = DateTime.Now.AddDays(-1);
        }
        public string GetCookiesValue(string cookiesName)
        {
            string ret = "";
            if (HttpContext.Current.Request.Cookies[cookiesName] != null)
            {
                ret = HttpContext.Current.Response.Cookies[cookiesName].Value;
            }
            return ret;
        }
        public string GetAllRequestFormValue()
        {
            string ret = String.Empty;
            string[] keys = HttpContext.Current.Request.Form.AllKeys;
            for (int i = 0; i < keys.Length; i++)
            {
                ret += keys[i] + "=" + HttpContext.Current.Request[keys[i]] + "|###|";
            }
            return ret;
        }
        public bool isNumber(string input)
        {
            Int32 a;
            if (Int32.TryParse(input, out a))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public string GetAppsettingValue(string keyname)
        {
            string ret = "";
            if (ConfigurationManager.AppSettings[keyname] != null)
            {
                ret = ConfigurationManager.AppSettings[keyname].ToString();
            }
            return ret;
        }
        public int[] BubbleSort(int[] arr)
        {
            int temp = 0;
            //Array.Sort(arr);//the quickiest method to sort the array data
            for (int write = 0; write < arr.Length; write++)
            {
                for (int sort = 0; sort < arr.Length; sort++)
                {
                    if (arr[sort] > arr[sort + 1])
                    {
                        temp = arr[sort + 1];
                        arr[sort + 1] = arr[sort];
                        arr[sort] = temp;
                    }
                }
            }
            return arr;
        }
        public Array SortData(Array arr)
        {
            Array.Sort(arr);
            return arr;
        }
        public void AlertSound(string SoundPath, bool Looped = false)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(SoundPath);
            if (Looped == false)
            {
                player.Play();
            }
            else
            {
                player.PlayLooping();
            }
        }
        public void StopSound(string SoundPath)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(SoundPath);
            player.Stop();
        }
        public void PlayClientAlert(string sound_source = "Sounds/alert.wav")
        {
            HttpContext.Current.Response.Write("<embed src=\"" + sound_source + "\" height=\"1px\" width=\"1px\" style=\"visibility: hidden\"/>");
        }
        public void ClearSessions()
        {
            HttpContext.Current.Session.RemoveAll();
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Response.Cookies["AYAQQGU774F92C8ASPB89708B583F0VA"].Expires = DateTime.Now.AddSeconds(-30);
        }
        public String FilterQuote(string strVal)
        {
            if (string.IsNullOrEmpty(strVal))
            {
                strVal = "";
            }
            var str = strVal.Trim();

            if (!string.IsNullOrEmpty(str))
            {
                str = str.Replace(";", "");
                //str = str.Replace(",", "");
                str = str.Replace("--", "");
                str = str.Replace("'", "");
                str = str.Replace("/*", "");
                str = str.Replace("*/", "");
                str = str.Replace(" select ", "");
                str = str.Replace(" insert ", "");
                str = str.Replace(" update ", "");
                str = str.Replace(" delete ", "");
                str = str.Replace(" drop ", "");
                str = str.Replace(" truncate ", "");
                str = str.Replace(" create ", "");
                str = str.Replace(" begin ", "");
                str = str.Replace(" end ", "");
                str = str.Replace(" char(", "");
                str = str.Replace(" exec ", "");
                str = str.Replace(" xp_cmd ", "");
                str = str.Replace("<script", "");
            }
            else
            {
                str = "null";
            }
            return str;
        }
        public String FilterQuoteEmail(string strVal)
        {
            if (string.IsNullOrEmpty(strVal))
            {
                strVal = "";
            }
            var str = strVal.Trim();

            if (!string.IsNullOrEmpty(str))
            {
                //str = str.Replace(";", "");
                //str = str.Replace(",", "");
                str = str.Replace("--", "");
                str = str.Replace("'", "");
                str = str.Replace("/*", "");
                str = str.Replace("*/", "");
                str = str.Replace(" select ", "");
                str = str.Replace(" insert ", "");
                str = str.Replace(" update ", "");
                str = str.Replace(" delete ", "");
                str = str.Replace(" drop ", "");
                str = str.Replace(" truncate ", "");
                str = str.Replace(" create ", "");
                str = str.Replace(" begin ", "");
                str = str.Replace(" end ", "");
                str = str.Replace(" char(", "");
                str = str.Replace(" exec ", "");
                str = str.Replace(" xp_cmd ", "");
                str = str.Replace("<script", "");
            }
            else
            {
                str = "null";
            }
            return str;
        }
        public String FilterQuoteDR(string strVal)
        {
            if (string.IsNullOrEmpty(strVal))
            {
                strVal = "";
            }
            var str = strVal.Trim();

            if (!string.IsNullOrEmpty(str))
            {
                str = str.Replace(";", "");
                //str = str.Replace(",", "");
                str = str.Replace("--", "");
                str = str.Replace("'", "''");
                str = str.Replace("/*", "");
                str = str.Replace("*/", "");
                str = str.Replace(" select ", "");
                str = str.Replace(" insert ", "");
                str = str.Replace(" update ", "");
                str = str.Replace(" delete ", "");
                str = str.Replace(" drop ", "");
                str = str.Replace(" truncate ", "");
                str = str.Replace(" create ", "");
                str = str.Replace(" begin ", "");
                str = str.Replace(" end ", "");
                str = str.Replace(" char(", "");
                str = str.Replace(" exec ", "");
                str = str.Replace(" xp_cmd ", "");
                str = str.Replace("<script", "");
            }
            else
            {
                str = "null";
            }
            return str;
        }
        public String FilterStringDR(string strVal)
        {
            var str = FilterQuoteDR(strVal);

            if (str.ToLower() != "null")
                str = "'" + str + "'";

            return str.TrimEnd().TrimStart();
        }
        public DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            StreamReader sr = new StreamReader(strFilePath);
            string[] headers = sr.ReadLine().Split(',');
            DataTable dt = new DataTable();
            foreach (string header in headers)
            {
                dt.Columns.Add(header);
            }
            while (!sr.EndOfStream)
            {
                string[] rows = Regex.Split(sr.ReadLine(), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                DataRow dr = dt.NewRow();
                for (int i = 0; i < headers.Length; i++)
                {
                    try
                    {
                        dr[i] = FilterQuote(rows[i]);
                    }
                    catch
                    {
                        dr[i] = "";
                    }
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        public String singleQuote(string strVal, string defaultValue = "")
        {
            if (String.IsNullOrEmpty(strVal))
                strVal = defaultValue;
            var str = FilterQuote(strVal);

            if (str.ToLower() != "null")
                str = "'" + str + "'";

            return str.TrimEnd().TrimStart();
        }
        public String doubleQuote(string strVal, string defaultValue = "")
        {
            if (String.IsNullOrEmpty(strVal))
                strVal = defaultValue;
            var str = FilterQuote(strVal);

            if (str.ToLower() != "null")
                str = "\"" + str + "\"";
            return str.TrimEnd().TrimStart();
        }
        public String singleQuoteUnicode(string strVal)
        {
            var str = FilterQuote(strVal);

            if (str.ToLower() != "null")
                str = "N'" + str + "'";

            return str.TrimEnd().TrimStart();
        }
        public String singleHTMLQuoteUnicode(string strVal)
        {
            var str = FilterQuoteEmail(strVal);

            if (str.ToLower() != "null")
                str = "N'" + str + "'";

            return str.TrimEnd().TrimStart();
        }
        public String CharFromIndex(String Input, int index = 0)
        {
            if (Input.Length > 0)
            {
                return Input[index].ToString().ToUpper();
            }
            return Input;
        }
        public string ToFormattedThousands(string number)
        {
            var fnumber = string.Format("{0:0.00}", Convert.ToDecimal(number));
            string[] data = fnumber.Split('.');
            return Convert.ToInt32(data[0]).ToString("N0") + "." + data[1];
        }
        public string generateCheckSum(List<string> input)
        {
            string checksum = "";
            for (int i = 0; i < input.Count; i++)
            {
                if (i < input.Count - 1)
                {
                    checksum = checksum + input[i] + "|";
                }
                else
                {
                    checksum = checksum + input[i];
                }
            }
            return checksum;
        }
        public String GetImageBase64FromPath(string imageName, string virtualPath)
        {
            var dir = HttpContext.Current.Server.MapPath(virtualPath);
            try
            {
                if (String.IsNullOrEmpty(imageName))
                {
                    return "";
                }
                else
                {
                    byte[] imageArray = System.IO.File.ReadAllBytes(dir + "\\" + imageName);
                    string imageBase64 = Convert.ToBase64String(imageArray);
                    return string.Format("data:image/jpeg;base64,{0}", imageBase64);
                }
            }
            catch
            {
                return "";
            }
        }
    }
}
