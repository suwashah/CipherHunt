using Repository.Common;
using Repository.DAL;
using System;
using System.Collections.Generic;
using System.Data;

namespace Repository.CPanel
{
    public interface ICpanelUserRepository
    {
        CPanelDetail LoginUser(string UserName, string Password);
        CommonData RequestAuthorization(string ID, string AuthKey);
        CommonData ChangePassword(string OldPassword, string NewPassword, string ID);
        List<CPanelDetail> GetALLCpanelUsers(string ID);
        CPanelDetail GetCPanelUserDetails(string ID);
        CommonData AddCpanelUser(CPanelDetail inp);
        CommonData UpdateCpanelUser(CPanelDetail inp);
        CommonData UpdateAuthentication(String Flag, String ID, String User);
        List<CPANEL_USER_REMARKS> GetCpanelUserRemark(string ID);
    }
    public class CpanelUserRepository : ICpanelUserRepository
    {
        RepositoryDao dao;
        public CpanelUserRepository()
        {
            dao = new RepositoryDao();
        }
        public CPanelDetail LoginUser(string UserName, string Password)
        {
            var ret = new CPanelDetail();
            string sql = "spa_CP_userLogin @UserName=" + dao.singleQuote(UserName) +
                ",@Password=" + dao.singleQuote(Password);
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {

                ret.CODE = dt.Rows[0]["Code"].ToString();
                ret.MESSAGE = dt.Rows[0]["Message"].ToString();
                if (ret.CODE == "0")
                {
                    ret.UserName = dt.Rows[0]["USERNAME"].ToString();
                    ret.StaffName = dt.Rows[0]["STAFFNAME"].ToString();
                    ret.Email = dt.Rows[0]["EMAIL"].ToString();
                    ret.UNIQUEID = dt.Rows[0]["ID"].ToString();
                    ret.Token = dt.Rows[0]["TOKEN"].ToString();
                    ret.Auth_Approved_By = dt.Rows[0]["AUTH_APPROVED_BY"].ToString();
                    ret.Auth_Approved_Ts = dt.Rows[0]["AUTH_APPROVED_TS"].ToString();
                    ret.Auth_DateTime = dt.Rows[0]["AUTH_DATETIME"].ToString();
                    ret.Auth_Required = Convert.ToBoolean(dt.Rows[0]["AUTH_REQUIRED"]);
                    ret.Enable_Pwd_Change = Convert.ToBoolean(dt.Rows[0]["ENABLE_PWD_CHANGE"]);
                }
            }
            return ret;
        }
        public CommonData RequestAuthorization(string ID, string AuthKey)
        {
            var ret = new CPanelDetail();
            string sql = "spa_CPANEL_USER @flag='uat',@ID=" + dao.singleQuote(ID) +
                ",@Auth_key=" + dao.singleQuote(AuthKey);
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                ret.CODE = dt.Rows[0]["Code"].ToString();
                ret.MESSAGE = dt.Rows[0]["Message"].ToString();
            }
            return ret;
        }
        public CommonData ChangePassword(string OldPassword, string NewPassword, string ID)
        {
            var ret = new CPanelDetail();
            string sql = "spa_CPANEL_USER @flag='pw'" +
                ",@oldpassword=" + dao.singleQuote(OldPassword) +
                ",@password=" + dao.singleQuote(NewPassword) +
                ",@ID=" + dao.singleQuote(ID);
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                ret.CODE = dt.Rows[0]["Code"].ToString();
                ret.MESSAGE = dt.Rows[0]["Message"].ToString();
            }
            return ret;
        }
        public List<CPanelDetail> GetALLCpanelUsers(string ID)
        {
            List<CPanelDetail> lst = new List<CPanelDetail>();
            DataTable dt = dao.ExecuteDataTable("spa_CPANEL_USER @flag='s',@ID=" + dao.singleQuote(ID));
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    lst.Add(new CPanelDetail
                    {
                        UNIQUEID = dr["ID"].ToString(),
                        UserName = dr["USERNAME"].ToString(),
                        StaffName = dr["STAFFNAME"].ToString(),
                        Email = dr["EMAIL"].ToString(),
                        Create_TS = dr["CREATETS"].ToString(),
                        Create_By = dr["CREATEBY"].ToString(),
                        Lock_Status = Convert.ToBoolean(dr["LOCK_STATUS"]),
                        Lock_Icon = dr["LOCK_ICON"].ToString(),
                        Lock_By = dr["LOCK_BY"].ToString(),
                        Lock_Reason = dr["LOCK_REASON"].ToString(),
                        Prof_Pic = "",
                        Enable_Pwd_Change = Convert.ToBoolean(dr["ENABLE_PWD_CHANGE"])
                    });
                }
            }
            return lst;
        }
        public CPanelDetail GetCPanelUserDetails(string ID)
        {
            var ret = new CPanelDetail();
            string sql = "spa_CPANEL_USER @flag='ss',@ID=" + dao.singleQuote(ID);
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                ret.UserName = dt.Rows[0]["USERNAME"].ToString();
                ret.StaffName = dt.Rows[0]["STAFFNAME"].ToString();
                ret.Email = dt.Rows[0]["EMAIL"].ToString();
                ret.UNIQUEID = dt.Rows[0]["ID"].ToString();
                ret.Token = dt.Rows[0]["TOKEN"].ToString();
                ret.Create_By = dt.Rows[0]["CREATEBY"].ToString();
                ret.Create_TS = dt.Rows[0]["CREATETS"].ToString();
                ret.Update_By = dt.Rows[0]["UPDATEBY"].ToString();
                ret.Updte_TS = dt.Rows[0]["UPDATETS"].ToString();
                ret.Lock_Status = Convert.ToBoolean(dt.Rows[0]["LOCK_STATUS"]);
                ret.Lock_By = dt.Rows[0]["LOCK_BY"].ToString();
                ret.Lock_Reason = dt.Rows[0]["LOCK_REASON"].ToString();
                ret.Enable_Pwd_Change = Convert.ToBoolean(dt.Rows[0]["ENABLE_PWD_CHANGE"]);
                ret.LastLoginDate = dt.Rows[0]["LAST_LOGIN_DATE"].ToString();
                ret.Auth_key = dt.Rows[0]["AUTH_KEY"].ToString();
                ret.Auth_Approved_By = dt.Rows[0]["AUTH_APPROVED_BY"].ToString();
                ret.Auth_Approved_Ts = dt.Rows[0]["AUTH_APPROVED_TS"].ToString();
                ret.Auth_DateTime = dt.Rows[0]["AUTH_DATETIME"].ToString();
                ret.Auth_Required = Convert.ToBoolean(dt.Rows[0]["AUTH_REQUIRED"]);
                ret.isEnable = Convert.ToBoolean(dt.Rows[0]["STATUS"]);
            }
            return ret;
        }
        public CommonData AddCpanelUser(CPanelDetail inp)
        {
            CommonData ret = new CommonData();
            string sql = "spa_CPANEL_USER @flag='i'" +
                ",@username =" + dao.singleQuote(inp.UserName) +
                ",@staffname =" + dao.singleQuote(inp.StaffName) +
                ",@email=" + dao.singleQuote(inp.Email) +
                ",@create_by=" + dao.singleQuote(inp.Create_By);
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                ret.CODE = dt.Rows[0]["Code"].ToString();
                ret.MESSAGE = dt.Rows[0]["Message"].ToString();
            }
            return ret;
        }
        public CommonData UpdateCpanelUser(CPanelDetail inp)
        {
            CommonData ret = new CommonData();
            string sql = "spa_CPANEL_USER @flag='u'" +
                ",@staffname =" + dao.singleQuote(inp.StaffName) +
                ",@email=" + dao.singleQuote(inp.Email) +
                ",@ID=" + dao.singleQuote(inp.UNIQUEID) +
                ",@Lock_Status=" + inp.Lock_Status +
                ",@status=" + inp.isEnable +
                ",@Auth_Required=" + inp.Auth_Required +
                ",@create_by=" + dao.singleQuote(inp.Create_By);
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                ret.CODE = dt.Rows[0]["Code"].ToString();
                ret.MESSAGE = dt.Rows[0]["Message"].ToString();
            }
            return ret;
        }
        public CommonData UpdateAuthentication(String Flag, String ID, String User)
        {
            CommonData ret = new CommonData();
            string sql = "spa_CPANEL_USER @flag=" + dao.singleQuote(Flag) +
                ",@ID=" + dao.singleQuote(ID) +
                ",@create_by=" + dao.singleQuote(User);
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                ret.CODE = dt.Rows[0]["Code"].ToString();
                ret.MESSAGE = dt.Rows[0]["Message"].ToString();
            }
            return ret;
        }
        public List<CPANEL_USER_REMARKS> GetCpanelUserRemark(string ID)
        {
            List<CPANEL_USER_REMARKS> lst = new List<CPANEL_USER_REMARKS>();
            DataTable dt = dao.ExecuteDataTable("spa_CPANEL_USER @flag='crem',@ID=" + dao.singleQuote(ID));
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    lst.Add(new CPANEL_USER_REMARKS
                    {
                        REMARK_ID = Convert.ToInt32(dr["REMARK_ID"].ToString()),
                        USER_ID = Convert.ToInt32(dr["USER_ID"].ToString()),
                        REMARK = dr["REMARK"].ToString(),
                        REMARK_TYPE = dr["REMARK_TYPE"].ToString(),
                        UPDATE_BY = dr["UPDATE_BY"].ToString(),
                        UPDATE_TS = dr["UPDATE_TS"].ToString()
                    });
                }
            }
            return lst;
        }
    }
}
