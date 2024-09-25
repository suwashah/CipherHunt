using Repository.Common;
using Repository.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Repository.Customer
{
    public interface ICustomerRepository
    {
        CustomerDetail CustomerLogin(string UserName, string Password);
        CommonData RegisterUser(CustomerDetail u);
        Task<CommonData> asyncRegisterUser(CustomerDetail inp);
        List<CustomerDetail> AllCustomerList();
        CustomerDetail GetCustomerDetail(string ID, string Flag = "CUST");
    }
    public class CustomerRepository : ICustomerRepository
    {
        RepositoryDao dao;
        public CustomerRepository()
        {
            dao = new RepositoryDao();
        }
        public CustomerDetail CustomerLogin(string UserName, string Password)
        {
            var ret = new CustomerDetail();
            string sql = "spa_CustomerLogin @UserName=" + dao.singleQuote(UserName) +
                ",@Password=" + dao.singleQuote(Password);
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {

                ret.CODE = dt.Rows[0]["Code"].ToString();
                ret.MESSAGE = dt.Rows[0]["Message"].ToString();
                if (ret.CODE == "0")
                {
                    ret.TOKEN = dt.Rows[0]["TOKEN"].ToString();
                    ret.ID = dt.Rows[0]["ID"].ToString();
                    ret.UNIQUEID = dt.Rows[0]["ID"].ToString();
                    ret.MEMBERID = dt.Rows[0]["MEMBERID"].ToString();
                    ret.ENABLEPASSWORDCHANGE = Convert.ToBoolean(dt.Rows[0]["ENABLEPASSWORDCHANGE"]);
                    ret.USERNAME = dt.Rows[0]["USERNAME"].ToString();
                    ret.FULLNAME = dt.Rows[0]["FULLNAME"].ToString();
                    ret.EMAIL = dt.Rows[0]["EMAIL"].ToString();
                }
            }
            return ret;
        }
        public CommonData RegisterUser(CustomerDetail u)
        {
            var cvm = new CommonData();
            String sql = "spa_CustomerSignup @flag='i'" +
                ",@FIRSTNAME=" + dao.singleQuote(u.FIRSTNAME) +
                ",@MIDDLENAME=" + dao.singleQuote(u.MIDDLENAME) +
                ",@LASTNAME=" + dao.singleQuote(u.LASTNAME) +
                ",@EMAIL=" + dao.singleQuote(u.EMAIL) +
                ",@PASSWORD=" + dao.singleQuote(u.PASSWORD) +
                ",@PHONENUMBER=" + dao.singleQuote(u.PHONE) +
                ",@GENDER=" + dao.singleQuote(u.GENDER) +
                ",@INVITECODE=" + dao.singleQuote(u.INVITE_CODE);
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                cvm.CODE = dt.Rows[0]["Code"].ToString();
                cvm.MESSAGE = dt.Rows[0]["Message"].ToString();
                if (cvm.CODE == "0")
                {
                    cvm.UNIQUEID = dt.Rows[0]["Customer_Sno"].ToString();
                }
            }
            return cvm;
        }
        public Task<CommonData> asyncRegisterUser(CustomerDetail inp)
        {
            var ret = RegisterUser(inp);
            Thread.Sleep(2000);
            return Task.FromResult(ret);
        }
        public List<CustomerDetail> AllCustomerList()
        {
            List<CustomerDetail> lst = new List<CustomerDetail>();
            DataTable dt = dao.ExecuteDataTable("spa_Customers @flag='a'");
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    lst.Add(new CustomerDetail
                    {
                        ID = dr["ID"].ToString(),
                        MEMBERID = dr["MEMBERID"].ToString(),
                        FIRSTNAME = dr["FIRSTNAME"].ToString(),
                        MIDDLENAME = dr["MIDDLENAME"].ToString(),
                        LASTNAME = dr["LASTNAME"].ToString(),
                        FULLNAME = dr["FULLNAME"].ToString(),
                        EMAIL = dr["EMAIL"].ToString(),
                        PHONE = dr["PHONENUMBER"].ToString(),
                        ADDRESS = dr["ADDRESS"].ToString(),
                        GENDER = dr["GENDER"].ToString(),
                        ISENABLE = Convert.ToBoolean(dr["ISENABLE"]),
                        ISACTIVE = Convert.ToBoolean(dr["ISACTIVE"])
                    });
                }
            }
            return lst;
        }
        public CustomerDetail GetCustomerDetail(string ID, string Flag = "CUST")
        {
            var cvm = new CustomerDetail();
            String sql = "spa_Customers @flag=" + dao.singleQuote(Flag) +
                ",@id=" + dao.singleQuote(ID);
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                cvm.ID = dt.Rows[0]["ID"].ToString();
                cvm.MEMBERID = dt.Rows[0]["MEMBERID"].ToString();
                cvm.FIRSTNAME = dt.Rows[0]["FIRSTNAME"].ToString();
                cvm.MIDDLENAME = dt.Rows[0]["MIDDLENAME"].ToString();
                cvm.LASTNAME = dt.Rows[0]["LASTNAME"].ToString();
                cvm.FULLNAME = dt.Rows[0]["FULLNAME"].ToString();
                cvm.EMAIL = dt.Rows[0]["EMAIL"].ToString();
                cvm.EMAILCONFIRMED = Convert.ToBoolean(dt.Rows[0]["EMAILCONFIRMED"]);
                cvm.TOKEN = dt.Rows[0]["TOKEN"].ToString();
                cvm.PHONE = dt.Rows[0]["PHONENUMBER"].ToString();
                cvm.PHONENUMBERCONFIRMED = Convert.ToBoolean(dt.Rows[0]["PHONENUMBERCONFIRMED"]);
                cvm.ACCESSFAILEDCOUNT = dt.Rows[0]["ACCESSFAILEDCOUNT"].ToString();
                cvm.USERNAME = dt.Rows[0]["USERNAME"].ToString();
                cvm.CITY = dt.Rows[0]["CITY"].ToString();
                cvm.ADDRESS = dt.Rows[0]["ADDRESS"].ToString();
                cvm.ISENABLE = Convert.ToBoolean(dt.Rows[0]["ISENABLE"]);
                cvm.ISACTIVE = Convert.ToBoolean(dt.Rows[0]["ISACTIVE"]);
                cvm.POSTALCODE = dt.Rows[0]["POSTALCODE"].ToString();
                cvm.FAX = dt.Rows[0]["FAX"].ToString();
                cvm.GENDER = dt.Rows[0]["GENDER"].ToString();
                cvm.LASTLOGINPC = dt.Rows[0]["LASTLOGINPC"].ToString();
                cvm.LASTLOGINDATE = dt.Rows[0]["LASTLOGINDATE"].ToString();
                cvm.LOCKSTATUS = Convert.ToBoolean(dt.Rows[0]["LOCKSTATUS"]);
                cvm.LOCKDATE = dt.Rows[0]["LOCKDATE"].ToString();
                cvm.KYCSTATUS = dt.Rows[0]["KYCSTATUS"].ToString();
                cvm.KYCAPPROVEBY = dt.Rows[0]["KYCAPPROVEBY"].ToString();
                cvm.KYCAPPROVEDATE = dt.Rows[0]["KYCAPPROVEDATE"].ToString();
                cvm.REFERENCESOURCE = dt.Rows[0]["REFERENCESOURCE"].ToString();
                cvm.REFERENCEID = dt.Rows[0]["REFERENCEID"].ToString();
                cvm.REFEREDBY = dt.Rows[0]["REFEREDBY"].ToString();
                cvm.ENABLEPASSWORDCHANGE = Convert.ToBoolean(dt.Rows[0]["ENABLEPASSWORDCHANGE"]);
                cvm.DATEOFBIRTH = dt.Rows[0]["DATEOFBIRTH"].ToString();
                cvm.PROFILEIMAGE = dt.Rows[0]["PROFILEIMAGE"].ToString();
                cvm.CREATETS = dt.Rows[0]["CREATETS"].ToString();
                cvm.CREATEBY = dt.Rows[0]["CREATEBY"].ToString();
                cvm.UPDATETS = dt.Rows[0]["UPDATETS"].ToString();
                cvm.UPDATEBY = dt.Rows[0]["UPDATEBY"].ToString();

            }
            return cvm;
        }
    }
}
