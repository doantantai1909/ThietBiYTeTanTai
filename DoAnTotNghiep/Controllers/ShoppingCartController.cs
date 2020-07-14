using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using DoAnTotNghiep.Models;

namespace DoAnTotNghiep.Controllers
{
    public class ShoppingCartController : Controller
    {
        QuanLiBanHang1Entities db = new QuanLiBanHang1Entities();
        private string strCart = "Cart";
        // GET: ShoppingCart
        public ActionResult Index()
        {
            return View();
        }
    }
}