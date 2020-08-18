using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChallengeOne.Repository
{
    public class MenuRepository
    {
        private readonly List<MenuItem> _menu = new List<MenuItem>();

        // CREATE
        public void AddMenuItem(MenuItem item)
        {
            _menu.Add(item);
        }

        // READ
        public List<MenuItem> GetAllItems()
        {
            return _menu;
        }

        public MenuItem GetItemByNumber(int num)
        {
            foreach (MenuItem item in _menu)
            {
                if (item.Number == num)
                {
                    return item;
                }
            }
            return null;
        }

        public MenuItem GetItemByName(string name)
        {
            foreach (MenuItem item in _menu)
            {
                if (item.Name.ToLower() == name.ToLower())
                {
                    return item;
                }
            }
            return null;
        }

        // Useful for people with a budget
        public List<MenuItem> GetAllItemsUnderPrice(double budget)
        {
            List<MenuItem> itemsUnderBudget = new List<MenuItem>();
            foreach (MenuItem item in _menu)
            {
                if (item.Price <= budget)
                {
                    itemsUnderBudget.Add(item);
                }
            }
            return itemsUnderBudget;
        }

        // Useful for people with allergies
        public List<MenuItem> GetAllItemsWithIngredient(string ingredient)
        {
            List<MenuItem> itemsWithIngredient = new List<MenuItem>();
            foreach (MenuItem item in _menu)
            {
                if (item.Ingredients.Contains(ingredient))
                {
                    itemsWithIngredient.Add(item);
                }
            }
            return itemsWithIngredient;
        }

        // UPDATE
        public bool UpdateMenuItem(MenuItem updatedItem, string name)
        {
            MenuItem targetItem = GetItemByName(name);
            if (targetItem != null)
            {
                int itemIndex = _menu.IndexOf(targetItem);
                _menu[itemIndex] = updatedItem;
                return true;
            }
            return false;
        }

        //DELETE
        public bool DeleteMenuItem(MenuItem item)
        {
            return _menu.Remove(item);
        }

        public bool DeleteItemByNumber(int num)
        {
            MenuItem targetItem = GetItemByNumber(num);
            return DeleteMenuItem(targetItem);
        }

        public bool DeleteItemByName(string name)
        {
            MenuItem targetItem = GetItemByName(name);
            return DeleteMenuItem(targetItem);
        }

        public bool DeleteItemsByIngredient(string ingredient)
        {
            List<MenuItem> itemsWithIngredient = GetAllItemsWithIngredient(ingredient);
            bool success = false;
            foreach (MenuItem item in itemsWithIngredient)
            {
                _menu.Remove(item);
                success = true;
            }
            return success;
        }
    }
}
