using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Xml;

namespace Repository.HelperFunction
{
    public interface IUtilityHelper
    {
        string maskCardNumber(string cardNo, int prefixDisplayCount, int suffixDisplayCount, int cardLength = 16);
        bool checkCardIdFormat(string id);
        string CreateUserPaymentXml(string user_card_id, string card_exp_month, string card_exp_year);
        string GetClientIP();
        string GetClientMachineIP();
        string GetMAC();
        string GetSHA256(string text);
        string GetIPAddressClient();
        string generateCardID();
        string GetIPAddress(string url);
        string GetParam();
        string ToXML(Object oObject);
        string ToXMLString(String oObject);
        string getXMlValue(XmlDocument xmlObj, string NodeName);
        string getXMlNodeAttributesValue(XmlDocument xmlObj, string NodeName, string InnerAttributes);
        void SendHTTPRequest(string request_string);
        string requestForm(string var);
        string formateDate(DateTime inputDate);
        string PostRedirection(string URL, string msg, string code);
        string Decrypt(string base64EncodedData, string salt);
        String ConstructQueryString(NameValueCollection parameters);
        bool emailIsValid(string email);
        bool HasSpecialCharacter(string myString);
        string ConvertDate(string dateString);
        string MD5Encode(string str_encode);
        string GetMaskedEmail(string emailaddress);
        string changeFilename(string input);
        int decimal_round();
        string ConfigPaymentType();
        string DecimalAmountFormatter(string amount);
        string DecimalAmountFormatterCustomRound(string amount, int roundvalue = 0);
        string AmountCommaRemover(string str);

        string StringToDecimalwithRetString(string input);
        string EncryptSession(string userid);
        void AcquireRequestState();
        string DefaultEncryptionKey();
        string replaceByhyphen(string str);
        string CheckSession(string SessionName, bool isAgent = false);
        void setSecureProtocol(bool bSecure);
        object isNull(object input, string replace);
        object isNull(object input, int replace);
        string isNull(string input, string replacement);
        string GetUnixTimeStamp();
        void setCookies(string cookiesName, string cookiesValue);
        void DestroyCookies(string cookiesName);
        string GetCookiesValue(string cookiesName);
        string GetAllRequestFormValue();
        bool isNumber(string input);
        string GetAppsettingValue(string keyname);
        int[] BubbleSort(int[] arr);
        Array SortData(Array arr);
        void AlertSound(string SoundPath, bool Looped = false);
        void StopSound(string SoundPath);
        void PlayClientAlert(string sound_source = "Sounds/alert.wav");
        string CheckClientSession(string SessionName);
        string EncryptString(string clearText, string EncryptionKey);
        string DecryptString(string cipherText, string EncryptionKey);
        String FilterQuote(string strVal);
        String FilterQuoteEmail(string strVal);
        String FilterQuoteDR(string strVal);
        String FilterStringDR(string strVal);
        DataTable ConvertCSVtoDataTable(string strFilePath);
        String singleQuote(string strVal, string defaultValue = "");
        String doubleQuote(string strVal, string defaultValue = "");
        String singleQuoteUnicode(string strVal);
        String singleHTMLQuoteUnicode(string strVal);
        String CharFromIndex(String Input, int index = 0);
        string ToFormattedThousands(string number);
        string generateCheckSum(List<string> input);
        String GetImageBase64FromPath(string imageName, string virtualPath);
    }
}
