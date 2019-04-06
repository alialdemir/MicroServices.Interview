using FluentMigrator;

namespace MicroServices.Interview.Personnel.API.Migrations
{
    [Migration(1)]
    public class FirstMigration : Migration
    {
        /// <summary>
        /// Personel tablosunu veri tabanındaki sütunlarını ayarlar
        /// </summary>
        public override void Up()
        {
            Create.Table("Personnel")
                 .WithColumn("PersonnelId")
                 .AsInt32()
                 .PrimaryKey()
                 .Identity()

                 .WithColumn("FullName")
                 .AsString(150)
                 .NotNullable()

                 .WithColumn("Age")
                 .AsByte()
                 .NotNullable()
                 .WithDefaultValue(1)

                 .WithColumn("City")
                 .AsString(200)
                 .NotNullable();
        }

        public override void Down()
        {
            Delete.Table("Personnel");
        }
    }
}