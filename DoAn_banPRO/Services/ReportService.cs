using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using DoAn_banPRO.Models;
using DoAn_banPRO.Repositories;

namespace DoAn_banPRO.Services
{
    public class ReportService
    {
        public void ExportLinhKienToCSV()
        {
            try
            {
                var repository = new LinhKienRepository();
                var list = repository.GetAll().ToList();

                var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"BaoCaoLinhKien_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
                
                var strBuilder = new StringBuilder();
                // Create CSV Header
                strBuilder.AppendLine("MaLK,TenLK,TenLoai,TenNCC,TonKho,DonViTinh,DonGia,ThanhTien");

                foreach (var item in list)
                {
                    decimal thanhTien = item.TonKho * item.DonGia;
                    // Escape commas in string properties if any
                    string safeName = item.TenLK?.Replace(",", " ");
                    string safeLoai = item.TenLoai?.Replace(",", " ");
                    string safeNcc = item.TenNCC?.Replace(",", " ");

                    strBuilder.AppendLine($"{item.MaLK},{safeName},{safeLoai},{safeNcc},{item.TonKho},{item.DonViTinh},{item.DonGia},{thanhTien}");
                }

                File.WriteAllText(filePath, strBuilder.ToString(), Encoding.UTF8);
                MessageBox.Show($"Xuất báo cáo thành công tại: {filePath}", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xuất báo cáo: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}