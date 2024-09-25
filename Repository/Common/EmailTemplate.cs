using System;

namespace Repository.Common
{
    public class EmailTemplate
    {
        public int TEMP_ID { get; set; }
        public String TEMP_NAME { get; set; }
        public String TEMP_EMAIL_SUBJECT { get; set; }
        public String TEMP_EMAIL_BODY { get; set; }
        public String IS_ENABLE { get; set; }
        public bool enable { get; set; }
        public String RESPONSE_TO_ADMIN { get; set; }
        public bool Response { get; set; }
    }
}
