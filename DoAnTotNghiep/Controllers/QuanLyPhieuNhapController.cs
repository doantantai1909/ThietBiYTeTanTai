using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnTotNghiep.Models;

namespace DoAnTotNghiep.Controllers
{
    [Authorize(Roles = "QuanTri")]
    public class QuanLyPhieuNhapController : Controller
    {
        QuanLiBanHang1Entities db = new QuanLiBanHang1Entities();
        // GET: QuanLyPhieuNhap
        [HttpGet]
        public ActionResult IndexNhapHang()
        {
            //ViewBag.MaNCC = db.NhaCungCaps;
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.ListSP = db.SanPhams;
            return View();
        }
        [HttpPost]
        public ActionResult IndexNhapHang(PhieuNhap model, IEnumerable<ChiTietPN> lstModel)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.ListSP = db.SanPhams;
            // gán DaXoa bằng false
            model.DaXoa = false;
            db.PhieuNhaps.Add(model);
            db.SaveChanges();
            SanPham sp;
            foreach (var item in lstModel)
            {
                //cập nhập số lượng tồn
                sp = db.SanPhams.Single(n => n.MaSP == item.MaSP);
                sp.SoLuongTon += item.SoLuongNhap;
                sp.DonGia = item.DonGiaNhap;
                // gán mã phiếu nhập cho all cgi tiết phiéu nhập
                item.MaPN = model.MaPN;
            }
            db.ChiTietPNs.AddRange(lstModel);
            db.SaveChanges();
            return View();
        }
        [HttpGet]
        public ActionResult DSSPHetHang()
        {
          
            var lstSP = db.SanPhams.Where(n => n.DaXoa == false && n.SoLuongTon <= 5);
           
                return View(lstSP);

        }
        [HttpGet]
        public ActionResult NhapHangDon(int? id)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            return View(sp);
        }
        [HttpPost]
        public ActionResult NhapHangDon(PhieuNhap model, ChiTietPN ctpn)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", model.MaNCC);
            model.NgayNhap = DateTime.Now;
            model.DaXoa = false;
            db.PhieuNhaps.Add(model);
            db.SaveChanges();
            ctpn.MaPN = model.MaPN;
            SanPham sp = db.SanPhams.Single(n => n.MaSP == ctpn.MaSP);
            sp.SoLuongTon += ctpn.SoLuongNhap;
            db.ChiTietPNs.Add(ctpn);
            db.SaveChanges();
            return View();
        }
    }
}