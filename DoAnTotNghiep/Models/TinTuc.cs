//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DoAnTotNghiep.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TinTuc
    {
        public int MaTinTuc { get; set; }
        public string TieuDeTinTuc { get; set; }
        public string NoiDungTinTuc { get; set; }
        public Nullable<System.DateTime> NgayDang { get; set; }
        public Nullable<bool> DaXoa { get; set; }
        public string HinhAnh { get; set; }
        public string NoiDungQuangCao { get; set; }
        public Nullable<int> LuotXem { get; set; }
        public Nullable<int> LuotBinhChon { get; set; }
        public Nullable<int> LuotBinhLuan { get; set; }
    }
}
