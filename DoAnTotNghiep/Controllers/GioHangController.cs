using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnTotNghiep.Models;
using PayPal.Api;

namespace DoAnTotNghiep.Controllers
{

    public class GioHangController : Controller
    {
        QuanLiBanHang1Entities db = new QuanLiBanHang1Entities();

        //LẤy giỏ hàng
        public List<ItemGioHang> LayGioHang()
        {
            // giỏ hàng đã tồn tại
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                //nếu session giỏ hàng chưa tồn tại thì ta khởi tạo  1 list giỏ hàng
                lstGioHang = new List<ItemGioHang>();
                Session["GioHang"] = lstGioHang;

            }
            return lstGioHang;
        }

        //Thêm giỏ hàng thông thường load lại trang
        public ActionResult ThemGioHang(int MaSP, string strURL)
        {
            // Kiểm tra sản phẩm có tòn tại trong csdl hay không?
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                // trang đường dẫn không hợp lệ
                Response.StatusCode = 404;
                return null;
            }
            //Lấy giỏ hàng
            List<ItemGioHang> lstGioHang = LayGioHang();
            // trường hợp 1: sản phẩm đã tồn tại trong giỏ hàng
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck != null)
            {
                //Kiểm tra số lường tồn trước khi cho khách hàng mua hàng
                if (sp.SoLuongTon < spCheck.SoLuong)
                {
                    return View("View");
                }
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                return Redirect(strURL);
            }


            ItemGioHang itemGH = new ItemGioHang(MaSP);
            //Kiểm tra số lường tồn trước khi cho khách hàng mua hàng
            if (sp.SoLuongTon < itemGH.SoLuong)
            {
                return View("View");
            }
            lstGioHang.Add(itemGH);
            return Redirect(strURL);
        }
        // tính tổng số lượng
        public double TinhTongSoLuong()
        {
            //Lấy giỏ hàng
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.SoLuong);
        }
        // tính tổng tiền
        public decimal TinhTongTien()
        {
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.ThanhTien);
        }

        // GET: GioHang
        public ActionResult XemGioHang()
        {
            // lẤy Item Giỏ hàng
            List<ItemGioHang> lstGioHang = LayGioHang();
            ViewBag.TongTien = TinhTongTien();
            var TongChuaGiam = TinhTongTien();
            ViewBag.TongDaGiam = Convert.ToDouble(TongChuaGiam) - Convert.ToDouble(TongChuaGiam) * 0.1;

            return View(lstGioHang);



        }
        public ActionResult XemGioHangKQ()
        {
           

            return View();



        }


        // chỉnh sửa giỏ hàng
        [HttpGet]
        public ActionResult SuaGioHang(int MaSP)
        {
            // kiểm tra secction tồn tại chưa?
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index");
            }
            // Kiểm tra sản phẩm có tòn tại trong csdl hay không?
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                // trang đường dẫn không hợp lệ
                Response.StatusCode = 404;
                return null;
            }
            // Lấy list giỏ hàng từ session
            List<ItemGioHang> lstGioHang = LayGioHang();
            //kiểm tra sp có trong giỏ hàng hay k
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");

            }
            //lấy list giỏ hàng tạo giao diện
            ViewBag.GioHang = lstGioHang;


            // nếu tồn tại rồi
            return View(spCheck);
        }

        //Xử lý cập nhật giỏ hàng
        [HttpPost]
        public ActionResult CapNhatGioHang(ItemGioHang itemGH)
        {
            //Kiểm tra số lượng tồn
            SanPham spCheck = db.SanPhams.SingleOrDefault(n => n.MaSP == itemGH.MaSP);
            if (spCheck.SoLuongTon < itemGH.SoLuong)
            {
                return View("View");
            }
            // cập nhật số lượng trong session giỏ hàng
            // bước 1: lấy list<GioHang> từ sesstion["GioHang"]
            List<ItemGioHang> lstGH = LayGioHang();
            //bước 2: lấy sản phẩm cần cập nhật từ trong list<GIoHang>
            ItemGioHang itemGHUpdate = lstGH.Find(n => n.MaSP == itemGH.MaSP);
            // bước 3: tiến hành cập nhập lại số lượng và thành tiền
            itemGHUpdate.SoLuong = itemGH.SoLuong;
            itemGHUpdate.ThanhTien = itemGHUpdate.SoLuong * itemGHUpdate.DonGia;


            return RedirectToAction("XemGioHang");

        }
        public ActionResult GioHangPartial()
        {
            if (TinhTongSoLuong() == 0)
            {
                ViewBag.TongTien = 0;
                ViewBag.TongSoLuong = 0;
                return PartialView();
            }
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            return PartialView();
        }






        public ActionResult XoaGioHang(int MaSP)
        {
            // kiểm tra secction tồn tại chưa?
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            // Kiểm tra sản phẩm có tòn tại trong csdl hay không?
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                // trang đường dẫn không hợp lệ
                Response.StatusCode = 404;
                return null;
            }
            // Lấy list giỏ hàng từ session
            List<ItemGioHang> lstGioHang = LayGioHang();
            //kiểm tra sp có trong giỏ hàng hay k
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");

            }
            // Xoá item trong giỏ hàng
            lstGioHang.Remove(spCheck);
            return RedirectToAction("XemGioHang");
        }

        //xây dựng chức năng đặt hàng
        public ActionResult DatHang(KhachHang kh)
        {
            // kiểm tra secction tồn tại chưa?
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            KhachHang khang = new KhachHang();
            if (Session["LuuTaiKhoan"] == null)
            {
                // Thêm khách hàng vào bảng khách hàng với khách chưa có tài khoản              
                khang = kh;
                db.KhachHangs.Add(khang);
                db.SaveChanges();
            }
            else
            {
                // Đối với khách hàng là thành viên
                ThanhVien tv = Session["LuuTaiKhoan"] as ThanhVien;
                khang.TenKH = tv.Hoten;
                khang.DiaChi = tv.DiaChi;
                khang.Email = tv.Email;

                khang.SDT = tv.SDT;
                khang.MaThanhVien = tv.MaThanhVien;
                db.KhachHangs.Add(khang);
                db.SaveChanges();
            }

            // Thêm đơn hàng
            DonDatHang ddh = new DonDatHang();
            ddh.MaKH = khang.MaKH;
            ddh.NgayDat = DateTime.Now;
            ddh.TinhTrangGiaoHang = false;
            ddh.DaThanhToan = false;
            ddh.UuDai = 0;
            ddh.DaXoa = false;
            ddh.DaHuy = false;
            db.DonDatHangs.Add(ddh);
            db.SaveChanges();
            // thêm chi tiết dơn đạt hàng
            List<ItemGioHang> lstGH = LayGioHang();
            foreach (var item in lstGH)
            {
                ChiTietDDH chiTietDDH = new ChiTietDDH();
                chiTietDDH.MaDDH = ddh.MaDDH;
                chiTietDDH.MaSP = item.MaSP;
                chiTietDDH.TenSP = item.TenSP;
                chiTietDDH.SoLuong = item.SoLuong;
                chiTietDDH.DonGia = item.DonGia;
                db.ChiTietDDHs.Add(chiTietDDH);
            }
            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XemGioHangKQ");
        }


        // Thêm giỏ hàng Ajax
        public ActionResult ThemGioHangAjax(int MaSP, string strURL)
        {
            // Kiểm tra sản phẩm có tòn tại trong csdl hay không?
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                // trang đường dẫn không hợp lệ
                Response.StatusCode = 404;
                return null;
            }
            //Lấy giỏ hàng
            List<ItemGioHang> lstGioHang = LayGioHang();
            // trường hợp 1: sản phẩm đã tồn tại trong giỏ hàng
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            //spCheck.SoLuong = 0;
            if (spCheck != null)
            {
                //Kiểm tra số lường tồn trước khi cho khách hàng mua hàng
                if (sp.SoLuongTon < spCheck.SoLuong)
                {
                    return Content("<script> alert(\"Sản Phẩm đã hết hàng!\")</script>");
                }
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                ViewBag.TongSoLuong = TinhTongSoLuong();
                ViewBag.TongTien = TinhTongTien();

                return PartialView("GioHangPartial");
            }
            ItemGioHang itemGH = new ItemGioHang(MaSP);
            //Kiểm tra số lường tồn trước khi cho khách hàng mua hàng
            if (sp.SoLuongTon < itemGH.SoLuong)
            {
                return Content("<script> alert(\"Sản Phẩm đã hết hàng!\")</script>");
            }
            lstGioHang.Add(itemGH);
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            return PartialView("GioHangPartial");
        }

        


        public ActionResult GiamGia(ItemGioHang itemGH, string MaGG)
        {
           
            var magiamgia = db.MGGs.SingleOrDefault(n => n.MaGG == MaGG);
            ViewBag.TongDaGiam = 0;
            Session["MGG"] = magiamgia;
            if (magiamgia != null)
            {              
                List<ItemGioHang> lstGH = LayGioHang();                          
                var TongChuaGiam = TinhTongTien();
                ViewBag.TongDaGiam = Convert.ToDouble(TongChuaGiam) - Convert.ToDouble(TongChuaGiam) * magiamgia.trigia;
                
                return RedirectToAction("XemGioHang");
            }

            return Content("<script> alert(\"Không đúng mã giảm giá!\")</script>");
        }
















    }
}