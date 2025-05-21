using System.Collections.Generic;
using PersonalFinanceManager.Model;

namespace PersonalFinanceManager.Repository.Interface
{
    public interface IExpenseCategoryRepository
    {
        bool AddExpenseCategory(string categoryName, decimal budget);

        List<ExpenseCategory> GetExpenseCategoriesList();

        bool RemoveExpenseCategory(int id);
    }
}