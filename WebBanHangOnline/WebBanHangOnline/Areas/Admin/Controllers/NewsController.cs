using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class NewsController : Controller
    {
        // GET: Admin/News
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(string Searchtext ,int? page)
        {
            IEnumerable<News> items = db.news.OrderByDescending(x => x.Id);
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
           
            
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items=items.Where(x=>x.Alias.Contains(Searchtext)||x.Title.Contains(Searchtext));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex,pageSize);

            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        public ActionResult Add() 
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(News model)
        {
            if(ModelState.IsValid)
            {
                model.CreateDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = WebBanHangOnline.Models.commons.Filter.RemoveSign4VietnameseString(model.Title);
                model.CategoryId = 3;
                db.news.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(model);
        }
        public ActionResult Edit(int id)
        {
            return View(db.news.Find(id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(News model)
        {
            if (ModelState.IsValid)
            {
                model.ModifiedDate = DateTime.Now;
                model.Alias = WebBanHangOnline.Models.commons.Filter.RemoveSign4VietnameseString(model.Title);
                model.CategoryId = 3;
                db.news.Attach(model);
                db.Entry(model).State= System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
                
            var item = db.news.Find(id);
            if (item != null)
            {
                db.news.Remove(item);
                db.SaveChanges();
                return Json (new { success = true });
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public ActionResult IsActive(int id)
        {

            var item = db.news.Find(id);
            if (item != null)   
            {
                item.IsActive = !item.IsActive;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true , isActive = item.IsActive});
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public ActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids)){
                var items =ids.Split(',');
                if (items.Any() && items != null)
                {
                    foreach(var item in items)
                    {
                        var obj = db.news.Find(Convert.ToInt32(item));
                        db.news.Remove(obj);
                        db.SaveChanges();
                    }
                }
                return Json(new { success = true});
            }
            return Json(new { success = false});
        }
    }
}