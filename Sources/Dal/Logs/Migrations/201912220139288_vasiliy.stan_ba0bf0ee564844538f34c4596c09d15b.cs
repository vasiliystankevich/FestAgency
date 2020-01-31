namespace Logs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vasiliystan_ba0bf0ee564844538f34c4596c09d15b : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BackendWebApiLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Thread = c.String(nullable: false, maxLength: 255),
                        Level = c.String(nullable: false, maxLength: 50),
                        Logger = c.String(nullable: false, maxLength: 255),
                        Message = c.String(nullable: false, maxLength: 4000),
                        Exception = c.String(nullable: false, maxLength: 2000),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BackendWebApiLogs");
        }
    }
}
