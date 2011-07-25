using System.Data;
using System.Linq;
using System.Web.Mvc;
using Shamrock_WebSite.App_GlobalResources;
using Shamrock_WebSite.Models;

namespace Shamrock_WebSite.Controllers
{
    [Authorize]
    public class StaffMemberController : Controller
    {
        ShamrockEntities db = new ShamrockEntities();

        public ActionResult Index()
        {
            var staffMembers = db.StaffMembers;
            return View(staffMembers);
        }

        public ActionResult Create()
        {
            var staffMember = new StaffMember();
            return View(staffMember);
        }

        [HttpPost]
        public ActionResult Create(StaffMember staffMember)
        {
            if (ModelState.IsValid)
            {
                db.StaffMembers.AddObject(staffMember);

                db.SaveChanges();

                TempData["Result"] = Resource.ChangesSaved;
                return RedirectToAction("Index");
            }
            else
            {
                return Create();
            }
        }

        public ActionResult Edit(int id)
        {
            var staffMember = db.StaffMembers.SingleOrDefault(sm => sm.Id == id);

            if (staffMember == null)
            {
                TempData["Result"] = Resource.RecordNotFound;
                return RedirectToAction("Index", "DishCategory");
            }
            else
            {
                return View(staffMember);
            }
        }

        [HttpPost]
        public ActionResult Edit(StaffMember staffMember)
        {
            if (staffMember == null)
            {
                TempData["Result"] = Resource.RecordNotFound;
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.StaffMembers.Attach(staffMember);
                    db.ObjectStateManager.ChangeObjectState(staffMember, EntityState.Modified);

                    db.SaveChanges();

                    TempData["Result"] = Resource.ChangesSaved;
                }
                else
                {
                    return Edit(staffMember.Id);
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var staffMember = db.StaffMembers.SingleOrDefault(sm => sm.Id == id);

            if (staffMember == null)
            {
                TempData["Result"] = Resource.RecordNotFound;
            }
            else
            {
                db.StaffMembers.DeleteObject(staffMember);

                db.SaveChanges();

                TempData["Result"] = Resource.ChangesSaved;
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