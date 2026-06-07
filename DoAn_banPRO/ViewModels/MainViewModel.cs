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
            ShowLinhKienCommand = new RelayCommand<object>(p => CurrentViewModel = new LinhKienViewModel(), p => true);
            ShowLoaiHangCommand = new RelayCommand<object>(p => CurrentViewModel = new LoaiHangViewModel(), p => true);
            ShowPhieuNhapCommand = new RelayCommand<object>(p => CurrentViewModel = new PhieuNhapViewModel(), p => true);
            ShowPhieuXuatCommand = new RelayCommand<object>(p => CurrentViewModel = new PhieuXuatViewModel(), p => true);
            ShowNhaCungCapCommand = new RelayCommand<object>(p => CurrentViewModel = new NhaCungCapViewModel(), p => true);
            ShowThongKeCommand = new RelayCommand<object>(p => CurrentViewModel = new ThongKeViewModel(), p => true);
            LogoutCommand = new RelayCommand<Window>(p => Logout(p), p => true);
            
            // Trang m?c ??nh
            CurrentViewModel = new LinhKienViewModel();
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