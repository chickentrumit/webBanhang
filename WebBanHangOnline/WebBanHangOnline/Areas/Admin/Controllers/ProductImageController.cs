using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class ProductImageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/ProductImage
        public ActionResult Index(int id)
        {
            ViewBag.ProductId = id;
            var item = db.productImages.Where(x=>x.ProductId==id).ToList();
            return View(item);
        }

        [HttpPost]
        public ActionResult AddImage(int productId,string url)
        {
            db.productImages.Add(new ProductImage()
            {
                ProductId = productId,
                Image=url,
                Isdefault=false
            });
            db.SaveChanges();
            return Json(new {success=true});
        }

        [HttpPost]
        public ActionResult Delete (int id)
        {
            var item = db.productImages.Find(id);
            db.productImages.Remove(item);  
            db.SaveChanges();
            return Json(new {success=true});
        }
    }
}