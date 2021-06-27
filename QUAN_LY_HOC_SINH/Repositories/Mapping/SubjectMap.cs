using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Repositories.Models;

namespace Repositories.Mapping
{
    public class SubjectMap : ClassMapping<Subject>
    {
        public SubjectMap()
        {
            Table("MON_HOC");
            Id(x => x.Id, m =>
            {
                m.Generator(Generators.GuidComb);
                m.Column("ID");
            });
            Version(x => x.Version, m =>
            {
                m.Column("VERSION");
            });
            Property(x => x.SubjectId, m =>
            {
                m.Column(c => c.SqlType("numeric(3,0)"));
                m.NotNullable(true);
                m.Unique(true);
                m.Column("MA_SO");
            });
            Property(x => x.Name, m =>
            {
                m.Column(c => c.SqlType("nvarchar(255)"));
                m.NotNullable(true);
                m.Unique(true);
                m.Column("TEN");
            });
        }
    }
}
