using Repository.DAL;
using System;
using System.Collections.Generic;
using System.Data;

namespace Repository.Common
{
    public class CommonData
    {
        public String UNIQUEID { get; set; }
        public String CODE { get; set; }
        public string ACTION { get; set; }
        public string STATUS { get; set; }
        public string MESSAGE { get; set; }
    }

    public interface ICommonRepository
    {
        Dictionary<string, string> GetCategoryList();
        Dictionary<string, string> GetSPDropdown(string sql, string OptionValue, string OptionName);
        List<StaticDataType> GetStaticDataTypes();
        StaticDataType GetStaticDataTypeDetail(string TypeID);
        List<Static_Data> GetStaticDatas(string TypeID);
        CommonData SaveStaticData(Static_Data inp);
        Static_Data GetStaticData(string ID);
        CommonData DeleteStaticData(string ID);
        List<SystemActivity> GetNotification(String flag = "s");
        List<SystemActivity> ViewAllNotification(String userid);
        List<SystemActivity> GetTopThreeNotification();
        string NotificationCount();
        CommonData SaveError(string ErrorDescription, string Path);
        Error_Log GetErrorDetail(string ErrorId);

    }
    public class CommonRepository : ICommonRepository
    {
        RepositoryDao dao;
        public CommonRepository()
        {
            dao = new RepositoryDao();
        }
        public Dictionary<string, string> GetCategoryList()
        {
            string sql = "spa_Product @flag='c'";
            return GetSPDropdown(sql, "DATA", "VALUE");
        }
        public Dictionary<string, string> GetSPDropdown(string sql, string OptionValue, string OptionName)//Option name will ve Text and OptionValue will be Value
        {
            var dictionary = new Dictionary<string, string>();
            var dt = dao.ExecuteDataTable(sql);
            if (null == dt || dt.Rows.Count == 0)
            {
                return dictionary;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dictionary.Add(dt.Rows[i][OptionValue].ToString(), dt.Rows[i][OptionName].ToString());
            }
            return dictionary;
        }

        public List<StaticDataType> GetStaticDataTypes()
        {
            var lst = new List<StaticDataType>();
            string sql = "spa_STATIC_DATA_TYPE @flag='s'";
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    lst.Add(new StaticDataType
                    {
                        TYPE_ID = dr["TYPE_ID"].ToString(),
                        TYPE_NAME = dr["TYPE_NAME"].ToString(),
                        TYPE_DESCRIPTION = dr["TYPE_DESCRIPTION"].ToString()
                    });
                }
            }
            return lst;
        }
        public StaticDataType GetStaticDataTypeDetail(string TypeID)
        {
            StaticDataType ret = new StaticDataType();
            string sql = "spa_STATIC_DATA_TYPE @flag='sd'" +
                ",@TYPE_ID=" + dao.singleQuote(TypeID);
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows.Count > 0)
                {
                    ret = new StaticDataType
                    {
                        TYPE_ID = dt.Rows[0]["TYPE_ID"].ToString(),
                        TYPE_NAME = dt.Rows[0]["TYPE_NAME"].ToString(),
                        TYPE_DESCRIPTION = dt.Rows[0]["TYPE_DESCRIPTION"].ToString()
                    };
                }
            }
            return ret;
        }
        public List<Static_Data> GetStaticDatas(string TypeID)
        {
            var lst = new List<Static_Data>();
            string sql = "spa_STATIC_DATA_TYPE @flag='g',@type_id=" + dao.singleQuote(TypeID);
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    lst.Add(new Static_Data
                    {
                        ID = dr["ID"].ToString(),
                        TYPE_ID = dr["TYPE_ID"].ToString(),
                        STATIC_VALUE = dr["STATIC_VALUE"].ToString(),
                        STATIC_DATA = dr["STATIC_DATA"].ToString(),
                        DESCRIPTION = dr["DESCRIPTION"].ToString(),
                        HELPDESK_DETAIL = dr["HELPDESK_DETAIL"].ToString(),
                        OTHER_VALUE_1 = dr["OTHER_VALUE_1"].ToString(),
                        OTHER_VALUE_2 = dr["OTHER_VALUE_2"].ToString(),
                        OTHER_VALUE_3 = dr["OTHER_VALUE_3"].ToString(),
                        OTHER_VALUE_4 = dr["OTHER_VALUE_4"].ToString()
                    });
                }
            }
            return lst;
        }
        public CommonData SaveStaticData(Static_Data inp)
        {
            CommonData ret = new CommonData();
            string sql = "spa_STATIC_DATA_TYPE @flag=" + dao.singleQuote(inp.Flag) +
                ",@ID=" + dao.singleQuote(inp.ID) +
                ",@TYPE_ID=" + dao.singleQuote(inp.TYPE_ID) +
                ",@static_value=" + dao.singleQuote(inp.STATIC_VALUE) +
                ",@static_data=" + dao.singleQuote(inp.STATIC_DATA) +
                ",@description=" + dao.singleQuote(inp.DESCRIPTION) +
                ",@additional_value=" + dao.singleQuote(inp.OTHER_VALUE_1) +
                ",@additional_value2=" + dao.singleQuote(inp.OTHER_VALUE_2) +
                ",@helpdesk_detail=" + dao.singleQuote(inp.HELPDESK_DETAIL) +
                ",@additional_value3=" + dao.singleQuote(inp.OTHER_VALUE_3) +
                ",@additional_value4=" + dao.singleQuote(inp.OTHER_VALUE_4);
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
        public Static_Data GetStaticData(string ID)
        {
            Static_Data ret = new Static_Data();
            string sql = "spa_STATIC_DATA_TYPE @flag='c'" +
                ",@ID=" + dao.singleQuote(ID);
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows.Count > 0)
                {
                    ret = new Static_Data
                    {
                        ID = dt.Rows[0]["ID"].ToString(),
                        TYPE_ID = dt.Rows[0]["TYPE_ID"].ToString(),
                        STATIC_DATA = dt.Rows[0]["STATIC_DATA"].ToString(),
                        STATIC_VALUE = dt.Rows[0]["STATIC_VALUE"].ToString(),
                        DESCRIPTION = dt.Rows[0]["DESCRIPTION"].ToString(),
                        HELPDESK_DETAIL = dt.Rows[0]["HELPDESK_DETAIL"].ToString(),
                        OTHER_VALUE_1 = dt.Rows[0]["OTHER_VALUE_1"].ToString(),
                        OTHER_VALUE_2 = dt.Rows[0]["OTHER_VALUE_2"].ToString(),
                        OTHER_VALUE_3 = dt.Rows[0]["OTHER_VALUE_3"].ToString(),
                        OTHER_VALUE_4 = dt.Rows[0]["OTHER_VALUE_4"].ToString()
                    };
                }
            }
            return ret;
        }
        public CommonData DeleteStaticData(string ID)
        {
            CommonData ret = new CommonData();
            string sql = "spa_STATIC_DATA_TYPE @flag='d'" +
                ",@ID=" + dao.singleQuote(ID);
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows.Count > 0)
                {
                    ret = new CommonData
                    {
                        CODE = dt.Rows[0]["CODE"].ToString(),
                        MESSAGE = dt.Rows[0]["MESSAGE"].ToString(),
                    };
                }
            }
            return ret;
        }
        public List<SystemActivity> GetNotification(String flag = "s")
        {
            List<SystemActivity> ret = new List<SystemActivity>();
            DataTable dt = dao.ExecuteDataTable("spa_CP_System_Activity @flag=" + dao.singleQuote(flag));
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ret.Add(new SystemActivity
                    {
                        LOGID = dr["LOGID"].ToString(),
                        MESSAGE = dr["MESSAGE"].ToString(),
                        MSG = dr["MSG"].ToString(),
                        CREATE_TS = dr["CREATE_TS"].ToString(),
                        TABLE_NAME = dr["TABLE_NAME"].ToString(),
                        ICON = dr["ICON"].ToString(),
                        BTNCLASS = dr["BTNCLASS"].ToString(),
                        POSTED_ON = dr["POSTED_ON"].ToString(),
                        SEEN_STATUS = dr["SEEN_STATUS"].ToString(),
                        SEEN_BY = dr["SEEN_BY"].ToString()
                        //SEEN_USER_PIC = fup.GetImageSource(dr["SEEN_USER_PIC"])
                    });
                }
            }                
            return ret;
        }

        public List<SystemActivity> ViewAllNotification(String userid)
        {
            List<SystemActivity> ret = new List<SystemActivity>();
            DataTable dt = dao.ExecuteDataTable("spa_CP_System_Activity @flag='sa'" +
                ",@userid=" + dao.singleQuote(userid));
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ret.Add(new SystemActivity
                    {
                        LOGID = dr["LOGID"].ToString(),
                        MESSAGE = dr["MESSAGE"].ToString(),
                        MSG = dr["MSG"].ToString(),
                        CREATE_TS = dr["CREATE_TS"].ToString(),
                        TABLE_NAME = dr["TABLE_NAME"].ToString(),
                        ICON = dr["ICON"].ToString(),
                        BTNCLASS = dr["BTNCLASS"].ToString(),
                        POSTED_ON = dr["POSTED_ON"].ToString(),
                        SEEN_STATUS = dr["SEEN_STATUS"].ToString(),
                        SEEN_BY = dr["SEEN_BY"].ToString()
                        //SEEN_USER_PIC = fup.GetImageSource(dr["SEEN_USER_PIC"])
                    });
                }
            }
            return ret;
        }
        public List<SystemActivity> GetTopThreeNotification()
        {
            List<SystemActivity> ret = new List<SystemActivity>();
            DataTable dt = dao.ExecuteDataTable("spa_CP_System_Activity @flag='h'");
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ret.Add(new SystemActivity
                    {
                        LOGID = dr["LOGID"].ToString(),
                        MESSAGE = dr["MESSAGE"].ToString(),
                        MSG = dr["MSG"].ToString(),
                        CREATE_TS = dr["CREATE_TS"].ToString(),
                        TABLE_NAME = dr["TABLE_NAME"].ToString(),
                        ICON = dr["ICON"].ToString(),
                        BTNCLASS = dr["BTNCLASS"].ToString(),
                        POSTED_ON = dr["POSTED_ON"].ToString(),
                        SEEN_STATUS = dr["SEEN_STATUS"].ToString(),
                        SEEN_BY = dr["SEEN_BY"].ToString()
                        //SEEN_USER_PIC = fup.GetImageSource(dr["SEEN_USER_PIC"])
                    });
                }
            }
            return ret;
        }
        public string NotificationCount()
        {
            int ret = 0;
            string sql = "spa_CP_System_Activity @flag='n' ";
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                ret = Convert.ToInt32(dt.Rows[0]["CountNo"]);
            }
            return ret.ToString();
        }
        public CommonData SaveError(string ErrorDescription, string Path)
        {
            CommonData ret = new CommonData();
            string sql = "spa_Error_Log @flag = 'i'" +
            ",@errorDesc = " + dao.singleQuote(ErrorDescription) +
            ",@path=" + dao.singleQuote(Path) +
            ",@ipaddress=" + dao.singleQuote("");
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                ret.CODE = dt.Rows[0]["Code"].ToString();
                ret.MESSAGE = dt.Rows[0]["Message"].ToString();
                ret.UNIQUEID = dt.Rows[0]["ERROR_ID"].ToString();
            }
            return ret;
        }
        public Error_Log GetErrorDetail(string ErrorId)
        {
            Error_Log ret = new Error_Log();
            string sql = "spa_Error_Log @flag = 'a'" +
            ",@errorid = " + dao.singleQuote(ErrorId);
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                ret.ERROR_ID = dt.Rows[0]["ERROR_ID"].ToString();
                ret.ERROR_DESCRIPTION = dt.Rows[0]["ERROR_DESCRIPTION"].ToString();
                ret.SCRIPT = dt.Rows[0]["SCRIPT"].ToString();
                ret.CREATE_TS = dt.Rows[0]["CREATE_TS"].ToString();
                ret.IP = dt.Rows[0]["IP"].ToString();
            }
            return ret;
        }
    }
}
