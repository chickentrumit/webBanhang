using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Admin/Products
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(int? page)
        {
            IEnumerable<Product> items = db.products.OrderByDescending(x => x.Id);
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);

            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }

        public ActionResult Add()
        {
            ViewBag.ProductCategory = new SelectList(db.productCategories.ToList(), "Id", "Title");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Product model, List<string> Images, List<int> rdefault)
        {
            using (var transaction = db.Database.BeginTransaction())
            {

                try
                {
                    if (ModelState.IsValid)
                    {
                        if (Images.Count > 0 && Images != null)
                        {
                            for (int i = 0; i < Images.Count; i++)
                            {
                                if (i + 1 == rdefault[0])
                                {
                                    model.Image = Images[i];
                                    
                                    model.ProductImages.Add(new ProductImage
                                    {
                                        ProductId = model.Id,
                                        Image = Images[i],
                                        Isdefault = true,
                                        
                                    });

                                }
                                else
                                {
                                    model.Image = Images[i];
                                    model.ProductImages.Add(new ProductImage
                                    {
                                        ProductId = model.Id,
                                        Image = Images[i],
                                        Isdefault = false
                                    });
                                }
                            }
                        }
                        model.CreateDate = DateTime.Now;
                        model.ModifiedDate = DateTime.Now;
                        if (string.IsNullOrEmpty(model.SeoTitle))
                        {
                            model.SeoTitle = model.Title;
                        }
                        if (string.IsNullOrEmpty(model.Alias))
                            model.Alias = WebBanHangOnline.Models.commons.Filter.RemoveSign4VietnameseString(model.Title);
                        db.products.Add(model);
                        db.SaveChanges();
                        transaction.Commit();
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw new Exception("lỗi khi thêm . vui lòng kiểm tra đầu vào của bạn và thử lại!!!.");
                }
            }

            ViewBag.ProductCategory = new SelectList(db.productCategories.ToList(), "Id", "Title");
            return View(model);

        }
        public ActionResult Edit(int id)
        {
            ViewBag.ProductCategory= new SelectList(db.productCategories.ToList(), "Id", "Title");
            var item = db.products.Find(id);
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product model)
        {
            if (ModelState.IsValid)
            {
                model.ModifiedDate = DateTime.Now;
                model.Alias = WebBanHangOnline.Models.commons.Filter.RemoveSign4VietnameseString(model.Title);
                db.products.Attach(model);
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {

            var item = db.products.Find(id);
            if (item != null)
            {
                db.products.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public ActionResult IsActiveSale(int id)
        {

            var item = db.products.Find(id);
            if (item != null)
            {
                item.IsSale = !item.IsSale;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, isSale = item.IsSale });
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public ActionResult IsActiveHome(int id)
        {

            var item = db.products.Find(id);
            if (item != null)
            {
                item.IsHome = !item.IsHome;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, isHome = item.IsHome });
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public ActionResult IsActive(int id)
        {

            var item = db.products.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, isActive = item.IsActive });
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public ActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items.Any() && items != null)
                {
                    foreach (var item in items)
                    {
                        var obj = db.products.Find(Convert.ToInt32(item));
                        db.products.Remove(obj);
                        db.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}