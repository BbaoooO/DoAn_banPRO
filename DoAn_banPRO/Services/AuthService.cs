using DoAn_banPRO.Models;
using DoAn_banPRO.Repositories;

namespace DoAn_banPRO.Services
{
    public class AuthService
    {
        private readonly INguoiDungRepository _repository;
        public static NguoiDung CurrentUser { get; private set; }

        public AuthService()
        {
            _repository = new NguoiDungRepository();
        }

        public bool Login(string taiKhoan, string matKhau)
        {
            var user = _repository.GetByTaiKhoanMatKhau(taiKhoan, matKhau);
            if (user != null)
            {
                CurrentUser = user;
                return true;
            }
            return false;
        }

        public void Logout()
        {
            CurrentUser = null;
        }

        public static bool IsAdmin() => CurrentUser?.Quyen == "Qu?n tr? viên";
        public static bool IsThuKho() => CurrentUser?.Quyen == "Th? kho" || IsAdmin();
        public static bool IsNhanVien() => CurrentUser?.Quyen == "Nhân viên bán hàng" || IsAdmin();
    }
}