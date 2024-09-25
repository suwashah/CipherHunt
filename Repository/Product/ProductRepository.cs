using Repository.Common;
using Repository.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace Repository.Product
{
    public interface IProductRepository
    {
        CommonData SaveCategory(Category inp);
        List<Category> GetAllCategories();
        Category GetCategoryDetail(string Cat_ID);
        CommonData DeleteCategory(string Cat_ID);
        CommonData SaveProduct(TblProduct inp);
        List<TblProduct> GetAllProducts(string flag);
        TblProduct GetProductDetail(string ProductID);
        CommonData VerifyProduct(string ID, string UserName);
        List<TblProduct> GetComboMeal();
        List<TblProduct> GetHomeMenu();
        List<TblProduct> GetRelatedMealByCategory(string CategoryID, string ProductID);
    }
    public class ProductRepository : IProductRepository
    {
        RepositoryDao dao;
        public ProductRepository()
        {
            dao = new RepositoryDao();
        }
        public CommonData SaveCategory(Category inp)
        {
            var ret = new CommonData();
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@flag",inp.FLAG),
                new SqlParameter("@CATEGORY_NAME",inp.CATEGORY_NAME),
                new SqlParameter("@CATEGORY_DESCRIPTION",inp.CATEGORY_DESCRIPTION),
                new SqlParameter("@CATEGORY_IMAGE",inp.CATEGORY_IMAGE),
                new SqlParameter("@CREATE_BY",inp.CREATE_BY),
                new SqlParameter("@IS_ENABLE",inp.IS_ENABLE),
                new SqlParameter("@CAT_ID",inp.CAT_ID)
            };
            //sqlParam.DbType = DbType.Binary;
            DataTable dt = dao.ExecuteDataTableSP("[dbo].[spa_TBL_CATEGORY]", param, CommandType.StoredProcedure);
            if (dt != null && dt.Rows.Count > 0)
            {
                ret.CODE = dt.Rows[0]["Code"].ToString();
                ret.MESSAGE = dt.Rows[0]["Message"].ToString();
            }
            else
            {
                ret.CODE = "404";
                ret.MESSAGE = "Data not found";
            }
            return ret;
        }
        public List<Category> GetAllCategories()
        {
            var lst = new List<Category>();
            string sql = "spa_TBL_CATEGORY @flag='a'";
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    lst.Add(new Category
                    {
                        CAT_ID = dr["CAT_ID"].ToString(),
                        CATEGORY_NAME = dr["CATEGORY_NAME"].ToString(),
                        CATEGORY_DESCRIPTION = dr["CATEGORY_DESCRIPTION"].ToString(),
                        IS_ENABLE = Convert.ToBoolean(dr["IS_ENABLE"]),
                        CREATE_BY = dr["CREATE_BY"].ToString(),
                        CREATE_TS = dr["CREATE_TS"].ToString()
                    });
                }
            }
            return lst;
        }
        public Category GetCategoryDetail(string Cat_ID)
        {
            var lst = new Category();
            string sql = "spa_TBL_CATEGORY @flag='s'" +
                ",@CAT_ID=" + dao.singleQuote(Cat_ID);
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                lst.CAT_ID = dt.Rows[0]["CAT_ID"].ToString();
                lst.CATEGORY_NAME = dt.Rows[0]["CATEGORY_NAME"].ToString();
                lst.CATEGORY_DESCRIPTION = dt.Rows[0]["CATEGORY_DESCRIPTION"].ToString();
                lst.CREATE_TS = dt.Rows[0]["CREATE_TS"].ToString();
                lst.CREATE_BY = dt.Rows[0]["CREATE_BY"].ToString();
                lst.IS_ENABLE = Convert.ToBoolean(dt.Rows[0]["IS_ENABLE"]);
            }
            return lst;
        }
        public CommonData DeleteCategory(string Cat_ID)
        {
            var lst = new CommonData();
            string sql = "spa_TBL_CATEGORY @flag='d'" +
                ",@CAT_ID=" + dao.singleQuote(Cat_ID);
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                lst.CODE = dt.Rows[0]["Code"].ToString();
                lst.MESSAGE = dt.Rows[0]["Message"].ToString();
            }
            return lst;
        }

        public CommonData SaveProduct(TblProduct inp)
        {
            var ret = new CommonData();
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@flag",inp.FLAG),
                new SqlParameter("@PRODUCTID",inp.PRODUCTID),
                new SqlParameter("@CAT_ID",inp.CAT_ID),
                new SqlParameter("@NAME",inp.NAME),
                new SqlParameter("@DESCRIPTION",inp.DESCRIPTION),
                new SqlParameter("@PRICE",inp.PRICE),
                new SqlParameter("@IMAGE",inp.IMAGE),
                new SqlParameter("@IMAGENAME",inp.IMAGENAME),
                new SqlParameter("@IS_ENABLE",inp.IS_ENABLE),
                new SqlParameter("@CREATE_BY",inp.CREATE_BY),
                new SqlParameter("@UPDATE_BY",inp.UPDATE_BY),
                new SqlParameter("@CALORIES",inp.CALORIES)
            };
            //sqlParam.DbType = DbType.Binary;
            DataTable dt = dao.ExecuteDataTableSP("[dbo].[spa_Product]", param, CommandType.StoredProcedure);
            if (dt != null && dt.Rows.Count > 0)
            {
                ret.CODE = dt.Rows[0]["Code"].ToString();
                ret.MESSAGE = dt.Rows[0]["Message"].ToString();
            }
            else
            {
                ret.CODE = "404";
                ret.MESSAGE = "Data not found";
            }
            return ret;
        }
        public List<TblProduct> GetAllProducts(string flag)
        {
            var lst = new List<TblProduct>();
            string sql = "spa_Product @flag=" + dao.singleQuote(flag);
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    lst.Add(new TblProduct
                    {
                        PRODUCTID = dr["PRODUCTID"].ToString(),
                        CAT_ID = dr["CAT_ID"].ToString(),
                        CATEGORY_NAME = dr["CATEGORY_NAME"].ToString(),
                        NAME = dr["NAME"].ToString(),
                        DESCRIPTION = dr["DESCRIPTION"].ToString(),
                        PRICE = dao.DecimalAmountFormatter(dr["PRICE"].ToString()),
                        CURRENCY = dr["CURRENCY"].ToString(),
                        PRODUCTURL = GetProductImage(dr["IMAGENAME"].ToString()),
                        IMAGENAME = dr["IMAGENAME"].ToString(),
                        STATUS = dr["STATUS"].ToString(),
                        IS_ENABLE = Convert.ToBoolean(dr["IS_ENABLE"]),
                        CREATE_BY = dr["CREATE_BY"].ToString(),
                        CREATE_TS = dr["CREATE_TS"].ToString(),
                        UPDATE_BY = dr["UPDATE_BY"].ToString(),
                        UPDATE_TS = dr["UPDATE_TS"].ToString(),
                        IS_VERIFIED = Convert.ToBoolean(dr["IS_VERIFIED"]),
                        VERIFIED_BY = dr["VERIFIED_BY"].ToString(),
                        CALORIES = dr["CALORIES"].ToString()
                    });
                }
            }
            return lst;
        }
        public TblProduct GetProductDetail(string ProductID)
        {
            var lst = new TblProduct();
            string sql = "spa_Product @flag='s'" +
                ",@PRODUCTID=" + dao.singleQuote(ProductID);
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                lst.PRODUCTID = dt.Rows[0]["PRODUCTID"].ToString();
                lst.CAT_ID = dt.Rows[0]["CAT_ID"].ToString();
                lst.NAME = dt.Rows[0]["NAME"].ToString();
                lst.CATEGORY_NAME = dt.Rows[0]["CATEGORY_NAME"].ToString();
                lst.DESCRIPTION = dt.Rows[0]["DESCRIPTION"].ToString();
                lst.CREATE_TS = dt.Rows[0]["CREATE_TS"].ToString();
                lst.CREATE_BY = dt.Rows[0]["CREATE_BY"].ToString();
                lst.UPDATE_TS = dt.Rows[0]["UPDATE_TS"].ToString();
                lst.UPDATE_BY = dt.Rows[0]["UPDATE_BY"].ToString();
                lst.CURRENCY = dt.Rows[0]["CURRENCY"].ToString();
                lst.PRICE = dao.DecimalRounder(dt.Rows[0]["PRICE"].ToString());
                //string img = dao.GetBytesImage(dt.Rows[0]["IMAGE"]);
                //if (dao.GetAppsettingValue("SaveImageBytes") == "n")
                //{
                //    img = dao.GetImageBase64FromPath(dt.Rows[0]["IMAGENAME"].ToString(), dao.GetAppsettingValue("ProductPath"));
                //}
                lst.PRODUCTURL = GetProductImage(dt.Rows[0]["IMAGENAME"].ToString());
                lst.IMAGENAME = dt.Rows[0]["IMAGENAME"].ToString();
                lst.STATUS = dt.Rows[0]["STATUS"].ToString();
                lst.IS_ENABLE = Convert.ToBoolean(dt.Rows[0]["IS_ENABLE"]);
                lst.IS_VERIFIED = Convert.ToBoolean(dt.Rows[0]["IS_VERIFIED"]);
                lst.VERIFIED_BY = dt.Rows[0]["VERIFIED_BY"].ToString();
                lst.CALORIES = dt.Rows[0]["CALORIES"].ToString();
            }
            return lst;
        }
        public CommonData VerifyProduct(string ID, string UserName)
        {
            var lst = new CommonData();
            string sql = "spa_Product @flag='ve'" +
                ",@PRODUCTID = " + dao.singleQuote(ID) +
                ",@UPDATE_BY = " + dao.singleQuote(UserName);
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                lst.CODE = dt.Rows[0]["Code"].ToString();
                lst.MESSAGE = dt.Rows[0]["Message"].ToString();
            }
            return lst;
        }
        public List<TblProduct> GetComboMeal()
        {
            var lst = new List<TblProduct>();
            string sql = "spa_Web_HomePage @flag='c'";
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    lst.Add(new TblProduct
                    {
                        PRODUCTID = dr["PRODUCTID"].ToString(),
                        NAME = dr["NAME"].ToString(),
                        DESCRIPTION = dr["DESCRIPTION"].ToString(),
                        PRICE = dao.DecimalAmountFormatterCustomRound(dr["PRICE"].ToString()),
                        CURRENCY = dr["CURRENCY"].ToString(),
                        PRODUCTURL = GetProductImage(dr["IMAGENAME"].ToString()),
                        IMAGENAME = dr["IMAGENAME"].ToString()
                    });
                }
            }
            return lst;
        }
        public List<TblProduct> GetHomeMenu()
        {
            var lst = new List<TblProduct>();
            string sql = "spa_Web_HomePage @flag='s'";
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    lst.Add(new TblProduct
                    {
                        CAT_ID = dr["CAT_ID"].ToString(),
                        CATEGORY_NAME = dr["CATEGORY_NAME"].ToString(),
                        CATEGORY_TAB = dr["CATEGORY_NAME"].ToString(),
                        PRODUCTID = dr["PRODUCTID"].ToString(),
                        NAME = dr["NAME"].ToString(),
                        DESCRIPTION = dr["DESCRIPTION"].ToString(),
                        PRICE = dao.DecimalAmountFormatterCustomRound(dr["PRICE"].ToString()),
                        CURRENCY = dr["CURRENCY"].ToString(),
                        PRODUCTURL = GetProductImage(dr["IMAGENAME"].ToString()),
                        IMAGENAME = dr["IMAGENAME"].ToString()
                    });
                }
            }
            return lst;
        }
        public List<TblProduct> GetRelatedMealByCategory(string CategoryID, string ProductID)
        {
            var lst = new List<TblProduct>();
            string sql = "spa_Web_HomePage @flag='cp'" +
                ",@CAT_ID=" + dao.singleQuote(CategoryID) +
                ",@ProductID=" + dao.singleQuote(ProductID);
            DataTable dt = dao.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    lst.Add(new TblProduct
                    {
                        CATEGORY_NAME = dr["CATEGORY_NAME"].ToString(),
                        PRODUCTID = dr["PRODUCTID"].ToString(),
                        NAME = dr["NAME"].ToString(),
                        DESCRIPTION = dr["DESCRIPTION"].ToString(),
                        PRICE = dao.DecimalAmountFormatterCustomRound(dr["PRICE"].ToString()),
                        CURRENCY = dr["CURRENCY"].ToString(),
                        PRODUCTURL = GetProductImage(dr["IMAGENAME"].ToString()),
                        IMAGENAME = dr["IMAGENAME"].ToString()
                    });
                }
            }
            return lst;
        }
        private string GetProductImage(string ImageName)
        {
            string img = "";
            if (dao.GetAppsettingValue("SaveImageBytes") == "n")
            {
                img = dao.GetImageBase64FromPath(ImageName, dao.GetAppsettingValue("ProductPath"));
                img = dao.GetAppsettingValue("ProductPath") + "/" + ImageName;
            }
            else
            {
                img = dao.GetCompressedBytesImage(ImageName);
            }
            return img;
        }
    }
}
