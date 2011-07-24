using System;
using System.Linq;
using System.Web.Mvc;
using Shamrock_WebSite.Models;
using System.Web;
using System.Web.Helpers;
using Shamrock_WebSite.Services;
using Shamrock_WebSite.App_GlobalResources;
using Shamrock_WebSite.Helpers;
using System.Data;
using System.IO;

namespace Shamrock_WebSite.Controllers
{
    public class ImageLinkController : Controller
    {
        ShamrockEntities db = new ShamrockEntities();

        //[OutputCache(Duration = 600, VaryByParam = "imageLinksBlockName")]
        [ChildActionOnly]
        public ActionResult GetImageLinksBlock(string imageLinksBlockName)
        {
            if (String.IsNullOrWhiteSpace(imageLinksBlockName))
            {
                var imageLinksBlock = db.ImageLinkCategories.SingleOrDefault(ilc => ilc.Name == imageLinksBlockName);
                if (imageLinksBlock != null)
                {
                    ViewBag.ImageLinksBlockName = imageLinksBlockName;
                    return PartialView(imageLinksBlock.ImageLinks);
                }
            }
            return null;            
        }

        [Authorize]
        public ActionResult Create()
        {
            var imageLinkCategories = db.ImageLinkCategories;
            ViewBag.ImageLinkCategories = new SelectList(imageLinkCategories, "Id", "Name", imageLinkCategories.FirstOrDefault());

            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(ImageLink imageLink, HttpPostedFileBase image)
        {
            if (image != null)
            {
                var webImage = new WebImage(image.InputStream) { FileName = image.FileName };
                imageLink.ImagePath = ImagesManager.UploadImage(webImage, "ImageLink");

                if (ModelState.IsValid)
                {
                    db.ImageLinks.AddObject(imageLink);

                    db.SaveChanges();

                    TempData["Result"] = Resource.ChangesSaved;
                    return RedirectToAction("Index", "Home", null);
                }
            }
            return Create();
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var _imageLink = db.ImageLinks.SingleOrDefault(il => il.Id == id);

            if (_imageLink == null)
            {
                TempData["Result"] = Resource.RecordNotFound;
                return Redirect(Url.Home());
            }
            else
            {
                var imageLinkCategories = db.ImageLinkCategories;
                ViewBag.ImageLinkCategories = new SelectList(imageLinkCategories, "Id", "Name", imageLinkCategories.FirstOrDefault());

                return View(_imageLink);
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(ImageLink imageLink, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    if (!String.IsNullOrWhiteSpace(imageLink.ImagePath))
                        ImagesManager.DeleteImage(Path.GetFileName(imageLink.ImagePath), "ImageLink");
                    var webImage = new WebImage(image.InputStream) { FileName = image.FileName };
                    imageLink.ImagePath = ImagesManager.UploadImage(webImage, "ImageLink");

                    db.ImageLinks.Attach(imageLink);
                    db.ObjectStateManager.ChangeObjectState(imageLink, EntityState.Modified);

                    db.SaveChanges();

                    TempData["Result"] = Resource.ChangesSaved;
                    return Redirect(Url.Home());
                }
            }
            return Edit(imageLink.Id);
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            var imageLink = db.ImageLinks.SingleOrDefault(il => il.Id == id);

            if (imageLink == null)
            {
                TempData["Result"] = Resource.RecordNotFound;
            }
            else
            {
                if (!String.IsNullOrWhiteSpace(imageLink.ImagePath))
                    ImagesManager.DeleteImage(Path.GetFileName(imageLink.ImagePath), "ImageLink");

                db.ImageLinks.DeleteObject(imageLink);

                db.SaveChanges();

                TempData["Result"] = Resource.ChangesSaved;
            }

            return Redirect(Url.Home());
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}