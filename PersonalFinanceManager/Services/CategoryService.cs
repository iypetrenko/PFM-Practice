using PersonalFinanceManager.Models;
using PersonalFinanceManager.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services
{
    public class CategoryService : ICategoryService
    {
        // Временное хранилище в памяти
        private readonly List<Category> _categories = new();
        private int _nextId = 1;

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await Task.FromResult(new List<Category>());
        }

        public async Task AddCategory(Category category)
        {
            category.Id = _nextId++;
            _categories.Add(category);
            await Task.CompletedTask;
        }
    }
}