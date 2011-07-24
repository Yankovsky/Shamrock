using System;
using System.Linq;
using System.Web.Mvc;
using Shamrock_WebSite.App_GlobalResources;
using Shamrock_WebSite.Models;

namespace Shamrock_WebSite.Controllers
{
    public class CommentsController : Controller
    {
        ShamrockEntities db = new ShamrockEntities();

        [Authorize]
        public ActionResult Index(int? page)
        {
            var pageSize = 5;
            if (!page.HasValue || page.Value == 0)
                page = 1;
            var comments = db.Comments.OrderByDescending(c => c.Date).Skip((page.Value - 1) * pageSize).Take(pageSize);

            var commentsCount = db.Comments.Count();
            ViewBag.TotalPages = (int)Math.Ceiling((double)commentsCount / pageSize);
            ViewBag.CurrentPage = page;
            if (Request.IsAjaxRequest())
            {
                return PartialView(comments);
            }
            return View(comments);
        }

        public ActionResult Create()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("_CommentForm");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(Comment comment, string returlUrl)
        {
            if (ModelState.IsValid)
            {
                comment.Date = DateTime.Now;
                db.Comments.AddObject(comment);
                db.SaveChanges();

                var success = String.Format(Resource.CommentSuccess, comment.Author);
                if (Request.IsAjaxRequest())
                {
                    return Content(success);
                }
                TempData["Result"] = success;
                return Redirect(returlUrl ?? Url.Action("Index", "About"));
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_CommentForm");
                }
                return View();
            }
        }

        [Authorize]
        public RedirectResult Delete(int id, string returnUrl)
        {
            var comment = db.Comments.SingleOrDefault(e => e.Id == id);

            if (comment == null)
            {
                TempData["Result"] = Resource.RecordNotFound;
            }
            else
            {
                db.Comments.DeleteObject(comment);
                db.SaveChanges();

                TempData["Result"] = Resource.ChangesSaved;
            }
            return Redirect(returnUrl ?? Url.Action("Index"));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}