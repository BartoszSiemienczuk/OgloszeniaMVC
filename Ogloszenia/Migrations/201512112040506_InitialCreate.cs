namespace Ogloszenia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ad",
                c => new
                    {
                        AdID = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        Content = c.String(),
                        ExpirationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AdID);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        CategoryID = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        ParentCategory_CategoryID = c.Long(),
                        Ad_AdID = c.Long(),
                    })
                .PrimaryKey(t => t.CategoryID)
                .ForeignKey("dbo.Category", t => t.ParentCategory_CategoryID)
                .ForeignKey("dbo.Ad", t => t.Ad_AdID)
                .Index(t => t.ParentCategory_CategoryID)
                .Index(t => t.Ad_AdID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Category", "Ad_AdID", "dbo.Ad");
            DropForeignKey("dbo.Category", "ParentCategory_CategoryID", "dbo.Category");
            DropIndex("dbo.Category", new[] { "Ad_AdID" });
            DropIndex("dbo.Category", new[] { "ParentCategory_CategoryID" });
            DropTable("dbo.Category");
            DropTable("dbo.Ad");
        }
    }
}
