namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateForeignKeyproduct : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.tb_ProductImage", "ProductId");
            AddForeignKey("dbo.tb_ProductImage", "ProductId", "dbo.tb_Product", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tb_ProductImage", "ProductId", "dbo.tb_Product");
            DropIndex("dbo.tb_ProductImage", new[] { "ProductId" });
        }
    }
}
