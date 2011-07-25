using System;
using System.Linq;
using System.Web.Mvc;
using Shamrock_WebSite.App_GlobalResources;
using Shamrock_WebSite.Models;

namespace Shamrock_WebSite.Controllers
{
    public class ReservationController : Controller
    {
        ShamrockEntities db = new ShamrockEntities();

        public ActionResult Index(string year, string month, string day)
        {
            DateTime date;
            if (!DateTime.TryParse(String.Join("-", year, month, day), out date) || date < DateTime.Today)
                date = DateTime.Today;

            ViewBag.Date = date;
            var reservationData = db.TableReservations.Where(tr => tr.Date.Year == date.Year && 
                                                                   tr.Date.Month == date.Month &&
                                                                   tr.Date.Day == date.Day);
            return View(reservationData);
        }

        public ActionResult Reserve(int tableId, string year, string month, string day, string returnUrl)
        {            
            DateTime date;
            if (!DateTime.TryParse(String.Join("-", year, month, day), out date) || date < DateTime.Today)
            {
                TempData["Result"] = Resource.RecordNotFound;
                return Redirect(returnUrl ?? Url.Action("Index"));
            }
            else
            {
                if (db.TableReservations.Any(tr => tr.TableId == tableId && tr.Date.Day == date.Day && tr.Date.Month == date.Month && tr.Date.Year == date.Year))
                {
                    var alreadyReserved = Resource.AlreadyReserved;
                    if (Request.IsAjaxRequest())
                    {
                        return Content(alreadyReserved);
                    }
                    TempData["Result"] = alreadyReserved;
                    return Redirect(returnUrl ?? Url.Action("Index"));
                }

                var reservation = new TableReservation();
                reservation.Date = date;
                reservation.TableId = tableId;
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_ReserveForm", reservation);
                }
                return View(reservation);
            }
        }

        [HttpPost]
        public ActionResult Reserve(TableReservation reservation, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (db.TableReservations.Any(tr => tr.Date == reservation.Date && tr.TableId == reservation.TableId))
                {
                    TempData["Result"] = Resource.AlreadyReserved;
                    return Redirect(returnUrl ?? Url.Action("Index"));
                }
                db.TableReservations.AddObject(reservation);

                db.SaveChanges();

                var success = String.Format(Resource.ReserveSuccess, reservation.Name, reservation.TableId);
                if (Request.IsAjaxRequest())
                {
                    return Content(success);
                }
                TempData["Result"] = success;
                return Redirect(returnUrl ?? Url.Action("Index"));
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_ReserveForm");
                }
                return View();
            }
        }
    }
}