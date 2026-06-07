using System.Collections.Generic;
using DoAn_banPRO.Models;

namespace DoAn_banPRO.Repositories
{
    public interface ILoaiHangRepository
    {
        IEnumerable<LoaiHang> GetAll();
        void Add(LoaiHang loaiHang);
        void Update(LoaiHang loaiHang);
        void Delete(string maLoai);
    }
}