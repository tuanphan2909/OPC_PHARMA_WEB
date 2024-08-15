using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web4.Models;

using static web4.Controllers.SERVERController;

namespace web4.Controllers
{
    public class LoadDataController : Controller
    {
        // GET: LoadData
        private static SqlConnection con = new SqlConnection();
        public static class LoadData
        {

            public static List<MauInChungTu> LoadDmDt(string Ma_dvcs, HttpRequestBase request)
            {
                SqlConnectionHelper.ConnectSQL(con);

                Ma_dvcs = request.Cookies["ma_dvcs"].Value;
                List<MauInChungTu> dataItems = new List<MauInChungTu>();
                string appendedString = Ma_dvcs == "OPC" ? "_B1_0120" : "_01"; // Dòng này cộng chuỗi dựa trên giá trị của Ma_dvcs
                using (SqlConnection connection = new SqlConnection(con.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("[usp_DmDtTdv_SAP_MauIn]", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@_Ma_Dvcs", Ma_dvcs + appendedString);
                        command.CommandTimeout = 950;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MauInChungTu dataItem = new MauInChungTu
                                {
                                    Ma_Dt = reader["ma_dt"].ToString(),
                                    Ten_Dt = reader["ten_dt"].ToString(),
                                    Dia_Chi = reader["Dia_Chi"].ToString(),
                                    Dvcs = reader["Dvcs"].ToString(),
                                    Dvcs1 = reader["Dvcs1"].ToString()
                                };
                                dataItems.Add(dataItem);
                            }
                        }
                    }
                }

                return dataItems;
            }
            public static List<MauInChungTu> LoadDmDt2(HttpRequestBase request)
            {
                SqlConnectionHelper.ConnectSQL(con);

                var Ma_dvcs = request.Cookies["ma_dvcs_2"].Value;
                List<MauInChungTu> dataItems = new List<MauInChungTu>();
                string appendedString = Ma_dvcs == "OPC" ? "B1_0120" : "_01"; // Dòng này cộng chuỗi dựa trên giá trị của Ma_dvcs
                using (SqlConnection connection = new SqlConnection(con.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("[usp_DmDtTdv_SAP_MauIn]", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@_Ma_Dvcs", Ma_dvcs + appendedString);
                        command.CommandTimeout = 950;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MauInChungTu dataItem = new MauInChungTu
                                {
                                    Ma_Dt = reader["ma_dt"].ToString(),
                                    Ten_Dt = reader["ten_dt"].ToString(),
                                    Dia_Chi = reader["Dia_Chi"].ToString(),
                                    Dvcs = reader["Dvcs"].ToString(),
                                    Dvcs1 = reader["Dvcs1"].ToString()
                                };
                                dataItems.Add(dataItem);
                            }
                        }
                    }
                }

                return dataItems;
            }

            //KH cap 2
            public static List<CTVCap2> LoadDmDtCap2(string Ma_dvcs, HttpRequestBase request)
            {
                SqlConnectionHelper.ConnectSQL(con);

                Ma_dvcs = request.Cookies["ma_dvcs"].Value;
                List<CTVCap2> dataItems = new List<CTVCap2>();
                string appendedString = Ma_dvcs == "OPC_B1" ? "_010203" : "_01"; // Dòng này cộng chuỗi dựa trên giá trị của Ma_dvcs
                using (SqlConnection connection = new SqlConnection(con.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("[DanhMucKhachHangCap2]", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@_Ma_Dvcs", Ma_dvcs);
                        command.CommandTimeout = 950;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CTVCap2 dataItem = new CTVCap2
                                {

                                    Ma_Dt = reader["Ma_Dt"].ToString(),
                                    Ten_Dt = reader["Ten_Dt"].ToString(),
                                    Dvcs = reader["Dvcs"].ToString()

                                };
                                dataItems.Add(dataItem);
                            }
                        }
                    }
                }

                return dataItems;
            }
            public  static List<BKHoaDonGiaoHang> LoadDmTDV(HttpRequestBase request)
            {
                string ma_dvcs = request.Cookies["MA_DVCS"] != null ? request.Cookies["MA_DVCS"].Value : "";
                SqlConnectionHelper.ConnectSQL(con);
                if (string.IsNullOrEmpty(ma_dvcs))
                {
                    return null; // Trả về null nếu ma_dvcs rỗng
                }
                List<BKHoaDonGiaoHang> dataItems = new List<BKHoaDonGiaoHang>();

                using (SqlConnection connection = new SqlConnection(con.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("[usp_DanhSachTDV]", connection))
                    {
                        command.CommandTimeout = 950;
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@_Ma_Dvcs", ma_dvcs);

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
                                    BKHoaDonGiaoHang dataItem = new BKHoaDonGiaoHang
                                    {
                                        Ma_CbNv = row["Ma_CbNv"].ToString(),
                                        hoten = row["hoten"].ToString(),
                                        Ma_Dvcs = row["Ma_Dvcs"].ToString()
                                    };

                                    dataItems.Add(dataItem);
                                }
                            }
                        }
                    }
                }

                return dataItems;
            }

            public static List<BangKeNopTien> LoadDmTDVBK(HttpRequestBase request)
            {
                string ma_dvcs = request.Cookies["MA_DVCS"] != null ? request.Cookies["MA_DVCS"].Value : "";
                SqlConnectionHelper.ConnectSQL(con);

                List<BangKeNopTien> dataItems = new List<BangKeNopTien>();

                using (SqlConnection connection = new SqlConnection(con.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("[usp_DanhSachTDV]", connection))
                    {
                        command.CommandTimeout = 950;
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@_Ma_Dvcs", ma_dvcs);

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
                                        Ma_TDV = row["Ma_CbNv"].ToString(),
                                        Ten_TDV = row["hoten"].ToString(),
                                        Dvcs = row["Ma_Dvcs"].ToString()
                                    };

                                    dataItems.Add(dataItem);
                                }
                            }
                        }
                    }
                }

                return dataItems;
            }
            public static List<BKHoaDonGiaoHang> LoadDmTDV2(HttpRequestBase request)
            {
                string ma_dvcs = request.Cookies["MA_DVCS_2"] != null ? request.Cookies["MA_DVCS_2"].Value : "";
                SqlConnectionHelper.ConnectSQL(con);
                if (string.IsNullOrEmpty(ma_dvcs))
                {
                    return null; // Trả về null nếu ma_dvcs rỗng
                }
                List<BKHoaDonGiaoHang> dataItems = new List<BKHoaDonGiaoHang>();

                using (SqlConnection connection = new SqlConnection(con.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("[usp_DanhSachTDV]", connection))
                    {
                        command.CommandTimeout = 950;
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@_Ma_Dvcs", ma_dvcs);

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
                                    BKHoaDonGiaoHang dataItem = new BKHoaDonGiaoHang
                                    {
                                        Ma_CbNv = row["Ma_CbNv"].ToString(),
                                        hoten = row["hoten"].ToString(),
                                        Ma_Dvcs = row["Ma_Dvcs"].ToString()
                                    };

                                    dataItems.Add(dataItem);
                                }
                            }
                        }
                    }
                }

                return dataItems;
            }
            public  static List<BKHoaDonGiaoHang> LoadDmVt()
            {

                SqlConnectionHelper.ConnectSQL(con);

                List<BKHoaDonGiaoHang> dataItems = new List<BKHoaDonGiaoHang>();

                using (SqlConnection connection = new SqlConnection(con.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("[usp_DanhMucVt_SAP]", connection))
                    {
                        command.CommandTimeout = 950;
                        command.CommandType = CommandType.StoredProcedure;
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
                                    BKHoaDonGiaoHang dataItem = new BKHoaDonGiaoHang
                                    {
                                        Ma_Vt = row["Ma_Vt"].ToString(),
                                        Ten_Vt = row["Ten_Vt"].ToString(),
                                        Gia = row["Gia"].ToString()
                                    };

                                    dataItems.Add(dataItem);
                                }
                            }
                        }
                    }
                }

                return dataItems;
            }
            public static List<MauInChungTu> LoadDmSalesUnit()
            {
                SqlConnectionHelper.ConnectSQL(con);

                List<MauInChungTu> dataItems = new List<MauInChungTu>();

                using (SqlConnection connection = new SqlConnection(con.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("[usp_DanhMucSalesUnit_SAP]", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 950;

                        using (SqlDataAdapter sda = new SqlDataAdapter(command))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);

                            if (ds.Tables.Count > 0)
                            {
                                DataTable dt = ds.Tables[0];

                                foreach (DataRow row in dt.Rows)
                                {
                                    MauInChungTu dataItem = new MauInChungTu
                                    {
                                        Ma_Unit = row["Ma_Unit"].ToString(),
                                        Ten_Sales_Unit = row["Ten_Sales_Unit"].ToString()
                                    };
                                    dataItems.Add(dataItem);
                                }
                            }
                        }
                    }
                }

                return dataItems;
            }
        

        public static List<Kho> LoadDmKho()
            {

                SqlConnectionHelper.ConnectSQL(con);

                List<Kho> dataItems = new List<Kho>();

                using (SqlConnection connection = new SqlConnection(con.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("[usp_DanhMucKho_SAP]", connection))
                    {
                        command.CommandTimeout = 950;
                        command.CommandType = CommandType.StoredProcedure;
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
                                    Kho dataItem = new Kho
                                    {
                                        Ma_Kho = row["Ma_Kho"].ToString(),
                                        Ten_Kho = row["Ten_Kho"].ToString(),
                                        Dvcs = row["Dvcs"].ToString(),
                                        Logistics_Area = row["Logistics_Area_ID"].ToString()
                                    };

                                    dataItems.Add(dataItem);
                                }
                            }
                        }
                    }
                }

                return dataItems;
            }
        }
    
    }
}