using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Repositories.Models;

namespace Repositories.Mapping
{
    public class ClassMap : ClassMapping<Class>
    {
        public ClassMap()
        {
            Table("LOP");
            Id(x => x.Id, m =>
            {
                m.Generator(Generators.GuidComb);
                m.Column("ID");
            });
            Version(x => x.Version, m =>
            {
                m.Column("VERSION");
            });
            Property(x => x.Name, m =>
            {
                m.Column(c => c.SqlType("nvarchar(255)"));
                m.NotNullable(true);
                m.Unique(true);
                m.Column("TEN");
            });
            Set(x => x.Students, m =>
            {
                m.Key(k => k.Column("LOP_ID"));
                m.Table("HOC_SINH");
            }, r => r.OneToMany());
        }
    }
}
