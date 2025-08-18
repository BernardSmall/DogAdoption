using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DogAdoption
{
    public class StaffMember : Person, IManageDogs
    {
        public StaffMember(string name, string contactInfo) : base(name, contactInfo)
        {
          
        }

        // Keep your original GetDetails override
        public override string GetDetails()
        {
            return $"Staff Name: {Name}, Contact: {ContactInfo}";
        }

        public void ManageDogs()
        {
            bool keepManaging = true;
            while (keepManaging)
            {
                Console.Clear();
                Console.WriteLine("Staff Dog Management Menu:");
                Console.WriteLine("1. Add Dog");
                Console.WriteLine("2. Remove Dog");
                Console.WriteLine("3. Update Dog");
                Console.WriteLine("4. Return to Main Menu");
                Console.Write("Choose an option: ");

                int.TryParse(Console.ReadLine(), out int choice);

                switch (choice)
                {
                    case 1:
                        AddDogMenu();
                        break;
                    case 2:
                        RemoveDogMenu();
                        break;
                    case 3:
                        UpdateDogMenu();
                        break;
                    case 4:
                        keepManaging = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Press Enter.");
                        Console.ReadLine();
                        break;
                }
            }
        }


        // IManageDogs implementation
        public void AddDog(Dog dog, List<Dog> dogList)
        {
            dogList.Add(dog);
            Console.WriteLine($"Dog {dog.Name} added successfully. Press Enter.");
            Console.ReadLine();
        }

        public void RemoveDog(int id, List<Dog> dogList)
        {
            Dog dog = dogList.FirstOrDefault(d => d.Id == id);
            if (dog != null)
            {
                dogList.Remove(dog);
                Console.WriteLine($"Dog {dog.Name} removed successfully. Press Enter.");
            }
            else
            {
                Console.WriteLine("Dog not found. Press Enter.");
            }
            Console.ReadLine();
        }

        public void UpdateDog(int id, List<Dog> dogList)
        {
            Dog dog = dogList.FirstOrDefault(d => d.Id == id);
            if (dog != null)
            {
                Console.Write("Enter new name (leave blank to keep current): ");
                string name = Console.ReadLine();
                if (!string.IsNullOrEmpty(name)) dog.Name = name;

                Console.Write("Enter new breed (leave blank to keep current): ");
                string breed = Console.ReadLine();
                if (!string.IsNullOrEmpty(breed)) dog.Breed = breed;

                Console.Write("Enter new age (leave blank to keep current): ");
                string ageInput = Console.ReadLine();
                if (int.TryParse(ageInput, out int age)) dog.Age = age;

                Console.Write("Enter new size (leave blank to keep current): ");
                string size = Console.ReadLine();
                if (!string.IsNullOrEmpty(size)) dog.Size = size;

                Console.WriteLine("Dog updated successfully. Press Enter.");
            }
            else
            {
                Console.WriteLine("Dog not found. Press Enter.");
            }
            Console.ReadLine();
        }

        // Helper menus for input
        private void AddDogMenu()
        {
            Console.Write("Enter Dog ID: ");
            int.TryParse(Console.ReadLine(), out int id);

            Console.Write("Enter Dog Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter Breed: ");
            string breed = Console.ReadLine();

            Console.Write("Enter Age: ");
            int.TryParse(Console.ReadLine(), out int age);

            Console.Write("Enter Size: ");
            string size = Console.ReadLine();

            Dog newDog = new Dog(id, name, breed, age, size);
            AddDog(newDog, Program.availableDogs);
        }

        private void RemoveDogMenu()
        {
            Console.Write("Enter Dog ID to remove: ");
            int.TryParse(Console.ReadLine(), out int id);
            RemoveDog(id, Program.availableDogs);
        }

        private void UpdateDogMenu()
        {
            Console.Write("Enter Dog ID to update: ");
            int.TryParse(Console.ReadLine(), out int id);
            UpdateDog(id, Program.availableDogs);
        }
    }
}
