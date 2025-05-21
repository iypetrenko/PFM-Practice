using System.Collections.Generic;
using System.Linq;
using PersonalFinanceManager.Consts;
using PersonalFinanceManager.Model;
using PersonalFinanceManager.Repository.Interface;

namespace PersonalFinanceManager.Repository
{
    public class ExpenseCategoryRepository : IExpenseCategoryRepository
    {
        protected virtual PersonalFinanceManagerContext GetContext()
        {
            return new PersonalFinanceManagerContext();
        }

        public bool AddExpenseCategory(string categoryName, decimal budget)
        {
            using (var db = GetContext())
            {
                var newCategory = new ExpenseCategory
                {
                    Name = categoryName,
                    MonthlyBudget = budget,
                    UserId = SessionInfo.UserId
                };
                db.ExpenseCategories.Add(newCategory);
                var result = db.SaveChanges();
                return result > 0;
            }
        }

        public List<ExpenseCategory> GetExpenseCategoriesList()
        {
            using (var context = new PersonalFinanceManagerContext())
            {
                return context.ExpenseCategories
                    .Where(c => c.UserId == SessionInfo.UserId)
                    .ToList();
            }
        }

        public bool RemoveExpenseCategory(int id)
        {
            using (var db = GetContext())
            {
                var toDo = new ExpenseCategory { Id = id };
                db.ExpenseCategories.Attach(toDo);
                db.ExpenseCategories.Remove(toDo);
                var result = db.SaveChanges();
                return result > 0;
            }
        }
    }
}
