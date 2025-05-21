using System.Collections.Generic;
using PersonalFinanceManager.Model;

namespace PersonalFinanceManager.Repository.Interface
{
    public interface IExpenseCategoryRepository
    {
        List<ExpenseCategory> GetExpenseCategoriesList();
        bool AddExpenseCategory(string categoryName, decimal budget);
        bool RemoveExpenseCategory(int id);
    }
}