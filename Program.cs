using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static DogAdoption.Program;

namespace DogAdoption
{
    public class Program
    {
        // Global lists to store dogs, adopters and adoption applications
        public static List<Dog> availableDogs = new List<Dog>();
        public static List<Adopter> adopters = new List<Adopter>();
        public static List<AdoptionApplication> applications = new List<AdoptionApplication>();

        // Keeps track of the last adoption notification shown
        private static int lastNotifiedApplicationIndex = -1;
        //Custom delegate for menu actions
        public delegate void MenuActionDelegate();

        // Store menu actions using delegates
        private static Dictionary<int, MenuActionDelegate> menuActions;


        // Enum for main menu options
        public enum MainMenu1
        {
            View_Available_Dogs = 1,
            Apply_for_Adoption,
            View_Adopters,
            Search_Dogs_by_Name_or_ID,
            View_Adopted_Dogs,
            Manage_Dogs_as_Staff,
            View_Sorted_Dogs,
            Exit,
        }

        static void Main()
        {
            // === INITIALIZE EVENT SYSTEM ===
            EventManager.InitializeEventHandlers();
            EventManager.TriggerLog("System started");

            // Initialize menu actions using delegates
            InitializeMenuActions();

            // Setup terminal look and feel
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = "SPCA Adoption — Terminal";

            // Show welcome splash screen
            TerminalArt.Splash();

            bool menuBool = true;

            // Start a background thread that shows notifications when a dog is adopted
            Thread notificationThread = new Thread(DogAdoptionNotification)
            {
                IsBackground = true
            };
            notificationThread.Start();

            do
            {
                Dog.SeedDogs(availableDogs);
                int choice = DisplayMainMenu();

                // USE DELEGATE: Execute menu action using delegate
                if (menuActions.ContainsKey(choice))
                {
                    try
                    {
                        menuActions[choice].Invoke();
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"An error occurred: {ex.Message}");
                        Console.ResetColor();
                        Console.WriteLine("Press Enter to continue...");
                        Console.ReadLine();
                        EventManager.TriggerLog($"Error in menu action {choice}: {ex.Message}");
                    }
                }
                else if (choice == 8) // Exit
                {
                    CloseProgramLoadScreen();
                    Console.WriteLine("Program closed");
                    EventManager.TriggerLog("System shutdown");
                    Thread.Sleep(2000);
                    Environment.Exit(0);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid option! Please select 1-8.");
                    Console.ResetColor();
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                }
            } while (menuBool);
        }

    private static void InitializeMenuActions()
        {
            menuActions = new Dictionary<int, MenuActionDelegate>
             {
                 { 1, ViewAvailableDogs },
                 { 2, ApplyForAdoption },
                 { 3, ViewAdopters },
                 { 4, SearchDogs },
                 { 5, ViewAdoptedDogs },
                 { 6, ManageDogsAsStaff },
                 { 7, ViewSortedDogs } // NEW
             };

        }


        // Shows the main menu and returns the user’s choice
        private static int DisplayMainMenu()
        {
            Console.Clear();
            TerminalArt.Header("DogMenu");

            foreach (MainMenu1 item in Enum.GetValues(typeof(MainMenu1)))
            {
                Console.WriteLine("{0}. {1}", (int)item, item);
            }
            // Get validated menu choice
            int? choice = ValidationUtils.GetValidatedIntInput(
                "Choose an option (1-8): ",
                option => option >= 1 && option <= 8,
                "Please select a valid option between 1 and 8."
            );

            return choice ?? 0; // Return 0 if validation failed (will show invalid option message)
        }

        // Shows a little “closing animation” when program exits
        public static void CloseProgramLoadScreen()
        {
            Console.WriteLine("Exiting");
            Thread.Sleep(1000);
            for (int i = 0; i < 5; i++)
            {
                Console.Clear();
                Console.WriteLine("Exiting" + new string('.', i + 1));
                Thread.Sleep(300);
            }
        }

        private static void ViewAvailableDogs()
        {
            Console.Clear();
            TerminalArt.Header("Available Dogs");
            var availableOnly = availableDogs.Where(d => d.IsAvailable).ToList();

            if (availableOnly.Count == 0)
            {
                Console.WriteLine("No dogs available for adoption at the moment.");
            }
            else
            {
                foreach (var dog in availableOnly)
                {
                    Console.WriteLine(dog.GetDogDetails());
                }
            }

            EventManager.TriggerLog($"Viewed available dogs list ({availableOnly.Count} dogs available)");
            Console.WriteLine("Press Enter to return.");
            Console.ReadLine();
        }

        // Handles adoption application process
        private static void ApplyForAdoption()
        {
            try
            {
                // Create adopter with validation
                Adopter adopter = Adopter.CreateAdopterWithValidation();
                if (adopter == null)
                {
                    Console.WriteLine("Adoption application cancelled. Press Enter to return.");
                    Console.ReadLine();
                    return;
                }

                adopters.Add(adopter);

                Console.Clear();
                TerminalArt.Header("Apply for Adoption");
                var availableOnly = availableDogs.Where(d => d.IsAvailable).ToList();

                if (availableOnly.Count == 0)
                {
                    Console.WriteLine("Sorry, no dogs are currently available for adoption.");
                    Console.WriteLine("Press Enter to return.");
                    Console.ReadLine();
                    return;
                }

                foreach (var dog in availableOnly)
                {
                    Console.WriteLine(dog.GetDogDetails());
                }

                // Get validated dog ID
                int? dogId = ValidationUtils.GetValidatedIntInput(
                    "\nEnter Dog ID to adopt: ",
                    id => availableDogs.Any(d => d.Id == id && d.IsAvailable),
                    "Please enter a valid ID of an available dog."
                );

                if (dogId == null)
                {
                    Console.WriteLine("Adoption application cancelled. Press Enter to return.");
                    Console.ReadLine();
                    return;
                }
                Dog selectedDog = availableDogs.First(d => d.Id == dogId.Value);

                // USE EVENT SYSTEM: Change status which triggers events
                selectedDog.ChangeAvailabilityStatus(false);

                AdoptionApplication app = new AdoptionApplication(selectedDog, adopter);
                applications.Add(app);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n🎉 SUCCESS! {adopter.Name} successfully adopted {selectedDog.Name}!");
                Console.ResetColor();
                EventManager.TriggerNotification($"🎉 Successful adoption: {adopter.Name} adopted {selectedDog.Name}!");

                Console.WriteLine("Press Enter to continue.");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred during adoption: {ex.Message}");
                Console.ResetColor();
                EventManager.TriggerLog($"Error during adoption process: {ex.Message}");
                Console.WriteLine("Press Enter to return.");
                Console.ReadLine();
            }
        }


        // Displays all adopters
        private static void ViewAdopters()
        {
            Console.Clear();
            TerminalArt.Header("All Adopters");

            if (adopters.Count == 0)
            {
                Console.WriteLine("No adopters registered yet.");
            }
            else
            {
                // INBUILT INTERFACE USAGE: Sort adopters by name using LINQ
                var sortedAdopters = adopters.OrderBy(a => a.Name).ToList();
                foreach (var adopter in sortedAdopters)
                {
                    Console.WriteLine(adopter.GetDetails());
                }
            }

            EventManager.TriggerLog($"Viewed adopters list ({adopters.Count} adopters)");
            Console.WriteLine("Press Enter to return.");
            Console.ReadLine();
        }

        private static void SearchDogs()
        {
            Console.Clear();
            TerminalArt.Header("Search Dogs");
            Console.Write("Enter Dog Name or ID: ");
            string input = Console.ReadLine();

            var results = availableDogs.Where(d => d.Name.Equals(input, StringComparison.OrdinalIgnoreCase)
                                                || d.Id.ToString() == input).ToList();

            Console.WriteLine("\nSearch Results:");
            if (results.Count == 0)
            {
                Console.WriteLine("No dogs found matching your search criteria.");
            }
            else
            {
                foreach (var dog in results)
                {
                    Console.WriteLine(dog.GetDogDetails());
                }
            }


            EventManager.TriggerLog($"Search performed for: '{input}' - {results.Count} results found");
            Console.WriteLine("Press Enter to return.");
            Console.ReadLine();

            Dog.SearchDogs(availableDogs);
        }

        // Shows a list of dogs that have already been adopted
        private static void ViewAdoptedDogs()
        {
            Console.Clear();
            TerminalArt.Header("Adopted Dogs");

            if (applications.Count == 0)
            {
                Console.WriteLine("No adoptions have been processed yet.");
            }
            else
            {
                // INBUILT INTERFACE USAGE: Sort applications using IComparable
                var sortedApplications = applications.ToList();
                sortedApplications.Sort(); // Uses IComparable<AdoptionApplication>.CompareTo

                foreach (var app in sortedApplications)
                {
                    Console.WriteLine(app.GetApplicationDetails());
                }
            }

            EventManager.TriggerLog($"Viewed adopted dogs list ({applications.Count} adoptions)");
            Console.WriteLine("Press Enter to return.");
            Console.ReadLine();
        }

        // Staff login and menu with enhanced validation
        private static void ManageDogsAsStaff()
        {
            int maxAttempts = 3;
            int attempts = 0;
            bool authenticated = false;

            while (attempts < maxAttempts && !authenticated)
            {
                Console.Clear();
                TerminalArt.Header("Staff Login");

                string staffName = ValidationUtils.GetValidatedInput(
                    "Enter staff name: ",
                    ValidationUtils.IsValidName,
                    "Staff name must be 2-50 characters and contain only letters, spaces, hyphens, and apostrophes.",
                    1 // Only one attempt per login cycle
                );

                if (staffName == null)
                {
                    attempts++;
                    continue;
                }

                string password = ValidationUtils.GetValidatedInput(
                    "Enter password: ",
                    ValidationUtils.IsValidPassword,
                    "Password must be 6-50 characters long.",
                    1 // Only one attempt per login cycle
                );

                if (password == null)
                {
                    attempts++;
                    continue;
                }

                // Basic staff login validation
                if (staffName.Equals("Member01", StringComparison.OrdinalIgnoreCase) && password == "password")
                {
                    authenticated = true;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n✓ Access granted! Welcome to staff management.");
                    Console.ResetColor();
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();

                    try
                    {
                        StaffMember staff = new StaffMember(staffName, "Staff Contact Info");
                        EventManager.TriggerLog($"Staff member logged in: {staffName}");
                        staff.ManageDogs();
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Error creating staff member: {ex.Message}");
                        Console.ResetColor();
                        EventManager.TriggerLog($"Error creating staff member: {ex.Message}");
                        Console.WriteLine("Press Enter to return...");
                        Console.ReadLine();
                    }
                }
                else
                {
                    attempts++;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n❌ Invalid credentials. Attempts remaining: {maxAttempts - attempts}");
                    Console.ResetColor();
                    EventManager.TriggerLog($"Failed login attempt for staff: {staffName}");

                    if (attempts < maxAttempts)
                    {
                        Console.WriteLine("Press Enter to try again...");
                        Console.ReadLine();
                    }
                }
            }

            if (!authenticated)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n🚫 Too many failed attempts! Access denied.");
                Console.ResetColor();
                EventManager.TriggerLog("Staff login blocked - too many failed attempts");
                Console.WriteLine("Returning to main menu...");
                Thread.Sleep(2000);
            }
        }
        /*        // Staff login and menu
                private static void ManageDogsAsStaff()
                {
                    int attempts = 0;
                    bool authenticated = false;

                    while (attempts < 3 && !authenticated)
                    {
                        Console.Clear();
                        TerminalArt.Header("Staff Login");

                        Console.Write("Enter staff name: ");
                        string staffName = Console.ReadLine();

                        Console.Write("Enter contact info (password): ");
                        string contact = Console.ReadLine();

                        if (staffName == "Member01" && contact == "password")
                        {
                            authenticated = true;
                            Console.WriteLine("\nAccess granted! Press Enter.");
                            Console.ReadLine();

                            StaffMember staff = new StaffMember(staffName, contact);
                            EventManager.TriggerLog($"Staff member logged in: {staffName}");
                            staff.ManageDogs();
                        }
                        else
                        {
                            attempts++;
                            Console.WriteLine($"\nInvalid credentials. Attempts left: {3 - attempts}");
                            EventManager.TriggerLog($"Failed login attempt for staff: {staffName}");
                            Thread.Sleep(1000);
                        }
                    }

                    if (!authenticated)
                    {
                        Console.WriteLine("\nToo many failed attempts! Returning to main menu...");
                        EventManager.TriggerLog("Staff login blocked - too many failed attempts");
                        Thread.Sleep(1500);
                    }

                }*/

        // Runs in the background and shows a green notification when someone adopts a dog
        private static void DogAdoptionNotification()
        {
            while (true)
            {
                try
                {
                    if (applications.Count > 0 && applications.Count - 1 > lastNotifiedApplicationIndex)
                    {
                        var newApp = applications.Last();

                        var prevColor = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\n[Notification] {newApp.Adopter.Name} just adopted {newApp.Dog.Name}!\n");
                        Console.ForegroundColor = prevColor;

                        lastNotifiedApplicationIndex = applications.Count - 1;
                    }
                }
                catch (Exception ex)
                {
                    EventManager.TriggerLog($"Error in notification thread: {ex.Message}");
                }
                Thread.Sleep(1000);
            }
        }

        private static void ViewSortedDogs()
        {
            Console.Clear();
            TerminalArt.Header("Dogs Sorted by Age and Name");

            // INBUILT INTERFACE USAGE: Sort using IComparable implementation
            var sortedDogs = availableDogs.ToList();
            sortedDogs.Sort(); // Uses IComparable<Dog>.CompareTo method

            Console.WriteLine("Dogs sorted by Age (ascending), then by Name:");
            foreach (var dog in sortedDogs)
            {
                Console.WriteLine(dog.GetDogDetails());
            }

            Console.WriteLine("\nPress Enter to return.");
            Console.ReadLine();

        }
    } 
}

