namespace ThePlanner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EnumProp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Gender", c => c.Int(nullable: false));
            DropColumn("dbo.AspNetUsers", "Location");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Location", c => c.String());
            DropColumn("dbo.AspNetUsers", "Gender");
        }
    }
}
