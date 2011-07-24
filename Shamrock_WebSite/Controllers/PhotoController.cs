using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Shamrock_WebSite.App_GlobalResources;
using Shamrock_WebSite.Models;
using Shamrock_WebSite.Services;

namespace Shamrock_WebSite.Controllers
{
    public class PhotoController : Controller
    {
        ShamrockEntities db = new ShamrockEntities();

        [ChildActionOnly]
        public ActionResult Index(int photoAlbumId)
        {
            var photoAlbum = db.PhotoAlbums.SingleOrDefault(pa => pa.Id == photoAlbumId);

            if (photoAlbum == null)
            {
                return null;            
            }
            else
            {
                var photos = ImagesManager.GetImages(Path.Combine("PhotoAlbum", photoAlbum.Id.ToString()));
                ViewBag.PhotoAlbumId = photoAlbum.Id;
                ViewBag.PhotoAlbumName = photoAlbum.Locale.DisplayName;
                return PartialView(photos);
            }
        }

        [Authorize]
        public ActionResult Upload(int photoAlbumId, IEnumerable<HttpPostedFileBase> images)
        {
            if (images != null && images.All(i => i != null))
            {
                foreach (var image in images)
                {
                    var webImage = new WebImage(image.InputStream) { FileName = image.FileName };
                    ImagesManager.UploadImage(webImage, Path.Combine("PhotoAlbum", photoAlbumId.ToString()), true, 120, 120, true);
                }
                TempData["Result"] = Resource.ChangesSaved;
            }
            else
            {
                TempData["Result"] = Resource.NoImagesChosen;
            }
            return RedirectToAction("Details", "PhotoAlbum", new { id = photoAlbumId });
        }

        [Authorize]
        public ActionResult Delete(int photoAlbumId, string fileName)
        {
            ImagesManager.DeleteImage(Path.GetFileName(fileName), Path.Combine("PhotoAlbum", photoAlbumId.ToString()));
            TempData["Result"] = Resource.ChangesSaved;
            return RedirectToAction("Details", "PhotoAlbum", new { id = photoAlbumId });
        }

    }
}