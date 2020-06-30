namespace BadBrokerTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Currencies",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        SellCurrency = c.Int(nullable: false),
                        BuyCurrency = c.Int(nullable: false),
                        Id = c.Guid(nullable: false),
                        ExchangeRate = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.Date, t.SellCurrency, t.BuyCurrency });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Currencies");
        }
    }
}
