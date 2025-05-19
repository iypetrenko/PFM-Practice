using System.Collections.Generic;
using System.Linq;
using PersonalFinanceManager.Consts;
using PersonalFinanceManager.Model;
using PersonalFinanceManager.Repository.Interface;

namespace PersonalFinanceManager.Repository
{
    public class ExpenseCategoryRepository : IExpenseCategoryRepository
    {
        public bool AddExpenseCategory(string categoryName, decimal budget)
        {
            using (var db = new PersonalFinanceManagerContext())
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
            using (var db = new PersonalFinanceManagerContext())
            {
                return db.ExpenseCategories.Where(p => p.UserId == SessionInfo.UserId).ToList();
            }
        }

        public bool RemoveExpenseCategory(int id)
        {
            using (var db = new PersonalFinanceManagerContext())
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