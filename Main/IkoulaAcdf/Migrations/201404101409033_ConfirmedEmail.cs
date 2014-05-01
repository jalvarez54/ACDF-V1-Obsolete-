namespace IkoulaACDF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConfirmedEmail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ConfirmedEmail", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ConfirmedEmail");
        }
    }
}
