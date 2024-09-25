using System;

namespace Repository.Common
{
    public class Category
    {
        public String FLAG { get; set; }
        public String CAT_ID { get; set; }
        public String CATEGORY_NAME { get; set; }
        public String CATEGORY_DESCRIPTION { get; set; }
        public byte[] CATEGORY_IMAGE { get; set; }
        public String CREATE_BY { get; set; }
        public String CREATE_TS { get; set; }
        public Boolean IS_ENABLE { get; set; }
    }
    public class TblProduct
    {
        public String FLAG { get; set; }
        public String PRODUCTID { get; set; }
        public String CAT_ID { get; set; }
        public String CATEGORY_NAME { get; set; }
        public String CATEGORY_TAB { get; set; }
        public String NAME { get; set; }
        public String DESCRIPTION { get; set; }

        public String PRICE { get; set; }
        public String CURRENCY { get; set; }

        public byte[] IMAGE { get; set; }
        public String IMAGENAME { get; set; }

        public String PRODUCTURL { get; set; }

        public String DATEAVAILABLE { get; set; }

        public String CHANGEDATE { get; set; }
        public String STATUS { get; set; }

        public Boolean IS_ENABLE { get; set; }

        public String CREATE_TS { get; set; }
        public String CREATE_BY { get; set; }

        public String UPDATE_TS { get; set; }
        public String UPDATE_BY { get; set; }

        public Boolean IS_VERIFIED { get; set; }
        public String VERIFIED_BY { get; set; }
        public String CALORIES { get; set; }

    }
}
