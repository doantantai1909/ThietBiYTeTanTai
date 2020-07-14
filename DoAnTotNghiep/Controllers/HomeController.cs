using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnTotNghiep.Models;
using CaptchaMvc.HtmlHelpers;
using CaptchaMvc;
using System.Web.Security;

namespace DoAnTotNghiep.Controllers
{
   
    public class HomeController : Controller
    {
        // GET: Home
        QuanLiBanHang1Entities db = new QuanLiBanHang1Entities();
        public ActionResult Index()
        {
            // list sản phẩm có số lượng mua cao
            var lstSP = db.SanPhams.Where(n => n.SoLanMua > 200);
            ViewBag.ListSP = lstSP;

            var ltt = db.TinTucs.OrderBy(x => Guid.NewGuid()).Take(3);
            ViewBag.ltt = ltt;

            var lgt = db.GioiThieux.OrderBy(x => Guid.NewGuid()).Take(3);
            ViewBag.lgt = lgt;

            var ldv = db.DichVus.OrderBy(x => Guid.NewGuid()).Take(5);
            ViewBag.ldv = ldv;

            var lstTinTuc = db.TinTucs;
            ViewBag.ListTT = lstTinTuc;
            return View();
        }
        public ActionResult MenuPartial()
        {
            // lấy ra 1 list các loại sản phẩm
            var lstSP = db.LoaiSanPhams;
            return PartialView(lstSP);
        }

     

        public ActionResult FooterGioiThieuPartial()
        {
            var lstGioiThieu = db.GioiThieux.Where(n => n.MaLoaiGT == 1);
            ViewBag.ListGT = lstGioiThieu;
            return PartialView(lstGioiThieu);
        }
        public ActionResult FooterDichVuPartial()
        {
            var lstDichVu = db.DichVus.Where(n => n.MaLoaiDV == 1);
            ViewBag.ListDV = lstDichVu;
            return PartialView(lstDichVu);
        }
        public ActionResult FooterChinhSachPartial()
        {
            var lstChinhSach = db.ChinhSaches;
            ViewBag.ListCS = lstChinhSach;
            return PartialView(lstChinhSach);
        }





        public ActionResult MenuDocPartial()
        {
            var lstSPdoc = db.LoaiSanPhams;
            return PartialView(lstSPdoc);
        }

        public ActionResult MenuMobliePartial()
        {
            var lstSPmobile = db.LoaiSanPhams;
            return PartialView(lstSPmobile);
        }
      

        [HttpGet]
        public ActionResult DangKy()
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            return View();
        }
       

        [HttpPost]
        public ActionResult DangKy(ThanhVien tv, FormCollection f)
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            //Kiểm tra capcha hợp lê
            if (this.IsCaptchaValid("Captcha is not valid"))
            {
                ViewBag.ThongBao = "Thêm Thành Công";
                // Thêm khách hàng vào cơ sở dữ liệu
                //if (Convert.ToString(f["NhapLaiMatKhau"]) == Convert.ToString(f["MatKhau"]))
                //{
                    db.ThanhViens.Add(tv);
                    db.SaveChanges();

                    return View("");
               // }
            }
            ViewBag.ThongBao = "Sai Mã Captcha";

            return View();
        }

        //Load câu hỏi bí mật tĩnh
        public List<string> LoadCauHoi()
        {
            List<string> lstCauHoi = new List<string>();
            lstCauHoi.Add("Con vật mà bạn yêu thích?");
            lstCauHoi.Add("Ca sĩ mà bạn yêu thích?");
            lstCauHoi.Add("Hiện tại bạn đang làm công việc gì?");
            return lstCauHoi;
        }

        public ActionResult DemoDN()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
          
            string TaiKhoan = f["TaiKhoanDN"].ToString();
            string MatKhau = f["MatKhauDN"].ToString();
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == TaiKhoan && n.MatKhau == MatKhau);
            if (tv != null)
            {
                // Lấy ra list quyền của thành viên tương ứng với loại  thành viên
                var lstQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == tv.MaLoaiTV);
                //// duyệt list quyền

                string Quyen = " ";
                foreach (var item in lstQuyen)
                {
                    Quyen += item.Quyen.MaQuyen + ",";

                }
                Quyen = Quyen.Substring(0, Quyen.Length - 1); //Cắt đi dấu , cuối cùng (Chuỗi sau khi cắt: “DangKy,QuanLyDonHang,QuanLySanPham”)
                PhanQuyen(tv.TaiKhoan.ToString(), Quyen);
                Session["LuuTaiKhoan"] = tv;
                return Content("<script> window.location.reload();  </script>");
            }
            return Content("Tài Khoản hoặc Mật Khẩu không đúng!");
        }

        [HttpPost]
        public ActionResult DangNhapAdmin(FormCollection f)
        {

            string TaiKhoan = f["TaiKhoanDNAdmin"].ToString();
            string MatKhau = f["MatKhauDNAdmin"].ToString();
            ThanhVien tvAdmin = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == TaiKhoan && n.MatKhau == MatKhau);
            if (tvAdmin != null)
            {
                // Lấy ra list quyền của thành viên tương ứng với loại  thành viên
                var lstQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == tvAdmin.MaLoaiTV);
                //// duyệt list quyền

                string Quyen = " ";
                foreach (var item in lstQuyen)
                {
                    Quyen += item.Quyen.MaQuyen + ",";

                }
                Quyen = Quyen.Substring(0, Quyen.Length - 1); //Cắt đi dấu , cuối cùng (Chuỗi sau khi cắt: “DangKy,QuanLyDonHang,QuanLySanPham”)
                PhanQuyen(tvAdmin.TaiKhoan.ToString(), Quyen);

                Session["LuuTaiKhoanAdmin"] = tvAdmin;
                return RedirectToAction("IndexAdmin2", "Admin2");
            }
           return Content("Tài Khoản hoặc Mật Khẩu không đúng!");

        }

        public void PhanQuyen(string TaiKhoan, string Quyen)
        {
            FormsAuthentication.Initialize();
            var ticket = new FormsAuthenticationTicket(1,
            TaiKhoan, //user
            DateTime.Now, //begin
            DateTime.Now.AddHours(3), //timeout
            false, //remember?
            Quyen, // permission.. "admin" or for more than one  "DangKy,QuanLySanPham,QuanLyDonHang"

            FormsAuthentication.FormsCookiePath);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;
            Response.Cookies.Add(cookie);
        }














        public ActionResult DangXuat()
        {
            Session["LuuTaiKhoan"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");

        }

        // tạo trang ngăn chặn quyền truy cập
        public ActionResult LoiPhanQuyen()
        {
            return View();
        }



        public ActionResult TinTuc()
        {
            var lstTinTuc = db.TinTucs;
            return PartialView(lstTinTuc);
        }
        public ActionResult ChiTietTinTuc(int? id)
        {
            TinTuc tt = db.TinTucs.SingleOrDefault(n => n.MaTinTuc == id);
            if (tt == null)
            {
                return HttpNotFound();
            }
            return View(tt);
        }

        public ActionResult GioiThieu()
        {
            var lstGioiThieu = db.GioiThieux;
            return PartialView(lstGioiThieu);
            //var lstGioiThieu = db.GioiThieux.Where(n=>n.MaLoaiGT==2);
        }
        public ActionResult ChiTietGioiThieu(int? id)
        {
            GioiThieu gt = db.GioiThieux.SingleOrDefault(n => n.MaGioiThieu == id);
            if (gt == null)
            {
                return HttpNotFound();
            }
            return View(gt);
        }
        public ActionResult DichVu()
        {
            var lstDichVu = db.DichVus;
            //var lstDichVu = db.DichVus.Where(n=>n.MaLoaiDV==7);
            return PartialView(lstDichVu);
        }
        public ActionResult ChiTietDichVu(int? id)
        {
            DichVu dv = db.DichVus.SingleOrDefault(n => n.MaDichVu == id);
            if (dv == null)
            {
                return HttpNotFound();
            }
            return View(dv);
        }


        public ActionResult TuyenDung()
        {
            var lstTuyenDung = db.TuyenDungs;
            return PartialView(lstTuyenDung);
           
        }
        public ActionResult ChiTietTuyenDung(int? id)
        {
            TuyenDung td = db.TuyenDungs.SingleOrDefault(n => n.MaTuyenDung == id);
            if (td == null)
            {
                return HttpNotFound();
            }
            return View(td);
        }


        public ActionResult LienHe()
        {
          
            return PartialView();
        }


        public ActionResult ChiTietChinhSach(int? id)
        {
            ChinhSach cs = db.ChinhSaches.SingleOrDefault(n => n.MaChinhSach == id);
            if (cs == null)
            {
                return HttpNotFound();
            }
            return View(cs);
        }


        [ChildActionOnly]
        public ActionResult TinTucTuc()
        {
            return PartialView();
        }
        [ChildActionOnly]
        public ActionResult DichVuVu()
        {
            return PartialView();
        }


    }
}