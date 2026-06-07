using System;
using System.Collections.Generic;
using System.Linq;
using DoAn_banPRO.Models;

namespace DoAn_banPRO.Repositories
{
    public class LoaiHangRepository : ILoaiHangRepository
    {
        public IEnumerable<LoaiHang> GetAll()
        {
            using (var context = new KhodientuContext())
            {
                return context.LoaiHangs.ToList();
            }
        }

        public void Add(LoaiHang loaiHang)
        {
            using (var context = new KhodientuContext())
            {
                context.LoaiHangs.Add(loaiHang);
                context.SaveChanges();
            }
        }

        public void Update(LoaiHang loaiHang)
        {
            using (var context = new KhodientuContext())
            {
                context.LoaiHangs.Update(loaiHang);
                context.SaveChanges();
            }
        }

        public void Delete(string maLoai)
        {
            using (var context = new KhodientuContext())
            {
                // check if exists in LinhKien first
                var isUsed = context.LinhKiens.Any(lk => lk.MaLoai == maLoai);
                if (isUsed)
                {
                    throw new Exception("Lo?i hàng này ?ang ???c s? d?ng cho linh ki?n! Không th? xóa.");
                }

                var lh = context.LoaiHangs.Find(maLoai);
                if (lh != null)
                {
                    context.LoaiHangs.Remove(lh);
                    context.SaveChanges();
                }
            }
        }
    }
}