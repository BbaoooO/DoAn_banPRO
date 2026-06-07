using System.Linq;
using DoAn_banPRO.Models;

namespace DoAn_banPRO.Repositories
{
    public interface INguoiDungRepository
    {
        NguoiDung GetByTaiKhoanMatKhau(string taiKhoan, string matKhau);
    }

    public class NguoiDungRepository : INguoiDungRepository
    {
        public NguoiDung GetByTaiKhoanMatKhau(string taiKhoan, string matKhau)
        {
            using (var context = new KhodientuContext())
            {
                return context.NguoiDungs.FirstOrDefault(x => x.TaiKhoan == taiKhoan && x.MatKhau == matKhau);
            }
        }
    }
}