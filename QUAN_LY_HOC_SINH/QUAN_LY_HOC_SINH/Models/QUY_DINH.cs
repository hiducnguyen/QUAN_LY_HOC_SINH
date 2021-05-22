using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QUAN_LY_HOC_SINH.Models
{
    public class QUY_DINH
    {
        public virtual int ID { get; set; }
        public virtual string TEN { get; set; }
        public virtual float GIA_TRI { get; set; }
    }
}