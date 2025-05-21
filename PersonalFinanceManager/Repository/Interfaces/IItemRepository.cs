using System;
using System.Collections.Generic;
using PersonalFinanceManager.Model;

namespace PersonalFinanceManager.Repository.Interface
{
    public interface IItemRepository
    {
        IEnumerable<Item> GetItems(int categoryId);
        IEnumerable<Item> GetUserItems(int userId);
        List<Item> GetAllItems();
        bool AddNewItem(Item todoItem);
        bool DeleteItem(Item item);
        bool UpdateItemPrice(int itemId, decimal newPrice);
        List<Item> FilterItems(string searchText, string selectedItem, DateTime date);
    }

}