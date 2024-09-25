using System;
using System.Configuration;

namespace Repository.CommonClass
{
    public class Strings
    {
        public static class ClaimType
        {
            public const string AgentCode = "AgentCode";
            public const string AgentBranchCode = "AgentBranchCode";
            public const string BranchCodeChar = "BranchCodeChar";
            public const string CPanelUserToken = "CPanelUserToken";
            public const string CPanelUserID = "CPanelUserID";
            public const string CPanelUserName = "CPanelUserName";
        }
    }
    public static class GlobalVariables
    {
        public static string Code = "0";
        public static String Receiver = ConfigurationManager.AppSettings["Receiver"].ToString();
        public static String Receivers = ConfigurationManager.AppSettings["Receivers"].ToString();
    }
}
