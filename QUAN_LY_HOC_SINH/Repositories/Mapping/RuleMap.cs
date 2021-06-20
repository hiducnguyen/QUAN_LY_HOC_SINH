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
    public class RuleMap : ClassMapping<Rule>
    {
        public RuleMap()
        {
            Table("QUY_DINH");
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
            Property(x => x.Type, m => {
                m.NotNullable(true);
                m.Column("KIEU_DU_LIEU");
            });
            Property(x => x.Value, m => {
                m.NotNullable(true);
                m.Column("GIA_TRI");
            });
        }
    }
}
