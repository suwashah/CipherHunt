using Repository.DAL;
using System;
using System.Collections.Generic;
using System.Data;

namespace Repository.Common
{
    public class Error_Log
    {
        public string ERROR_ID { get; set; }
        public string ERROR_DESCRIPTION { get; set; }
        public string SCRIPT { get; set; }
        public string CREATE_TS { get; set; }
        public string IP { get; set; }
    }
    public class WebAppConfig
    {
        public int LOGO_HEIGHT { get; set; }
        public int LOGO_WIDTH { get; set; }
        public string HEIGHT { get; set; }
        public string WIDTH { get; set; }
        public string LOGO { get; set; }
        public string BACKGROUND_IMAGE { get; set; }
        public string DATE_FORMAT { get; set; }
        public string DATE_REGULAR_EXPRESSION { get; set; }
        public string DATE_MASKING_FORMAT { get; set; }
        public string COMPANY_NAME { get; set; }
        public string WEB_URL { get; set; }
        public string SYSTEM_NAME { get; set; }
        public string CLIENT_EMAIL { get; set; }
        public string ADDRESS { get; set; }
        public string CLIENT_PHONE { get; set; }
    }
    public interface IApplicationConfig
    {
        WebAppConfig WebAppConfig();
        string GetAppConfig(string Key);
        Dictionary<string, string> AppConfig();

    }
    public class ApplicationConfig : IApplicationConfig
    {
        RepositoryDao dao;
        public ApplicationConfig()
        {
            dao = new RepositoryDao();
        }
        public WebAppConfig WebAppConfig()
        {
            WebAppConfig obj = new WebAppConfig();
            string sql = "spa_Application_Config @flag='app'";
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                obj.LOGO_HEIGHT = Convert.ToInt32(dt.Rows[0]["LOGO_HEIGHT"]);
                obj.LOGO_WIDTH = Convert.ToInt32(dt.Rows[0]["LOGO_WIDTH"]);
                obj.HEIGHT = dt.Rows[0]["LOGO_HEIGHT"].ToString();
                obj.WIDTH = dt.Rows[0]["LOGO_WIDTH"].ToString(); //Red
                obj.LOGO = dt.Rows[0]["LOGO"].ToString();
                obj.BACKGROUND_IMAGE = dt.Rows[0]["BACKGROUND_IMAGE"].ToString();
                obj.DATE_FORMAT = dt.Rows[0]["DATE_FORMAT"].ToString();
                obj.DATE_REGULAR_EXPRESSION = dt.Rows[0]["DATE_REGULAR_EXPRESSION"].ToString();
                obj.DATE_MASKING_FORMAT = dt.Rows[0]["DATE_MASKING_FORMAT"].ToString();
                obj.COMPANY_NAME = dt.Rows[0]["COMPANY_NAME"].ToString();
                obj.WEB_URL = dt.Rows[0]["WEB_URL"].ToString();
                obj.SYSTEM_NAME = dt.Rows[0]["SYSTEM_NAME"].ToString();
                obj.CLIENT_EMAIL = dt.Rows[0]["CLIENT_EMAIL"].ToString();
                obj.ADDRESS = dt.Rows[0]["ADDRESS"].ToString();
                obj.CLIENT_PHONE = dt.Rows[0]["CLIENT_PHONE"].ToString();
            }
            return obj;
        }
        public string GetAppConfig(string Key)
        {
            return dao.GetAppsettingValue(Key);
        }
        public Dictionary<string, string> AppConfig()
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            string sql = "select CONFIG_PARAM,CONFIG_VALUE from TBL_APPLICATION_CONFIG";
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ret.Add(dr["CONFIG_PARAM"].ToString(), dr["CONFIG_VALUE"].ToString());
                }
            }
            return ret;
        }
    }
}
