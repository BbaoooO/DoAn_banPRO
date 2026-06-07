using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using DoAn_banPRO.Helpers;
using DoAn_banPRO.Models;
using DoAn_banPRO.Repositories;

namespace DoAn_banPRO.ViewModels
{
    public class LoaiHangViewModel : BaseViewModel
    {
        private readonly ILoaiHangRepository _repository;
        private ObservableCollection<LoaiHang> _loaiHangList;
        private LoaiHang _selectedLoaiHang;

        public ObservableCollection<LoaiHang> LoaiHangList
        {
            get => _loaiHangList;
            set
            {
                _loaiHangList = value;
                OnPropertyChanged();
            }
        }

        public LoaiHang SelectedLoaiHang
        {
            get => _selectedLoaiHang;
            set
            {
                _selectedLoaiHang = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }

        public LoaiHangViewModel()
        {
            _repository = new LoaiHangRepository();
            SelectedLoaiHang = new LoaiHang(); // Reset object the first time
            
            LoadCommand = new RelayCommand<object>(p => LoadData(), p => true);
            AddCommand = new RelayCommand<object>(p => AddData(), p => CanAddOrUpdate());
            UpdateCommand = new RelayCommand<object>(p => UpdateData(), p => SelectedLoaiHang != null && CanAddOrUpdate());
            DeleteCommand = new RelayCommand<object>(p => DeleteData(), p => SelectedLoaiHang != null && !string.IsNullOrWhiteSpace(SelectedLoaiHang.MaLoai));

            LoadData();
        }

        private void LoadData()
        {
            var data = _repository.GetAll();
            LoaiHangList = new ObservableCollection<LoaiHang>(data);
        }

        private bool CanAddOrUpdate()
        {
            if (SelectedLoaiHang == null) return false;
            return !string.IsNullOrWhiteSpace(SelectedLoaiHang.MaLoai) && !string.IsNullOrWhiteSpace(SelectedLoaiHang.TenLoai);
        }

        private void AddData()
        {
            try
            {
                _repository.Add(SelectedLoaiHang);
                LoadData();
                MessageBox.Show("Thêm thành công!", "Thông báo");
                SelectedLoaiHang = new LoaiHang(); // Reset
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"L?i: {ex.Message}", "L?i");
            }
        }

        private void UpdateData()
        {
            try
            {
                _repository.Update(SelectedLoaiHang);
                LoadData();
                MessageBox.Show("C?p nh?t thành công!", "Thông báo");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"L?i: {ex.Message}", "L?i");
            }
        }

        private void DeleteData()
        {
            try
            {
                var result = MessageBox.Show("B?n có ch?c mu?n xóa lo?i hàng này?", "Xác nh?n", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    _repository.Delete(SelectedLoaiHang.MaLoai);
                    LoadData();
                    MessageBox.Show("Xóa thành công!", "Thông báo");
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"L?i: {ex.Message}", "L?i");
            }
        }
    }
}