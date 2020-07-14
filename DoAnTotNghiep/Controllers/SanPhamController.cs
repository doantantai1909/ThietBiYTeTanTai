using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using PagedList;
using System.Web.Mvc;
using DoAnTotNghiep.Models;


namespace DoAnTotNghiep.Controllers
{
   
    public class SanPhamController : Controller
    {
        // GET: SanPham
        QuanLiBanHang1Entities db = new QuanLiBanHang1Entities();
       [ChildActionOnly]
        public ActionResult SanPhamStyle1Partial()
        {
            return PartialView();
        }       
        [ChildActionOnly]
        public ActionResult SanPhamStyle2Partial()
        {
            return PartialView();
        }
        [ChildActionOnly]
        public ActionResult SanPhamBanChayPartial()
        {
            var lstSP = db.SanPhams.Where(n => n.SoLanMua > 300);
            //var lstSP = db.SanPhams.OrderBy(n=>n.SoLanMua);
            return PartialView(lstSP);
        }
        [ChildActionOnly]
        public ActionResult TinTucPartial()
        {
            var lstTT = db.TinTucs;         
            return PartialView(lstTT);
        }
        // Xâu dựng trang xem chi tiết
        public ActionResult XemChiTietSP(int? id, string tensp)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Response.Charset = "utf-8";
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp==null) 
            {
                return HttpNotFound();
            }
         
            return View(sp);
        }
        // Action load sản phẩm theo mã loại sản phẩm (hoặc mã nhà sản xuất)
        public ActionResult SanPhamTheoMaSP(int? MaLoaiSP, int? page)
        {
                 if (MaLoaiSP == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Load sản phẩm theo tiêu chí cũng Loại sản phẩm
            var lstSP = db.SanPhams.Where(n => n.MaLoaiSP == MaLoaiSP);
           // var lstSPrandom = db.SanPhams.Where(n => n.MaLoaiSP == MaLoaiSP).OrderBy(x => Guid.NewGuid()).Take(15);
           // ViewBag.lstSPrandom = lstSPrandom;


            if (lstSP.Count()==0)
            {
                //Thông báo nếu như không có sản phẩm
                return HttpNotFound();
            }
            // Thực hiện chức năng phân trang
            // tạo biến số sản phẩm trên trang
            if (Request.HttpMethod != "Get")
            {
                page = 1;
            } 
            int PageSize = 9;
            // Tạo biến số trang hiện tại
            int PageNumber = (page ?? 1);
            
            ViewBag.MaLoaiSP = MaLoaiSP;
            return View(lstSP.OrderBy(n=>n.MaSP).ToPagedList(PageNumber, PageSize));
        }

        public static List<T> RandomizeGenericList<T>(IList<T> originalList)
        {
            List<T> randomList = new List<T>();
            Random random = new Random();
            T value = default(T);

            //now loop through all the values in the list
            while (originalList.Count() > 0)
            {
                //pick a random item from th original list
                var nextIndex = random.Next(0, originalList.Count());
                //get the value for that random index
                value = originalList[nextIndex];
                //add item to the new randomized list
                randomList.Add(value);
                //remove value from original list (prevents
                //getting duplicates
                originalList.RemoveAt(nextIndex);
            }

            //return the randomized list
            return randomList;
        }
        [ChildActionOnly]
        public ActionResult SPtuongtu()
        {
             var lstSPrandom = db.SanPhams.OrderBy(x => Guid.NewGuid()).Take(12);
           // var lstSPTT = db.SanPhams.Where(n => n.MaLoaiSP == MaLoaiSP);
          

            return PartialView(lstSPrandom);
        }





















    }
}