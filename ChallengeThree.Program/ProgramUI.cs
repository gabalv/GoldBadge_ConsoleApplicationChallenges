using ChallengeThree.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChallengeThree.Program
{
    public class ProgramUI
    {
        private bool _isRunning = true;
        private BadgeRepository _badgeRepo = new BadgeRepository();

        public void Start()
        {
            SeedBadges();
            RunMenu();
        }

        public void RunMenu()
        {
            while (_isRunning)
            {
                string userInput = GetMenuSelection();
                OpenMenu(userInput);
            }
        }

        public string GetMenuSelection()
        {
            Console.Clear();
            Console.WriteLine("Hello Security Admin, what would you like to do?\n" +
                "1. Add a badge\n" +
                "2. Edit a badge\n" +
                // Add a door
                // Remove a door
                // Clear all doors
                // Give new list of doors
                "3. List all badges\n" +
                // Filter by door access
                "4. Revoke door access from all badges\n" +
                "5. Delete a badge\n" +
                "6. Exit");
            return Console.ReadLine();
        }

        public void OpenMenu(string userInput)
        {
            Console.Clear();
            switch (userInput)
            {
                case "1":
                    AddBadge();
                    break;
                case "2":
                    EditBadge();
                    break;
                case "3":
                    ListBadges();
                    break;
                case "4":
                    RevokeDoorFromAllBadges();
                    break;
                case "5":
                    DeleteBadge();
                    break;
                case "6":
                    _isRunning = false;
                    return;
                default:
                    Console.WriteLine("Invalid input. Please try again.");
                    break;
            }
            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();
        }

        public void AddBadge()
        {
            Console.Write("What is the number on the badge: ");
            int id = int.Parse(Console.ReadLine());

            if (!_badgeRepo.CheckExistence(id))
            {
                List<string> doors = AddDoors();
                Badge badge = new Badge(id, doors);
                _badgeRepo.AddToDictionary(badge);
            }
            else
            {
                Console.WriteLine("\nBadge already exists.");
            }
        }

        public List<string> AddDoors()
        {
            List<string> doors = new List<string>();
            bool notDone = true;
            while (notDone)
            {
                Console.Write("List a door that it needs access to: ");
                string door = Console.ReadLine().ToUpper();
                doors.Add(door);
                Console.Write("Any other doors(y/n)?");
                string userInput = Console.ReadLine();
                if (userInput == "y") { }
                else
                {
                    notDone = false;
                }
            }
            return doors;
        }

        public void EditBadge()
        {
            Console.Write("What is the badge number to update: ");
            int userInput = int.Parse(Console.ReadLine());
            if (_badgeRepo.CheckExistence(userInput))
            {
                List<string> doors = _badgeRepo.GetDoorsByID(userInput);
                string output = $"\nBadge {userInput} has access to doors ";
                for (int i = 0; i < (doors.Count - 1); i++)
                {
                    output += $"{doors[i]}, ";
                }
                if (doors.Count > 0)
                {
                    output += $"& {doors[(doors.Count - 1)]}.";
                    Console.WriteLine(output);
                }
                else
                {
                    Console.WriteLine($"Badge {userInput} has no door access.");
                }
                EditMenu(userInput);
            }
            else
            {
                Console.WriteLine("\nThat badge does not exist.");
            }
        }

        public void EditMenu(int id)
        {
            Console.WriteLine("\nWhat would you like to do?\n" +
                "1. Add a door\n" +
                "2. Remove a door\n" +
                "3. Remove all doors\n" +
                "4. Add a list of doors\n" +
                "5. Cancel");
            string userInput = Console.ReadLine();
            Console.Clear();
            switch (userInput)
            {
                case "1":
                    Console.Write("Enter door: ");
                    Console.WriteLine(_badgeRepo.AddDoor(id, Console.ReadLine()));
                    break;
                case "2":
                    Console.Write("Enter door: ");
                    Console.WriteLine(_badgeRepo.RemoveDoor(id, Console.ReadLine()));
                    break;
                case "3":
                    Console.WriteLine("Are you sure you want to remove all doors from this badge(y/n)?");
                    if (Console.ReadLine() == "y")
                    {
                        Dictionary<int, List<string>> fullDict = _badgeRepo.GetAllPairs();
                        if (fullDict[id].Count > 0)
                        {
                            _badgeRepo.DeleteDoorsFromBadge(id);
                            Console.WriteLine("Doors cleared.");
                        }
                        else
                        {
                            Console.WriteLine("This badge has no access to any doors.");
                        }
                    }
                    break;
                case "4":
                    List<string> doors = AddDoors();
                    Console.WriteLine(_badgeRepo.AddSeveralDoors(id, doors));
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Invalid input. Please try again.");
                    EditMenu(id);
                    break;
            }

        }

        public void ListBadges()
        {
            Dictionary<int, List<string>> fullDict = _badgeRepo.GetAllPairs();
            DisplayBadges(fullDict);
            Console.WriteLine("\n\nTo filter list by door access, press f. Otherwise, press anything else.");
            ConsoleKeyInfo userInput = Console.ReadKey();
            if (userInput.Key == ConsoleKey.F)
            {
                FilterBadges(fullDict);
            }
        }

        public void DisplayBadges(Dictionary<int, List<string>> dict)
        {
            Console.WriteLine("{0, -15} {1, -20}", "Badge #", "Door Access");
            for (int i = 0; i < dict.Count; i++)
            {
                string values = string.Join(", ", dict.ElementAt(i).Value);
                Console.WriteLine("{0, -15} {1, -20}", dict.ElementAt(i).Key, values);
            }
        }

        public void FilterBadges(Dictionary<int, List<string>> dict)
        {
            Console.Clear();
            Console.Write("Enter the door you want to filter by: ");
            string input = Console.ReadLine().ToUpper();
            Dictionary<int, List<string>> filteredDict = _badgeRepo.GetAllBadgesWithDoor(input, dict);
            DisplayBadges(filteredDict);
            Console.WriteLine("\n\nTo further filter list by door access, press f. Otherwise, press anything else.");
            ConsoleKeyInfo userInput = Console.ReadKey();
            if (userInput.Key == ConsoleKey.F)
            {
                FilterBadges(filteredDict);
            }
        }

        public void RevokeDoorFromAllBadges()
        {
            Console.Write("Choose door to delete from all badges: ");
            string userInput = Console.ReadLine().ToUpper();
            Dictionary<int, List<string>> fullDict = _badgeRepo.GetAllPairs();
            Dictionary<int, List<string>> filteredDict = _badgeRepo.GetAllBadgesWithDoor(userInput, fullDict);
            Console.WriteLine($"\nWARNING: You will be deleting door {userInput} from {filteredDict.Count} badges. Do you wish to continue(y/n)?");
            ConsoleKeyInfo input = Console.ReadKey();
            if (input.Key == ConsoleKey.Y)
            {
                _badgeRepo.DeleteDoorFromAllBadges(userInput);
                Console.WriteLine($"\nDoor {userInput} successfully removed from {filteredDict.Count} badges.");
            }
        }

        public void DeleteBadge()
        {
            Console.Write("Enter the ID of the badge you wish to delete: ");
            int userInput = int.Parse(Console.ReadLine());
            Console.WriteLine("");
            if (_badgeRepo.CheckExistence(userInput))
            {
                Console.WriteLine($"WARNING: You are about to delete Badge {userInput}. This is irreversible. Do you wish to continue(y/n)?");
                ConsoleKeyInfo input = Console.ReadKey();
                if (input.Key == ConsoleKey.Y)
                {
                    _badgeRepo.DeleteBadge(userInput);
                    Console.WriteLine($"\nBadge {userInput} successfully deleted.");
                }
            }
            else
            {
                Console.WriteLine("Badge not found.");
            }
        }


        public void SeedBadges()
        {
            Badge badgeOne = new Badge(12345, new List<string>() { "A1", "B1", "C1", "D1" });
            Badge badgeTwo = new Badge(22345, new List<string>() { "A2", "B2", "C2", "D2" });
            Badge badgeThree = new Badge(32345, new List<string>() { "A1", "A2", "B1", "B2" });
            Badge badgeFour = new Badge(42345, new List<string>() { "C1", "C2", "D1", "D2" });
            Badge badgeFive = new Badge(52345, new List<string>() { "A1", "B2", "C1", "D2" });
            Badge badgeSix = new Badge(13345, new List<string>() { "A2", "B1", "C2", "D1" });
            Badge badgeSeven = new Badge(23345, new List<string>() { "A1", "A2", "D1", "D2" });
            Badge badgeEight = new Badge(33345, new List<string>() { "B1", "B2", "C1", "C2" });
            Badge badgeNine = new Badge(43345, new List<string>() { "A1", "A2", "C1", "C2" });
            Badge badgeTen = new Badge(53345, new List<string>() { "B1", "B2", "D1", "D2" });

            _badgeRepo.AddToDictionary(badgeOne);
            _badgeRepo.AddToDictionary(badgeTwo);
            _badgeRepo.AddToDictionary(badgeThree);
            _badgeRepo.AddToDictionary(badgeFour);
            _badgeRepo.AddToDictionary(badgeFive);
            _badgeRepo.AddToDictionary(badgeSix);
            _badgeRepo.AddToDictionary(badgeSeven);
            _badgeRepo.AddToDictionary(badgeEight);
            _badgeRepo.AddToDictionary(badgeNine);
            _badgeRepo.AddToDictionary(badgeTen);
        }
    }
}
