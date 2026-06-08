using System.Windows;
using System.Windows.Input;
using DoAn_banPRO.Helpers;
using DoAn_banPRO.Services;
using DoAn_banPRO.Views;

namespace DoAn_banPRO.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private object _currentViewModel;

        public object CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }

        public string UserInfo => $"Xin chào, {AuthService.CurrentUser?.HoTen} ({AuthService.CurrentUser?.Quyen})";

        public ICommand ShowLinhKienCommand { get; }
        public ICommand ShowLoaiHangCommand { get; }
        public ICommand ShowPhieuNhapCommand { get; }
        public ICommand ShowPhieuXuatCommand { get; }
        public ICommand ShowNhaCungCapCommand { get; }
        public ICommand ShowThongKeCommand { get; }
        public ICommand LogoutCommand { get; }

        public MainViewModel()
        {
            ShowLinhKienCommand = new RelayCommand<object>(
                p => CurrentViewModel = new LinhKienViewModel(),
                p => CanOpenLinhKien()
            );

            ShowLoaiHangCommand = new RelayCommand<object>(
                p => CurrentViewModel = new LoaiHangViewModel(),
                p => CanOpenLoaiHang()
            );

            ShowNhaCungCapCommand = new RelayCommand<object>(
                p => CurrentViewModel = new NhaCungCapViewModel(),
                p => CanOpenNhaCungCap()
            );

            ShowPhieuNhapCommand = new RelayCommand<object>(
                p => CurrentViewModel = new PhieuNhapViewModel(),
                p => CanOpenPhieuNhap()
            );

            ShowPhieuXuatCommand = new RelayCommand<object>(
                p => CurrentViewModel = new PhieuXuatViewModel(),
                p => CanOpenPhieuXuat()
            );

            ShowThongKeCommand = new RelayCommand<object>(
                p => CurrentViewModel = new ThongKeViewModel(),
                p => CanOpenThongKe()
            );

            LogoutCommand = new RelayCommand<Window>(
                p => Logout(p),
                p => true
            );

            SetDefaultViewByRole();
        }

        private bool CanOpenLinhKien()
        {
            return AuthService.IsAdmin()
                || AuthService.IsThuKho()
                || AuthService.IsNhanVien();
        }

        private bool CanOpenLoaiHang()
        {
            return AuthService.IsAdmin()
                || AuthService.IsThuKho();
        }

        private bool CanOpenNhaCungCap()
        {
            return AuthService.IsAdmin()
                || AuthService.IsThuKho();
        }

        private bool CanOpenPhieuNhap()
        {
            return AuthService.IsThuKho();
        }

        private bool CanOpenPhieuXuat()
        {
            return AuthService.IsThuKho()
                || AuthService.IsNhanVien();
        }

        private bool CanOpenThongKe()
        {
            return AuthService.IsAdmin();
        }

        private void SetDefaultViewByRole()
        {
            if (CanOpenLinhKien())
            {
                CurrentViewModel = new LinhKienViewModel();
            }
            else if (CanOpenPhieuXuat())
            {
                CurrentViewModel = new PhieuXuatViewModel();
            }
            else
            {
                CurrentViewModel = null;
            }
        }

        private void Logout(Window window)
        {
            var auth = new AuthService();
            auth.Logout();

            var loginWindow = new LoginWindow();
            loginWindow.Show();

            window?.Close();
        }
    }
}