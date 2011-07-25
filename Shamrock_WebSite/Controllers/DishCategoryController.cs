using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Shamrock_WebSite.App_GlobalResources;
using Shamrock_WebSite.Models;

namespace Shamrock_WebSite.Controllers
{
    public class DishCategoryController : Controller
    {
        ShamrockEntities db = new ShamrockEntities();

        public ActionResult Index(string dishCategoryName)
        {
            if (!String.IsNullOrWhiteSpace(dishCategoryName) && db.DishCategories.FirstOrDefault(dc => dc.Name == dishCategoryName) == null)
            {
                TempData["Result"] = Resource.RecordNotFound;
                dishCategoryName = null;
            }

            var dishCategories = db.DishCategories.OrderBy(dc => dc.Order);

            if (String.IsNullOrWhiteSpace(dishCategoryName) && dishCategories.Count() != 0)
                dishCategoryName = dishCategories.First().Name;

            ViewBag.DishCategoryName = dishCategoryName;

            if (Request.IsAjaxRequest())
            {
                return PartialView("_DishesByDishCategory", dishCategoryName);
            }
            return View(dishCategories);
        }

        [Authorize]
        public ActionResult Create()
        {
            var dishCategory = new DishCategory();

            foreach (var culture in SupportedCulture.GetList())
                dishCategory.DishCategories_Locale.Add(new DishCategory_Locale() { Culture = culture });

            return View(dishCategory);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(DishCategory dishCategory, IList<DishCategory_Locale> locales)
        {
            foreach (var locale in locales)
                locale.DishCategory = dishCategory;
            
            dishCategory.Name = dishCategory.DishCategories_Locale.First(l => l.Culture == SupportedCulture.En).DisplayName.Replace(" ", "-");

            if (ModelState.IsValid)
                if (db.DishCategories.Any(dc => dc.Name == dishCategory.Name))
                    ModelState.AddModelError("Unique", Resource.Unique);

            if (ModelState.IsValid)
            {
                dishCategory.Order = db.DishCategories.Any() ? db.DishCategories.Max(dc => dc.Order) + 1 : 1;
                db.DishCategories.AddObject(dishCategory);

                db.SaveChanges();

                TempData["Result"] = Resource.ChangesSaved;
                return RedirectToAction("Index");
            }
            else
            {
                return Create();
            }
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var dishCategory = db.DishCategories.SingleOrDefault(dc => dc.Id == id);

            if (dishCategory == null)
            {
                TempData["Result"] = Resource.RecordNotFound;
                return RedirectToAction("Index");
            }
            else
            {
                return View(dishCategory);
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(DishCategory dishCategory, IList<DishCategory_Locale> locales)
        {
            if (dishCategory == null)
            {
                TempData["Result"] = Resource.RecordNotFound;
            }
            else
            {
                dishCategory.Name = locales.First(l => l.Culture == SupportedCulture.En).DisplayName.Replace(" ", "-");

                if (ModelState.IsValid)
                    if (db.DishCategories.Any(dc => dc.Name == dishCategory.Name && dc.Id != dishCategory.Id))
                        ModelState.AddModelError("Unique", Resource.Unique);

                if (ModelState.IsValid)
                {
                    foreach (var locale in locales)
                    {
                        db.DishCategory_Locale.Attach(locale);
                        db.ObjectStateManager.ChangeObjectState(locale, EntityState.Modified);
                    }

                    db.DishCategories.Attach(dishCategory);
                    db.ObjectStateManager.ChangeObjectState(dishCategory, EntityState.Modified);

                    db.SaveChanges();

                    TempData["Result"] = Resource.ChangesSaved;
                }
                else
                {
                    return Edit(dishCategory.Id);
                }
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            var dishCategory = db.DishCategories.SingleOrDefault(dc => dc.Id == id);

            if (dishCategory == null)
            {
                TempData["Result"] = Resource.RecordNotFound;
            }
            else
            {
                foreach (var locale in dishCategory.DishCategories_Locale.ToList())
                    db.DishCategory_Locale.DeleteObject(locale);

                foreach (var dish in dishCategory.Dishes.ToList())
                {
                    foreach (var locale in dish.Dishes_Locale.ToList())
                        db.Dish_Locale.DeleteObject(locale);

                    db.Dishes.DeleteObject(dish);
                }

                db.DishCategories.DeleteObject(dishCategory);

                db.SaveChanges();

                TempData["Result"] = Resource.ChangesSaved;
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult MoveUp(int id)
        {
            var dishCategory = db.DishCategories.SingleOrDefault(dc => dc.Id == id);

            if (dishCategory == null)
            {
                TempData["Result"] = Resource.RecordNotFound;
            }
            else
            {
                var dishCategoryToSwap = db.DishCategories.Where(dc => dc.Order < dishCategory.Order).OrderByDescending(dc => dc.Order).FirstOrDefault();
                if (dishCategoryToSwap == null)
                {
                    TempData["Result"] = Resource.ItemCannotBeMoved;
                }
                else
                {
                    var tempOrder = dishCategory.Order;
                    dishCategory.Order = dishCategoryToSwap.Order;
                    dishCategoryToSwap.Order = tempOrder;

                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult MoveDown(int id)
        {
            var dishCategory = db.DishCategories.SingleOrDefault(dc => dc.Id == id);

            if (dishCategory == null)
            {
                TempData["Result"] = Resource.RecordNotFound;
            }
            else
            {
                var dishCategoryToSwap = db.DishCategories.Where(dc => dc.Order > dishCategory.Order).OrderBy(dc => dc.Order).FirstOrDefault();
                if (dishCategoryToSwap == null)
                {
                    TempData["Result"] = Resource.ItemCannotBeMoved;
                }
                else
                {
                    var tempOrder = dishCategory.Order;
                    dishCategory.Order = dishCategoryToSwap.Order;
                    dishCategoryToSwap.Order = tempOrder;

                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}