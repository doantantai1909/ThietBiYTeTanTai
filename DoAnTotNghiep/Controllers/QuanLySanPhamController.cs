using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnTotNghiep.Models;

namespace DoAnTotNghiep.Controllers
{
    [Authorize(Roles = "QuanTri, QuanLySanPham")]
    public class QuanLySanPhamController : Controller
    {
        QuanLiBanHang1Entities db = new QuanLiBanHang1Entities();
        // GET: QuanLySanPham
      
        public ActionResult QuanLySanPham()
        {

            return View(db.SanPhams.Where(n => n.DaXoa == false).OrderByDescending(n => n.MaSP));
        }
        
        public ActionResult KQTKAdminPartial(string sTuKhoa)
        {
            //tìm kiếm theo tên sp
            var lstSP = db.SanPhams.Where(n => n.TenSP.Contains(sTuKhoa));
            ViewBag.TuKhoa = sTuKhoa;
            return PartialView(lstSP);
        }
        [HttpGet]
      
        public ActionResult ThemSanPham()
        {
            //Load dropdownlist NCC
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.TenLoai), "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ThemSanPham(SanPham sp, HttpPostedFileBase HinhAnh, HttpPostedFileBase HinhAnh2, HttpPostedFileBase HinhAnh3, HttpPostedFileBase HinhAnh4)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.TenLoai), "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
            // kiểm tra hình ảnh đã có trong csdl chưa
            if (HinhAnh.ContentLength > 0)
            {
                // lấy hình ảnh
                var fileName = Path.GetFileName(HinhAnh.FileName);              
                // lấy ảnh chuyển vào thư mục hình ảnh
                var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                // kiểm tra coi tồn tại hay chưa
                if (System.IO.File.Exists(path))
                {
                    ViewBag.uploadHinh = "Hình đã tồn tại";
                    return View();
                }
                else
                {
                    // lưu hình
                    HinhAnh.SaveAs(path);
                    sp.HinhAnh = fileName;                  
                }
            }
            sp.DaXoa = false;
            db.SanPhams.Add(sp);
            db.SaveChanges();
            return RedirectToAction("QuanLySanPham");
            //// kiểm tra hình ảnh đã có trong csdl chưa
            // if (HinhAnh2.ContentLength > 0)
            // {
            //     // lấy hình ảnh
            //     var fileName2 = Path.GetFileName(HinhAnh2.FileName);
            //     // lấy ảnh chuyển vào thư mục hình ảnh
            //     var path2 = Path.Combine(Server.MapPath("~/Content/images"), fileName2);
            //     // kiểm tra coi tồn tại hay chưa             
            //         // lưu hình
            //         HinhAnh2.SaveAs(path2);
            //         sp.HinhAnh2 = fileName2;                
            // }
            //// kiểm tra hình ảnh đã có trong csdl chưa
            // if (HinhAnh3.ContentLength > 0)
            // {
            //     // lấy hình ảnh
            //     var fileName3 = Path.GetFileName(HinhAnh3.FileName);

            //     // lấy ảnh chuyển vào thư mục hình ảnh
            //     var path3 = Path.Combine(Server.MapPath("~/Content/images"), fileName3);

            //         // lưu hình
            //         HinhAnh.SaveAs(path3);
            //         sp.HinhAnh3 = fileName3;               
            // }
            // // kiểm tra hình ảnh đã có trong csdl chưa
            // if (HinhAnh4.ContentLength > 0)
            // {
            //     // lấy hình ảnh
            //     var fileName4 = Path.GetFileName(HinhAnh.FileName);

            //     // lấy ảnh chuyển vào thư mục hình ảnh
            //     var path4 = Path.Combine(Server.MapPath("~/Content/images"), fileName4);
            //     // kiểm tra coi tồn tại hay chưa              
            //         // lưu hình
            //         HinhAnh.SaveAs(path4);
            //         sp.HinhAnh4 = fileName4;               
            // }

        }
        [HttpGet]
       
        public ActionResult ChinhSua(int? id)
        {
            //lấy sản phẩm theo id ra để chỉnh sửa
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
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.TenLoai), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.TenNSX), "MaNSX", "TenNSX", sp.MaNSX);
            return View(sp);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSua(SanPham model, HttpPostedFileBase[] HinhAnh)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", model.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.TenLoai), "MaLoaiSP", "TenLoai", model.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.TenNSX), "MaNSX", "TenNSX", model.MaNSX);
            if (HinhAnh[0].ContentLength > 0)
            {
                // lấy hình ảnh
                var fileName = Path.GetFileName(HinhAnh[0].FileName);

                // lấy ảnh chuyển vào thư mục hình ảnh
                var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                // lưu hình
                HinhAnh[0].SaveAs(path);
                model.HinhAnh = fileName;
            }
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("QuanLySanPham");
        }
        [HttpGet]
       
        public ActionResult Xoa(int? id)
        {
            //lấy sản phẩm theo id ra để chỉnh sửa
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
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.TenLoai), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.TenNSX), "MaNSX", "TenNSX", sp.MaNSX);
            return View(sp);
        }
        [HttpPost]
        public ActionResult Xoa(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            SanPham model = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            db.SanPhams.Remove(model);
            db.SaveChanges();
            return RedirectToAction("QuanLySanPham");
        }

      
        public ActionResult QuanLyLoaiSanPham()
        {

            return View(db.LoaiSanPhams);
        }
        public ActionResult AdminTKLoaiSP(string sTuKhoaLSP)
        {
            //tìm kiếm theo tên sp
            var lstLSP = db.LoaiSanPhams.Where(n => n.TenLoai.Contains(sTuKhoaLSP));
            ViewBag.TuKhoaLSP = sTuKhoaLSP;
            return PartialView(lstLSP);
        }
       
        [HttpGet]
        [Authorize(Roles = "QuanLySanPham")]
        public ActionResult ThemLoaiSP()
        {
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ThemLoaiSP(LoaiSanPham lsp)
        {


            db.LoaiSanPhams.Add(lsp);
            db.SaveChanges();
            return RedirectToAction("QuanLySanPham");
        }

        [HttpGet]
        
        public ActionResult ChinhSuaLSP(int? id)
        {
            //lấy sản phẩm theo id ra để chỉnh sửa
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            LoaiSanPham lsp = db.LoaiSanPhams.SingleOrDefault(n => n.MaLoaiSP == id);
            if (lsp == null)
            {
                return HttpNotFound();
            }
           
            return View(lsp);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSuaLSP(LoaiSanPham model)
        {
           
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("QuanLyLoaiSanPham");
        }
        [HttpGet]
      
        public ActionResult XoaLSP(int? id)
        {
            //lấy sản phẩm theo id ra để chỉnh sửa
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            LoaiSanPham lsp = db.LoaiSanPhams.SingleOrDefault(n => n.MaLoaiSP == id);
            if (lsp == null)
            {
                return HttpNotFound();
            }

            return View(lsp);
        }
        [HttpPost]
        public ActionResult XoaLSP(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            LoaiSanPham model = db.LoaiSanPhams.SingleOrDefault(n => n.MaLoaiSP == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            db.LoaiSanPhams.Remove(model);
            db.SaveChanges();
            return RedirectToAction("QuanLyLoaiSanPham");
        }



    }
}