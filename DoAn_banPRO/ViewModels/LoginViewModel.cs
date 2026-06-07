using System.Windows;
using System.Windows.Input;
using DoAn_banPRO.Helpers;
using DoAn_banPRO.Services;

namespace DoAn_banPRO.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly AuthService _authService;
        private string _taiKhoan;
        private string _matKhau;

        public string TaiKhoan
        {
            get => _taiKhoan;
            set
            {
                _taiKhoan = value;
                OnPropertyChanged();
            }
        }

        public string MatKhau
        {
            get => _matKhau;
            set
            {
                _matKhau = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            _authService = new AuthService();
            LoginCommand = new RelayCommand<Window>(p => Login(p), p => CanLogin());
        }

        private bool CanLogin()
        {
            return !string.IsNullOrWhiteSpace(TaiKhoan) && !string.IsNullOrWhiteSpace(MatKhau);
        }

        private void Login(Window window)
        {
            if (_authService.Login(TaiKhoan, MatKhau))
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
                window?.Close();
            }
            else
            {
                MessageBox.Show("Tài kho?n ho?c m?t kh?u không chính xác!", "L?i ??ng nh?p", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}