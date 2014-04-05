namespace IkoulaACDF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RegistrationDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "RegistrationDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "RegistrationDate");
        }
    }
}
