using System.Collections.Generic;
using DoAn_banPRO.Models;

namespace DoAn_banPRO.Repositories
{
    public interface IPhieuXuatRepository
    {
        void SavePhieuXuat(PhieuXuat phieuXuat, List<ChiTietPhieuXuat> chiTietList);
        string GetNextMaPhieu();
    }
}