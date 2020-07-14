using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnTotNghiep.Models;
namespace DoAnTotNghiep.Controllers
{
    [Authorize(Roles = "QuanTri, QuanLyWebsite")]
    public class QuanLyWebsiteController : Controller
    {
        // GET: QuanLyWebsite
        QuanLiBanHang1Entities db = new QuanLiBanHang1Entities();
        public ActionResult QuanLyTinTuc()
        {
            
            return View(db.TinTucs);
        }
        public ActionResult AdminTKTT(string sTuKhoaTT)
        {
            //tìm kiếm theo tên sp
            var lstTT = db.TinTucs.Where(n => n.TieuDeTinTuc.Contains(sTuKhoaTT));
            ViewBag.TuKhoa = sTuKhoaTT;
            return PartialView(lstTT);
        }
        [HttpGet]
        public ActionResult ThemTinTuc()
        {
           
          
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ThemTinTuc(TinTuc tt, HttpPostedFileBase HinhAnh)
        {
          
            // kiểm tra hình ảnh đã có trong csdl chưa
            if (HinhAnh.ContentLength > 0)
            {
                // lấy hình ảnh
                var fileName = Path.GetFileName(HinhAnh.FileName);
                // lấy ảnh chuyển vào thư mục hình ảnh
                var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                // kiểm tra coi tồn tại hay chưa
               
                    // lưu hình
                    HinhAnh.SaveAs(path);
                    tt.HinhAnh = fileName;               
            }
            db.TinTucs.Add(tt);
            db.SaveChanges();
            return RedirectToAction("QuanLyTinTuc");
        }

        [HttpGet]
        public ActionResult ChinhSuaTT(int? id)
        {
            //lấy sản phẩm theo id ra để chỉnh sửa
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            TinTuc tt = db.TinTucs.SingleOrDefault(n => n.MaTinTuc == id);
            if (tt == null)
            {
                return HttpNotFound();
            }
         
            return View(tt);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSuaTT(TinTuc model, HttpPostedFileBase[] HinhAnh)
        {
            
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
            return RedirectToAction("QuanLyTinTuc");
        }
        [HttpGet]
        public ActionResult XoaTT(int? id)
        {
            //lấy sản phẩm theo id ra để chỉnh sửa
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            TinTuc tt = db.TinTucs.SingleOrDefault(n => n.MaTinTuc == id);
            if (tt == null)
            {
                return HttpNotFound();
            }
         
            return View(tt);
        }
        [HttpPost]
        public ActionResult XoaTT(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            TinTuc model = db.TinTucs.SingleOrDefault(n => n.MaTinTuc == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            db.TinTucs.Remove(model);
            db.SaveChanges();
            return RedirectToAction("QuanLyTinTuc");
        }


        public ActionResult QuanLyTuyenDung()
        {

            return View(db.TuyenDungs);
        }
        public ActionResult AdminTKTD(string sTuKhoaTD)
        {
            //tìm kiếm theo tên sp
            var lstTD = db.TuyenDungs.Where(n => n.TieuDeTuyenDung.Contains(sTuKhoaTD));
            ViewBag.TuKhoaTD = sTuKhoaTD;
            return PartialView(lstTD);
        }

        [HttpGet]
        public ActionResult ThemTD()
        {


            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ThemTD(TuyenDung td, HttpPostedFileBase HinhAnh)
        {

            // kiểm tra hình ảnh đã có trong csdl chưa
            if (HinhAnh.ContentLength > 0)
            {
                // lấy hình ảnh
                var fileName = Path.GetFileName(HinhAnh.FileName);
                // lấy ảnh chuyển vào thư mục hình ảnh
                var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                // kiểm tra coi tồn tại hay chưa

                // lưu hình
                HinhAnh.SaveAs(path);
                td.HinhAnh = fileName;
            }
            db.TuyenDungs.Add(td);
            db.SaveChanges();
            return RedirectToAction("QuanLyTuyenDung");
        }

        [HttpGet]
        public ActionResult ChinhSuaTD(int? id)
        {
            //lấy sản phẩm theo id ra để chỉnh sửa
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            TuyenDung td = db.TuyenDungs.SingleOrDefault(n => n.MaTuyenDung == id);
            if (td == null)
            {
                return HttpNotFound();
            }

            return View(td);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSuaTD(TuyenDung model, HttpPostedFileBase[] HinhAnh)
        {

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
            return RedirectToAction("QuanLyTuyenDung");
        }

        [HttpGet]
        public ActionResult XoaTD(int? id)
        {
            //lấy sản phẩm theo id ra để chỉnh sửa
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            TuyenDung td = db.TuyenDungs.SingleOrDefault(n => n.MaTuyenDung == id);
            if (td == null)
            {
                return HttpNotFound();
            }

            return View(td);
        }
        [HttpPost]
        public ActionResult XoaTD(int id)
        {
            if (id == null) 
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            TuyenDung model = db.TuyenDungs.SingleOrDefault(n => n.MaTuyenDung == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            db.TuyenDungs.Remove(model);
            db.SaveChanges();
            return RedirectToAction("QuanLyTuyenDung");
        }

        public ActionResult QuanLyGT()
        {
          
            return View(db.GioiThieux);
        }
        public ActionResult AdminTKGT(string sTuKhoaGT)
        {
            //tìm kiếm theo tên sp
            var lstGT = db.GioiThieux.Where(n => n.TieuDeGT.Contains(sTuKhoaGT));
            ViewBag.TuKhoaGT = sTuKhoaGT;
            return PartialView(lstGT);
        }
        [HttpGet]
        public ActionResult ThemGT()
        {


            ViewBag.MaLoaiGT = new SelectList(db.LoaiGioiThieux.OrderBy(n => n.MaLoaiGT), "MaLoaiGT", "TenLoaiGT");

            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ThemGT(GioiThieu gt, HttpPostedFileBase HinhAnh)
        {
            ViewBag.MaLoaiGT = new SelectList(db.LoaiGioiThieux.OrderBy(n => n.MaLoaiGT), "MaLoaiGT", "TenLoaiGT");


            // kiểm tra hình ảnh đã có trong csdl chưa
            if (HinhAnh.ContentLength > 0)
            {
                // lấy hình ảnh
                var fileName = Path.GetFileName(HinhAnh.FileName);
                // lấy ảnh chuyển vào thư mục hình ảnh
                var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                // kiểm tra coi tồn tại hay chưa

                // lưu hình
                HinhAnh.SaveAs(path);
                gt.HinhAnh = fileName;
            }
         
            db.GioiThieux.Add(gt);
            db.SaveChanges();
            return RedirectToAction("QuanLyGT");
        }

        [HttpGet]
        public ActionResult ChinhSuaGT(int? id)
        {
            //lấy sản phẩm theo id ra để chỉnh sửa
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            GioiThieu gt = db.GioiThieux.SingleOrDefault(n => n.MaGioiThieu == id);
            if (gt == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoaiGT = new SelectList(db.LoaiGioiThieux.OrderBy(n => n.MaLoaiGT), "MaLoaiGT", "TenLoaiGT", gt.MaLoaiGT);

            return View(gt);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSuaGT(GioiThieu model, HttpPostedFileBase[] HinhAnh)
        {

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
            ViewBag.MaLoaiGT = new SelectList(db.LoaiGioiThieux.OrderBy(n => n.MaLoaiGT), "MaLoaiGT", "TenLoaiGT", model.MaLoaiGT);


            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("QuanLyGT");
        }

        [HttpGet]
        public ActionResult XoaGT(int? id)
        {
            //lấy sản phẩm theo id ra để chỉnh sửa
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            GioiThieu gt = db.GioiThieux.SingleOrDefault(n => n.MaGioiThieu == id);
            if (gt == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoaiGT = new SelectList(db.LoaiGioiThieux.OrderBy(n => n.MaLoaiGT), "MaLoaiGT", "TenLoaiGT", gt.MaLoaiGT);

            return View(gt);
        }
        [HttpPost]
        public ActionResult XoaGT(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            GioiThieu model = db.GioiThieux.SingleOrDefault(n => n.MaGioiThieu == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            db.GioiThieux.Remove(model);
            db.SaveChanges();
            return RedirectToAction("QuanLyGT");
        }





        public ActionResult QuanLyDichVu()
        {

            return View(db.DichVus);
        }
        public ActionResult AdminTKDV(string sTuKhoaDV)
        {
            //tìm kiếm theo tên sp
            var lstDV = db.DichVus.Where(n => n.TieuDeDichVu.Contains(sTuKhoaDV));
            ViewBag.TuKhoaDV = sTuKhoaDV;
            return PartialView(lstDV);
        }
        [HttpGet]
        public ActionResult ThemDV()
        {
            ViewBag.MaLoaiDV = new SelectList(db.LoaiDichVus.OrderBy(n => n.MaLoaiDV), "MaLoaiDV", "TenLoaiDV");

            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ThemDV(DichVu dv, HttpPostedFileBase HinhAnh)
        {
            ViewBag.MaLoaiDV = new SelectList(db.LoaiDichVus.OrderBy(n => n.MaLoaiDV), "MaLoaiDV", "TenLoaiDV");
            // kiểm tra hình ảnh đã có trong csdl chưa
            if (HinhAnh.ContentLength > 0)
            {
                // lấy hình ảnh
                var fileName = Path.GetFileName(HinhAnh.FileName);
                // lấy ảnh chuyển vào thư mục hình ảnh
                var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                // kiểm tra coi tồn tại hay chưa

                // lưu hình
                HinhAnh.SaveAs(path);
                dv.HinhAnh = fileName;
            }
            db.DichVus.Add(dv);
            db.SaveChanges();
            return RedirectToAction("QuanLyDichVu");
        }

        [HttpGet]
        public ActionResult ChinhSuaDV(int? id)
        {
            //lấy sản phẩm theo id ra để chỉnh sửa
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            DichVu dv = db.DichVus.SingleOrDefault(n => n.MaDichVu == id);
            if (dv == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoaiDV = new SelectList(db.LoaiDichVus.OrderBy(n => n.MaLoaiDV), "MaLoaiDV", "TenLoaiDV", dv.MaLoaiDV);

            return View(dv);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSuaDV(DichVu model, HttpPostedFileBase[] HinhAnh)
        {

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
            ViewBag.MaLoaiDV = new SelectList(db.LoaiDichVus.OrderBy(n => n.MaLoaiDV), "MaLoaiDV", "TenLoaiDV", model.MaLoaiDV);


            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("QuanLyDichVu");
        }

        [HttpGet]
        public ActionResult XoaDV(int? id)
        {
            //lấy sản phẩm theo id ra để chỉnh sửa
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            DichVu dv = db.DichVus.SingleOrDefault(n => n.MaDichVu == id);
            if (dv == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoaiDV = new SelectList(db.LoaiDichVus.OrderBy(n => n.MaLoaiDV), "MaLoaiDV", "TenLoaiDV", dv.MaLoaiDV);

            return View(dv);
        }
        [HttpPost]
        public ActionResult XoaDV(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            DichVu model = db.DichVus.SingleOrDefault(n => n.MaDichVu == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            db.DichVus.Remove(model);
            db.SaveChanges();
            return RedirectToAction("QuanLyDichVu");
        }

        public ActionResult QuanLyChinhSach()
        {
            return View(db.ChinhSaches);
        }
        [HttpGet]
        public ActionResult ChinhSuaCS(int? id)
        {
            //lấy sản phẩm theo id ra để chỉnh sửa
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ChinhSach cs = db.ChinhSaches.SingleOrDefault(n => n.MaChinhSach == id);
            if (cs == null)
            {
                return HttpNotFound();
            }
          
            return View(cs);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSuaCS(ChinhSach model)
        {
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("QuanLyChinhSach");
        }
    }
}