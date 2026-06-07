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
    public class PhieuXuatViewModel : BaseViewModel
    {
        private readonly IPhieuXuatRepository _repository;
        private readonly ILinhKienRepository _lkRepository;

        private ObservableCollection<LinhKien> _danhSachLinhKien;
        private ObservableCollection<ChiTietPhieuXuat> _chiTietPhieuXuats;

        private PhieuXuat _currentPhieuXuat;
        private LinhKien _selectedLinhKien;
        private int _soLuongXuat = 1;
        private decimal _donGiaXuat = 0;
        private ChiTietPhieuXuat _selectedChiTiet;

        public ObservableCollection<LinhKien> DanhSachLinhKien
        {
            get => _danhSachLinhKien;
            set
            {
                _danhSachLinhKien = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ChiTietPhieuXuat> ChiTietPhieuXuats
        {
            get => _chiTietPhieuXuats;
            set
            {
                _chiTietPhieuXuats = value;
                OnPropertyChanged();
            }
        }

        public PhieuXuat CurrentPhieuXuat
        {
            get => _currentPhieuXuat;
            set
            {
                _currentPhieuXuat = value;
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
                
                if (_selectedLinhKien != null)
                {
                    // L?y giá bán = DonGia g?c ho?c custom
                    DonGiaXuat = _selectedLinhKien.DonGia;
                }
            }
        }

        public int SoLuongXuat
        {
            get => _soLuongXuat;
            set
            {
                _soLuongXuat = value;
                OnPropertyChanged();
            }
        }

        public decimal DonGiaXuat
        {
            get => _donGiaXuat;
            set
            {
                _donGiaXuat = value;
                OnPropertyChanged();
            }
        }

        public ChiTietPhieuXuat SelectedChiTiet
        {
            get => _selectedChiTiet;
            set
            {
                _selectedChiTiet = value;
                OnPropertyChanged();
            }
        }

        public decimal TongTienPhieu => ChiTietPhieuXuats?.Sum(x => (x.SoLuong ?? 0) * (x.DonGiaXuat ?? 0)) ?? 0;

        public ICommand AddChiTietCommand { get; }
        public ICommand RemoveChiTietCommand { get; }
        public ICommand SavePhieuXuatCommand { get; }

        public PhieuXuatViewModel()
        {
            _repository = new PhieuXuatRepository();
            _lkRepository = new LinhKienRepository();

            DanhSachLinhKien = new ObservableCollection<LinhKien>(_lkRepository.GetAll());
            ChiTietPhieuXuats = new ObservableCollection<ChiTietPhieuXuat>();
            
            InitNewPhieu();

            AddChiTietCommand = new RelayCommand<object>(p => AddChiTiet(), p => CanAddChiTiet());
            RemoveChiTietCommand = new RelayCommand<object>(p => RemoveChiTiet(), p => SelectedChiTiet != null);
            SavePhieuXuatCommand = new RelayCommand<object>(p => SavePhieuXuat(), p => CanSavePhieuXuat());
        }

        private void InitNewPhieu()
        {
            CurrentPhieuXuat = new PhieuXuat
            {
                MaPhieu = _repository.GetNextMaPhieu(),
                NgayLap = DateTime.Now,
                MaND = AuthService.CurrentUser?.MaND ?? ""
            };
            ChiTietPhieuXuats.Clear();
            SoLuongXuat = 1;
            DonGiaXuat = 0;
            OnPropertyChanged(nameof(TongTienPhieu));
        }

        private bool CanAddChiTiet()
        {
            // Ki?m tra t?n kho ngay khi b?m Thêm
            if (SelectedLinhKien == null || SoLuongXuat <= 0 || DonGiaXuat < 0) return false;
            
            var alreadyInList = ChiTietPhieuXuats.Where(x => x.MaLK == SelectedLinhKien.MaLK).Sum(x => x.SoLuong) ?? 0;
            return (alreadyInList + SoLuongXuat) <= SelectedLinhKien.TonKho;
        }

        private void AddChiTiet()
        {
            var existing = ChiTietPhieuXuats.FirstOrDefault(x => x.MaLK == SelectedLinhKien.MaLK);
            if (existing != null)
            {
                existing.SoLuong += SoLuongXuat;
                existing.DonGiaXuat = DonGiaXuat; 
            }
            else
            {
                ChiTietPhieuXuats.Add(new ChiTietPhieuXuat
                {
                    MaLK = SelectedLinhKien.MaLK,
                    SoLuong = SoLuongXuat,
                    DonGiaXuat = DonGiaXuat
                });
            }
            OnPropertyChanged(nameof(TongTienPhieu));
            var temp = ChiTietPhieuXuats.ToList();
            ChiTietPhieuXuats = new ObservableCollection<ChiTietPhieuXuat>(temp);
        }

        private void RemoveChiTiet()
        {
            if (SelectedChiTiet != null)
            {
                ChiTietPhieuXuats.Remove(SelectedChiTiet);
                OnPropertyChanged(nameof(TongTienPhieu));
            }
        }

        private bool CanSavePhieuXuat()
        {
            return ChiTietPhieuXuats.Count > 0 && !string.IsNullOrWhiteSpace(CurrentPhieuXuat.MaPhieu);
        }

        private void SavePhieuXuat()
        {
            try
            {
                CurrentPhieuXuat.TongTien = TongTienPhieu;
                _repository.SavePhieuXuat(CurrentPhieuXuat, ChiTietPhieuXuats.ToList());
                
                MessageBox.Show("Xu?t kho thành công!", "Thông báo");
                
                DanhSachLinhKien = new ObservableCollection<LinhKien>(_lkRepository.GetAll());
                InitNewPhieu();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"L?i xu?t kho: {ex.Message}", "L?i", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}