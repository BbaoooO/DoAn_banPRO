using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using DoAn_banPRO.Helpers;
using DoAn_banPRO.Models;
using DoAn_banPRO.Repositories;

namespace DoAn_banPRO.ViewModels
{
    public class NhaCungCapViewModel : BaseViewModel
    {
        private readonly INhaCungCapRepository _repository;
        private ObservableCollection<NhaCungCap> _nhaCungCapList;
        private NhaCungCap _selectedNCC;

        public ObservableCollection<NhaCungCap> NhaCungCapList
        {
            get => _nhaCungCapList;
            set
            {
                _nhaCungCapList = value;
                OnPropertyChanged();
            }
        }

        public NhaCungCap SelectedNCC
        {
            get => _selectedNCC;
            set
            {
                _selectedNCC = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }

        public NhaCungCapViewModel()
        {
            _repository = new NhaCungCapRepository();
            SelectedNCC = new NhaCungCap(); // Start empty form
            
            LoadCommand = new RelayCommand<object>(p => LoadData(), p => true);
            AddCommand = new RelayCommand<object>(p => AddData(), p => CanAddOrUpdate());
            UpdateCommand = new RelayCommand<object>(p => UpdateData(), p => SelectedNCC != null && CanAddOrUpdate());
            DeleteCommand = new RelayCommand<object>(p => DeleteData(), p => SelectedNCC != null && !string.IsNullOrWhiteSpace(SelectedNCC.MaNCC));

            LoadData();
        }

        private void LoadData()
        {
            NhaCungCapList = new ObservableCollection<NhaCungCap>(_repository.GetAll());
        }

        private bool CanAddOrUpdate()
        {
            if (SelectedNCC == null) return false;
            return !string.IsNullOrWhiteSpace(SelectedNCC.MaNCC) && 
                   !string.IsNullOrWhiteSpace(SelectedNCC.TenNCC);
        }

        private void AddData()
        {
            try
            {
                _repository.Add(SelectedNCC);
                LoadData();
                MessageBox.Show("Thêm thành công!", "Thông báo");
                SelectedNCC = new NhaCungCap(); // Reset Form
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi");
            }
        }

        private void UpdateData()
        {
            try
            {
                _repository.Update(SelectedNCC);
                LoadData();
                MessageBox.Show("Cập nhật thành công!", "Thông báo");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi");
            }
        }

        private void DeleteData()
        {
            try
            {
                var result = MessageBox.Show("Bạn có chắc muốn xóa nhà cung cấp này?", "Xác nhận", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    _repository.Delete(SelectedNCC.MaNCC);
                    LoadData();
                    MessageBox.Show("Xóa thành công!", "Thông báo");
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi");
            }
        }
    }
}