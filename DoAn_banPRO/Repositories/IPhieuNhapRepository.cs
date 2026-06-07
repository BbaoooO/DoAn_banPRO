using System.Collections.Generic;
using DoAn_banPRO.Models;

namespace DoAn_banPRO.Repositories
{
    public interface IPhieuNhapRepository
    {
        void SavePhieuNhap(PhieuNhap phieuNhap, List<ChiTietPhieuNhap> chiTietList);
        string GetNextMaPhieu(); 
    }
}