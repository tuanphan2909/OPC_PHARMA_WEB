using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web4.Models;
using static web4.Controllers.SERVERController;
using static web4.Controllers.LoadDataController;

namespace web4.Controllers
{
    public class FormBkNopTienNHController : Controller
    {
        SqlConnection con = new SqlConnection();
        SqlCommand sqlc = new SqlCommand();
        SqlDataReader dt;
        public ActionResult Index()
        {
            DataSet ds = new DataSet();
            SqlConnectionHelper.ConnectSQL(con);

            string Ma_DvCs = Request.Cookies["MA_DVCS"].Value;
          
            string Pname = "DanhSachBangKeNopTienNH";


            using (SqlCommand cmd = new SqlCommand(Pname, con))
            {
                cmd.CommandTimeout = 950;

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;


                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {

                    cmd.Parameters.AddWithValue("@_Ma_Dvcs", Ma_DvCs);

                    sda.Fill(ds);

                }
            }


            return View(ds);
        }
        public ActionResult MauInBangKe()
        {
            DataSet ds = new DataSet();
            SqlConnectionHelper.ConnectSQL(con);

            string Dvcs = Request.Cookies["MA_DVCS"].Value;
            string Pname = "BangKeNopTienNH_SAP";
            string SoBK = Request.QueryString["soBK"];
            using (SqlCommand cmd = new SqlCommand(Pname, con))
            {
                cmd.CommandTimeout = 950;

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;


                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {

                    cmd.Parameters.AddWithValue("@_So_Ct", SoBK);
                    cmd.Parameters.AddWithValue("@_Ma_Dvcs", Dvcs);
                    sda.Fill(ds);

                }
            }


            return View(ds);

        }
        public ActionResult InsertBangKeNH()
        {
            List<BangKeNopTien> dmDlistTDV = LoadData.LoadDmTDVBK(Request);

            ViewBag.DataTDV = dmDlistTDV;

            return View();
        }
        public ActionResult InsertBangKeNHLoadHD()
        {
            List<BangKeNopTien> dmDlistTDV = LoadData.LoadDmTDVBK(Request);
            List<BangKeNopTien> dmListHD = LoadHD();
            ViewBag.DataTDV = dmDlistTDV;
            ViewBag.DataHD = dmListHD;
            return View();
        }
        public List<BangKeNopTien> LoadHD()
        {
            string ma_dvcs = Request.Cookies["MA_DVCS"] != null ? Request.Cookies["MA_DVCS"].Value : "";
            string Ma_cbnv = Request.Cookies["Ma_NVGH"] != null ? Request.Cookies["Ma_NVGH"].Value : "";
            SqlConnectionHelper.ConnectSQL(con);

            List<BangKeNopTien> dataItems = new List<BangKeNopTien>();

            using (SqlConnection connection = new SqlConnection(con.ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("[usp_RpCongNoTDV_SAP]", connection))
                {
                    command.CommandTimeout = 950;
                    command.CommandType = CommandType.StoredProcedure;

                    // command.Parameters.AddWithValue("@_Ma_Dvcs", ma_dvcs);
                    command.Parameters.AddWithValue("@_Ma_CbNv", Ma_cbnv);


                    using (SqlDataAdapter sda = new SqlDataAdapter(command))
                    {
                        DataSet ds = new DataSet();
                        sda.Fill(ds);

                        // Kiểm tra xem DataSet có bảng dữ liệu hay không
                        if (ds.Tables.Count > 0)
                        {
                            DataTable dt = ds.Tables[0];

                            foreach (DataRow row in dt.Rows)
                            {
                                BangKeNopTien dataItem = new BangKeNopTien
                                {
                                    So_CT1 = row["so_ct"].ToString(),
                                    Ngay_HD = row["Ngay_Ct"].ToString(),
                                    So_HD = row["So_HD"].ToString(),
                                    Ma_Dt = row["Ma_dt"].ToString(),
                                    Ten_Dt = row["Ten_Dt"].ToString(),
                                    Tien_Truoc_Thue = string.IsNullOrEmpty(row["Tien_Truoc_Thue"].ToString()) ? 0 : float.Parse(row["Tien_Truoc_Thue"].ToString()),
                                    Tien_HD = string.IsNullOrEmpty(row["Cong_No"].ToString()) ? 0 : float.Parse(row["Cong_No"].ToString()),
                                    Thue = string.IsNullOrEmpty(row["Tien_Thue"].ToString()) ? 0 : float.Parse(row["Tien_Thue"].ToString()),
                                    Tien_CKTT = string.IsNullOrEmpty(row["Tien_CKTT"].ToString()) ? 0 : float.Parse(row["Tien_CKTT"].ToString()),
                                    Tien_Phai_Thu = (decimal)(string.IsNullOrEmpty(row["Tien_Phai_Thu"].ToString()) ? 0 : float.Parse(row["Tien_Phai_Thu"].ToString())),
                                    Ngay_Den_Han = row["Ngay_Den_Han"].ToString()

                                };

                                dataItems.Add(dataItem);
                            }
                        }
                    }
                }
            }

            return dataItems;
        }
        public ActionResult SaveHD(BangKeNopTien BK)
        {
            BK.Dvcs = Request.Cookies["MA_DVCS"] != null ? Request.Cookies["MA_DVCS"].Value : "";
            BK.Tong_Tien = Request.Cookies["Tong_Tien"] != null ? Request.Cookies["Tong_Tien"].Value : "";

            string result = "Error!";
            SqlConnectionHelper.ConnectSQL(con);
            if (BK != null && BK.Details != null)
            {
                try
                {
                    var detailsTable = new DataTable();
                    detailsTable.Columns.Add("So_HD", typeof(string));
                    detailsTable.Columns.Add("So_CT", typeof(string));
                    detailsTable.Columns.Add("Ngay_HD", typeof(DateTime));
                    detailsTable.Columns.Add("Ngay_Den_Han", typeof(DateTime));
                    detailsTable.Columns.Add("Bien_Lai", typeof(string));
                    detailsTable.Columns.Add("Ma_Dt", typeof(string));
                    detailsTable.Columns.Add("Ten_Dt", typeof(string));
                    detailsTable.Columns.Add("Tien_Truoc_Thue", typeof(float));
                    detailsTable.Columns.Add("Tien_HD", typeof(float));
                    detailsTable.Columns.Add("Tien_CKTT", typeof(float));
                    detailsTable.Columns.Add("Thue", typeof(float));
                    detailsTable.Columns.Add("Tien_Phai_Thu", typeof(decimal));
                    detailsTable.Columns.Add("Tien_Con_Lai", typeof(decimal));

                    foreach (var detail in BK.Details)
                    {
                        if (detail != null)
                        {
                            DateTime ngayHD;
                            DateTime ngayDenHan;

                            // Chuyển đổi chuỗi ngày thành DateTime
                            if (!DateTime.TryParseExact(detail.Ngay_HD, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out ngayHD))
                            {
                                throw new Exception("Ngày HD không đúng định dạng.");
                            }

                            if (!DateTime.TryParseExact(detail.Ngay_Den_Han, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out ngayDenHan))
                            {
                                throw new Exception("Ngày đến hạn không đúng định dạng.");
                            }

                            detailsTable.Rows.Add(detail.So_HD, detail.So_CT, ngayHD, ngayDenHan, detail.Bien_Lai, detail.Ma_Dt, detail.Ten_Dt, detail.Tien_Truoc_Thue, detail.Tien_HD, detail.Tien_CKTT, detail.Thue, detail.Tien_Phai_Thu, detail.Tien_Con_Lai);
                        }
                    }

                    using (var connection = new SqlConnection(con.ConnectionString))
                    {
                        connection.Open();

                        using (var command = new SqlCommand("InsertBangKeNopTienNH", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            command.Parameters.AddWithValue("@_Ngay_Nop_Tien", BK.Ngay_Nop_Tien);
                            command.Parameters.AddWithValue("@_So_BK", BK.So_BK);
                            command.Parameters.AddWithValue("@_Ma_TDV", BK.Ma_TDV);
                            command.Parameters.AddWithValue("@_Ten_TDV", BK.Ten_TDV);
                            command.Parameters.AddWithValue("@_Tong_Tien", BK.Tong_Tien);
                            command.Parameters.AddWithValue("@_Dvcs", BK.Dvcs);
                            command.Parameters.AddWithValue("@_Noi_Dung", BK.Noi_Dung ?? "");
                            command.Parameters.AddWithValue("@_Nguoi_Nop_Tien", BK.Nguoi_Nop_Tien ?? "");

                            // Pass details as a TVP parameter
                            var detailsParam = command.Parameters.AddWithValue("@_Details", detailsTable);
                            detailsParam.SqlDbType = SqlDbType.Structured;
                            detailsParam.TypeName = "B30BKNTNHdetailType"; // Replace with your actual type name

                            command.ExecuteNonQuery();

                            result = "Success! Đã Lưu";
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions appropriately
                    result = $"Error! {ex.Message}";
                }
            }

            if (result.StartsWith("Success"))
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // Trả về lỗi với JSON để dễ dàng hiển thị trên client
                return new HttpStatusCodeResult(500, result);
            }
        }
        public ActionResult UpdateBangKeNH()
        {
            List<BangKeNopTien> dmDlistTDV = LoadData.LoadDmTDVBK(Request);
            List<BangKeNopTien> dmListHD = LoadHDUpdate();
            ViewBag.DataTDV = dmDlistTDV;
            ViewBag.DataHD = dmListHD;

            DataSet ds = new DataSet();
            SqlConnectionHelper.ConnectSQL(con);

            string Pname = "[EditBangKeNopTienNH]";
            string Stt = Request.QueryString["Stt"];

            using (SqlCommand cmd = new SqlCommand(Pname, con))
            {
                cmd.CommandTimeout = 950;

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;


                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {

                    cmd.Parameters.AddWithValue("@_Stt", Stt);
                    sda.Fill(ds);

                }
            }


            return View(ds);
        }
        public List<BangKeNopTien> LoadHDUpdate()
        {
            string ma_dvcs = Request.Cookies["MA_DVCS"] != null ? Request.Cookies["MA_DVCS"].Value : "";
            string Ma_cbnv = Request.Cookies["Ma_CbNv"] != null ? Request.Cookies["Ma_CbNv"].Value : "";
            SqlConnectionHelper.ConnectSQL(con);

            List<BangKeNopTien> dataItems = new List<BangKeNopTien>();

            using (SqlConnection connection = new SqlConnection(con.ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("[usp_RpCongNoTDV_SAP]", connection))
                {
                    command.CommandTimeout = 950;
                    command.CommandType = CommandType.StoredProcedure;

                    // command.Parameters.AddWithValue("@_Ma_Dvcs", ma_dvcs);
                    command.Parameters.AddWithValue("@_Ma_CbNv", Ma_cbnv);


                    using (SqlDataAdapter sda = new SqlDataAdapter(command))
                    {
                        DataSet ds = new DataSet();
                        sda.Fill(ds);

                        // Kiểm tra xem DataSet có bảng dữ liệu hay không
                        if (ds.Tables.Count > 0)
                        {
                            DataTable dt = ds.Tables[0];

                            foreach (DataRow row in dt.Rows)
                            {
                                BangKeNopTien dataItem = new BangKeNopTien
                                {
                                    So_CT1 = row["so_ct"].ToString(),
                                    Ngay_HD = row["Ngay_Ct"].ToString(),
                                    So_HD = row["So_HD"].ToString(),
                                    Ma_Dt = row["Ma_dt"].ToString(),
                                    Ten_Dt = row["Ten_Dt"].ToString(),
                                    Tien_Truoc_Thue = string.IsNullOrEmpty(row["Tien_Truoc_Thue"].ToString()) ? 0 : float.Parse(row["Tien_Truoc_Thue"].ToString()),
                                    Tien_HD = string.IsNullOrEmpty(row["Cong_No"].ToString()) ? 0 : float.Parse(row["Cong_No"].ToString()),
                                    Thue = string.IsNullOrEmpty(row["Tien_Thue"].ToString()) ? 0 : float.Parse(row["Tien_Thue"].ToString()),
                                    Tien_CKTT = string.IsNullOrEmpty(row["Tien_CKTT"].ToString()) ? 0 : float.Parse(row["Tien_CKTT"].ToString()),
                                    Tien_Phai_Thu = (decimal)(string.IsNullOrEmpty(row["Tien_Phai_Thu"].ToString()) ? 0 : float.Parse(row["Tien_Phai_Thu"].ToString())),
                                    Ngay_Den_Han = row["Ngay_Den_Han"].ToString()

                                };

                                dataItems.Add(dataItem);
                            }
                        }
                    }
                }
            }

            return dataItems;
        }
        public ActionResult UpdateBangKeNHLoadHD()
        {
            List<BangKeNopTien> dmDlistTDV = LoadData.LoadDmTDVBK(Request);
            List<BangKeNopTien> dmListHD = LoadHDUpdate();
            ViewBag.DataTDV = dmDlistTDV;
            ViewBag.DataHD = dmListHD;
            return View();
        }
        public ActionResult SaveUpdate(BangKeNopTien BK)
        {



            BK.Dvcs = Request.Cookies["MA_DVCS"] != null ? Request.Cookies["MA_DVCS"].Value : "";
            BK.Tong_Tien = Request.Cookies["Tong_Tien"] != null ? Request.Cookies["Tong_Tien"].Value : "";
            var stt = Request.Cookies["Stt"].Value;

            string result = "Error!";
            SqlConnectionHelper.ConnectSQL(con);
            if (BK != null && BK.Details != null)
            {
                try
                {
                    var detailsTable = new DataTable();
                    detailsTable.Columns.Add("So_HD", typeof(string));
                    detailsTable.Columns.Add("So_CT", typeof(string));
                    detailsTable.Columns.Add("Ngay_HD", typeof(DateTime));
                    detailsTable.Columns.Add("Ngay_Den_Han", typeof(DateTime));
                    detailsTable.Columns.Add("Bien_Lai", typeof(string));
                    detailsTable.Columns.Add("Ma_Dt", typeof(string));
                    detailsTable.Columns.Add("Ten_Dt", typeof(string));
                    detailsTable.Columns.Add("Tien_Truoc_Thue", typeof(float));
                    detailsTable.Columns.Add("Tien_HD", typeof(float));
                    detailsTable.Columns.Add("Tien_CKTT", typeof(float));
                    detailsTable.Columns.Add("Thue", typeof(float));
                    detailsTable.Columns.Add("Tien_Phai_Thu", typeof(decimal));
                    detailsTable.Columns.Add("Tien_Con_Lai", typeof(decimal));


                    foreach (var detail in BK.Details)
                    {
                        if (detail != null)
                        {
                            DateTime ngayHD;
                            DateTime ngayDenHan;

                            // Chuyển đổi chuỗi ngày thành DateTime
                            if (!DateTime.TryParseExact(detail.Ngay_HD, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out ngayHD))
                            {
                                throw new Exception("Ngày HD không đúng định dạng.");
                            }

                            if (!DateTime.TryParseExact(detail.Ngay_Den_Han, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out ngayDenHan))
                            {
                                throw new Exception("Ngày đến hạn không đúng định dạng.");
                            }

                            detailsTable.Rows.Add(detail.So_HD, detail.So_CT, ngayHD, ngayDenHan, detail.Bien_Lai, detail.Ma_Dt, detail.Ten_Dt, detail.Tien_Truoc_Thue, detail.Tien_HD, detail.Tien_CKTT, detail.Thue, detail.Tien_Phai_Thu, detail.Tien_Con_Lai);
                        }
                    }

                    using (var connection = new SqlConnection(con.ConnectionString))
                    {
                        connection.Open();

                        using (var command = new SqlCommand("UpdateBangKeNopTienNH", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            command.Parameters.AddWithValue("@_Ngay_Ct", BK.Ngay_Nop_Tien);
                            command.Parameters.AddWithValue("@_So_BK", BK.So_BK);
                            command.Parameters.AddWithValue("@_Ma_TDV", BK.Ma_TDV);
                            command.Parameters.AddWithValue("@_Ten_TDV", BK.Ten_TDV);
                            command.Parameters.AddWithValue("@_Tong_Tien", BK.Tong_Tien);
                            command.Parameters.AddWithValue("@_Stt", stt);
                            //command.Parameters.AddWithValue("@_Dvcs", BK.Dvcs);
                            command.Parameters.AddWithValue("@_Noi_Dung", BK.Noi_Dung ?? "");
                            command.Parameters.AddWithValue("@_Nguoi_Nop_Tien", BK.Nguoi_Nop_Tien ?? "");



                            // Pass details as a TVP parameter
                            var detailsParam = command.Parameters.AddWithValue("@_Details", detailsTable);
                            detailsParam.SqlDbType = SqlDbType.Structured;
                            detailsParam.TypeName = "B30BKNTNHdetailType"; // Replace with your actual type name

                            command.ExecuteNonQuery();

                            result = "Success! Đã Sửa";
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions appropriately
                    result = $"Error! {ex.Message}";
                }

            }
            if (result.StartsWith("Success"))
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // Trả về lỗi với JSON để dễ dàng hiển thị trên client
                return new HttpStatusCodeResult(500, result);
            }

        }
        public ActionResult Delete(BangKeNopTien BK)
        {


            string result = "Error!";
            SqlConnectionHelper.ConnectSQL(con);
            try
            {
                using (var connection = new SqlConnection(con.ConnectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand("Delete_BangKeNopTienNH", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@_STT", BK.Stt);



                        command.ExecuteNonQuery();

                        result = "Success! Đã Xoá";
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                result = $"Error! {ex.Message}";
            }


            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}