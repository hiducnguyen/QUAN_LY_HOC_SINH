using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QUAN_LY_HOC_SINH.Models
{
    public class LOP
    {
        public virtual int ID { get; set; }
        public virtual string TEN_LOP { get; set; }
        public virtual IList<HOC_SINH> DANH_SACH_HOC_SINH { get; set; }
        public virtual int SI_SO { get => DANH_SACH_HOC_SINH.Count; }
    }
}