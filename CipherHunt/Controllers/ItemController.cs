using Repository.Product;
using System;
using System.Web.Mvc;
using CipherHunt.Library;
using CipherHunt.Models;

namespace CipherHunt.Controllers
{
    public class ItemController : Controller
    {
        // GET: Item
        private readonly IProductRepository _ipr;
        public ItemController(IProductRepository ipr)
        {
            _ipr = ipr;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ItemDetail(string route)
        {
            ProductModel model = new ProductModel();
            if (!String.IsNullOrEmpty(route))
            {
                var qry = StaticData.GetQueryParameters(route);
                string ID = qry["id"];
                var pro = _ipr.GetProductDetail(ID);
                model.PRODUCTID = pro.PRODUCTID;
                model.CAT_ID = pro.CAT_ID;
                model.CATEGORY_NAME = pro.CATEGORY_NAME;
                model.NAME = pro.NAME;
                model.DESCRIPTION = pro.DESCRIPTION;
                model.CREATE_TS = pro.CREATE_TS;
                model.CREATE_BY = pro.CREATE_BY;
                model.UPDATE_BY = pro.UPDATE_BY;
                model.UPDATE_TS = pro.UPDATE_TS;
                model.IS_ENABLE = pro.IS_ENABLE;
                model.CURRENCY = pro.CURRENCY;
                model.PRICE = pro.PRICE;
                model.ImageSrc = pro.PRODUCTURL;
                model.STATUS = pro.STATUS;
                model.IS_ENABLE = pro.IS_ENABLE;
                model.IS_VERIFIED = pro.IS_VERIFIED;
                model.VERIFIED_BY = pro.VERIFIED_BY;
                model.RelatedProducts = _ipr.GetRelatedMealByCategory(pro.CAT_ID, pro.PRODUCTID);
            }
            return View(model);
        }
    }
}