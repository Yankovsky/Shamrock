using System;
using System.Collections.Generic;
using System.Data;
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
    public class EventsController : Controller
    {
        ShamrockEntities db = new ShamrockEntities();

        [ChildActionOnly]
        public PartialViewResult ClosestEvent()
        {
            var date = DateTime.Now.Date;
            var closestEvent = db.Events.Where(e => e.Date >= date).OrderBy(e => e.Date).FirstOrDefault();
            if (closestEvent == null)
                return null;
            return PartialView("_Event", closestEvent);
        }

        public ActionResult Index(int? page)
        {
            var pageSize = 3;
            if (!page.HasValue || page.Value == 0)
                page = 1;

            var today = DateTime.Now.Date;
            var futureEvents = db.Events.Where(e => e.Date >= today).OrderBy(c => c.Date);
            var futureEventsByPage = futureEvents.Skip((page.Value - 1) * pageSize).Take(pageSize);

            var futureEventsCount = futureEvents.Count();
            ViewBag.TotalPages = (int)Math.Ceiling((double)futureEventsCount / pageSize);
            ViewBag.CurrentPage = page;

            if (Request.IsAjaxRequest())
            {
                return PartialView("_FutureEvents", futureEventsByPage);
            }
            return View(futureEventsByPage);
        }

        public ActionResult Archive(int? page)
        {
            var pageSize = 3;
            if (!page.HasValue || page.Value == 0)
                page = 1;

            var today = DateTime.Now.Date;
            var pastEvents = db.Events.Where(e => e.Date < today).OrderByDescending(c => c.Date);
            var pastEventsByPage = pastEvents.Skip((page.Value - 1) * pageSize).Take(pageSize);

            var pastEventsCount = pastEvents.Count();
            ViewBag.TotalPages = (int)Math.Ceiling((double)pastEventsCount / pageSize);
            ViewBag.CurrentPage = page;

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Archive", pastEventsByPage);
            }
            return View(pastEventsByPage);
        }

        public ActionResult Details(int id, string returnUrl)
        {
            var _event = db.Events.SingleOrDefault(e => e.Id == id);
            if (_event == null)
            {
                TempData["Result"] = Resource.RecordNotFound;
                return Redirect(returnUrl ?? Url.Action("Index"));
            }
            else
            {
                return View(_event);
            }
        }

        [Authorize]
        public ViewResult Create()
        {
            var eventTypes = db.EventTypes;
            ViewBag.EventTypes = new SelectList(eventTypes, "Id", "Name", db.EventTypes.FirstOrDefault());

            var _event = new Event() { Date = DateTime.Now.AddDays(7) };

            foreach (var culture in SupportedCulture.GetList())
                _event.Events_Locale.Add(new Event_Locale() { Culture = culture });

            return View(_event);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(Event _event, IList<Event_Locale> locales, HttpPostedFileBase image)
        {
            foreach (var locale in locales)
                locale.Event = _event;

            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var webImage = new WebImage(image.InputStream) { FileName = image.FileName };
                    _event.ImagePath = ImagesManager.UploadImage(webImage, "Event", true, 240, 180);
                }
                db.Events.AddObject(_event);

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
            var _event = db.Events.SingleOrDefault(e => e.Id == id);

            if (_event == null)
            {
                TempData["Result"] = Resource.RecordNotFound;
                return RedirectToAction("Index");
            }
            else
            {
                var eventTypes = db.EventTypes;
                ViewBag.EventTypes = new SelectList(eventTypes, "Id", "Name", db.EventTypes.FirstOrDefault());

                return View(_event);
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(Event _event, IList<Event_Locale> locales, HttpPostedFileBase image, string returnUrl)
        {
            if (_event == null)
            {
                TempData["Result"] = Resource.RecordNotFound;
                return Redirect(returnUrl ?? Url.Action("Index"));
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (image != null)
                    {
                        if (!String.IsNullOrWhiteSpace(_event.ImagePath))
                            ImagesManager.DeleteImage(Path.GetFileName(_event.ImagePath), "Event");
                        var webImage = new WebImage(image.InputStream) { FileName = image.FileName };
                        _event.ImagePath = ImagesManager.UploadImage(webImage, "Event", true, 240, 180);
                    }

                    foreach (var locale in locales)
                    {
                        db.Event_Locale.Attach(locale);
                        db.ObjectStateManager.ChangeObjectState(locale, EntityState.Modified);
                    }

                    db.Events.Attach(_event);
                    db.ObjectStateManager.ChangeObjectState(_event, EntityState.Modified);

                    db.SaveChanges();

                    TempData["Result"] = Resource.ChangesSaved;
                    return Redirect(returnUrl ?? Url.Action("Index"));
                }
                else
                {
                    return Edit(_event.Id);
                }
            }
        }

        [Authorize]
        public ActionResult Delete(int id, string returnUrl)
        {
            var _event = db.Events.SingleOrDefault(e => e.Id == id);

            if (_event == null)
            {
                TempData["Result"] = Resource.RecordNotFound;
            }
            else
            {
                foreach (var locale in _event.Events_Locale.ToList())
                    db.Event_Locale.DeleteObject(locale);

                if (!String.IsNullOrWhiteSpace(_event.ImagePath))
                    ImagesManager.DeleteImage(Path.GetFileName(_event.ImagePath), "Event");

                db.Events.DeleteObject(_event);

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