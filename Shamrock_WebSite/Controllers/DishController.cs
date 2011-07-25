using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Shamrock_WebSite.App_GlobalResources;
using Shamrock_WebSite.Models;

namespace Shamrock_WebSite.Controllers
{
    public class DishController : Controller
    {
        ShamrockEntities db = new ShamrockEntities();

        [ChildActionOnly]
        public ActionResult Index(string dishCategoryName, int? page)
        {
            var dishCategory = db.DishCategories.SingleOrDefault(dc => dc.Name.Equals(dishCategoryName, StringComparison.InvariantCultureIgnoreCase));

            if (dishCategory == null)
                return null;
            else
            {
                var pageSize = 10;
                if (!page.HasValue || page.Value == 0)
                    page = 1;

                var today = DateTime.Now.Date;
                var dishes = dishCategory.Dishes;
                var dishesByPage = dishes.Skip((page.Value - 1) * pageSize).Take(pageSize);

                var dishesCount = dishes.Count();
                ViewBag.TotalPages = (int)Math.Ceiling((double)dishesCount / pageSize);
                ViewBag.CurrentPage = page;
                return PartialView(dishesByPage);
            }
        }

        [Authorize]
        public ActionResult Create()
        {
            var dish = new Dish();

            foreach (var culture in SupportedCulture.GetList())
                dish.Dishes_Locale.Add(new Dish_Locale() { Culture = culture });

            return View(dish);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(string dishCategoryName, Dish dish, IList<Dish_Locale> locales)
        {
            var dishCategory = db.DishCategories.SingleOrDefault(dc => dc.Name.Equals(dishCategoryName, StringComparison.InvariantCultureIgnoreCase));

            if (dishCategory == null)
            {
                TempData["Result"] = Resource.RecordNotFound;
            }
            else
            {
                dish.DishCategory = dishCategory;

                foreach (var locale in locales)
                    locale.Dish = dish;

                if (ModelState.IsValid)
                {
                    db.Dishes.AddObject(dish);

                    db.SaveChanges();

                    TempData["Result"] = Resource.ChangesSaved;
                }
                else
                {
                    return Create();
                }
            }
            return RedirectToAction("Index", "DishCategory", new { dishCategoryName = dishCategoryName });
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var dish = db.Dishes.SingleOrDefault(e => e.Id == id);

            if (dish == null)
            {
                TempData["Result"] = Resource.RecordNotFound;
                return RedirectToAction("Index", "DishCategory");
            }
            else
            {
                return View(dish);
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(string dishCategoryName, Dish dish, IList<Dish_Locale> locales)
        {
            if (dish == null || !db.DishCategories.Any(dc => dc.Name.Equals(dishCategoryName, StringComparison.InvariantCultureIgnoreCase)))
            {
                TempData["Result"] = Resource.RecordNotFound;
            }
            else
            {
                if (ModelState.IsValid)
                {
                    foreach (var locale in locales)
                    {
                        db.Dish_Locale.Attach(locale);
                        db.ObjectStateManager.ChangeObjectState(locale, EntityState.Modified);
                    }

                    db.Dishes.Attach(dish);
                    db.ObjectStateManager.ChangeObjectState(dish, EntityState.Modified);

                    db.SaveChanges();

                    TempData["Result"] = Resource.ChangesSaved;
                }
                else
                {
                    return Edit(dish.Id);
                }
            }

            return RedirectToAction("Index", "DishCategory");
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            var dish = db.Dishes.SingleOrDefault(e => e.Id == id);

            if (dish == null)
            {
                TempData["Result"] = Resource.RecordNotFound;
            }
            else
            {
                foreach (var locale in dish.Dishes_Locale.ToList())
                    db.Dish_Locale.DeleteObject(locale);

                db.Dishes.DeleteObject(dish);

                db.SaveChanges();

                TempData["Result"] = Resource.ChangesSaved;
            }

            return RedirectToAction("Index", "DishCategory");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}