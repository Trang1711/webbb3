namespace WebsiteTMDT.ViewModels
{
    public class ProductsVM
    {
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public string MoTa { get; set; }
        public string HinhAnh { get; set; }
        public double Gia { get; set; }
        public int SoLuong { get; set; }
        public int MaLoai { get; set; }
        public string TenLoai { get; set; }
    }

    public class ProductsDetailVM
    {
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public string MoTa { get; set; }
        public string HinhAnh { get; set; }
        public double Gia { get; set; }
        public int SoLuong { get; set; }
        public int MaLoai { get; set; }
        public string TenLoai { get; set; }
        public string ChiTiet { get; set; }
        public int DiemDanhGia { get; set; }
        public int SoLuongTon{ get; set; }
        public string MauSac { get; set; }
        public string DungLuong { get; set; }
    }
}
