namespace ThePlanner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Occasions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Topic = c.String(),
                        Date = c.DateTime(nullable: false),
                        Location = c.String(),
                        MembersLimitCount = c.Int(nullable: false),
                        MembersCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InputFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.String(),
                        OccasionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Occasions", t => t.OccasionId, cascadeDelete: true)
                .Index(t => t.OccasionId);
            
            CreateTable(
                "dbo.OccasionApplicationUsers",
                c => new
                    {
                        Occasion_Id = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Occasion_Id, t.ApplicationUser_Id })
                .ForeignKey("dbo.Occasions", t => t.Occasion_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.Occasion_Id)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OccasionApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.OccasionApplicationUsers", "Occasion_Id", "dbo.Occasions");
            DropForeignKey("dbo.InputFields", "OccasionId", "dbo.Occasions");
            DropIndex("dbo.OccasionApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.OccasionApplicationUsers", new[] { "Occasion_Id" });
            DropIndex("dbo.InputFields", new[] { "OccasionId" });
            DropTable("dbo.OccasionApplicationUsers");
            DropTable("dbo.InputFields");
            DropTable("dbo.Occasions");
        }
    }
}
