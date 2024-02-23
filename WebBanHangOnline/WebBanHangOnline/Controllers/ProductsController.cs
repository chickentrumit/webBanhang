using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.Controllers
{
    public class ProductsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Products
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Partial_ItemsByCateId() {
            var items = db.products.Where(x=>x.IsHome&&x.IsActive).Take(12).ToList();
            return PartialView(items);
        }
        public ActionResult Partial_ProductSales()
        {
            var items = db.products.Where(x => x.IsActive&&x.IsSale).Take(12).ToList();
            return PartialView(items);
        }
    }
}