using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using web4.Models;

namespace web4.Controllers
{
    public class DanhMucKhachHangTDVController : Controller
    {
        private readonly string connectionString = "Data Source=118.69.109.109;Initial Catalog=SAP_OPC;User ID=OPC;Password=OpcaBc@135#";

        // GET: DanhMucKhachHangTDV
        public ActionResult Index()
        {
            List<DmKhachHang> dmKhachHangs = new List<DmKhachHang>();

            // Lấy giá trị MaCbNv từ cookie
            string maCbNv = Request.Cookies["Ma_CbNv"]?.Value;

            if (!string.IsNullOrEmpty(maCbNv))
            {
                dmKhachHangs = GetDmKhachHangData(maCbNv);
            }

            return View(dmKhachHangs);
        }

        private List<DmKhachHang> GetDmKhachHangData(string maCbNv)
        {
            List<DmKhachHang> dmKhachHangs = new List<DmKhachHang>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("usp_dmdtTDVHD_SAP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@_Ma_CbNv", maCbNv);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DmKhachHang kh = new DmKhachHang
                            {
                                Ma_Dt = reader["Ma_Dt"].ToString(),
                                Ten_Dt = reader["Ten_Dt"].ToString(),
                                DiaChi = reader["Dia_Chi"].ToString(),
                                So_Dien_Thoai = reader["Dien_Thoai"].ToString(),
                                So_Hop_Dong = reader["So_Hop_Dong"].ToString(),
                                MST = reader["MST"].ToString(),
                                Doanh_Thu_Nam = reader.IsDBNull(reader.GetOrdinal("Doanh_Thu_Nam")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Doanh_Thu_Nam"))
                            };
                            dmKhachHangs.Add(kh);
                        }
                    }
                }
            }

            return dmKhachHangs;
        }
    }
}
