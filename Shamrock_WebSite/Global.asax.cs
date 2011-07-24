using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Data.Entity;
using Shamrock_WebSite.Models;
using Microsoft.Practices.Unity;
using System.Globalization;
using System.Threading;
using System.Web.Caching;
using Shamrock_WebSite.Services;
using System.IO;

namespace Shamrock_WebSite
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                null,
                "Comment/Page/{page}",
                new { controller = "Comment", action = "Index" },
                new { page = @"\d+" }
            );

            routes.MapRoute(
                null,
                "Event/Page/{page}",
                new { controller = "Event", action = "Index" },
                new { page = @"\d+" }
            );

            routes.MapRoute(
                null,
                "Event/Archive/Page/{page}",
                new { controller = "Event", action = "Archive" },
                new { page = @"\d+" }
            );

            routes.MapRoute(
                null,
                "Menu/{dishCategoryName}",
                new { controller = "DishCategory", action = "Index", dishCategoryName = UrlParameter.Optional }
            );

            routes.MapRoute(
                null,
                "Gallery/{photoAlbumName}",
                new { controller = "PhotoAlbum", action = "Index", photoAlbumName = UrlParameter.Optional }
            );

            routes.MapRoute(
                null,
                "Reservation/{year}/{month}/{day}",
                new { controller = "Reservation", action = "Index" },
                new { year = @"\d{4}", month = @"\d{1,2}", day = @"\d{1,2}" });

            routes.MapRoute(
                "Reservation",
                "Reservation",
                new { controller = "Reservation", action = "Index" }
                );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        private const string _removeExpiredData = "RemoveExpiredData";
        private const string _sendNotifications = "SendNotifications";
        private static CacheItemRemovedCallback OnCacheRemove = null;

        private void AddTask(string name, int seconds)
        {
            OnCacheRemove = new CacheItemRemovedCallback(CacheItemRemoved);
            HttpRuntime.Cache.Insert(name, seconds, null,
                DateTime.Now.AddSeconds(seconds), Cache.NoSlidingExpiration,
                CacheItemPriority.NotRemovable, OnCacheRemove);
        }

        public void CacheItemRemoved(string name, object obj, CacheItemRemovedReason reason)
        {
            if (name == _removeExpiredData)
            {
                var db = new ShamrockEntities();
                var date = DateTime.Today.AddDays(-2);
                var reservationData = db.TableReservations.Where(tr => tr.Date < date).ToList();
                foreach (var reservation in reservationData)
                    db.TableReservations.DeleteObject(reservation);

                db.SaveChanges();
            }
            if (name == _sendNotifications)
            {
                using (var db = new ShamrockEntities())
                {
                    var unnotifiedTableReservations = db.TableReservations.Where(tr => tr.StaffNotified == false);

                    var staff = ConfigWrapper.Config.Root.Element("staff").Elements("staffMember");
                    foreach (var tr in unnotifiedTableReservations)
                    {
                        var success = true;
                        foreach (var staffMember in staff)
                        {
                            var staffMemberName = staffMember.Attribute("name").Value;
                            var staffMemberPhoneNumber = staffMember.Attribute("phoneNumber").Value;

                            var text = String.Format("{0}, {1} забронировал столик №{2} на {3} {4}. Пожелания: {5}, номер: {6}",
                                staffMemberName, tr.Name, tr.TableId, tr.Date.ToShortDateString(), tr.Time, tr.Wishes, tr.PhoneNumber);
                            success &= SMSService.TrySendSMS(text, staffMemberPhoneNumber);
                        }
                        if (success)
                        {
                            tr.StaffNotified = true;
                            db.SaveChanges();
                        }
                    }
                }
            }
            AddTask(name, Convert.ToInt32(obj));
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            UnityControllerFactory unityFactory = new UnityControllerFactory(new UnityContainer());
            ControllerBuilder.Current.SetControllerFactory(unityFactory);

            ConfigWrapper.SetServerPath(Server.MapPath("~/"));

            //Cheat, using cache for scheduling
            AddTask(_removeExpiredData, 86400); //1 day 86400
            AddTask(_sendNotifications, 1800); //30 minutes 1800
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session != null)
            {
                var cultureInfo = (CultureInfo)Session["Culture"];
                if (cultureInfo == null)
                {

                    if (Request.UserLanguages != null && Request.UserLanguages.Count() != 0 && Request.UserLanguages[0].Substring(0, 2).ToLower() == SupportedCulture.En)
                        cultureInfo = new CultureInfo(SupportedCulture.En);
                    else
                        cultureInfo = new CultureInfo(SupportedCulture.Ru);

                    Session["Culture"] = cultureInfo;
                }

                Thread.CurrentThread.CurrentUICulture = cultureInfo;
                Thread.CurrentThread.CurrentCulture = cultureInfo;
            }
        }
    }
}