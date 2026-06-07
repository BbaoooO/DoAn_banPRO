using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using DoAn_banPRO.Helpers;
using DoAn_banPRO.Models;
using DoAn_banPRO.Repositories;
using DoAn_banPRO.Services;

namespace DoAn_banPRO.ViewModels
{
    public class PhieuNhapViewModel : BaseViewModel
    {
        private readonly IPhieuNhapRepository _repository;
        private readonly ILinhKienRepository _lkRepository;

        private ObservableCollection<LinhKien> _danhSachLinhKien;
        private ObservableCollection<ChiTietPhieuNhap> _chiTietPhieuNhaps;

        private PhieuNhap _currentPhieuNhap;
        private LinhKien _selectedLinhKien;
        private int _soLuongNhap = 1;
        private decimal _donGiaNhap = 0;
        private ChiTietPhieuNhap _selectedChiTiet;

        public ObservableCollection<LinhKien> DanhSachLinhKien
        {
            get => _danhSachLinhKien;
            set
            {
                _danhSachLinhKien = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ChiTietPhieuNhap> ChiTietPhieuNhaps
        {
            get => _chiTietPhieuNhaps;
            set
            {
                _chiTietPhieuNhaps = value;
                OnPropertyChanged();
            }
        }

        public PhieuNhap CurrentPhieuNhap
        {
            get => _currentPhieuNhap;
            set
            {
                _currentPhieuNhap = value;
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
                
                // T? ??ng gán ??n giá n?u ch?n LK
                if (_selectedLinhKien != null)
                {
                    DonGiaNhap = _selectedLinhKien.DonGia;
                }
            }
        }

        public int SoLuongNhap
        {
            get => _soLuongNhap;
            set
            {
                _soLuongNhap = value;
                OnPropertyChanged();
            }
        }

        public decimal DonGiaNhap
        {
            get => _donGiaNhap;
            set
            {
                _donGiaNhap = value;
                OnPropertyChanged();
            }
        }

        public ChiTietPhieuNhap SelectedChiTiet
        {
            get => _selectedChiTiet;
            set
            {
                _selectedChiTiet = value;
                OnPropertyChanged();
            }
        }

        public decimal TongTienPhieu => ChiTietPhieuNhaps?.Sum(x => (x.SoLuong ?? 0) * (x.DonGiaNhap ?? 0)) ?? 0;

        public ICommand AddChiTietCommand { get; }
        public ICommand RemoveChiTietCommand { get; }
        public ICommand SavePhieuNhapCommand { get; }

        public PhieuNhapViewModel()
        {
            _repository = new PhieuNhapRepository();
            _lkRepository = new LinhKienRepository();

            DanhSachLinhKien = new ObservableCollection<LinhKien>(_lkRepository.GetAll());
            ChiTietPhieuNhaps = new ObservableCollection<ChiTietPhieuNhap>();
            
            InitNewPhieu();

            AddChiTietCommand = new RelayCommand<object>(p => AddChiTiet(), p => CanAddChiTiet());
            RemoveChiTietCommand = new RelayCommand<object>(p => RemoveChiTiet(), p => SelectedChiTiet != null);
            SavePhieuNhapCommand = new RelayCommand<object>(p => SavePhieuNhap(), p => CanSavePhieuNhap());
        }

        private void InitNewPhieu()
        {
            CurrentPhieuNhap = new PhieuNhap
            {
                MaPhieu = _repository.GetNextMaPhieu(),
                NgayLap = DateTime.Now,
                MaND = AuthService.CurrentUser?.MaND ?? ""
            };
            ChiTietPhieuNhaps.Clear();
            SoLuongNhap = 1;
            DonGiaNhap = 0;
            OnPropertyChanged(nameof(TongTienPhieu));
        }

        private bool CanAddChiTiet()
        {
            return SelectedLinhKien != null && SoLuongNhap > 0 && DonGiaNhap >= 0;
        }

        private void AddChiTiet()
        {
            // Ki?m tra xem ?ã có trong list ch?a, n?u có thì c?ng d?n
            var existing = ChiTietPhieuNhaps.FirstOrDefault(x => x.MaLK == SelectedLinhKien.MaLK);
            if (existing != null)
            {
                existing.SoLuong += SoLuongNhap;
                // N?u mu?n ghi ?è ??n giá ho?c tính trung bình, có th? custom ? ?ây
                existing.DonGiaNhap = DonGiaNhap; 
            }
            else
            {
                ChiTietPhieuNhaps.Add(new ChiTietPhieuNhap
                {
                    MaLK = SelectedLinhKien.MaLK,
                    SoLuong = SoLuongNhap,
                    DonGiaNhap = DonGiaNhap
                });
            }
            // Trigger refresh UI cho DataGrid và T?ng ti?n
            OnPropertyChanged(nameof(TongTienPhieu));
            var temp = ChiTietPhieuNhaps.ToList();
            ChiTietPhieuNhaps = new ObservableCollection<ChiTietPhieuNhap>(temp);
        }

        private void RemoveChiTiet()
        {
            if (SelectedChiTiet != null)
            {
                ChiTietPhieuNhaps.Remove(SelectedChiTiet);
                OnPropertyChanged(nameof(TongTienPhieu));
            }
        }

        private bool CanSavePhieuNhap()
        {
            return ChiTietPhieuNhaps.Count > 0 && !string.IsNullOrWhiteSpace(CurrentPhieuNhap.MaPhieu);
        }

        private void SavePhieuNhap()
        {
            try
            {
                CurrentPhieuNhap.TongTien = TongTienPhieu;
                _repository.SavePhieuNhap(CurrentPhieuNhap, ChiTietPhieuNhaps.ToList());
                
                MessageBox.Show("Nhập kho thành công!", "Thông báo");
                
                // Refresh danh sách linh ki?n phòng tr??ng h?p mu?n xem l?i t?n kho
                DanhSachLinhKien = new ObservableCollection<LinhKien>(_lkRepository.GetAll());
                
                // Reset phi?u
                InitNewPhieu();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi nhập kho: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}