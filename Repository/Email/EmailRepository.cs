using Repository.Common;
using Repository.DAL;
using System;
using System.Collections.Generic;
using System.Data;

namespace Repository.Email
{
    public class EmailRepository : IEmailRepository
    {
        RepositoryDao dao;
        public EmailRepository()
        {
            dao = new RepositoryDao();
        }
        public List<EmailTemplate> EmailTemplateList()
        {
            List<EmailTemplate> lst = new List<EmailTemplate>();
            DataTable dt = dao.ExecuteDataTable("[spa_EMAIL_TEMPLATE] @flag='s'");
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    lst.Add(new EmailTemplate
                    {
                        TEMP_ID = Convert.ToInt32(dr["TEMP_ID"]),
                        TEMP_NAME = dr["TEMP_NAME"].ToString(),
                        TEMP_EMAIL_SUBJECT = dr["TEMP_EMAIL_SUBJECT"].ToString(),
                        TEMP_EMAIL_BODY = dr["TEMP_EMAIL_BODY"].ToString(),
                        IS_ENABLE = dr["IS_ENABLE"].ToString(),
                        RESPONSE_TO_ADMIN = dr["RESPONSE_TO_ADMIN"].ToString()
                    });
                }
            }
            return lst;
        }
        public EmailTemplate EditTemplate(string ID)
        {
            EmailTemplate ret = new EmailTemplate();
            string sql = "[spa_EMAIL_TEMPLATE] @flag='e',@TEMP_ID=" + dao.singleQuote(ID);
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows.Count > 0)
                {
                    ret = new EmailTemplate
                    {
                        TEMP_ID = Convert.ToInt32(dt.Rows[0]["TEMP_ID"]),
                        TEMP_NAME = dt.Rows[0]["TEMP_NAME"].ToString(),
                        TEMP_EMAIL_SUBJECT = dt.Rows[0]["TEMP_EMAIL_SUBJECT"].ToString(),
                        TEMP_EMAIL_BODY = dt.Rows[0]["TEMP_EMAIL_BODY"].ToString(),
                        enable = Convert.ToBoolean(dt.Rows[0]["IS_ENABLE"]),
                        Response = Convert.ToBoolean(dt.Rows[0]["RESPONSE_TO_ADMIN"])
                    };
                }
            }
            return ret;
        }
        public CommonData SaveTemplate(string flag, int Temp_ID, string TempName, string EmailSubject, string EmailBody, bool isEnable, bool ResponseToAdmin)
        {
            CommonData ret = new CommonData();
            string sql = "[spa_EMAIL_TEMPLATE] @flag=" + dao.singleQuote(flag) +
                ",@TEMP_ID=" + dao.singleQuote(Temp_ID.ToString()) +
                ",@TEMP_NAME=" + dao.singleQuote(TempName) +
                ",@TEMP_EMAIL_SUBJECT=" + dao.singleQuote(EmailSubject) +
                ",@TEMP_EMAIL_BODY=" + dao.singleHTMLQuoteUnicode(EmailBody) +
                ",@IS_ENABLE=" + isEnable +
                ",@RESPONSE_TO_ADMIN=" + ResponseToAdmin;
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows.Count > 0)
                {
                    ret = new CommonData
                    {
                        CODE = dt.Rows[0]["Code"].ToString(),
                        MESSAGE = dt.Rows[0]["Message"].ToString()
                    };
                }
            }
            return ret;
        }
        public List<Static_Data> GetEmailIDList()
        {
            List<Static_Data> lst = new List<Static_Data>();
            DataTable dt = dao.ExecuteDataTable("[spa_EMAIL_TEMPLATE] @flag='t'");
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    lst.Add(new Static_Data
                    {
                        STATIC_DATA = dr["STATIC_DATA"].ToString(),
                        DESCRIPTION = dr["DESCRIPTION"].ToString()
                    });
                }
            }
            return lst;
        }
    }
}
