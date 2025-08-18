using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DogAdoption
{
    public class Program
    {
        // Global collections for objects – make them public for StaffMember
        public static List<Dog> availableDogs = new List<Dog>();
        public static List<Adopter> adopters = new List<Adopter>();
        public static List<AdoptionApplication> applications = new List<AdoptionApplication>();

        // Track last notified adoption
        private static int lastNotifiedApplicationIndex = -1;

        // Enum with menu options
        public enum MainMenu1
        {
            View_Available_Dogs = 1,
            Apply_for_Adoption,
            View_Adopters,
            Search_Dogs_by_Name_or_ID,
            View_Adopted_Dogs,
            Manage_Dogs_as_Staff,
            Exit,
        }

        static void Main()
        {
            // --- terminal cosmetics ---
            Console.OutputEncoding = Encoding.UTF8;              // box-drawing characters
            Console.Title = "SPCA Adoption — Terminal";

            // One-time splash screen (banner + dog + welcome)
            TerminalArt.Splash();

            bool menuBool = true;

            // Start notification thread
            Thread notificationThread = new Thread(DogAdoptionNotification)
            {
                IsBackground = true // stops with program exit
            };
            notificationThread.Start();

            do
            {
                SeedDogs();
                switch (DisplayMainMenu())
                {
                    case 1:
                        ViewAvailableDogs();
                        break;
                    case 2:
                        ApplyForAdoption();
                        break;
                    case 3:
                        ViewAdopters();
                        break;
                    case 4:
                        SearchDogs();
                        break;
                    case 5:
                        ViewAdoptedDogs();
                        break;
                    case 6:
                        ManageDogsAsStaff();
                        break;
                    case 7:
                        CloseProgramLoadScreen();
                        Console.WriteLine("Program closed");
                        Thread.Sleep(2000);
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid option! Press Enter.");
                        Console.ReadLine();
                        break;
                }
            } while (menuBool);
        }

        private static void SeedDogs()
        {
            if (availableDogs.Count == 0) // avoid duplicates
            {
                availableDogs.Add(new Dog(1, "Buddy", "Labrador", 3, "Medium"));
                availableDogs.Add(new Dog(2, "Milo", "Beagle", 2, "Small"));
                availableDogs.Add(new Dog(3, "Daisy", "German Shepherd", 4, "Large"));
            }
        }

        private static int DisplayMainMenu()
        {
            Console.Clear();

            // Tidy header via TerminalArt
            TerminalArt.Header("DogMenu");

            foreach (MainMenu1 item in Enum.GetValues(typeof(MainMenu1)))
            {
                Console.WriteLine("{0}. {1}", (int)item, item);
            }
            Console.Write("Choose an option: ");
            int.TryParse(Console.ReadLine(), out int option);
            return option;
        }

        public static void CloseProgramLoadScreen()
        {
            Console.WriteLine("Exiting");
            Thread.Sleep(1000);
            for (int i = 0; i < 5; i++)
            {
                Console.Clear();
                Console.WriteLine("Exiting" + new string('.', i + 1));
                Thread.Sleep(200);
            }
        }

        private static void ViewAvailableDogs()
        {
            Console.Clear();
            TerminalArt.Header("Available Dogs");
            foreach (var dog in availableDogs.Where(d => d.IsAvailable))
            {
                Console.WriteLine(dog.GetDogDetails());
            }
            Console.WriteLine("Press Enter to return.");
            Console.ReadLine();
        }

        private static void ApplyForAdoption()
        {
            Console.Clear();
            TerminalArt.Header("Apply for Adoption");

            Console.Write("Enter your name: ");
            string name = Console.ReadLine();
            Console.Write("Enter your contact info (Phone Number): ");
            string contact = Console.ReadLine();
            Console.Write("Enter your email address: ");
            string email = Console.ReadLine();

            Adopter adopter = new Adopter(name, contact, email);
            adopters.Add(adopter);

            Console.WriteLine("\nAvailable Dogs:");
            foreach (var dog in availableDogs.Where(d => d.IsAvailable))
            {
                Console.WriteLine(dog.GetDogDetails());
            }

            Console.Write("\nEnter Dog ID to adopt: ");
            int.TryParse(Console.ReadLine(), out int id);

            Dog selectedDog = availableDogs.FirstOrDefault(d => d.Id == id && d.IsAvailable);
            if (selectedDog != null)
            {
                selectedDog.IsAvailable = false;
                AdoptionApplication app = new AdoptionApplication(selectedDog, adopter);
                applications.Add(app);
                Console.WriteLine($"\nSuccess! {adopter.Name} adopted {selectedDog.Name}. Press Enter.");
            }
            else
            {
                Console.WriteLine("\nInvalid or already adopted dog. Press Enter.");
            }
            Console.ReadLine();
        }

        private static void ViewAdopters()
        {
            Console.Clear();
            TerminalArt.Header("All Adopters");
            foreach (var adopter in adopters)
            {
                Console.WriteLine(adopter.GetDetails());
            }
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
                                                || d.Id.ToString() == input);

            Console.WriteLine("\nSearch Results:");
            foreach (var dog in results)
            {
                Console.WriteLine(dog.GetDogDetails());
            }
            Console.WriteLine("Press Enter to return.");
            Console.ReadLine();
        }

        private static void ViewAdoptedDogs()
        {
            Console.Clear();
            TerminalArt.Header("Adopted Dogs");
            foreach (var app in applications)
            {
                Console.WriteLine(app.GetApplicationDetails());
            }
            Console.WriteLine("Press Enter to return.");
            Console.ReadLine();
        }

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

                // Replace with actual staff credentials
                if (staffName == "Member01" && contact == "password")
                {
                    authenticated = true;
                    Console.WriteLine("\nAccess granted! Press Enter.");
                    Console.ReadLine();

                    StaffMember staff = new StaffMember(staffName, contact);
                    staff.ManageDogs(); // Open management menu
                }
                else
                {
                    attempts++;
                    Console.WriteLine($"\nInvalid credentials. Attempts left: {3 - attempts}");
                    Thread.Sleep(1000);
                }
            }

            if (!authenticated)
            {
                Console.WriteLine("\nToo many failed attempts! Returning to main menu...");
                Thread.Sleep(1500);
            }
        }

        // NOTIFICATION THREAD: prints each new adoption once
        private static void DogAdoptionNotification()
        {
            while (true)
            {
                // Check if there are new adoptions
                if (applications.Count > 0 && applications.Count - 1 > lastNotifiedApplicationIndex)
                {
                    var newApp = applications.Last();

                    // Save current color
                    var prevColor = Console.ForegroundColor;

                    // Change to green
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n[Notification] {newApp.Adopter.Name} just adopted {newApp.Dog.Name}!\n");

                    // Reset to previous color
                    Console.ForegroundColor = prevColor;

                    // Update index so we don't repeat this notification
                    lastNotifiedApplicationIndex = applications.Count - 1;
                }

                Thread.Sleep(1000); // check every 1 second
            }
        }
    }
}
