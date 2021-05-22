using QUAN_LY_HOC_SINH.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QUAN_LY_HOC_SINH.Models
{
    public class HOC_SINH
    {
        public virtual int ID { get; set; }
        public virtual string HO_TEN { get; set; }
        public virtual GioiTinh GIOI_TINH { get; set; }
        public virtual DateTime NGAY_SINH { get; set; }
        public virtual string DIA_CHI { get; set; }
        public virtual string EMAIL { get; set; }
        public virtual ISet<BANG_DIEM_MON_HOC> DANH_SACH_BANG_DIEM { get; set; }
    }
}