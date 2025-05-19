using System;
using System.Collections.Generic;
using PersonalFinanceManager.Model;

namespace PersonalFinanceManager.Repository.Interface
{
    internal interface IItemRepository
    {
        bool AddNewItem(Item todoItem);

        List<Item> GetItems(int todoId);

        List<Item> GetAllItems();

        bool DeleteItem(Item item);

        bool UpdateItemPrice(int itemId, decimal newPrice);

        List<Item> FilterItems(string searchText, string selectedItem, DateTime date);
    }
}