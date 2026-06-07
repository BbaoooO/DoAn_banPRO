namespace DoAn_banPRO.Models
{
    public class LinhKien
    {
        public string MaLK { get; set; }
        public string TenLK { get; set; }
        public int TonKho { get; set; }
        public string MaLoai { get; set; }
        public string MaNCC { get; set; }
        public decimal DonGia { get; set; }
        public string DonViTinh { get; set; }
        
        // Properties for UI display
        public string TenLoai { get; set; }
        public string TenNCC { get; set; }
    }
}
