using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnTotNghiep.Models;
namespace DoAnTotNghiep.Controllers
{
    public class SubmitListModelController : Controller
    {
        // GET: SubmitListModel
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(PhieuNhap pn, IEnumerable<ChiTietPN> ModelList)
        {
            return View();
        }
    }
}