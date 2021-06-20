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
    public class SubjectMap : ClassMapping<Subject>
    {
        public SubjectMap()
        {
            Table("MON_HOC");
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
            Set(x => x.Transcripts, m =>
            {
                m.Key(k => k.Column("MON_HOC_ID"));
                m.Table("BANG_DIEM");
            }, r => r.OneToMany());
        }
    }
}
