namespace PersonalFinanceManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Items", "ToDoListId");
            AddForeignKey("dbo.Items", "ToDoListId", "dbo.ExpenseCategories", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "ToDoListId", "dbo.ExpenseCategories");
            DropIndex("dbo.Items", new[] { "ToDoListId" });
        }
    }
}
