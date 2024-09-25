using Repository.DAL;
using System.Collections.Generic;
using System.Data;

namespace Repository.Common
{
    public class UserStatus
    {
        public int ID { get; set; }
        public string STATUS { get; set; }
        public string DESCRIPTION { get; set; }
        public string ICON { get; set; }
        public string USERID { get; set; }
        public string USERNAME { get; set; }
    }

    public interface IStatusRepository
    {
        List<UserStatus> GetStatuses();
        UserStatus GetUserStatus(string Flag, int userID, int StatusID = 0);
    }
    public class StatusRepository : IStatusRepository
    {
        RepositoryDao dao;
        public StatusRepository()
        {
            dao = new RepositoryDao();
        }
        public List<UserStatus> GetStatuses()
        {
            List<UserStatus> lst = new List<UserStatus>();
            string sql = "spa_User_Status @flag=" + dao.singleQuote("us");
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    lst.Add(new UserStatus()
                    {
                        ID = dao.convertToInt(dr["ID"]),
                        STATUS = dr["STATUS"].ToString(),
                        DESCRIPTION = dr["DESCRIPTION"].ToString(),
                        ICON = dr["ICON"].ToString()
                    });
                }
            }
            return lst;
        }
        public UserStatus GetUserStatus(string Flag, int userID, int StatusID = 0)
        {
            var ret = new UserStatus();
            string sql = "spa_User_Status @flag = " + dao.singleQuote(Flag) +
                ",@userid = " + dao.singleQuote(userID.ToString()) +
                ",@StatusID = " + dao.singleQuote(StatusID.ToString());
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                ret = new UserStatus
                {
                    ID = dao.convertToInt(dt.Rows[0]["ID"]),
                    STATUS = dt.Rows[0]["STATUS"].ToString(),
                    DESCRIPTION = dt.Rows[0]["DESCRIPTION"].ToString(),
                    ICON = dt.Rows[0]["ICON"].ToString(),
                    USERID = dt.Rows[0]["USERID"].ToString(),
                    USERNAME = dt.Rows[0]["USERNAME"].ToString()
                };
            }
            return ret;
        }
    }
}
