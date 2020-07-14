using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnTotNghiep.Models;
namespace DoAnTotNghiep.Controllers
{
    [Authorize(Roles = "QuanTri, QuanLyKhachHang")]
    public class QuanLyKhachHangController : Controller
    {
        // GET: QuanLyKhachHang
        QuanLiBanHang1Entities db = new QuanLiBanHang1Entities();
        public ActionResult IndexKhachHang()
        {
            return View(db.ThanhViens);
        }

        public ActionResult AdminTKTV(string sTuKhoaTV)
        {
            //tìm kiếm theo tên thành viên
            var lstTV = db.ThanhViens.Where(n => n.Hoten.Contains(sTuKhoaTV));
            ViewBag.TuKhoaNSX = sTuKhoaTV;
            return PartialView(lstTV);
        }
    }
}