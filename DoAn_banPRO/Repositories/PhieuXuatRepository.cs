using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DoAn_banPRO.Models;

namespace DoAn_banPRO.Repositories
{
    public class PhieuXuatRepository : IPhieuXuatRepository
    {
        public void SavePhieuXuat(PhieuXuat phieuXuat, List<ChiTietPhieuXuat> chiTietList)
        {
            using (var context = new KhodientuContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // 1. Thêm Phi?u Xu?t
                        context.PhieuXuats.Add(phieuXuat);
                        context.SaveChanges();

                        // 2. Thêm Chi ti?t & Tr? T?n Kho
                        foreach (var chitiet in chiTietList)
                        {
                            chitiet.MaPX = phieuXuat.MaPhieu;
                            context.ChiTietPhieuXuats.Add(chitiet);
                            
                            var linhkien = context.LinhKiens.Find(chitiet.MaLK);
                            if (linhkien != null)
                            {
                                if (linhkien.TonKho < chitiet.SoLuong)
                                {
                                    throw new Exception($"Linh ki?n '{linhkien.TenLK}' không ?? s? l??ng t?n kho (T?n: {linhkien.TonKho}, C?n: {chitiet.SoLuong}).");
                                }
                                
                                linhkien.TonKho -= chitiet.SoLuong ?? 0;
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
                var lastPhieu = context.PhieuXuats
                                       .OrderByDescending(p => p.MaPhieu)
                                       .FirstOrDefault();
                
                if (lastPhieu == null)
                    return "PX0001";
                
                var lastCode = lastPhieu.MaPhieu.Trim();
                if (lastCode.Length == 6 && lastCode.StartsWith("PX"))
                {
                    int numPart;
                    if (int.TryParse(lastCode.Substring(2), out numPart))
                    {
                        return $"PX{(numPart + 1):D4}";
                    }
                }
                
                return "PX0001";
            }
        }
    }
}