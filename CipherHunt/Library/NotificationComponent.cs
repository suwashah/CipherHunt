using Microsoft.AspNet.SignalR;
using Repository.Common;
using Repository.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CipherHunt.Library
{
    public class NotificationComponent
    {
        RepositoryDao dao;
        public NotificationComponent()
        {
            dao = new RepositoryDao();
        }
        //Here we will add a function for register notification (will add sql dependency)
        public void RegisterNotification(DateTime currentTime)
        {
            string conStr = ConfigurationManager.ConnectionStrings["WebDBConnString"].ConnectionString;
            string sqlCommand = @"SELECT[LOGID],[MESSAGE],[SEEN_STATUS] from[dbo].[SYSTEM_ACTIVITY] where[CREATE_TS] > @AddedOn";
            //you can notice here I have added table name like this [dbo].[Contacts] with [dbo], its mendatory when you use Sql Dependency
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(sqlCommand, con);
                cmd.Parameters.AddWithValue("@AddedOn", currentTime);
                if (con.State != System.Data.ConnectionState.Open)
                {
                    con.Open();
                }
                cmd.Notification = null;
                SqlDependency sqlDep = new SqlDependency(cmd);
                sqlDep.OnChange += sqlDep_OnChange;
                //we must have to execute the command here
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // nothing need to add here now
                }
            }
        }
        void sqlDep_OnChange(object sender, SqlNotificationEventArgs e)
        {
            //or you can also check => if (e.Info == SqlNotificationInfo.Insert) , if you want notification only for inserted record
            if (e.Type == SqlNotificationType.Change)
            {
                SqlDependency sqlDep = sender as SqlDependency;
                sqlDep.OnChange -= sqlDep_OnChange;
                //from here we will send notification message to client
                var notificationHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                notificationHub.Clients.All.notify("added");
                //re-register notification
                RegisterNotification(DateTime.Now);
            }
        }

        public List<CP_NOTIFICATION> GetMessage(DateTime afterDate)
        {
            List<CP_NOTIFICATION> lst = new List<CP_NOTIFICATION>();
            DataTable dt = dao.ExecuteDataTable("spa_CP_System_Activity @flag='s'");
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    lst.Add(new CP_NOTIFICATION
                    {
                        MSG = dr["MSG"].ToString(),
                        POSTED_ON = dr["POSTED_ON"].ToString()
                    });
                }
            }
            return lst;
            //return dc.Contacts.Where(a => a.AddedOn > afterDate).OrderByDescending(a => a.AddedOn).ToList();
        }
    }
}