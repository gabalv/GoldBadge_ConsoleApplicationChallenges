using ChallengeOne.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace GoldBadge_ConsoleApplicationChallenges
{
    public class ProgramUI
    {
        private bool _isRunning = true;
        private MenuRepository _menuRepo = new MenuRepository();

        public void Start()
        {
            SeedMenuItemExamples();
            RunMenu();
        }

        private void RunMenu()
        {
            while (_isRunning)
            {
                string userInput = GetMenuSelection();
                OpenMenuItem(userInput);
            }
        }

        private string GetMenuSelection()
        {
            Console.Clear();
            Console.WriteLine("Welcome to the Komodo Cafe menu!\n\n" +
                "Select Action:\n" +
                "1. Show All Menu Items\n" +
                "2. Find Menu Item By Number\n" +
                "3. Find Menu Item By Name\n" +
                "4. Find All Menu Items Under A Certain Price\n" +
                "5. Find All Menu Items With A Certain Ingredient\n" +
                "6. Add A Menu Item\n" +
                "7. Update A Menu Item\n" +
                "8. Remove A Menu Item By Number\n" +
                "9. Remove A Menu Item By Name\n" +
                "10. Delete All Menu Items With A Certain Ingredient\n" +
                "11. Exit");
            string userInput = Console.ReadLine();
            return userInput;
        }

        private void OpenMenuItem(string userInput)
        {
            Console.Clear();
            switch (userInput)
            {
                case "1":
                    DisplayAllItems();
                    break;
                case "2":
                    DisplayItemByNumber();
                    break;
                case "3":
                    DisplayItemByName();
                    break;
                case "4":
                    DisplayMenuItemsUnderPrice();
                    break;
                case "5":
                    DisplayItemsWithIngredient();
                    break;
                case "6":
                    AddNewItemToRepo(CreateNewMenuItem());
                    break;
                case "7":
                    UpdateMenuItem();
                    break;
                case "8":
                    RemoveItemByNumber();
                    break;
                case "9":
                    RemoveItemByName();
                    break;
                case "10":
                    RemoveAllItemsWithIngredient();
                    break;
                case "11":
                    _isRunning = false;
                    return;
                default:
                    Console.WriteLine("Invalid input. Please try again.");
                    break;
            }
            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();
        }

        private void DisplayMenuItem(MenuItem item)
        {
            string ingredientList = String.Join(", ", item.Ingredients);
            Console.WriteLine($"#{item.Number}\n" +
                $"Name: {item.Name}\n" +
                $"Description: {item.Description}\n" +
                $"Ingredients: { ingredientList }\n" +
                $"Price: ${item.Price}\n");
        }

        private void DisplayAllItems()
        {
            List<MenuItem> fullList = _menuRepo.GetAllItems();
            foreach (MenuItem item in fullList)
            {
                DisplayMenuItem(item);
            }
        }

        private void DisplayItemByNumber()
        {
            Console.Write("Enter the item number: ");
            int itemNum = Int32.Parse(Console.ReadLine());
            MenuItem searchResult = _menuRepo.GetItemByNumber(itemNum);
            if (searchResult != null)
            {
                DisplayMenuItem(searchResult);
            }
            else
            {
                Console.WriteLine("Item not found.");
            }
        }

        private void DisplayItemByName()
        {
            Console.Write("Enter the item name: ");
            string itemName = Console.ReadLine();
            MenuItem searchResult = _menuRepo.GetItemByName(itemName);
            if (searchResult != null)
            {
                DisplayMenuItem(searchResult);
            }
            else
            {
                Console.WriteLine("Item not found.");
            }
        }

        private void DisplayMenuItemsUnderPrice()
        {
            Console.Write("Enter a price: $");
            double itemPrice = double.Parse(Console.ReadLine());
            List<MenuItem> itemsUnderPrice = _menuRepo.GetAllItemsUnderPrice(itemPrice);
            Console.WriteLine($"\nAll items under ${itemPrice}:");
            if (itemsUnderPrice.Count > 0)
            {
                foreach (MenuItem item in itemsUnderPrice)
                {
                    DisplayMenuItem(item);
                }
            }
            else
            {
                Console.WriteLine("No items found.");
            }
        }

        private List<MenuItem> DisplayItemsWithIngredient()
        {
            Console.Write("Enter an ingredient: ");
            string itemIngredient = Console.ReadLine();
            List<MenuItem> itemsWithIngredient = _menuRepo.GetAllItemsWithIngredient(itemIngredient);
            Console.WriteLine($"\nAll items with {itemIngredient}:");
            if (itemsWithIngredient.Count > 0)
            {
                foreach (MenuItem item in itemsWithIngredient)
                {
                    DisplayMenuItem(item);
                }
            }
            else
            {
                Console.WriteLine("No items found.");
            }
            return itemsWithIngredient;
        }

        private MenuItem CreateNewMenuItem()
        {
            Console.WriteLine("Enter name of menu item:");
            string name = Console.ReadLine();

            Console.WriteLine("\nEnter description of menu item:");
            string description = Console.ReadLine();

            Console.WriteLine("\nEnter in all ingredients of menu item. When finished, enter in \"done\".");
            bool stillEntering = true;
            List<string> ingredientList = new List<string>();
            while (stillEntering)
            {
                string userInput = Console.ReadLine();
                if (userInput.ToLower() == "done")
                {
                    stillEntering = false;
                }
                else
                {
                    ingredientList.Add(userInput);
                }
            }

            Console.WriteLine("\nEnter the price of the item:");
            double price = double.Parse(Console.ReadLine());

            Console.WriteLine("\nEnter the number of menu item:");
            int number = Int32.Parse(Console.ReadLine());

            MenuItem newMenuItem = new MenuItem(number, name, description, ingredientList, price);
            
            return newMenuItem;
        }

        private void AddNewItemToRepo(MenuItem item)
        {
            _menuRepo.AddMenuItem(item);
            Console.WriteLine("\n\nItem created!");
        }

        private void UpdateMenuItem()
        {
            Console.Write("Enter the name of the menu item you want to update: ");
            string userInput = Console.ReadLine();
            if (_menuRepo.GetItemByName(userInput) != null)
            {
                Console.WriteLine("\nCreate your updated menu item:");
                _menuRepo.UpdateMenuItem(CreateNewMenuItem(), userInput);
                Console.WriteLine("\nItem updated!");
            }
            else
            {
                Console.WriteLine("Item not found.");
            }
        }

        private void RemoveItemByNumber()
        {
            Console.Write("Enter the number of the menu item you want to remove: ");
            int targetNumber = int.Parse(Console.ReadLine());
            if (_menuRepo.DeleteItemByNumber(targetNumber))
            {
                Console.WriteLine("\nItem deleted!");
            }
            else
            {
                Console.WriteLine("Item not found.");
            }
        }

        private void RemoveItemByName()
        {
            Console.Write("Enter the name of the menu item you want to remove: ");
            string targetName = Console.ReadLine();
            if (_menuRepo.DeleteItemByName(targetName))
            {
                Console.WriteLine("\nItem deleted!");
            }
            else
            {
                Console.WriteLine("Item not found.");
            }
        }

        private void RemoveAllItemsWithIngredient()
        {
            Console.Write("Enter the ingredient of the menu items you want to remove: ");
            string ingredient = Console.ReadLine();
            if (_menuRepo.DeleteItemsByIngredient(ingredient))
            {
                Console.WriteLine("\nItems deleted!");
            }
            else
            {
                Console.WriteLine("No items found.");
            }
        }

        private void SeedMenuItemExamples()
        {
            MenuItem itemOne = new MenuItem(1, "Example Eggs", "Tasty in theory.", new List<string>() { "eggs", "butter", "salt" }, 10);
            MenuItem itemTwo = new MenuItem(2, "Example BLT", "This is where the scent in your imagination is coming from.", new List<string>() { "bread", "bacon", "lettuce", "tomato" }, 20);
            MenuItem itemThree = new MenuItem(3, "Example Fried Rice", "This is why it pays to keep rice in mind.", new List<string>() { "rice", "eggs", "garlic", "onions", "butter", "soy sauce", "sesame oil", "oyster sauce" }, 30);
            MenuItem itemFour = new MenuItem(4, "Example French Toast", "Hey you, you're finally awake.", new List<string>() { "bread", "eggs", "milk", "cinnamon", "nutmeg", "vanilla extract" }, 40);
            MenuItem itemFive = new MenuItem(5, "Water", "This is just water.", new List<string>() { "water" }, 100);

            _menuRepo.AddMenuItem(itemOne);
            _menuRepo.AddMenuItem(itemTwo);
            _menuRepo.AddMenuItem(itemThree);
            _menuRepo.AddMenuItem(itemFour);
            _menuRepo.AddMenuItem(itemFive);
        }
    }
}
