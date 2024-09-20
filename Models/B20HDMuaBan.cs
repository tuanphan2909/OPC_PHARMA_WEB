using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using web4.Controllers;

namespace web4.Models
{
    public class B20HDMuaBan
    {
        public int Id { get; set; }
        public string Ma_Dvcs { get; set; }
        [Required(ErrorMessage = "Mã Đối Tượng là bắt buộc.")]
        public string Ma_Dt { get; set; }
        public string Ten_Dt { get; set; }
        public string Dia_Chi { get; set; }
        [Required(ErrorMessage = "Mã Trình Dược Viên là bắt buộc.")]
        public string Ma_TDV { get; set; }
        public string Ten_TDV { get; set; }
        public string Loai_KH { get; set; }
        [Required(ErrorMessage = "Payment ID - Text là bắt buộc.")]
        public string Payment_ID { get; set; }
        public string Payment_Text { get; set; }
        [Required(ErrorMessage = "CK là bắt buộc")]
        public string CK { get; set; }
        public string MST { get; set; }
        public string Ma_Dai_Dien { get; set; }
        public string Ten_Dai_Dien { get; set; }
        public string So_Dien_Thoai { get; set; }
        public DateTime? Ngay_Ky { get; set; }

        [Required(ErrorMessage = "Số Hợp Đồng là bắt buộc")]
        public string So_Hop_Dong { get; set; }
        public string Ky_Hieu_HD { get; set; }
        public int Hop_Dong_VIP { get; set; } = 0;
        public decimal Doanh_Thu_Nam { get; set; } = 0;
        public decimal Doanh_Thu_Q1 { get; set; } = 0;
        public decimal Doanh_Thu_Q2 { get; set; } = 0;
        public decimal Doanh_Thu_Q3 { get; set; } = 0;
        public decimal Doanh_Thu_Q4 { get; set; } = 0;
        public string TG_Ky_HD { get; set; }
        public string TG_Th_HD { get; set; }
        public int Isactive { get; set; } = 1;
        public string SelectedBranch { get; set; }
        public List<MauInChungTu> DataItemsDt { get; internal set; }
        public List<BKHoaDonGiaoHang> DataItemsTdv { get; internal set; }
        public List<PaymentData> PaymentData { get; internal set; }
        public List<KeyValuePair<string, string>> CKData { get; internal set; } // Change this line



    }

}