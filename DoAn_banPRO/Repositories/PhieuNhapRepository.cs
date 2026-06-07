using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DoAn_banPRO.Models;

namespace DoAn_banPRO.Repositories
{
    public class PhieuNhapRepository : IPhieuNhapRepository
    {
        public void SavePhieuNhap(PhieuNhap phieuNhap, List<ChiTietPhieuNhap> chiTietList)
        {
            using (var context = new KhodientuContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // 1. Thêm Phi?u Nh?p
                        context.PhieuNhaps.Add(phieuNhap);
                        context.SaveChanges(); // L?y MaPhieu (n?u là t? sinh, ? ?ây MaPhieu là char nên ?ã gán tr??c)

                        // 2. Thêm Chi ti?t phi?u nh?p
                        foreach (var chitiet in chiTietList)
                        {
                            chitiet.MaPN = phieuNhap.MaPhieu;
                            context.ChiTietPhieuNhaps.Add(chitiet);
                            
                            // 3. C?p nh?t T?n kho
                            var linhkien = context.LinhKiens.Find(chitiet.MaLK);
                            if (linhkien != null)
                            {
                                linhkien.TonKho += chitiet.SoLuong ?? 0;
                                context.LinhKiens.Update(linhkien);
                            }
                            else
                            {
                                throw new Exception($"Linh ki?n v?i mã {chitiet.MaLK} không t?n t?i!");
                            }
                        }

                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public string GetNextMaPhieu()
        {
            using (var context = new KhodientuContext())
            {
                var lastPhieu = context.PhieuNhaps
                                       .OrderByDescending(p => p.MaPhieu)
                                       .FirstOrDefault();
                
                if (lastPhieu == null)
                    return "PN0001";
                
                // Gi?i ??nh chu?i PN + 4 s?, VD: PN0001
                var lastCode = lastPhieu.MaPhieu.Trim();
                if (lastCode.Length == 6 && lastCode.StartsWith("PN"))
                {
                    int numPart;
                    if (int.TryParse(lastCode.Substring(2), out numPart))
                    {
                        return $"PN{(numPart + 1):D4}";
                    }
                }
                
                return "PN0001";
            }
        }
    }
}