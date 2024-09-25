using Repository.Common;
using System.Collections.Generic;

namespace Repository.Email
{
    public interface IEmailRepository
    {
        List<EmailTemplate> EmailTemplateList();
        EmailTemplate EditTemplate(string ID);
        CommonData SaveTemplate(string flag, int Temp_ID, string TempName, string EmailSubject, string EmailBody, bool isEnable, bool ResponseToAdmin);
        List<Static_Data> GetEmailIDList();
    }
}
