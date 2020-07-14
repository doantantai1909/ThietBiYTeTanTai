using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnTotNghiep.Models;
using PagedList;

namespace DoAnTotNghiep.Controllers
{
    public class TimKiemController : Controller
    {
        // GET: TimKiem
        QuanLiBanHang1Entities db = new QuanLiBanHang1Entities();
        [HttpGet]
        public ActionResult KQTimKiem( string sTuKhoa, int? page)
        {
            // Thực hiện chức năng phân trang
            // tạo biến số sản phẩm trên trang
            if (Request.HttpMethod != "Get")
            {
                page = 1;
            }
            int PageSize = 6;
            // Tạo biến số trang hiện tại
            int PageNumber = (page ?? 1);


            //tìm kiếm theo tên sp
            var lstSP = db.SanPhams.Where(n => n.TenSP.Contains(sTuKhoa));
            ViewBag.TuKhoa = sTuKhoa;
            return View(lstSP.OrderBy(n => n.TenSP).ToPagedList(PageNumber, PageSize));
        }
        [HttpPost]
        public ActionResult LayTuKhoaTimKiem(string sTuKhoa)
        {
             // gọi về hàm get Tìm Kiếm
            return RedirectToAction("KQTimKiem", new {sTuKhoa = sTuKhoa });
        }

        public ActionResult KQTimKiemPartial(string sTuKhoa)
        {
            //tìm kiếm theo tên sp
            var lstSP = db.SanPhams.Where(n => n.TenSP.Contains(sTuKhoa));
            ViewBag.TuKhoa = sTuKhoa;



            return PartialView(lstSP.OrderBy(n => n.DonGia));
        }


    }
}