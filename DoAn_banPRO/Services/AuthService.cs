using System;
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

        private static string GetRole()
        {
            return CurrentUser?.Quyen?.Trim() ?? "";
        }

        public static bool IsAdmin()
        {
            string role = GetRole();

            return role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase)
                || role == "Quản trị viên"
                || role == "Qu?n tr? viên";
        }

        public static bool IsThuKho()
        {
            string role = GetRole();

            return role.Equals("THU_KHO", StringComparison.OrdinalIgnoreCase)
                || role == "Thủ kho"
                || role == "Th? kho"
                || IsAdmin();
        }

        public static bool IsNhanVien()
        {
            string role = GetRole();

            return role.Equals("NHAN_VIEN", StringComparison.OrdinalIgnoreCase)
                || role == "Nhân viên bán hàng"
                || IsAdmin();
        }
    }
}