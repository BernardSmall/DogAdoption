using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogAdoption
{
    public class Program
    {
        private static List<Dog> availableDogs = new List<Dog>();
        private static List<Adopter> adopters = new List<Adopter>();
        private static List<AdoptionApplication> applications = new List<AdoptionApplication>();

        static void Main()
        {
            SeedDogs();
            DisplayMainMenu();
        }

        private static void SeedDogs()
        {
            availableDogs.Add(new Dog(1, "Buddy", "Labrador", 3, "Medium"));
            availableDogs.Add(new Dog(2, "Milo", "Beagle", 2, "Small"));
            availableDogs.Add(new Dog(3, "Daisy", "German Shepherd", 4, "Large"));
        }

        private static void DisplayMainMenu()
        {
            int option = 0;
            while (option != 6)
            {
                Console.Clear();
                Console.WriteLine("SPCA Adoption System");
                Console.WriteLine("1. View Available Dogs");
                Console.WriteLine("2. Apply for Adoption");
                Console.WriteLine("3. View Adopters");
                Console.WriteLine("4. Search Dogs by Name or ID");
                Console.WriteLine("5. View Adopted Dogs");
                Console.WriteLine("6. Exit");
                Console.Write("Select an option: ");
                int.TryParse(Console.ReadLine(), out option);

                switch (option)
                {
                    case 1:
                        ViewAvailableDogs(); break;
                    case 2:
                        ApplyForAdoption(); break;
                    case 3:
                        ViewAdopters(); break;
                    case 4:
                        SearchDogs(); break;
                    case 5:
                        ViewAdoptedDogs(); break;
                    case 6: 
                        Console.WriteLine("Exiting..."); break;

                    default:
                        Console.WriteLine("Invalid option! Press Enter.");
                        Console.ReadLine();
                        break;
                }
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
            Console.Write("Enter your contact info: ");
            string contact = Console.ReadLine();

            Adopter adopter = new Adopter(name, contact);
            adopters.Add(adopter);

            Console.WriteLine("Available Dogs:");
            foreach (var dog in availableDogs.Where(d => d.IsAvailable))
            {
                Console.WriteLine(dog.GetDogDetails());
            }

            Console.Write("Enter Dog ID to adopt: ");
            int id;
            int.TryParse(Console.ReadLine(), out id);

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
    }
}
