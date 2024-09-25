using Repository.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CipherHunt.Areas.Cpanel.Models
{
    public class EmailtemplateModel
    {
        public string FLAG { get; set; }
        public int TEMP_ID { get; set; }
        [Required(ErrorMessage = "Please fill out this field")]
        public String TEMP_NAME { get; set; }
        [Required(ErrorMessage = "Please fill out this field")]
        public String TEMP_EMAIL_SUBJECT { get; set; }
        [Required(ErrorMessage = "Please fill out this field")]
        [AllowHtml]
        public String TEMP_EMAIL_BODY { get; set; }
        public bool enable { get; set; }
        public bool Response { get; set; }
        public List<Static_Data> StaticDatas { get; set; }
    }
}