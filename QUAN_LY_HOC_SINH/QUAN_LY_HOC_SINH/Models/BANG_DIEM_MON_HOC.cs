using QUAN_LY_HOC_SINH.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QUAN_LY_HOC_SINH.Models
{
    public class BANG_DIEM_MON_HOC
    {
        public virtual int ID { get; set; }
        public virtual MON_HOC MON_HOC { get; set; }
        public virtual HocKi HOC_KY { get; set; }
        public virtual float Diem15phut { get; set; }
        public virtual float Diem1tiet { get; set; }
        public virtual float DiemCuoiKy { get; set; }
        public virtual float DiemTrungBinh { get => (Diem15phut + Diem1tiet * 2 + DiemCuoiKy * 3) / 6; }
    }
}