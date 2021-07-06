using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode.Conformist;

namespace Repositories.Mapping
{
    public class TranscriptOfClassMap : ClassMapping<TranscriptOfClass>
    {
        public TranscriptOfClassMap()
        {
            Table("BANG_DIEM_LOP");
            ComposedId(m =>
            {
                m.Property(x => x.SubjectId, mapSubjectId =>
                {
                    mapSubjectId.NotNullable(true);
                    mapSubjectId.Column("MA_SO_MON_HOC");
                });
                m.Property(x => x.ClassName, mapClassName =>
                {
                    mapClassName.NotNullable(true);
                    mapClassName.Column("TEN_LOP");
                });
                m.Property(x => x.Semester, mapSemester =>
                {
                    mapSemester.NotNullable(true);
                    mapSemester.Column("HOC_KY");
                });
            });
            Property(x => x.SubjectName, m =>
            {
                m.NotNullable(true);
                m.Column("TEN_MON_HOC");
            });
        }
    }
}
