namespace ThePlanner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OccasiongAdded : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.OccasionApplicationUsers", newName: "ApplicationUserOccasions");
            DropPrimaryKey("dbo.ApplicationUserOccasions");
            AddPrimaryKey("dbo.ApplicationUserOccasions", new[] { "ApplicationUser_Id", "Occasion_Id" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.ApplicationUserOccasions");
            AddPrimaryKey("dbo.ApplicationUserOccasions", new[] { "Occasion_Id", "ApplicationUser_Id" });
            RenameTable(name: "dbo.ApplicationUserOccasions", newName: "OccasionApplicationUsers");
        }
    }
}
