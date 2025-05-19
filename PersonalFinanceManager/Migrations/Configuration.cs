namespace PersonalFinanceManager.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PersonalFinanceManager.Model.PersonalFinanceManagerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(PersonalFinanceManager.Model.PersonalFinanceManagerContext context){}
    }
}
