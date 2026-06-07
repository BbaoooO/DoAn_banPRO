using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DoAn_banPRO.Models;

namespace DoAn_banPRO.Repositories
{
    public class LinhKienRepository : ILinhKienRepository
    {
        public IEnumerable<LinhKien> GetAll()
        {
            using (var context = new KhodientuContext())
            {
                var query = from lk in context.LinhKiens
                            join lh in context.LoaiHangs on lk.MaLoai equals lh.MaLoai into lhGroup
                            from lh in lhGroup.DefaultIfEmpty()
                            join ncc in context.NhaCungCaps on lk.MaNCC equals ncc.MaNCC into nccGroup
                            from ncc in nccGroup.DefaultIfEmpty()
                            select new LinhKien
                            {
                                MaLK = lk.MaLK,
                                TenLK = lk.TenLK,
                                TonKho = lk.TonKho,
                                MaLoai = lk.MaLoai,
                                MaNCC = lk.MaNCC,
                                DonGia = lk.DonGia,
                                DonViTinh = lk.DonViTinh,
                                TenLoai = lh != null ? lh.TenLoai : null,
                                TenNCC = ncc != null ? ncc.TenNCC : null
                            };
                return query.ToList();
            }
        }

        public void Add(LinhKien linhKien)
        {
            using (var context = new KhodientuContext())
            {
                context.LinhKiens.Add(linhKien);
                context.SaveChanges();
            }
        }

        public void Update(LinhKien linhKien)
        {
            using (var context = new KhodientuContext())
            {
                context.LinhKiens.Update(linhKien);
                context.SaveChanges();
            }
        }

        public void Delete(string maLK)
        {
            using (var context = new KhodientuContext())
            {
                var linhKien = context.LinhKiens.Find(maLK);
                if (linhKien != null)
                {
                    context.LinhKiens.Remove(linhKien);
                    context.SaveChanges();
                }
            }
        }

        public IEnumerable<LinhKien> Search(string keyword)
        {
            using (var context = new KhodientuContext())
            {
                var query = from lk in context.LinhKiens
                            where lk.TenLK.Contains(keyword) || lk.MaLK.Contains(keyword)
                            join lh in context.LoaiHangs on lk.MaLoai equals lh.MaLoai into lhGroup
                            from lh in lhGroup.DefaultIfEmpty()
                            join ncc in context.NhaCungCaps on lk.MaNCC equals ncc.MaNCC into nccGroup
                            from ncc in nccGroup.DefaultIfEmpty()
                            select new LinhKien
                            {
                                MaLK = lk.MaLK,
                                TenLK = lk.TenLK,
                                TonKho = lk.TonKho,
                                MaLoai = lk.MaLoai,
                                MaNCC = lk.MaNCC,
                                DonGia = lk.DonGia,
                                DonViTinh = lk.DonViTinh,
                                TenLoai = lh != null ? lh.TenLoai : null,
                                TenNCC = ncc != null ? ncc.TenNCC : null
                            };
                return query.ToList();
            }
        }
    }
}
