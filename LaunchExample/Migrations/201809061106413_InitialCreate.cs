namespace Cyclr.LaunchExample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Contact",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        EmailAddress = c.String(nullable: false, maxLength: 320),
                        PhoneNumber = c.String(),
                        Organisation_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organisation", t => t.Organisation_Id)
                .Index(t => t.EmailAddress, unique: true)
                .Index(t => t.Organisation_Id);
            
            CreateTable(
                "dbo.Organisation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Address = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OAuthToken",
                c => new
                    {
                        AccessToken = c.String(nullable: false, maxLength: 128),
                        RefreshToken = c.String(),
                        Expires = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AccessToken);
            
            CreateTable(
                "dbo.Opportunity",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DueDate = c.DateTime(),
                        Status = c.String(),
                        Contact_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contact", t => t.Contact_Id, cascadeDelete: true)
                .Index(t => t.Contact_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Opportunity", "Contact_Id", "dbo.Contact");
            DropForeignKey("dbo.Contact", "Organisation_Id", "dbo.Organisation");
            DropIndex("dbo.Opportunity", new[] { "Contact_Id" });
            DropIndex("dbo.Contact", new[] { "Organisation_Id" });
            DropIndex("dbo.Contact", new[] { "EmailAddress" });
            DropTable("dbo.Opportunity");
            DropTable("dbo.OAuthToken");
            DropTable("dbo.Organisation");
            DropTable("dbo.Contact");
        }
    }
}
