using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnTotNghiep.Models;

namespace DoAnTotNghiep.Controllers
{
    [Authorize(Roles = "QuanTri, QuanLyDoiTac")]
    public class QuanLyDoiTacController : Controller
    {
        // GET: QuanLyDoiTac
        QuanLiBanHang1Entities db = new QuanLiBanHang1Entities();
        public ActionResult QuanLyNSX()
        {

            return View(db.NhaSanXuats);
        }      
        public ActionResult AdminTKNSX(string sTuKhoaNSX)
        {
            //tìm kiếm theo tên sp
            var lstNSX = db.NhaSanXuats.Where(n => n.TenNSX.Contains(sTuKhoaNSX));
            ViewBag.TuKhoaNSX = sTuKhoaNSX;
            return PartialView(lstNSX);
        }      
        [HttpGet]
      
        public ActionResult ThemNSX()
        {
            return View();
        }
   
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ThemNSX(NhaSanXuat nsx, HttpPostedFileBase Logo)
        {          
            // kiểm tra hình ảnh đã có trong csdl chưa
            if (Logo.ContentLength > 0)
            {
                // lấy hình ảnh
                var fileNameNSX = Path.GetFileName(Logo.FileName);
                // lấy ảnh chuyển vào thư mục hình ảnh
                var path = Path.Combine(Server.MapPath("~/Content/images"), fileNameNSX);
                // kiểm tra coi tồn tại hay chưa
                if (System.IO.File.Exists(path))
                {
                    ViewBag.uploadHinhNSX = "Hình đã tồn tại";
                    return View();
                }
                else
                {
                    // lưu hình
                    Logo.SaveAs(path);
                    nsx.Logo = fileNameNSX;
                }
            }
            // kiểm tra hình ảnh đã có trong csdl chưa
          
            db.NhaSanXuats.Add(nsx);
            db.SaveChanges();
            return RedirectToAction("QuanLyNSX");
        }
        [HttpGet]
        public ActionResult ChinhSuaNSX(int? id)
        {
            //lấy sản phẩm theo id ra để chỉnh sửa
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            NhaSanXuat nsx = db.NhaSanXuats.SingleOrDefault(n => n.MaNSX == id);
            if (nsx == null)
            {
                return HttpNotFound();
            }
           
            return View(nsx);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSuaNSX(NhaSanXuat model, HttpPostedFileBase[] Logo)
        {
           
            if (Logo[0].ContentLength > 0)
            {
                // lấy hình ảnh
                var fileNameNSX = Path.GetFileName(Logo[0].FileName);

                // lấy ảnh chuyển vào thư mục hình ảnh
                var path = Path.Combine(Server.MapPath("~/Content/images"), fileNameNSX);
                // lưu hình
                Logo[0].SaveAs(path);
                model.Logo = fileNameNSX;
            }
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("QuanLyNSX");
        }
        [HttpGet]
        public ActionResult XoaNSX(int? id)
        {
            //lấy sản phẩm theo id ra để chỉnh sửa
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            NhaSanXuat nsx = db.NhaSanXuats.SingleOrDefault(n => n.MaNSX == id);
            if (nsx == null)
            {
                return HttpNotFound();
            }
           
            return View(nsx);
        }
        [HttpPost]
        public ActionResult XoaNSX(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            NhaSanXuat model = db.NhaSanXuats.SingleOrDefault(n => n.MaNSX == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            db.NhaSanXuats.Remove(model);
            db.SaveChanges();
            return RedirectToAction("QuanLyNSX");
        }











        public ActionResult QuanLyNCC()
       {

            return View(db.NhaCungCaps);
        }
        public ActionResult AdminTKNCC(string sTuKhoaNCC)
        {
            //tìm kiếm theo tên sp
            var lstNCC = db.NhaCungCaps.Where(n => n.TenNCC.Contains(sTuKhoaNCC));
            ViewBag.TuKhoaNCC = sTuKhoaNCC;
            return PartialView(lstNCC);
        }
        [HttpGet]
        public ActionResult ThemNCC()
        {
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ThemNCC(NhaCungCap ncc)
        {
           

            db.NhaCungCaps.Add(ncc);
            db.SaveChanges();
            return RedirectToAction("QuanLyNCC");
        }


        [HttpGet]
        public ActionResult ChinhSuaNCC(int? id)
        {
            //lấy sản phẩm theo id ra để chỉnh sửa
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            NhaCungCap ncc = db.NhaCungCaps.SingleOrDefault(n => n.MaNCC == id);
            if (ncc == null)
            {
                return HttpNotFound();
            }

            return View(ncc);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSuaNCC(NhaCungCap model)
        {

          
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("QuanLyNCC");
        }
        [HttpGet]
        public ActionResult XoaNCC(int? id)
        {
            //lấy sản phẩm theo id ra để chỉnh sửa
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            NhaCungCap ncc = db.NhaCungCaps.SingleOrDefault(n => n.MaNCC == id);
            if (ncc == null)
            {
                return HttpNotFound();
            }

            return View(ncc);
        }
        [HttpPost]
        public ActionResult XoaNCC(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            NhaCungCap model = db.NhaCungCaps.SingleOrDefault(n => n.MaNCC == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            db.NhaCungCaps.Remove(model);
            db.SaveChanges();
            return RedirectToAction("QuanLyNCC");
        }
    }
}