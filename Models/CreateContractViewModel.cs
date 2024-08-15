using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web4.Models
{
    public class CreateContractViewModel
    {
        public string SelectedBranch { get; set; }
        public List<MauInChungTu> Ma_Dts { get; set; }
        public List<BKHoaDonGiaoHang> Ma_TDVs { get; set; }
    }
}