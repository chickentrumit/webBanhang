using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class ProductCategoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/ProductCategory
        public ActionResult Index()
        {

            return View(db.productCategories);
        }

        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ProductCategory model)
        {
            if(ModelState.IsValid)
            {
                model.CreateDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = WebBanHangOnline.Models.commons.Filter.RemoveSign4VietnameseString(model.Title);
                db.productCategories.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {

            var item = db.productCategories.Find(id);
            if (item != null)
            {
                db.productCategories.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}