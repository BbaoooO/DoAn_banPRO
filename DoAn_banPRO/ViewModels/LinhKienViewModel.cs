using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using DoAn_banPRO.Helpers;
using DoAn_banPRO.Models;
using DoAn_banPRO.Repositories;

namespace DoAn_banPRO.ViewModels
{
    public class LinhKienViewModel : BaseViewModel
    {
        private readonly ILinhKienRepository _repository;
        private ObservableCollection<LinhKien> _linhKienList;
        private LinhKien _selectedLinhKien;
        private string _keyword;

        public ObservableCollection<LinhKien> LinhKienList
        {
            get => _linhKienList;
            set
            {
                _linhKienList = value;
                OnPropertyChanged();
            }
        }

        public LinhKien SelectedLinhKien
        {
            get => _selectedLinhKien;
            set
            {
                _selectedLinhKien = value;
                OnPropertyChanged();
            }
        }

        public string Keyword
        {
            get => _keyword;
            set
            {
                _keyword = value;
                OnPropertyChanged();
                SearchData(); // T? ??ng tìm ki?m ngay khi ?ang gõ (Live Search)
            }
        }

        public ICommand LoadCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SearchCommand { get; }

        public LinhKienViewModel()
        {
            _repository = new LinhKienRepository();
            // Kh?i t?o m?t ??i t??ng s?n ?? có th? nh?p luôn
            SelectedLinhKien = new LinhKien();

            LoadCommand = new RelayCommand<object>(p => LoadData(), p => true);
            AddCommand = new RelayCommand<object>(p => AddData(), p => CanAddOrUpdate());
            UpdateCommand = new RelayCommand<object>(p => UpdateData(), p => SelectedLinhKien != null && CanAddOrUpdate());
            DeleteCommand = new RelayCommand<object>(p => DeleteData(), p => SelectedLinhKien != null);
            SearchCommand = new RelayCommand<object>(p => SearchData(), p => true);

            LoadData();
        }

        private void LoadData()
        {
            var data = _repository.GetAll();
            LinhKienList = new ObservableCollection<LinhKien>(data);
        }

        private bool CanAddOrUpdate()
        {
            if (SelectedLinhKien == null) return false;
            return !string.IsNullOrWhiteSpace(SelectedLinhKien.MaLK) &&
                   !string.IsNullOrWhiteSpace(SelectedLinhKien.TenLK) &&
                   SelectedLinhKien.TonKho >= 0 &&
                   SelectedLinhKien.DonGia >= 0;
        }

        private void AddData()
        {
            try
            {
                _repository.Add(SelectedLinhKien);
                LoadData();
                MessageBox.Show("Thêm thành công!", "Thông báo");
                SelectedLinhKien = new LinhKien(); // Reset sau khi thêm
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
                _repository.Update(SelectedLinhKien);
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
                var result = MessageBox.Show("B?n có ch?c mu?n xóa linh ki?n này?", "Xác nh?n", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    _repository.Delete(SelectedLinhKien.MaLK);
                    LoadData();
                    MessageBox.Show("Xóa thành công!", "Thông báo");
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"L?i: {ex.Message}", "L?i");
            }
        }

        private void SearchData()
        {
            if (string.IsNullOrWhiteSpace(Keyword))
            {
                LoadData();
            }
            else
            {
                var data = _repository.Search(Keyword.Trim()); // T? ??ng c?t kho?ng tr?ng
                LinhKienList = new ObservableCollection<LinhKien>(data);
            }
        }
    }
}
