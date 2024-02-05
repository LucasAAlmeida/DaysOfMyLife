namespace DomL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTypetoMovieTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movie", "Type", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Movie", "Type");
        }
    }
}
