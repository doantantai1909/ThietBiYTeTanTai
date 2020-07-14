using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnTotNghiep.Models;
using System.Net.Mail;

namespace DoAnTotNghiep.Controllers
{
    [Authorize(Roles = "QuanTri, QuanLyDonHang")]
    public class QuanLyDonHangController : Controller
    {
        // GET: QuanLyDonHang
        QuanLiBanHang1Entities db = new QuanLiBanHang1Entities();
        public ActionResult ChuaThanhToan()
        {
            // Lấy danh sách các đơn hàng chưa duyệt
            var list = db.DonDatHangs.Where(n => n.DaThanhToan == false).OrderBy(n => n.NgayDat);
            return View(list);
        }
        public ActionResult ChuaGiao()
        {
            // Lấy danh sách đơn hàng chưa giao
            var lstDSDHCG = db.DonDatHangs.Where(n => n.TinhTrangGiaoHang == true && n.DaThanhToan == false).OrderBy(n => n.NgayDat);
            
            
            return View(lstDSDHCG);
        }
        public ActionResult DaGiaoDaThanhToan()
        {
            // Lấy danh sách đơn hàng chưa giao
            var lstDSDHCG = db.DonDatHangs.Where(n => n.TinhTrangGiaoHang == true && n.DaThanhToan == true);
            return View(lstDSDHCG);
        }
        [HttpGet]
        public ActionResult DuyetDonHang(int? id)
        {
            // kiểm tra id hợp lệ không
            if (id ==null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            DonDatHang model = db.DonDatHangs.SingleOrDefault(n => n.MaDDH == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            // Lấy danh sách chi tiết đơn hàng dẻ hiển thị cho người dùng thấy
            var lstChiTietDH = db.ChiTietDDHs.Where(n => n.MaDDH == id);
            ViewBag.ListChiTietDH = lstChiTietDH;
            var TongChuaGiam = lstChiTietDH.Sum(n => n.DonGia);
            ViewBag.TongDaGiam = Convert.ToDouble(TongChuaGiam) - Convert.ToDouble(TongChuaGiam) * 0.1;
            return View(model);
        }
        [HttpPost]
        public ActionResult DuyetDonHang(DonDatHang ddh)
        {
            // truy van lay du lieu cua don hang do
            DonDatHang ddhUpdate = db.DonDatHangs.Single(n => n.MaDDH == ddh.MaDDH);
            ddhUpdate.DaThanhToan = ddh.DaThanhToan;
            ddhUpdate.TinhTrangGiaoHang = ddh.TinhTrangGiaoHang;
            db.SaveChanges();
            //lấy danh sách chi tiết đơn hàng để hiển thị cho người dùng thấy
            var lstChiTietDH = db.ChiTietDDHs.Where(n => n.MaDDH == ddh.MaDDH);
            ViewBag.ListChiTietDH = lstChiTietDH;
            // gửi khách hàng 1 mail đã xác nhận  việc thanh toán
            GuiEmail("Xác nhận đơn hàng của ThietBiYTeBinhDuong", "doantantai1909@gmail.com", "doantantai1909@gmail.com", "19091998zxcpoi", "Đơn hang của bạn đax được đặt thành công!");
             
            return View(ddhUpdate);
        }
        public void GuiEmail(string Title, string ToEmail, string FromEmail, string PassWord, string Content)
        {
            // goi email
            MailMessage mail = new MailMessage();
            mail.To.Add(ToEmail); // Địa chỉ nhận
            mail.From = new MailAddress(FromEmail); // Địa chửi gửi
            mail.Subject = Title; // tiêu đề gửi
            mail.Body = Content; // Nội dung
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com"; // host gửi của Gmail
            smtp.Port = 587; //port của Gmail
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential
            (FromEmail, PassWord);//Tài khoản password người gửi
            smtp.EnableSsl = true; //kích hoạt giao tiếp an toàn SSL
            smtp.Send(mail); //Gửi mail đi
        }
    }
}