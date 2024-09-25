using Repository.Common;
using System;
using System.Collections.Generic;

namespace CipherHunt.Areas.Cpanel.Models
{
    public class ApplicationViewModel
    {
        public String TotalUnverifiedProducts { get; set; }
        public String TotalUnverifiedChallenges { get; set; }
        public String TotalCustomers { get; set; }
        public String TotalCategories { get; set; }
        public String TotalProducts { get; set; }
        public String TotalChallenges { get; set; }
        public List<CPanelDetail> CpanelUsers { get; set; }
        public List<SystemActivity> TopThreeNotification { get; set; }
    }
}