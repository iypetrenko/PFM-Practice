using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows.Documents;
using PersonalFinanceManager.Model;
using PersonalFinanceManager.Repository.Interface;


namespace PersonalFinanceManager.Repository
{
    public class ItemRepository : IItemRepository
    {
        public bool AddNewItem(Item todoItem)
        {
            using (var db = new PersonalFinanceManagerContext())
            {
                db.Items.Add(todoItem);
                var result = db.SaveChanges();
                return result > 0;
            }
        }

        public List<Item> GetItems(int todoId)
        {
            using (var db = new PersonalFinanceManagerContext())
            {
                var result = db.Items.Where(p => p.ToDoListId == todoId).ToList();
                return result;
            }
        }

        public List<Item> GetAllItems()
        {
            using (var db = new PersonalFinanceManagerContext())
            {
                var result = db.Items.ToList();
                return result;
            }
        }

        public bool DeleteItem(Item item)
        {
            using (var db = new PersonalFinanceManagerContext())
            {
                db.Items.Attach(item);
                db.Entry(item).State = EntityState.Deleted;
                var result = db.SaveChanges();
                return result > 0;
            }
        }

        public bool UpdateItemPrice(int itemId, decimal newPrice)
        {
            using (var db = new PersonalFinanceManagerContext())
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
            using (var db = new PersonalFinanceManagerContext())
            {
                List<Item> items;
                if (selectedItem == "All")
                {
                    items = db.Items.Where(p => p.Name.Contains(searchText)
                                               && p.BuyDate <= date).ToList();
                }
                else
                {
                    items = db.Items.ToList();

                    // for now we select all items here but here we could search only for one field
                    // eg: Price is selected in the combo box, then we search only by price
                 
                }
                return items;
            }
        }
    }
}