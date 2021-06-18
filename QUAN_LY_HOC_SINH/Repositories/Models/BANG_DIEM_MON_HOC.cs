using Repositories.Enums;

namespace Repositories.Models
{
    public class BANG_DIEM_MON_HOC
    {
        public virtual int ID { get; set; }
        public virtual int MON_HOC_ID { get; set; }
        public virtual HocKi HOC_KY { get; set; }
        public virtual float Diem15phut { get; set; }
        public virtual float Diem1tiet { get; set; }
        public virtual float DiemCuoiKy { get; set; }
        public virtual float DiemTrungBinh { get => (Diem15phut + Diem1tiet * 2 + DiemCuoiKy * 3) / 6; }
    }
}