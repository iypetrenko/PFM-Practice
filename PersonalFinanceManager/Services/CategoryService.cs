using PersonalFinanceManager.Data;
using PersonalFinanceManager.Models;
using System.Collections.Generic;
using System.Linq;

namespace PersonalFinanceManager.Services
{
    public class CategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService()
        {
            _context = new ApplicationDbContext();
        }

        public List<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }
    }
}