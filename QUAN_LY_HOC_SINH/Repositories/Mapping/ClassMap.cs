using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Mapping
{
    public class ClassMap : ClassMapping<Class>
    {
        public ClassMap()
        {
            Table("LOP");
            Id(x => x.Id, m => {
                m.Generator(Generators.GuidComb);
                m.Column("ID");
            });
            Version(x => x.Version, m => {
                m.Column("VERSION");
            });
            Property(x => x.Name, m => {
                m.NotNullable(true);
                m.Column("TEN");
            });
            Set(x => x.Students, m => {
                m.Key(k => k.Column("LOP_ID"));
                m.Table("HOC_SINH");
            }, r => r.OneToMany());
        }
    }
}
