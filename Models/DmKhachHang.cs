using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web4.Models
{
    public class DmKhachHang
    {
        public string Ma_Dt { get; set; }

        public string  Ten_Dt { get; set; }
        public string DiaChi { get; set; }

        public string MST { get; set; }
        public string So_Dien_Thoai { get; set; }
        public string  So_Hop_Dong { get; set; }

        public decimal Doanh_Thu_Nam { get; set; }
    }
}