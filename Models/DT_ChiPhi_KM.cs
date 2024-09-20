using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web4.Models
{
    public class DT_ChiPhi_KM
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public string Ma_Dvcs { get; set; }  // varchar(10)
        public DateTime Ngay_Ct { get; set; }  // smalldatetime
        public string So_Ct { get; set; }  // nvarchar(128)
        public string Ma_CTKM { get; set; }  // nvarchar(50)
        public string Ten_CTKM { get; set; }  // nvarchar(MAX)
        public decimal? Doanh_Thu { get; set; }  // numeric(18, 2)
        public decimal? CPKM { get; set; }  // numeric(18, 2)
        public decimal Ty_Le { get; set; }  // numeric(18, 2)
        public DateTime Create_At { get; set; }  // smalldatetime
        public DateTime? Edit_At { get; set; }  // smalldatetime (nullable)
        public bool Active { get; set; }  // bit
     
    }
}