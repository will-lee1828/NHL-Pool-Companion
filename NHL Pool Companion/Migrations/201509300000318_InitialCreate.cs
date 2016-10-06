namespace NHL_Pool_Companion.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        PlayerID = c.Int(nullable: false, identity: true),
                        Position = c.Int(nullable: false),
                        PlayerName = c.String(maxLength: 4000),
                        Team = c.String(maxLength: 4000),
                        Salary = c.Int(nullable: false),
                        Picked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PlayerID);
            
            CreateTable(
                "dbo.Statistics",
                c => new
                    {
                        StatisticsID = c.Int(nullable: false, identity: true),
                        GamesPlayed = c.Int(nullable: false),
                        Goals = c.Int(nullable: false),
                        Assits = c.Int(nullable: false),
                        Points = c.Int(nullable: false),
                        PlusMinus = c.Int(nullable: false),
                        Wins = c.Int(nullable: false),
                        Losses = c.Int(nullable: false),
                        OverTimeLosses = c.Int(nullable: false),
                        ShutOut = c.Int(nullable: false),
                        PlayerID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StatisticsID)
                .ForeignKey("dbo.Players", t => t.PlayerID, cascadeDelete: true)
                .Index(t => t.PlayerID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Statistics", "PlayerID", "dbo.Players");
            DropIndex("dbo.Statistics", new[] { "PlayerID" });
            DropTable("dbo.Statistics");
            DropTable("dbo.Players");
        }
    }
}
