using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web4.Models;
using System.Web.Mvc;
using System.Net;
using System.Data.SqlClient;
using System.Data;
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
using static web4.Controllers.LoadDataController;
namespace web4.Controllers
{
    public class BienBanKiemNhapHangController : Controller
    {
        // GET: BienBanKiemNhapHang
        SqlConnection con = new SqlConnection();
        SqlCommand sqlc = new SqlCommand();
     
   
        //public  List<Kho> LoadDmKho()
        //{

        //    SqlConnectionHelper.ConnectSQL(con);

        //    List<Kho> dataItems = new List<Kho>();

        //    using (SqlConnection connection = new SqlConnection(con.ConnectionString))
        //    {
        //        connection.Open();

        //        using (SqlCommand command = new SqlCommand("[usp_DanhMucKho_SAP]", connection))
        //        {
        //            command.CommandTimeout = 950;
        //            command.CommandType = CommandType.StoredProcedure;
        //            using (SqlDataAdapter sda = new SqlDataAdapter(command))
        //            {
        //                DataSet ds = new DataSet();
        //                sda.Fill(ds);

        //                // Kiểm tra xem DataSet có bảng dữ liệu hay không
        //                if (ds.Tables.Count > 0)
        //                {
        //                    DataTable dt = ds.Tables[0];

        //                    foreach (DataRow row in dt.Rows)
        //                    {
        //                        Kho dataItem = new Kho
        //                        {
        //                            Ma_Kho = row["Ma_Kho"].ToString(),
        //                            Ten_Kho = row["Ten_Kho"].ToString(),
        //                            Dvcs = row["Dvcs"].ToString(),
        //                            Logistics_Area = row["Logistics_Area_ID"].ToString()
        //                        };

        //                        dataItems.Add(dataItem);
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return dataItems;
        //}
        public ActionResult BienBanKiemKeNhapHang_Fill()
        {
            List<Kho> dmDlistKho = LoadDataController.LoadData.LoadDmKho();
            ViewBag.DataKho = dmDlistKho;
            List<BKHoaDonGiaoHang> dmDlistVT = LoadDataController.LoadData.LoadDmVt();
            ViewBag.DataVT = dmDlistVT;
            return View();
        }
        public ActionResult BienBanKiemKeNhapHang_In()
        {
            List<Kho> dmDlistKho = LoadDataController.LoadData.LoadDmKho();
            ViewBag.DataKho = dmDlistKho;
            var fromDate = Request.Cookies["From_date"].Value;
            var toDate = Request.Cookies["To_Date"].Value;
            DataSet ds = new DataSet();
            SqlConnectionHelper.ConnectSQL(con);
            var Ma_Vt = Request.Cookies["Ma_Vt"].Value;
            var Ma_Kho = Request.Cookies["Ma_Kho"].Value;
            var Ma_DV = Request.Cookies["Ma_DV"].Value;
            //string query = "exec usp_Vth_BC_BHCNTK_CN @_ngay_Ct1 = '" + Acc.From_date + "',@_Ngay_Ct2 ='"+ Acc.To_date+"',@_Ma_Dvcs='"+ Acc.Ma_DvCs_1+"'";
            string Pname = "[usp_BienBanKiemKeNhapHang_SAP]";

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
                    cmd.Parameters.AddWithValue("@_ma_dvcs", Ma_DV);
                    cmd.Parameters.AddWithValue("@_Ma_Kho", Ma_Kho);
                    cmd.Parameters.AddWithValue("@_Ma_Vt", Ma_Vt);
                    sda.Fill(ds);

                }
            }
            return View(ds);
           
        }
        private void FormatCell(ExcelRangeBase cell)
        {
            cell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            // Các định dạng khác của ô có thể thêm vào tại đây
        }
        public List<BienBanKiemNhapHang> LoadData()
        {
            SqlConnectionHelper.ConnectSQL(con);
            List<BienBanKiemNhapHang> dataItems = new List<BienBanKiemNhapHang>();
            using (SqlConnection connection = new SqlConnection(con.ConnectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("[usp_BienBanKiemKeNhapHang_SAP]", connection))
                {
                    var fromDate = Request.Cookies["From_date"].Value;
                    var toDate = Request.Cookies["To_Date"].Value;
                    var Ma_Vt = Request.Cookies["Ma_Vt"].Value;
                    var Ma_Kho = Request.Cookies["Ma_Kho"].Value;
                    var Ma_DV = Request.Cookies["Ma_DV"].Value;
                    cmd.CommandTimeout = 950;
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();
                        cmd.Parameters.AddWithValue("@_Tu_Ngay", fromDate);
                        cmd.Parameters.AddWithValue("@_Den_Ngay", toDate);
                        cmd.Parameters.AddWithValue("@_ma_dvcs", Ma_DV);
                        cmd.Parameters.AddWithValue("@_Ma_Kho", Ma_Kho);
                        cmd.Parameters.AddWithValue("@_Ma_Vt", Ma_Vt);
                        sda.Fill(ds);

                        // Kiểm tra xem DataSet có bảng dữ liệu hay không
                        if (ds.Tables.Count > 0)
                        {
                            DataTable dt = ds.Tables[0];

                            foreach (DataRow row in dt.Rows)
                            {
                                BienBanKiemNhapHang dataItem = new BienBanKiemNhapHang
                                {
                                    Ma_Vt = row["Ma_Vt"].ToString(),
                                    Ten_Vt = row["Ten_Vt"].ToString(),
                                    So_HD = row["So_HD"].ToString(),
                                    Dvt = row["Dvt"].ToString(),
                                    So_Lo = row["So_Lo"].ToString(),
                                    Han_Dung = row["Han_Dung"].ToString(),
                                    So_Luong_Nhap = Convert.ToDecimal(row["So_Luong_Nhap"].ToString()),
                                





                                };

                                dataItems.Add(dataItem);
                            }
                        }
                    }
                }
            }
            return dataItems;
        }
        public ActionResult ExportToExcel()
        {
            var fileName = $"BienBanKiemNhapHang{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            // Lấy dữ liệu từ cookie
            List<BienBanKiemNhapHang> combinedData = LoadData();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("MySheet");
                worksheet.View.ShowGridLines = false;

                // ... (Các bước tạo nội dung tệp Excel như bạn đã làm)
                // Đường dẫn đến hình ảnh trong thư mục 'image'
                var imagePath = Server.MapPath("~/assets/images/logo.png"); // Thay thế bằng đường dẫn thật
                var sumKhoan = 0;                                                            // Lấy giá trị từ biến Dvcs
                string Year = Request.Cookies["namCookie"] != null ? HttpUtility.UrlDecode(Request.Cookies["namCookie"].Value) : "";
                string Month = Request.Cookies["thangCookie"] != null ? HttpUtility.UrlDecode(Request.Cookies["thangCookie"].Value) : "";
                string TenNV = Request.Cookies["tenNVCookie"] != null ? HttpUtility.UrlDecode(Request.Cookies["tenNVCookie"].Value) : "";
                string SoCT = Request.Cookies["soCtCCookie"] != null ? HttpUtility.UrlDecode(Request.Cookies["soCtCCookie"].Value) : "";
                string Day = Request.Cookies["ngayCookie"] != null ? HttpUtility.UrlDecode(Request.Cookies["ngayCookie"].Value) : "";
                string TuyenGH = Request.Cookies["tenNVPhuCookie"] != null ? HttpUtility.UrlDecode(Request.Cookies["tenNVPhuCookie"].Value) : "";
                string Sum = Request.Cookies["tongCongCookie"] != null ? HttpUtility.UrlDecode(Request.Cookies["tongCongCookie"].Value) : "";
                string Sum1 = Request.Cookies["tongCongCookie1"] != null ? HttpUtility.UrlDecode(Request.Cookies["tongCongCookie1"].Value) : "";

                // Đặt font chữ "Arial" cho toàn bộ tệp Excel
                worksheet.Cells.Style.Font.Name = "Times New Roman";

                // Chèn hình ảnh từ tệp hình vào ô A1
                ExcelPicture picture = worksheet.Drawings.AddPicture("MyPicture", new FileInfo(imagePath));
                picture.SetSize(80, 65); // Đặt kích thước cho hình ảnh
                picture.From.Row = 2;
                picture.From.Column = 1;
                worksheet.Column(1).Width = 10;



                worksheet.Cells["A1:I1"].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                worksheet.Cells["A1:A5"].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                worksheet.Cells["A5:N5"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                worksheet.Cells["N1:N5"].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                worksheet.Cells["C1:C5"].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                worksheet.Cells["K1:K5"].Style.Border.Right.Style = ExcelBorderStyle.Medium;

                // Đặt văn bản vào ô A2
                worksheet.Cells["A1"].Value = "CÔNG TY CỔ PHẦN";
                worksheet.Cells["A1"].Style.Indent = 4;
                worksheet.Cells["A1"].Style.Font.Bold=true;
                worksheet.Cells["A2"].Value = "DƯỢC PHẨM OPC";
                worksheet.Cells["A2"].Style.Font.Bold = true;
                worksheet.Cells["A2"].Style.Indent = 6;
                worksheet.Cells["A1"].Style.Font.Size = 15;
                worksheet.Cells["A2"].Style.Font.Size = 15;
                var cellB1 = worksheet.Cells["A1"];
                cellB1.Style.Font.Bold = true;

                worksheet.Cells["B2"].Style.Indent = 1;

                worksheet.Cells["L2"].Value = "PHỤ LỤC 2";
                worksheet.Cells["L2"].Style.Indent = 10;
                worksheet.Cells["L2"].Style.Font.Bold = true;
                worksheet.Cells["L2"].Style.Font.Size = 15;
                worksheet.Cells["L3"].Value = "BO.03.2";
                worksheet.Cells["L3"].Style.Indent = 12;
                worksheet.Cells["L3"].Style.Font.Bold = true;
                worksheet.Cells["L3"].Style.Font.Size = 15;

                worksheet.Cells["F2"].Value = "BIÊN BẢN KIỂM NHẬP HÀNG";
                //worksheet.Cells["D2"].Style.Indent = 3;
                worksheet.Cells["F2"].Style.Font.Bold = true;
                worksheet.Cells["F2"].Style.Font.Size = 18;
                worksheet.Cells["F2"].Style.Indent = 2;
                worksheet.Cells["D3"].Value = $"Số Phiếu xuất kho kiêm vận chuyển nội bộ: .................Ngày:..................";
                worksheet.Cells["D3"].Style.Indent = 1;
                worksheet.Cells["D3"].Style.Font.Bold = true;
                worksheet.Cells["D3"].Style.Font.Size = 13;


                var startRow = 8;
                var startColumn = 1;

                var sttCell = worksheet.Cells[startRow - 1, startColumn, startRow, startColumn];
                sttCell.Value = "TT";
                sttCell.Merge = true;
                sttCell.Style.Font.Bold = true;
                sttCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
               
                sttCell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);


                var donViCell = worksheet.Cells[startRow - 1, startColumn + 1, startRow, startColumn + 1];
                donViCell.Value = "Mã SP";
                donViCell.Merge = true;
                donViCell.Style.Font.Bold = true;
                donViCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
               
                donViCell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

                var TenSPCell = worksheet.Cells[startRow - 1, startColumn + 2, startRow, startColumn + 2];
                TenSPCell.Value = "Tên SP - Nồng độ, hàm lượng, qui cách";
                TenSPCell.Style.WrapText = true;
                TenSPCell.Merge = true;
                TenSPCell.Style.Font.Bold = true;
                TenSPCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
               
                TenSPCell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);


                var hoaDonCell = worksheet.Cells[startRow - 1, startColumn + 3, startRow, startColumn + 3];
                hoaDonCell.Value = "Số HĐ";
                hoaDonCell.Merge = true;
                hoaDonCell.Style.Font.Bold = true;
                hoaDonCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
               
                hoaDonCell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

                var dvtCell = worksheet.Cells[startRow - 1, startColumn + 4, startRow, startColumn + 4];
                dvtCell.Value = "Đơn vị tính";
                dvtCell.Merge = true;
                dvtCell.Style.Font.Bold = true;
                dvtCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
             
                dvtCell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

                var soloCell = worksheet.Cells[startRow -1 , startColumn + 5, startRow , startColumn + 5];
                soloCell.Value = "Số lô";
                soloCell.Merge = true;
                soloCell.Style.Font.Bold = true;
                soloCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                soloCell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                soloCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                soloCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //var hanDungCell = worksheet.Cells[startRow - 1, startColumn + 6, startRow, startColumn + 6];
                //hanDungCell.Value = "Hạn dùng";
                //hanDungCell.Style.Font.Bold = true;
                var hanDungCell = worksheet.Cells[startRow - 1, startColumn + 6, startRow, startColumn + 6];
                hanDungCell.Value = "Hạn dùng";
                hanDungCell.Merge = true;
                hanDungCell.Style.Font.Bold = true;
                hanDungCell.Style.Fill.PatternType = ExcelFillStyle.Solid;

                hanDungCell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                hanDungCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                hanDungCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                var soLuongCell = worksheet.Cells[startRow -1, startColumn + 7, startRow -1, startColumn + 8];
                soLuongCell.Value = "Số Lượng";
                soLuongCell.Style.Font.Bold = true;
                soLuongCell.Merge = true;
                soLuongCell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                soLuongCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                soLuongCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                var chungTuCell = worksheet.Cells[startRow, startColumn + 7];
                chungTuCell.Value = "Chứng Từ";
                chungTuCell.Style.Font.Bold = true;
           
               
                chungTuCell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                chungTuCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                chungTuCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                var thucTeCell = worksheet.Cells[startRow, startColumn + 8];
                thucTeCell.Value = "Thực Tế";
                thucTeCell.Style.Font.Bold = true;
               
               
                thucTeCell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                thucTeCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                thucTeCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

              

                var tthhCell = worksheet.Cells[startRow-1, startColumn + 9,startRow-1,startColumn+11];
                tthhCell.Value = "Tình Trạng Hàng Hóa";
                tthhCell.Merge = true;
                tthhCell.Style.Font.Bold = true;
                tthhCell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                tthhCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                tthhCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                var thuaCell = worksheet.Cells[startRow, startColumn + 9];
                thuaCell.Value = "Thừa";
                thuaCell.Style.Font.Bold = true;
              
               
                thuaCell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                thuaCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                thuaCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                var thieuCell = worksheet.Cells[startRow, startColumn + 10];
                thieuCell.Value = "Thiếu";
                thieuCell.Style.Font.Bold = true;
   
              
                thieuCell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                thieuCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                thieuCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                var hongVoCell = worksheet.Cells[startRow, startColumn + 11];
                hongVoCell.Value = "Hỏng Vỡ";
                hongVoCell.Style.Font.Bold = true;
            
              
                hongVoCell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                hongVoCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                hongVoCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                var nhatXetCell = worksheet.Cells[startRow - 1, startColumn + 12, startRow, startColumn + 12];
                nhatXetCell.Value = "Nhận Xét Chất Lượng";
                nhatXetCell.Merge = true;
                nhatXetCell.Style.Font.Bold = true;
                nhatXetCell.Style.WrapText = true;
          
              
                nhatXetCell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                nhatXetCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nhatXetCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                var ghiChuCell = worksheet.Cells[startRow - 1, startColumn + 13, startRow, startColumn + 13];
                ghiChuCell.Value = "Ghi Chú";
                ghiChuCell.Merge = true;
                ghiChuCell.Style.Font.Bold = true;
                ghiChuCell.Style.WrapText = true;
          
           
                ghiChuCell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                ghiChuCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ghiChuCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                for (int col = 0; col < 8; col++)
                {
                    var columnHeaderCell = worksheet.Cells[startRow - 1, startColumn + col];
                    columnHeaderCell.Style.Font.Bold = true;
                    columnHeaderCell.Style.Font.Size = 10;
                    columnHeaderCell.Style.Font.Color.SetColor(Color.Black);
                    columnHeaderCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    columnHeaderCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    columnHeaderCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    columnHeaderCell.Style.Fill.BackgroundColor.SetColor(Color.White);
                }
                //var columnHeaderStyle = worksheet.Cells[startRow - 1, startColumn, startRow - 1, startColumn + 8].Style;
                //columnHeaderStyle.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black); // Đóng khung solid đen

                worksheet.Column(startColumn + 1).Width = 8; // Độ rộng cột cho "Tên sản phẩm"
                worksheet.Row(startRow - 1).Height = 25;
                worksheet.Row(startRow).Height = 25;

                worksheet.Column(startColumn + 2).Width = 25;
                worksheet.Column(startColumn + 3).Width = 10;
                worksheet.Column(startColumn + 4).Width = 10;
                worksheet.Column(startColumn + 5).Width = 15;
                worksheet.Column(startColumn + 6).Width = 17;
                worksheet.Column(startColumn + 7).Width = 12; 
                worksheet.Column(startColumn + 8).Width = 10;
                worksheet.Column(startColumn + 11).Width = 20;
                worksheet.Column(startColumn + 12).Width = 15;


                //Đảm bảo rằng có dữ liệu trong biến tableData
                if (combinedData != null && combinedData.Any())
                {
                    var stt = 1;
                    var start = startRow + 1;
            

                    // Lặp qua từng hàng dữ liệu trong tableData và ghi vào tệp Excel
                    for (int row = 0; row < combinedData.Count; row++)
                    {
                        var rowData = combinedData[row];

                        FormatCell(worksheet.Cells[start + row, startColumn]);

                        worksheet.Cells[start + row, startColumn].Value = stt;

                        // Kiểm tra giá trị của Ten_Dt so với hàng trước đó
                        //if (rowData.Ten_Vt != previousTenDt)
                        //{
                        //    worksheet.Cells[start + row, startColumn + 1].Value = rowData.Ten_Vt;
                        //    FormatCell(worksheet.Cells[start + row, startColumn + 1]);
                        //    previousTenDt = rowData.Ten_Vt; // Cập nhật giá trị mới
                        //}
                        //else
                        //{
                        //    worksheet.Cells[start + row, startColumn + 1].Value = null; // Để trống nếu giống với hàng trước
                        //    //FormatCellNoQH(worksheet.Cells[start + row, startColumn + 1]);
                        //}
                        worksheet.Cells[start + row, startColumn + 1].Value = rowData.Ma_Vt;
                        FormatCell(worksheet.Cells[start + row, startColumn + 1]);

                        worksheet.Cells[start + row, startColumn + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        worksheet.Cells[start + row, startColumn + 1].Style.WrapText = true;

                        // Các cột khác bạn xử lý như bình thường
                        FormatCell(worksheet.Cells[start + row, startColumn + 2]);
                        worksheet.Cells[start + row, startColumn + 2].Value = rowData.Ten_Vt;
                        worksheet.Cells[start + row, startColumn + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        worksheet.Cells[start + row, startColumn + 2].Style.WrapText = true;
                        FormatCell(worksheet.Cells[start + row, startColumn + 3]);
                        worksheet.Cells[start + row, startColumn + 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[start + row, startColumn + 3].Value = rowData.So_HD;

                        worksheet.Cells[start + row, startColumn + 4].Value = rowData.Dvt;
                        worksheet.Cells[start + row, startColumn + 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        FormatCell(worksheet.Cells[start + row, startColumn + 4]);

                        worksheet.Cells[start + row, startColumn + 5].Value = rowData.So_Lo;
                        worksheet.Cells[start + row, startColumn + 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        FormatCell(worksheet.Cells[start + row, startColumn + 5]);
                        FormatCell(worksheet.Cells[start + row, startColumn + 6]);
                        worksheet.Cells[start + row, startColumn + 6].Value = rowData.Han_Dung;
                        worksheet.Cells[start + row, startColumn + 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[start + row, startColumn +6].Style.WrapText = true;
                        FormatCell(worksheet.Cells[start + row, startColumn + 7]);
                        worksheet.Cells[start + row, startColumn + 7].Value = rowData.So_Luong_Nhap;
                        worksheet.Cells[start + row, startColumn + 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                       
                        
                        worksheet.Cells[start + row, startColumn + 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[start + row, startColumn + 8].Value = rowData.So_Luong_Nhap;
                        worksheet.Cells[start + row, startColumn + 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        FormatCell(worksheet.Cells[start + row, startColumn + 8]);
                        FormatCell(worksheet.Cells[start + row, startColumn +9]);
                        FormatCell(worksheet.Cells[start + row, startColumn + 10]);
                        FormatCell(worksheet.Cells[start + row, startColumn + 11]);
                        FormatCell(worksheet.Cells[start + row, startColumn + 12]);
                        FormatCell(worksheet.Cells[start + row, startColumn + 13]);

                        sumKhoan = stt;
                        stt++;
                    }
                }
                else
                {
                    worksheet.Cells[startRow, startColumn].Value = "Không có dữ liệu bảng từ cookie.";
                }

                // Xóa hàng tiêu đề mặc định

                var dataRowStyle = worksheet.Cells[startRow, startColumn, startRow, startColumn + 5].Style;
                dataRowStyle.Font.Bold = false;
                dataRowStyle.Font.Color.SetColor(Color.Black);
                dataRowStyle.Fill.PatternType = ExcelFillStyle.None;
                // Tạo bảng trong tệp Excel
                var rowTC = startRow + combinedData.Count;
                var TCCell = worksheet.Cells[rowTC + 1, startColumn, rowTC + 1, startColumn + 3];
                TCCell.Merge = true;
                TCCell.Value = $"Tổng Cộng: {sumKhoan} khoản";
                TCCell.Style.Font.Bold = true;
       
           
                FormatCell(TCCell);
                worksheet.Row(rowTC + 1).Height = 25;
                var sumCell = worksheet.Cells[rowTC + 1, startColumn + 4];
                sumCell.Value = Sum;
                sumCell.Style.Font.Bold = true;
                FormatCell(sumCell);


                var sum1Cell = worksheet.Cells[rowTC + 1, startColumn + 5];
                sum1Cell.Value = Sum1;
                sum1Cell.Style.Font.Bold = true;
               
                FormatCell(sum1Cell);
                FormatCell(worksheet.Cells[rowTC + 1, startColumn + 6]);
                FormatCell(worksheet.Cells[rowTC + 1, startColumn + 7]);
                FormatCell(worksheet.Cells[rowTC + 1, startColumn + 8]);
                FormatCell(worksheet.Cells[rowTC + 1, startColumn + 9]);
                FormatCell(worksheet.Cells[rowTC + 1, startColumn + 10]);
                FormatCell(worksheet.Cells[rowTC + 1, startColumn + 11]);
                FormatCell(worksheet.Cells[rowTC + 1, startColumn + 12]);
                FormatCell(worksheet.Cells[rowTC + 1, startColumn + 13]);
              
                var endRow = rowTC + 2;
             
                worksheet.Cells[endRow, startColumn].Style.Font.Bold = true;
                worksheet.Cells[endRow, startColumn].Style.Font.Size = 12;
                worksheet.Row(endRow + 1).Height = 30;
             
                worksheet.Cells[endRow + 1, startColumn].Value = "KẾT LUẬN:......................................................................................................................................................................";
                worksheet.Cells[endRow + 1, startColumn].Style.Font.Bold = true;
                worksheet.Row(endRow + 2).Height = 30;
                worksheet.Row(endRow + 3).Height = 30;

                worksheet.Cells[endRow + 2, startColumn].Value = "NHẬN XÉT VÀ Ý KIẾN ĐỀ NGHỊ:............................................................................................................................................";
                worksheet.Cells[endRow + 2, startColumn].Style.Font.Bold = true;

                worksheet.Cells[endRow + 3, startColumn].Value = "...................................................................................................................................................................................";
                worksheet.Cells[endRow + 3, startColumn].Style.Font.Bold = true;

                worksheet.Cells[endRow + 4, startColumn + 9].Value = "Ngày........../......./..........";
                worksheet.Cells[endRow + 4, startColumn + 9].Style.Font.Bold = true;
                worksheet.Cells[endRow + 4, startColumn + 9].Style.Indent = 3;


                worksheet.Cells[endRow + 5, startColumn + 2].Value = "BP.ĐBCL";
                worksheet.Cells[endRow + 5, startColumn + 2].Style.Font.Bold = true;

                worksheet.Cells[endRow + 5, startColumn + 10].Value = "Thủ Kho";
                worksheet.Cells[endRow + 5, startColumn + 10].Style.Font.Bold = true;


                //worksheet.Cells[nextRow + 6, startColumn + 4].Value = "Thủ Kho";
                //worksheet.Cells[nextRow + 6, startColumn + 4].Style.Font.Bold = true;
                //worksheet.Cells[nextRow + 6, startColumn + 8].Value = "Người Giao Hàng";
                //worksheet.Cells[nextRow + 6, startColumn + 8].Style.Font.Bold = true;


                package.Save();
                byte[] fileBytes = package.GetAsByteArray();

                // Trả về tệp Excel dưới dạng dữ liệu binary
                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

            }



            return View("BienBanKiemNhapHang_In");
        }
    }
}