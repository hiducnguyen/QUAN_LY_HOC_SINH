using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Repositories.Models;

namespace Repositories.Mapping
{
    public class StudentMap : ClassMapping<Student>
    {
        public StudentMap()
        {
            Table("HOC_SINH");
            Id(x => x.Id, m =>
            {
                m.Generator(Generators.GuidComb);
                m.Column("ID");
            });
            Version(x => x.Version, m =>
            {
                m.Column("VERSION");
            });
            Property(x => x.StudentId, m =>
            {
                m.Column(c => c.SqlType("numeric(4,0)"));
                m.NotNullable(true);
                m.Unique(true);
                m.Column("MA_SO");
            });
            Property(x => x.Name, m =>
            {
                m.Column(c => c.SqlType("nvarchar(255)"));
                m.NotNullable(true);
                m.Column("HO_TEN");
            });
            Property(x => x.Gender, m =>
            {
                m.NotNullable(true);
                m.Column("GIOI_TINH");
            });
            Property(x => x.BirthDate, m =>
            {
                m.Column(c => c.SqlType("date"));
                m.NotNullable(true);
                m.Column("NGAY_SINH");
            });
            Property(x => x.Address, m =>
            {
                m.Column(c => c.SqlType("nvarchar(255)"));
                m.NotNullable(true);
                m.Column("DIA_CHI");
            });
            Property(x => x.Email, m =>
            {
                m.Column(c => c.SqlType("nvarchar(50)"));
                m.NotNullable(true);
                m.Column("EMAIL");
            });
            Property(x => x.ClassId, m =>
            {
                m.Column("LOP_ID");
            });
            Set(x => x.Transcripts, m =>
            {
                m.Key(k => k.Column("HOC_SINH_ID"));
                m.Table("BANG_DIEM");
            }, r => r.OneToMany());
        }
    }
}
