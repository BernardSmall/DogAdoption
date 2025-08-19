using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DogAdoption
{
    // StaffMember inherits from Person and can manage dogs
    public class StaffMember : Person, IManageDogs
    {
        public delegate void StaffActionDelegate(string action, string details);
        public static event StaffActionDelegate StaffActionPerformed;
        // Constructor
        public StaffMember(string name, string contactInfo) : base(name, contactInfo)
        {
            StaffActionPerformed += OnStaffActionPerformed;
        }
        private static void OnStaffActionPerformed(string action, string details)
        {
            EventManager.TriggerLog($"Staff Action: {action} - {details}");
        }

        // Override GetDetails from Person
        public override string GetDetails()
        {
            return $"Staff Name: {Name}, Contact: {ContactInfo}";
        }

        // Staff menu to manage dogs
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

        // --- IManageDogs implementation ---

        // Add a new dog to the list
        public void AddDog(Dog dog, List<Dog> dogList)
        {
            dogList.Add(dog);
            Console.WriteLine($"Dog {dog.Name} added successfully. Press Enter.");

            StaffActionPerformed?.Invoke("ADD_DOG", $"Added dog: {dog.Name} (ID: {dog.Id})");
            EventManager.TriggerNotification($"🐕 New dog added to system: {dog.Name}");

            Console.ReadLine();
        }

        // Remove a dog by ID
        public void RemoveDog(int id, List<Dog> dogList)
        {
            Dog dog = dogList.FirstOrDefault(d => d.Id == id);
            if (dog != null)
            {
                dogList.Remove(dog);
                Console.WriteLine($"Dog {dog.Name} removed successfully. Press Enter.");

                StaffActionPerformed?.Invoke("REMOVE_DOG", $"Removed dog: {dog.Name} (ID: {id})");
                EventManager.TriggerNotification($"🗑️ Dog removed from system: {dog.Name}");
            }
            else
            {
                Console.WriteLine("Dog not found. Press Enter.");
                StaffActionPerformed?.Invoke("REMOVE_DOG_FAILED", $"Failed to remove dog with ID: {id}");
            }
            Console.ReadLine();
        }

        // Update an existing dog's details
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
                StaffActionPerformed?.Invoke("UPDATE_DOG", $"Updated dog ID {id}: {dog.Name}");
                EventManager.TriggerNotification($"✏️ Dog information updated: {dog.Name}");

            }
            else
            {
                Console.WriteLine("Dog not found. Press Enter.");
                StaffActionPerformed?.Invoke("UPDATE_DOG_FAILED", $"Failed to update dog with ID: {id}");

            }
            Console.ReadLine();
        }

        // --- Helper menus for input ---

        // Menu to add a new dog
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

        // Menu to remove a dog
        private void RemoveDogMenu()
        {
            Console.Write("Enter Dog ID to remove: ");
            int.TryParse(Console.ReadLine(), out int id);
            RemoveDog(id, Program.availableDogs);
        }

        // Menu to update a dog
        private void UpdateDogMenu()
        {
            Console.Write("Enter Dog ID to update: ");
            int.TryParse(Console.ReadLine(), out int id);
            UpdateDog(id, Program.availableDogs);
        }
    }
}
