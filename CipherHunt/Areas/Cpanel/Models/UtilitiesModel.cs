using Repository.Common;
using System.Collections.Generic;

namespace CipherHunt.Areas.Cpanel.Models
{
    public class SDViewModel
    {
        public string TYPE_ID { get; set; }
        public string TYPE_Name { get; set; }
        public List<Static_Data> StaticDatas { get; set; }
    }
    public class StaticDataModel
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
    public class UtilitiesModel
    {
    }
}