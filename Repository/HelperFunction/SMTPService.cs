using Repository.Common;
using Repository.DAL;
using System;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Repository.HelperFunction
{
    public interface ISMTPService
    {
        CommonData SendEmail(string ID, string to, string subject, string mailBody);
        void PushEmail();
    }
    public class SMTPService : ISMTPService
    {
        readonly string Sender = "CipherHunt@gmail.com";
        readonly string Password = "CipherHunt@1";
        readonly string Host = "smtp.gmail.com";
        readonly bool EnableSSL = true;
        readonly int SMTPPort = 587;
        RepositoryDao dao;
        CommonRepository icr;
        public SMTPService()
        {
            dao = new RepositoryDao();
            icr = new CommonRepository();
        }
        public CommonData SendEmail(string ID, string to, string subject, string mailBody)
        {
            var ret = new CommonData();          
            using (MailMessage mail = new MailMessage(Sender, to))
            {
                try
                {
                    mail.Subject = subject;
                    mail.Body = mailBody;
                    //if (FileUpload1.HasFile)
                    //{
                    //    string fileName = Path.GetFileName(FileUpload1.FileName);
                    //    mail.Attachments.Add(new Attachment(FileUpload1.PostedFile.InputStream, fileName));
                    //}
                    mail.IsBodyHtml = true;
                    NetworkCredential networkCredential = new NetworkCredential(Sender, Password);
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = Host;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Port = SMTPPort;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = networkCredential;
                    smtp.EnableSsl = EnableSSL;

                    smtp.Timeout = 20000;
                    smtp.Send(mail);
                    int a = UpdateSendFlag(ID);
                    ret.CODE = "0";
                    ret.MESSAGE = "Success";
                }
                catch (Exception ex)
                {
                    string message = ex.Message;
                    icr.SaveError(message, "SMTP Email");
                    UpdateActiveFlag(ID);
                    ret.CODE = "5001";
                    ret.MESSAGE = "Failed";
                }
            }
            return ret;
        }
        public void PushEmail()
        {
            string sql = "spa_TBL_EMAIL_REQUEST @flag='s'";
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                //foreach (DataRow drow in dt.Rows)
                //{
                //    SendEmail(drow["ID"].ToString(), drow["SEND_TO"].ToString(), drow["EMAIL_SUBJECT"].ToString(), drow["EMAIL_BODY"].ToString());
                //}
                Parallel.ForEach(dt.AsEnumerable(), drow =>
                {
                    SendEmail(drow["ID"].ToString(), drow["SEND_TO"].ToString(), drow["EMAIL_SUBJECT"].ToString(), drow["EMAIL_BODY"].ToString());
                });
            }
        }
        private int UpdateSendFlag(string ID)
        {
            string sql = "spa_TBL_EMAIL_REQUEST @flag='u',@ID=" + dao.singleQuote(ID);
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        private int UpdateActiveFlag(string ID)
        {
            string sql = "spa_TBL_EMAIL_REQUEST @flag='a',@ID=" + dao.singleQuote(ID);
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
