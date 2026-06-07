using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using DoAn_banPRO.Helpers;
using DoAn_banPRO.Repositories;
using DoAn_banPRO.Models;
using DoAn_banPRO.Services;

namespace DoAn_banPRO.ViewModels
{
    public class ThongKeViewModel : BaseViewModel
    {
        private readonly KhodientuContext _context;

        public int TongSoLinhKien { get; set; }
        public decimal TongGiaTriTonKho { get; set; }
        public int TongPhieuNhap { get; set; }

        public ObservableCollection<LinhKien> LinhKienTonKhoThap { get; set; }
        public ObservableCollection<PhieuNhap> PhieuNhapGanDay { get; set; }

        public ICommand HienThiBangThongKeCommand { get; }
        public ICommand XuatBaoCaoCommand { get; }

        public ThongKeViewModel()
        {
            _context = new KhodientuContext();
            
            HienThiBangThongKeCommand = new RelayCommand<object>(p => LoadThongKe(), p => true);
            XuatBaoCaoCommand = new RelayCommand<object>(p => SetupXuatBaoCao(), p => true);

            LoadThongKe();
        }

        private void LoadThongKe()
        {
            TongSoLinhKien = _context.LinhKiens.Count();
            TongGiaTriTonKho = _context.LinhKiens.Sum(lk => lk.TonKho * lk.DonGia) ;
            TongPhieuNhap = _context.PhieuNhaps.Count();
            
            OnPropertyChanged(nameof(TongSoLinhKien));
            OnPropertyChanged(nameof(TongGiaTriTonKho));
            OnPropertyChanged(nameof(TongPhieuNhap));

            // Load t?n kho th?p <= 5
            var dsThap = _context.LinhKiens.Where(lk => lk.TonKho <= 5).ToList();
            LinhKienTonKhoThap = new ObservableCollection<LinhKien>(dsThap);
            OnPropertyChanged(nameof(LinhKienTonKhoThap));

            // 10 phi?u nh?p g?n ?ây 
            var phieuNhap = _context.PhieuNhaps.OrderByDescending(p => p.NgayLap)
                                               .Take(10).ToList();
            PhieuNhapGanDay = new ObservableCollection<PhieuNhap>(phieuNhap);
            OnPropertyChanged(nameof(PhieuNhapGanDay));
        }

        private void SetupXuatBaoCao()
        {
            var rptService = new ReportService();
            rptService.ExportLinhKienToCSV();
        }
    }
}