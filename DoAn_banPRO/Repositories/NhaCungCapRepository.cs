using System;
using System.Collections.Generic;
using System.Linq;
using DoAn_banPRO.Models;

namespace DoAn_banPRO.Repositories
{
    public interface INhaCungCapRepository
    {
        IEnumerable<NhaCungCap> GetAll();
        void Add(NhaCungCap ncc);
        void Update(NhaCungCap ncc);
        void Delete(string maNCC);
    }

    public class NhaCungCapRepository : INhaCungCapRepository
    {
        public IEnumerable<NhaCungCap> GetAll()
        {
            using (var context = new KhodientuContext())
            {
                return context.NhaCungCaps.ToList();
            }
        }

        public void Add(NhaCungCap ncc)
        {
            using (var context = new KhodientuContext())
            {
                context.NhaCungCaps.Add(ncc);
                context.SaveChanges();
            }
        }

        public void Update(NhaCungCap ncc)
        {
            using (var context = new KhodientuContext())
            {
                context.NhaCungCaps.Update(ncc);
                context.SaveChanges();
            }
        }

        public void Delete(string maNCC)
        {
            using (var context = new KhodientuContext())
            {
                // Ki?m tra xem Nhà CC có ?ang cung c?p linh ki?n nào không
                var isUsed = context.LinhKiens.Any(lk => lk.MaNCC == maNCC);
                if (isUsed)
                {
                    throw new Exception("Nhà cung cấp này đang được sử dụng cho linh kiệnn nên không thể xoá!");
                }

                var ncc = context.NhaCungCaps.Find(maNCC);
                if (ncc != null)
                {
                    context.NhaCungCaps.Remove(ncc);
                    context.SaveChanges();
                }
            }
        }
    }
}