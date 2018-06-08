namespace ThePlanner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class additionalProps : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Age", c => c.Int(nullable: true));
            AddColumn("dbo.AspNetUsers", "Location", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Location");
            DropColumn("dbo.AspNetUsers", "Age");
        }
    }
}
