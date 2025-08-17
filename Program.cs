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
        

        //global collections for object
        private static List<Dog> availableDogs = new List<Dog>();
        private static List<Adopter> adopters = new List<Adopter>();
        private static List<AdoptionApplication> applications = new List<AdoptionApplication>();


      //enum with menu options
       public enum MianMenu1
        {
            View_Available_Dogs = 1,
            Apply_for_Adoption,
            View_Adopters,
            Search_Dogs_by_Name_or_ID,
            View_Adopted_Dogs,
            Exit
        }


        //main program
        static void Main()
        {
            //bool for menu
            bool menuBool = true;
            
            do
            {
                SeedDogs();
                switch (DisplayMainMenu2())
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

                        closeProgramLoadScreen();
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

        //populates collections with objects of dogs with data about dogs
        private static void SeedDogs()
        {
            availableDogs.Add(new Dog(1, "Buddy", "Labrador", 3, "Medium"));
            availableDogs.Add(new Dog(2, "Milo", "Beagle", 2, "Small"));
            availableDogs.Add(new Dog(3, "Daisy", "German Shepherd", 4, "Large"));
        }

        //method to display main menu
        private static int DisplayMainMenu2()
        {
            foreach (MianMenu1 item in Enum.GetValues(typeof(MianMenu1)))
            {
                Console.WriteLine("{0}, {1}", (int)item, item );
            }
            int option =int.Parse(Console.ReadLine());
            return option;
        }

        //Simulates loading/exiting screen
        public static void closeProgramLoadScreen()
        {
            Console.WriteLine("Exiting");
            Thread.Sleep(1000);
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("Exiting.");
                Thread.Sleep(200);
                Console.Clear();
                Console.WriteLine("Exiting..");
                Thread.Sleep(200);
                Console.Clear();
                Console.WriteLine("Exiting...");
                Thread.Sleep(200);
                Console.Clear();
                Console.WriteLine("Exiting....");
                Thread.Sleep(200);
                Console.Clear();
                Console.WriteLine("Exiting.....");
                Thread.Sleep(200);
                Console.Clear();

            }
        }

        //checks if there are dogs available to adopt
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
