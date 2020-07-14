using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnTotNghiep.Models;

namespace DoAnTotNghiep.Controllers
{
    [Authorize(Roles = "QuanLyQuyen")]
    public class QuyenController : Controller
    {
        // GET: QuanTri/Quyen
        QuanLiBanHang1Entities db = new QuanLiBanHang1Entities();

        public ActionResult IndexQuyen()
        {
            return View(db.Quyens.OrderBy(n => n.TenQuyen));
        }
        [HttpGet]
        public ActionResult ThemQuyen()
        {
            return View();

        }
        [HttpPost]
        public ActionResult ThemQuyen(Quyen quyen)
        {

            if (ModelState.IsValid)
            {
                db.Quyens.Add(quyen);
                db.SaveChanges();
            }

            return RedirectToAction("IndexQuyen");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                    db.Dispose();
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}