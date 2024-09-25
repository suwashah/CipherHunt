using Repository.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace CipherHunt.Models
{
    public class CategoryModel
    {
        [Required(ErrorMessage = "Please fill out this field")]
        public String CATEGORY_NAME { get; set; }
        [Required(ErrorMessage = "Please fill out this field")]
        public String CATEGORY_DESCRIPTION { get; set; }
        public HttpPostedFileBase CATEGORY_IMAGE { get; set; }
        public String CREATE_BY { get; set; }
        public String CAT_ID { get; set; }
        public String FLAG { get; set; }
        public Boolean IS_ENABLE { get; set; }
    }
    public class ProductModel
    {
        public String FLAG { get; set; }
        public String PRODUCTID { get; set; }
        [Required(ErrorMessage = "Please fill out this field")]
        public String CAT_ID { get; set; }
        public String CATEGORY_NAME { get; set; }
        [Required(ErrorMessage = "Please fill out this field")]
        public String NAME { get; set; }
        [Required(ErrorMessage = "Please fill out this field")]
        public String DESCRIPTION { get; set; }

        public String CURRENCY { get; set; }
        [Required(ErrorMessage = "Please fill out this field")]
        public String PRICE { get; set; }
        [Required(ErrorMessage = "Please fill out this field")]
        public String CALORIES { get; set; }
        //[Required(ErrorMessage = "Please fill out this field")]
        public HttpPostedFileBase ImageFile { get; set; }

        public String PRODUCTURL { get; set; }
        public String STATUS { get; set; }

        public Boolean IS_ENABLE { get; set; }
        public Boolean IS_VERIFIED { get; set; }
        public String VERIFIED_BY { get; set; }
        public String CREATE_BY { get; set; }
        public String CREATE_TS { get; set; }
        public String UPDATE_BY { get; set; }
        public String UPDATE_TS { get; set; }
        public String ImageSrc { get; set; }
        public String IMAGENAME { get; set; }
        public List<TblProduct> RelatedProducts { get; set; }
    }
}