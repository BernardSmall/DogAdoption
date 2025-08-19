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
        // Global lists to store dogs, adopters and adoption applications
        public static List<Dog> availableDogs = new List<Dog>();
        public static List<Adopter> adopters = new List<Adopter>();
        public static List<AdoptionApplication> applications = new List<AdoptionApplication>();

        // Keeps track of the last adoption notification shown
        private static int lastNotifiedApplicationIndex = -1;

        // Enum for main menu options
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

            // Main menu loop
            do
            {
                Dog.SeedDogs(availableDogs);

                switch (DisplayMainMenu())
                {
                    case 1:
                        Dog.ViewAvailableDogs(availableDogs);
                        break;
                    case 2:
                        ApplyForAdoption();
                        break;
                    case 3:
                        ViewAdopters();
                        break;
                    case 4:
                        Dog.SearchDogs(availableDogs);
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

        
       

        // Shows the main menu and returns the user’s choice
        private static int DisplayMainMenu()
        {
            Console.Clear();
            TerminalArt.Header("DogMenu");

            foreach (MainMenu1 item in Enum.GetValues(typeof(MainMenu1)))
            {
                Console.WriteLine("{0}. {1}", (int)item, item);
            }
            Console.Write("Choose an option: ");
            int.TryParse(Console.ReadLine(), out int option);
            return option;
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

       

        // Handles adoption application process
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

        // Displays all adopters
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

        

        // Shows a list of dogs that have already been adopted
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

        // Staff login and menu
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

                // Basic staff login (hard-coded for now)
                if (staffName == "Member01" && contact == "password")
                {
                    authenticated = true;
                    Console.WriteLine("\nAccess granted! Press Enter.");
                    Console.ReadLine();

                    StaffMember staff = new StaffMember(staffName, contact);
                    staff.ManageDogs();
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

        // Runs in the background and shows a green notification when someone adopts a dog
        private static void DogAdoptionNotification()
        {
            while (true)
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

                Thread.Sleep(1000);
            }
        }
    }
}
