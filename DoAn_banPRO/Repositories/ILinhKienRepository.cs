using System.Collections.Generic;
using DoAn_banPRO.Models;

namespace DoAn_banPRO.Repositories
{
    public interface ILinhKienRepository
    {
        IEnumerable<LinhKien> GetAll();
        void Add(LinhKien linhKien);
        void Update(LinhKien linhKien);
        void Delete(string maLK);
        IEnumerable<LinhKien> Search(string keyword);
    }
}
