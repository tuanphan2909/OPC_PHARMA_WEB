using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web4.Models
{
    public class BangKeNopTien
    {
        public string So_BK { get; set; }
        public List<B30BKNT_Detail> Details { get; set; }
        public DateTime Ngay_Nop_Tien { get; set; }
        public string Dvcs { get; set; }
        public string Ma_TDV { get; set; }
        public string Ten_TDV { get; set; }
        public string So_HD { get; set; }
        public string Tong_Tien { get; set; }
        //public DateTime Ngay_HD { get; set; }
        public string Noi_Dung { get; set; }
        public string Nguoi_Nop_Tien { get; set; }
        public string Stt { get; set; }
        public string So_CT1 { get; set; }
        public string Ngay_HD { get; set; }
        public string Ma_Dt { get; set; }
        public string Ten_Dt { get; set; }
        public float Tien_CKTT { get; set; }
        public float Tien_Truoc_Thue { get; set; }
        public float Tien_HD { get; set; }
        public decimal Tien_Phai_Thu { get; set; }
        public float Thue { get; set; }
        public decimal Tien_Con_Lai { get; set; }
        public string Ngay_Den_Han { get; set; }
        public string Bien_Lai { get; set; }





    }
}