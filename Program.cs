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
            bool menuBool = true;

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
                        Thread.Sleep(3000);
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
            if (availableDogs.Count == 0) // avoid adding duplicates
            {
                availableDogs.Add(new Dog(1, "Buddy", "Labrador", 3, "Medium"));
                availableDogs.Add(new Dog(2, "Milo", "Beagle", 2, "Small"));
                availableDogs.Add(new Dog(3, "Daisy", "German Shepherd", 4, "Large"));
            }
        }

        private static int DisplayMainMenu()
        {
            Console.Clear();
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
            Console.WriteLine("Available Dogs:");
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
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();
            Console.Write("Enter your contact info (Phone Number): ");
            string contact = Console.ReadLine();
            Console.Write("Enter your email address: ");
            string email = Console.ReadLine();

            Adopter adopter = new Adopter(name, contact, email);
            adopters.Add(adopter);

            Console.WriteLine("Available Dogs:");
            foreach (var dog in availableDogs.Where(d => d.IsAvailable))
            {
                Console.WriteLine(dog.GetDogDetails());
            }

            Console.Write("Enter Dog ID to adopt: ");
            int.TryParse(Console.ReadLine(), out int id);

            Dog selectedDog = availableDogs.FirstOrDefault(d => d.Id == id && d.IsAvailable);
            if (selectedDog != null)
            {
                selectedDog.IsAvailable = false;
                AdoptionApplication app = new AdoptionApplication(selectedDog, adopter);
                applications.Add(app);
                Console.WriteLine($"Success! {adopter.Name} adopted {selectedDog.Name}. Press Enter.");
            }
            else
            {
                Console.WriteLine("Invalid or already adopted dog. Press Enter.");
            }
            Console.ReadLine();
        }

        private static void ViewAdopters()
        {
            Console.Clear();
            Console.WriteLine("All Adopters:");
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
            Console.Write("Enter Dog Name or ID: ");
            string input = Console.ReadLine();

            var results = availableDogs.Where(d => d.Name.Equals(input, StringComparison.OrdinalIgnoreCase)
                                                || d.Id.ToString() == input);

            Console.WriteLine("Search Results:");
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
            Console.WriteLine("Adopted Dogs:");
            foreach (var app in applications)
            {
                Console.WriteLine(app.GetApplicationDetails());
            }
            Console.WriteLine("Press Enter to return.");
            Console.ReadLine();
        }

        // NEW: Handle staff dog management
        private static void ManageDogsAsStaff()
        {
            Console.Clear();
            Console.Write("Enter staff name: ");
            string staffName = Console.ReadLine();
            Console.Write("Enter contact info: ");
            string contact = Console.ReadLine();

            StaffMember staff = new StaffMember(staffName, contact);
            staff.ManageDogs();
        }
    }
}