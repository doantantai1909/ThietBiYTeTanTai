using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnTotNghiep.Models;
namespace DoAnTotNghiep.Controllers
{
    [Authorize(Roles = "QuanTri")]
    public class PhanQuyenController : Controller
    {
        // GET: QuanTri/PhanQuyen
        QuanLiBanHang1Entities db = new QuanLiBanHang1Entities();

        public ActionResult IndexPhanQuyen()
        {
            return View(db.LoaiThanhViens.OrderBy(n => n.TenLoai));
        }
        [HttpGet]
        public ActionResult PhanQuyen(int? id)
        {
            // lấy mã loại tv dựa vào id
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }


            LoaiThanhVien ltv = db.LoaiThanhViens.SingleOrDefault(n => n.MaLoaiTV == id);
            if (ltv == null)
            {
                return HttpNotFound();
            }
            // Lấy ra danh sách quyền
            ViewBag.MaQuyen = db.Quyens;
            ViewBag.LoaiTVQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == id);
            return View(ltv);
        }
        [HttpPost]
        public ActionResult PhanQuyen(int? MaLoaiTV, IEnumerable<LoaiThanhVien_Quyen> lstPhanQuyen)
        {

            var lstDaPhanQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == MaLoaiTV);
            if (lstDaPhanQuyen != null)
            {
                db.LoaiThanhVien_Quyen.RemoveRange(lstDaPhanQuyen);
                db.SaveChanges();
            }
            // kiểm tra list danh sách quyền đã check
            foreach (var item in lstPhanQuyen)
            {
                item.MaLoaiTV = int.Parse(MaLoaiTV.ToString());
                db.LoaiThanhVien_Quyen.Add(item);
            }
            db.SaveChanges();
            return RedirectToAction("IndexPhanQuyen");
        }

        public ActionResult DanhSachQuyen()
        {
            return View(db.Quyens.OrderBy(n => n.TenQuyen));
        }
        [HttpGet]
        public ActionResult ThemQuyen()
        {


            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ThemQuyen(Quyen q)
        {
            db.Quyens.Add(q);
            db.SaveChanges();
            return RedirectToAction("DanhSachQuyen");

        }
        [HttpGet]
        public ActionResult SuaQuyen(string id)
        {
            //lấy quyền theo id ra để chỉnh sửa
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            Quyen q = db.Quyens.SingleOrDefault(n => n.MaQuyen == id);
            if (q == null)
            {
                return HttpNotFound();
            }

            return View(q);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SuaQuyen(Quyen model)
        {


            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("DanhSachQuyen");
        }

        [HttpGet]
        public ActionResult XoaQuyen(string id)
        {
            //lấy sản phẩm theo id ra để chỉnh sửa
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            Quyen q = db.Quyens.SingleOrDefault(n => n.MaQuyen == id);
            if (q == null)
            {
                return HttpNotFound();
            }

            return View(q);
        }
        [HttpPost]
        public ActionResult XoaDV(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Quyen model = db.Quyens.SingleOrDefault(n => n.MaQuyen == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            db.Quyens.Remove(model);
            db.SaveChanges();
            return RedirectToAction("DanhSachQuyen");
        }






    }
}