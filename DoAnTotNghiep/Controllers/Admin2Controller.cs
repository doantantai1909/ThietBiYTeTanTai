using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnTotNghiep.Models;


namespace DoAnTotNghiep.Controllers
{
    [Authorize(Roles = "QuanTri, QuanLySanPham, QuanLyDonHang")]
    public class Admin2Controller : Controller
    {
        // GET: QuanTri/Admin2
        QuanLiBanHang1Entities db = new QuanLiBanHang1Entities();
        public ActionResult IndexAdmin2()
        {
            ViewBag.SoLuongTruyCap = HttpContext.Application["SoLuongTruyCap"].ToString();
            // lấy số lượng truy cập từ application
            ViewBag.SoLuongDangOnline = HttpContext.Application["SoLuongDangOnline"].ToString();
            ViewBag.TongDoanhThu = ThongKeDoanhThu(); // thống kê tổng doanh thu
            ViewBag.TongDDH = ThongKeDonHang(); //thống kê đơn hàng
            ViewBag.TongThanhVien = ThongKeThanhVien(); //thống kê thành viên
            return View();
        }

        public double ThongKeDonHang()
        {
            // đếm đơn đặt hàng
            double slddh = db.DonDatHangs.Count();
            return slddh;
        }

        public double ThongKeThanhVien()
        {
            // đếm đơn đặt hàng
            double sltv = db.ThanhViens.Count();
            return sltv;
        }




        public decimal ThongKeDoanhThu()
        {
            // thống kê theo tất cả doanh thu từ khi website thành lập
            decimal TongDoanhThu = db.ChiTietDDHs.Sum(n => n.SoLuong * n.DonGia).Value;
            return TongDoanhThu;
        }

        public decimal ThongKeDoanhThuThang(int thang, int nam)
        {
            var lstDDH = db.DonDatHangs.Where(n => n.NgayDat.Value.Month == thang && n.NgayDat.Value.Year == nam);
            decimal TongTien = 0;
            foreach (var item in lstDDH)
            {
                TongTien += decimal.Parse(item.ChiTietDDHs.Sum(n => n.SoLuong * n.DonGia).Value.ToString());
            }
            return TongTien;
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
        //public class searchMenu
        //{
        //    public string url { get; set; } =string.Empty;
        //    public string duongdan { get; set; }= string.Empty;

        //}

        //public ActionResult KQTKIndexAdmin(string sTuKhoaAdmin)
        //{
        //    //tìm kiếm theo tên sp
        //    var lstmenu = new List<searchMenu>();

        //    var obj1 = new searchMenu();
        //    obj1.url = "http://thienpnh.tk:82/QuanLySanPham/QuanLySanPham";
        //    obj1.duongdan = "Quản Lý Sản Phẩm";

        //    var obj2 = new searchMenu();
        //    obj2.url = "http://thienpnh.tk:82/QuanLySanPham/QuanLySanPham";
        //    obj2.duongdan = "Quản Lý Nhập Hàng";

        //    lstmenu.Add(obj1);
        //    lstmenu.Add(obj2);

        //    var lstKQTimKiem = new List<searchMenu>();
        //    foreach(var item in lstmenu)
        //    {
        //        if(item.duongdan == sTuKhoaAdmin) //so sanh bằng chuoi o day nha =)) so sanh chuoi ton tai trong chuoi c#
        //        {
        //            lstKQTimKiem.Add(item);
        //        }
        //    }

        //    //lstKQTimKiem ket qua tim dc

        //    if (String.Compare(sTuKhoaAdmin, "Quản Lý Sản Phẩm", true) ==0 )
        //    {
        //        Redirect("http://thienpnh.tk:82/QuanLySanPham/QuanLySanPham");
        //    }



     
        //    ViewBag.lstKQTimKiem = lstKQTimKiem;
        //    return PartialView();
        //}





    }
}