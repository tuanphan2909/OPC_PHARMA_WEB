using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using web4.Models;
using System.Drawing;
using OfficeOpenXml.Drawing;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using OfficeOpenXml.Table;
using Newtonsoft.Json;
using System.Globalization;
using static web4.Controllers.SERVERController;
namespace web4.Controllers
{
    public class ViengThamController : Controller
    {
        // GET: ViengTham
        SqlConnection con = new SqlConnection();
        SqlCommand sqlc = new SqlCommand();
   
      
        public ActionResult QuanLyTrungBay_Fill()
        {
            return View();
        }
        public ActionResult QuanLyTrungBay()
        {
            string ma_dvcs ;
            var fromDate = Request.Cookies["From_date"].Value;
            var toDate = Request.Cookies["To_Date"].Value;
            if (Request.Cookies["Ma_Dvcs_2"]!=null)
            {
                ma_dvcs = Request.Cookies["Ma_Dvcs_2"].Value;
            }
            else
            {
                ma_dvcs = Request.Cookies["MA_DVCS"].Value;
            }
       
            DataSet ds = new DataSet();
       
            SqlConnectionHelper.ConnectSQL(con);
      

            string Pname = "[Usp_BaoCaoTrungBay]";

            using (SqlCommand cmd = new SqlCommand(Pname, con))
            {
                cmd.CommandTimeout = 950;

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.AddWithValue("@_Tu_Ngay", fromDate);
                    cmd.Parameters.AddWithValue("@_Den_Ngay", toDate);
                    cmd.Parameters.AddWithValue("@_ma_dvcs", ma_dvcs);
                    sda.Fill(ds);

                }
            }
            return View(ds);
        }
        public class DetailViewModel
        {
            public List<string> ImageSources { get; set; }
            public DataSet DataSet { get; set; }
        }

        public ActionResult QuanLyTrungBay_Detail()
        {
            List<string> imageSources = new List<string>();
            var fromDate = Request.Cookies["From_date"].Value;
            var toDate = Request.Cookies["To_Date"].Value;
            var MaDt = Request.Cookies["Ma_Dt"].Value;
            DataSet ds = new DataSet();
            SqlConnectionHelper.ConnectSQL(con);

            string Pname = "[Usp_BaoCaoTrungBay]";

            using (SqlCommand cmd = new SqlCommand(Pname, con))
            {
                cmd.CommandTimeout = 950;

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.AddWithValue("@_Tu_Ngay", fromDate);
                    cmd.Parameters.AddWithValue("@_Den_Ngay", toDate);
                    cmd.Parameters.AddWithValue("@_Ma_Dt", MaDt);
                    sda.Fill(ds);
                }

                // Lấy danh sách hình ảnh từ DataSet
                foreach (DataRow row in ds.Tables[1].Rows)
                {
                    string base64String = row["Hinh_Anh"].ToString();
                    string imgSrc = $"data:image/GIF;base64,{base64String}";
                    imageSources.Add(imgSrc);
                }
            }

            // Tạo đối tượng ViewModel và gán dữ liệu
            var viewModel = new DetailViewModel
            {
                ImageSources = imageSources,
                DataSet = ds
            };

            return View(viewModel);
        }
        [HttpPost]
        public JsonResult UpdateTrangThai(string id, bool trangThai)
        {
            try
            {
                SqlConnectionHelper.ConnectSQL(con);
                string query = "UPDATE B20SPT SET Dong_y_Tra_Thuong = @trangThai WHERE  STT= @id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id",id);
                cmd.Parameters.AddWithValue("@trangThai", trangThai ? 1 : 0);
                con.Open();
                int result = cmd.ExecuteNonQuery();
                con.Close();

                return Json(new { success = result > 0 });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


    }
}