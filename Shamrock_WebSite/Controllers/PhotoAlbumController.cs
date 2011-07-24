using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shamrock_WebSite.Models;
using Shamrock_WebSite.App_GlobalResources;
using Shamrock_WebSite.Services;
using System.IO;
using System.Data;
using Shamrock_WebSite.Infrastructure;

namespace Shamrock_WebSite.Controllers
{
    public class PhotoAlbumController : Controller
    {
        ShamrockEntities db = new ShamrockEntities();

        public ViewResult Index()
        {
            var photoAlbums = db.PhotoAlbums.OrderBy(pa => pa.Order);

            return View(photoAlbums);
        }

        public ActionResult Details(int id)
        {
            var photoAlbum = db.PhotoAlbums.SingleOrDefault(pa => pa.Id == id);
            if (photoAlbum == null)
            {
                TempData["Result"] = Resource.RecordNotFound;
            }
            else
            {
                return View(photoAlbum);
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Create()
        {
            var photoAlbum = new PhotoAlbum();

            foreach (var culture in SupportedCulture.GetList())
                photoAlbum.PhotoAlbums_Locale.Add(new PhotoAlbum_Locale() { Culture = culture });

            return View(photoAlbum);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(PhotoAlbum photoAlbum, IList<PhotoAlbum_Locale> locales)
        {
            foreach (var locale in locales)
                locale.PhotoAlbum = photoAlbum;

            if (ModelState.IsValid)
            {

                photoAlbum.Order = db.PhotoAlbums.Any() ? db.PhotoAlbums.Max(dc => dc.Order) + 1 : 1;

                db.PhotoAlbums.AddObject(photoAlbum);

                db.SaveChanges();

                Directory.CreateDirectory(Server.MapPath(Path.Combine("~/Content/Images/PhotoAlbum", photoAlbum.Id.ToString())));

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
            var photoAlbum = db.PhotoAlbums.SingleOrDefault(pa => pa.Id == id);

            if (photoAlbum == null)
            {
                TempData["Result"] = Resource.RecordNotFound;
                return RedirectToAction("Index");
            }
            else
            {
                return View(photoAlbum);
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(PhotoAlbum photoAlbum, IList<PhotoAlbum_Locale> locales)
        {
            if (photoAlbum == null)
            {
                TempData["Result"] = Resource.RecordNotFound;
            }
            else
            {
                if (ModelState.IsValid)
                {
                    foreach (var locale in locales)
                    {
                        db.PhotoAlbum_Locale.Attach(locale);
                        db.ObjectStateManager.ChangeObjectState(locale, EntityState.Modified);
                    }

                    db.PhotoAlbums.Attach(photoAlbum);
                    db.ObjectStateManager.ChangeObjectState(photoAlbum, EntityState.Modified);

                    db.SaveChanges();

                    TempData["Result"] = Resource.ChangesSaved;
                }
                else
                {
                    return Edit(photoAlbum.Id);
                }
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            var photoAlbum = db.PhotoAlbums.SingleOrDefault(pa => pa.Id == id);

            if (photoAlbum == null)
            {
                TempData["Result"] = Resource.RecordNotFound;
            }
            else if (photoAlbum.IsSystem)
            {
                TempData["Result"] = Resource.SystemAlbumCannotBeDeleted;
            }
            else
            {
                foreach (var locale in photoAlbum.PhotoAlbums_Locale.ToList())
                    db.PhotoAlbum_Locale.DeleteObject(locale);

                DirectoryExtensions.DeleteDirectoryAndAllFilesInIt(Server.MapPath(Path.Combine("~/Content/Images/PhotoAlbum", photoAlbum.Id.ToString())));

                db.PhotoAlbums.DeleteObject(photoAlbum);

                db.SaveChanges();

                TempData["Result"] = Resource.ChangesSaved;
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult MoveUp(int id)
        {
            var photoAlbum = db.PhotoAlbums.SingleOrDefault(pa => pa.Id == id);

            if (photoAlbum == null)
            {
                TempData["Result"] = Resource.RecordNotFound;
            }
            else
            {
                var photoAlbumToSwap = db.PhotoAlbums.Where(pa => pa.Order < photoAlbum.Order).OrderByDescending(dc => dc.Order).FirstOrDefault();
                if (photoAlbumToSwap == null)
                {
                    TempData["Result"] = Resource.ItemCannotBeMoved;
                }
                else
                {
                    var tempOrder = photoAlbum.Order;
                    photoAlbum.Order = photoAlbumToSwap.Order;
                    photoAlbumToSwap.Order = tempOrder;

                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult MoveDown(int id)
        {
            var photoAlbum = db.PhotoAlbums.SingleOrDefault(pa => pa.Id == id);

            if (photoAlbum == null)
            {
                TempData["Result"] = Resource.RecordNotFound;
            }
            else
            {
                var photoAlbumToSwap = db.PhotoAlbums.Where(pa => pa.Order > photoAlbum.Order).OrderBy(dc => dc.Order).FirstOrDefault();
                if (photoAlbumToSwap == null)
                {
                    TempData["Result"] = Resource.ItemCannotBeMoved;
                }
                else
                {
                    var tempOrder = photoAlbum.Order;
                    photoAlbum.Order = photoAlbumToSwap.Order;
                    photoAlbumToSwap.Order = tempOrder;

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