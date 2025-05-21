using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PersonalFinanceManager.Model;
using PersonalFinanceManager.Repository.Interface;

namespace PersonalFinanceManager.Repository
{
    public class ItemRepository : IItemRepository
    {
        protected virtual PersonalFinanceManagerContext GetContext()
        {
            return new PersonalFinanceManagerContext();
        }
        public IEnumerable<Item> GetUserItems(int userId)
        {
            using (var context = new PersonalFinanceManagerContext())
            {
                return context.Items
                    .Include(i => i.ExpenseCategory) // Добавляем включение связанных данных
                    .Where(i => i.ExpenseCategory.UserId == userId)
                    .ToList();
            }
        }
        public bool AddNewItem(Item todoItem)
        {
            using (var db = GetContext())
            {
                db.Items.Add(todoItem);
                var result = db.SaveChanges();
                return result > 0;
            }
        }

        public IEnumerable<Item> GetItems(int categoryId)
        {
            using (var db = GetContext())
            {
                return db.Items.Where(p => p.ToDoListId == categoryId).ToList();
            }
        }

        public List<Item> GetAllItems()
        {
            using (var db = GetContext())
            {
                return db.Items.ToList();
            }
        }

        public bool DeleteItem(Item item)
        {
            using (var db = GetContext())
            {
                db.Items.Attach(item);
                db.Entry(item).State = EntityState.Deleted;
                var result = db.SaveChanges();
                return result > 0;
            }
        }

        public bool UpdateItemPrice(int itemId, decimal newPrice)
        {
            using (var db = GetContext())
            {
                var item = db.Items.FirstOrDefault(p => p.Id == itemId);
                item.Price = newPrice;
                db.Items.Attach(item);
                db.Entry(item).State = EntityState.Modified;
                var result = db.SaveChanges();
                return result > 0;
            }
        }

        public List<Item> FilterItems(string searchText, string selectedItem, DateTime date)
        {
            using (var db = GetContext())
            {
                if (selectedItem == "All")
                {
                    return db.Items.Where(p => p.Name.Contains(searchText)
                                               && p.BuyDate <= date).ToList();
                }
                else
                {
                    return db.Items.ToList(); // возможно, здесь логика будет расширяться
                }
            }
        }
    }
}
