using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Category
        public ActionResult Index()
        {
            var item = db.categories;
            return View(item);
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Category model)
        {
            if(ModelState.IsValid)
            {
                model.CreateDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = WebBanHangOnline.Models.commons.Filter.LoaiDau(model.Title);
                db.categories.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public ActionResult Edit(int id)
        {
            return View(db.categories.Find(id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category model)
        {
            if (ModelState.IsValid)
            {
                db.categories.Attach(model);
                
                model.ModifiedDate = DateTime.Now;
                model.Alias = WebBanHangOnline.Models.commons.Filter.LoaiDau(model.Title);
                db.Entry(model).Property(x=>x.Title).IsModified=true;
                db.Entry(model).Property(x => x.Description).IsModified = true;
                db.Entry(model).Property(x => x.Alias).IsModified = true;
                db.Entry(model).Property(x => x.SeoKeywords).IsModified = true;
                db.Entry(model).Property(x => x.SeoDescription).IsModified = true;
                db.Entry(model).Property(x => x.SeoTitle).IsModified = true;
                db.Entry(model).Property(x => x.Position).IsModified = true;
                db.Entry(model).Property(x => x.ModifiedDate).IsModified = true;
                db.Entry(model).Property(x => x.ModifiedBy).IsModified = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.categories.Find(id);
            if (item != null)
            {
                db.categories.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new {success=false});
        }
    }
}