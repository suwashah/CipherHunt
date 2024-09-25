using System;

namespace Repository.Common
{
    public class StaticDataType
    {
        public string TYPE_ID { get; set; }
        public string TYPE_NAME { get; set; }
        public string TYPE_DESCRIPTION { get; set; }
    }
    public class Static_Data
    {
        public string Flag { get; set; }
        public string ID { get; set; }
        public string TYPE_ID { get; set; }
        public string STATIC_VALUE { get; set; }
        public string STATIC_DATA { get; set; }
        public string DESCRIPTION { get; set; }
        public string HELPDESK_DETAIL { get; set; }
        public string OTHER_VALUE_1 { get; set; }
        public string OTHER_VALUE_2 { get; set; }
        public string OTHER_VALUE_3 { get; set; }
        public string OTHER_VALUE_4 { get; set; }
    }
    public class SYSTEM_ACTIVITY
    {
        public String LOGID { get; set; }

        public String MESSAGE { get; set; }

        public String CREATE_TS { get; set; }

        public String TABLE_NAME { get; set; }

        public Boolean SEEN_STATUS { get; set; }
        public String ACTION { get; set; }

        public String SEEN_BY { get; set; }
        public String CREATE_BY { get; set; }
    }
    public class CP_NOTIFICATION
    {
        public String MSG { get; set; }
        public String POSTED_ON { get; set; }
        public String ICON { get; set; }
        public String CreateTS { get; set; }
    }

    public class SystemActivity
    {
        public string LOGID { get; set; }
        public string MSG { get; set; }
        public string MESSAGE { get; set; }
        public string CREATE_TS { get; set; }
        public string TABLE_NAME { get; set; }
        public string ICON { get; set; }
        public string BTNCLASS { get; set; }
        public string POSTED_ON { get; set; }
        public string SEEN_STATUS { get; set; }
        public string SEEN_BY { get; set; }
        public string SEEN_USER_PIC { get; set; }
    }
}
