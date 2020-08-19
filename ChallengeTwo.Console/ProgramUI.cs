using ChallengeTwo.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ChallengeTwo.Program
{
    public class ProgramUI
    {
        private bool _isRunning = true;
        private ClaimRepository _claimRepo = new ClaimRepository();
        private List<Claim> _completedClaims = new List<Claim>();

        public void Start()
        {
            SeedClaimList();
            RunMenu();
        }

        public void RunMenu()
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
            Console.WriteLine("1. See All Claims\n" +
                "2. Take Care Of Next Claim\n" +
                "3. Enter A New Claim\n" +
                "4. Modify An Existing Claim\n" +
                "5. See All Completed Claims\n" +
                "6. Exit");
            string userInput = Console.ReadLine();
            return userInput;
        }

        private void OpenMenuItem(string userInput)
        {
            Console.Clear();
            switch (userInput)
            {
                case "1":
                    DisplayAllClaims();
                    break;
                case "2":
                    List<Claim> allClaims = _claimRepo.GetAllClaims();
                    if (allClaims.Count > 0)
                    {
                        TakeCareOfNextClaim();
                    }
                    else
                    {
                        Console.WriteLine("No current claims.");
                    }
                    break;
                case "3":
                    EnterNewClaim();
                    break;
                case "4":
                    ModifyClaim();
                    break;
                case "5":
                    DisplayCompletedClaims();
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

        public void DisplayAllClaims()
        {
            List<Claim> fullList = _claimRepo.GetAllClaims();
            Console.WriteLine("{0, -10} {1, -13} {2, -30} {3, -13} {4, -20} {5, -20} {6, -7}\n", "ClaimID", "Type", "Description", "Amount", "DateOfAccident", "DateOfClaim", "IsValid");
            foreach (Claim claim in fullList)
            {
                Console.WriteLine("{0, -10} {1, -13} {2, -30} {3, -13} {4, -20} {5, -20} {6, -7}", $"{claim.ClaimID}", $"{claim.ClaimType}", $"{claim.Description}", $"{claim.ClaimAmount:C}", $"{claim.DateOfIncident:d}", $"{claim.DateOfClaim:d}", $"{claim.IsValid}");
            }
        }

        public void DisplayClaim(Claim claim)
        {
            string valid = claim.IsValid ? "True" : "False";
            Console.WriteLine($"ClaimID: {claim.ClaimID}\n" +
            $"Type: {claim.ClaimType}\n" +
            $"Description: {claim.Description}\n" +
            $"Amount: {claim.ClaimAmount}\n" +
            $"DateOfAccident: {claim.DateOfIncident:d}\n" +
            $"DateOfClaim: {claim.DateOfClaim:d}\n" +
            $"IsValid: {valid}\n");
        }

        public void TakeCareOfNextClaim()
        {
            List<Claim> fullList = _claimRepo.GetAllClaims();
            Claim claim = fullList.First();
            Console.WriteLine("Here are the details for the next claim to be handled:\n\n");
            DisplayClaim(claim);
            Console.WriteLine($"\nDo you want to deal with this claim now(y/n)?");
            ConsoleKeyInfo userInput = Console.ReadKey();
            if (userInput.Key == ConsoleKey.Y)
            {
                claim.TimeOfCompletion = DateTime.Now;
                _completedClaims.Add(claim);
                _claimRepo.DeleteClaim(claim);
                Console.WriteLine("\nClaim completed.");
            }
        }

        public void EnterNewClaim()
        {
            Claim newClaim = ClaimCreation();
            _claimRepo.AddClaim(newClaim);
        }

        public Claim ClaimCreation()
        {
            Claim newClaim = new Claim();

            Console.Write("Enter the claim id: ");
            newClaim.ClaimID = int.Parse(Console.ReadLine());

            Console.WriteLine("\nEnter the claim type:\n" +
                "1. Car\n" +
                "2. Home\n" +
                "3. Theft");
            string userInput = Console.ReadLine();
            switch (userInput.ToLower())
            {
                case "1":
                case "car":
                    newClaim.ClaimType = ClaimType.Car;
                    break;
                case "2":
                case "home":
                    newClaim.ClaimType = ClaimType.Home;
                    break;
                case "3":
                case "theft":
                    newClaim.ClaimType = ClaimType.Theft;
                    break;
            }

            Console.Write("\nEnter a claim description (please stay under 30 characters): ");
            newClaim.Description = Console.ReadLine();

            Console.Write("\nAmount of Damage (up to $999,999.99): ");
            newClaim.ClaimAmount = double.Parse(Console.ReadLine());

            Console.Write("\nDate of Accident (mm/dd/yyyy): ");
            string iDate = Console.ReadLine();
            newClaim.DateOfIncident = TransformDate(iDate);

            Console.Write("\nDate of Claim (mm/dd/yyyy): ");
            string cDate = Console.ReadLine();
            newClaim.DateOfClaim = TransformDate(cDate);

            if (newClaim.IsValid)
            {
                Console.WriteLine("\nThis claim is valid.");
            }
            else
            {
                Console.WriteLine("\nThis claim is invalid.");
            }

            return newClaim;
        }

        public DateTime TransformDate(string userInput)
        {
            List<string> dateList = userInput.Split('/').ToList();
            List<int> intDateList = new List<int>();
            foreach (string part in dateList)
            {
                intDateList.Add(int.Parse(part));
            }
            return new DateTime(intDateList[2], intDateList[0], intDateList[1]);
        }

        public void ModifyClaim()
        {
            Console.Write("Enter the ID of the claim you want to modify: ");
            int targetID = int.Parse(Console.ReadLine());
            if (_claimRepo.GetClaimByID(targetID) != null)
            {
                Console.WriteLine("Create modified claim:\n\n");
                Claim newClaim = ClaimCreation();
                _claimRepo.UpdateClaim(targetID, newClaim);
            }
            else
            {
                Console.WriteLine("Claim not found.");
            }

        }

        public void DisplayCompletedClaims()
        {
            Console.WriteLine("{0, -10} {1, -50}\n", "ClaimID", "TimeOfCompletion");
            if (_completedClaims.Count > 0)
            {
                foreach (Claim claim in _completedClaims)
                {
                    Console.WriteLine("{0, -10} {1, -50}", claim.ClaimID, claim.TimeOfCompletion);
                }
                Console.WriteLine("\n\nTo view the details of a claim, press the corresponding ID number.");
                ConsoleKeyInfo userInput = Console.ReadKey();
                if (char.IsDigit(userInput.KeyChar))
                {
                    int convertedInput = int.Parse(userInput.KeyChar.ToString());
                    foreach (Claim item in _completedClaims)
                    {
                        if (convertedInput == item.ClaimID)
                        {
                            Console.Clear();
                            DisplayClaim(item);
                            Console.WriteLine($"Completed at: {item.TimeOfCompletion}\n\n");
                            Console.WriteLine("To return to completed claims, press b. Otherwise, press anything else.");
                            ConsoleKeyInfo response = Console.ReadKey();
                            if (response.Key == ConsoleKey.B)
                            {
                                Console.Clear();
                                DisplayCompletedClaims();
                                return;
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                    Console.WriteLine("Claim not found.");
                }
            }
            else
            {
                Console.WriteLine("No completed claims.");
            }
        }


        public void SeedClaimList()
        {
            Claim claimOne = new Claim(1, ClaimType.Car, "Car break-in", 3000, new DateTime(2000, 1, 20), new DateTime(2002, 3, 20));
            Claim claimTwo = new Claim(2, ClaimType.Car, "Car crash", 2000, new DateTime(2010, 1, 1), new DateTime(2010, 1, 25));
            Claim claimThree = new Claim(3, ClaimType.Home, "Break-in", 50000, new DateTime(2011, 6, 30), new DateTime(2011, 8, 2));
            Claim claimFour = new Claim(4, ClaimType.Theft, "Lost Car Keys", 50, new DateTime(2011, 2, 27), new DateTime(2011, 3, 14));
            Claim claimFive = new Claim(5, ClaimType.Theft, "Kidnapped Dog", 3500, new DateTime(2012, 9, 8), new DateTime(2012, 10, 21));

            _claimRepo.AddClaim(claimOne);
            _claimRepo.AddClaim(claimTwo);
            _claimRepo.AddClaim(claimThree);
            _claimRepo.AddClaim(claimFour);
            _claimRepo.AddClaim(claimFive);
        }
    }
}
