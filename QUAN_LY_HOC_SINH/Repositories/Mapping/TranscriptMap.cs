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
    public class TranscriptMap : ClassMapping<Transcript>
    {
        public TranscriptMap()
        {
            Table("BANG_DIEM");
            Id(x => x.Id, m => {
                m.Generator(Generators.GuidComb);
                m.Column("ID");
            });
            Version(x => x.Version, m => {
                m.Column("VERSION");
            });
            Property(x => x.Semester, m => {
                m.NotNullable(true);
                m.Column("HOC_KY");
            });
            Property(x => x.FifteenMinuteTestScore, m => {
                m.NotNullable(true);
                m.Column("DIEM_15_PHUT");
            });
            Property(x => x.FortyFiveMinuteTestScore, m => {
                m.NotNullable(true);
                m.Column("DIEM_1_TIET");
            });
            Property(x => x.FinalTestScore, m => {
                m.NotNullable(true);
                m.Column("DIEM_CUOI_KY");
            });
        }
    }
}
