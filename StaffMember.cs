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

        // Constructor with validation
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

        // Staff menu to manage dogs with validation
        public void ManageDogs()
        {
            bool keepManaging = true;
            while (keepManaging)
            {
                Console.Clear();
                TerminalArt.Header("Staff Dog Management");
                Console.WriteLine("1. Add Dog");
                Console.WriteLine("2. Remove Dog");
                Console.WriteLine("3. Update Dog");
                Console.WriteLine("4. View All Dogs");
                Console.WriteLine("5. Return to Main Menu");

                int? choice = ValidationUtils.GetValidatedIntInput(
                    "Choose an option (1-5): ",
                    option => option >= 1 && option <= 5,
                    "Please select a valid option between 1 and 5."
                );

                if (choice == null)
                {
                    Console.WriteLine("Press Enter to try again...");
                    Console.ReadLine();
                    continue;
                }

                try
                {
                    switch (choice.Value)
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
                            ViewAllDogsAsStaff();
                            break;
                        case 5:
                            keepManaging = false;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    Console.ResetColor();
                    EventManager.TriggerLog($"Error in staff management: {ex.Message}");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                }
            }
        }

        // --- IManageDogs implementation with validation ---
        // Add a new dog to the list
        public void AddDog(Dog dog, List<Dog> dogList)
        {
            if (dog == null)
            {
                throw new ArgumentNullException(nameof(dog), "Dog cannot be null.");
            }

            if (dogList.Any(d => d.Id == dog.Id))
            {
                throw new ArgumentException($"A dog with ID {dog.Id} already exists.");
            }

            if (!dog.IsValid())
            {
                throw new ArgumentException("Dog data is invalid.");
            }

            dogList.Add(dog);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✓ Dog {dog.Name} added successfully!");
            Console.ResetColor();

            StaffActionPerformed?.Invoke("ADD_DOG", $"Added dog: {dog.Name} (ID: {dog.Id})");
            EventManager.TriggerNotification($"🐕 New dog added to system: {dog.Name}");

            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        // Remove a dog by ID with validation
        public void RemoveDog(int id, List<Dog> dogList)
        {
            if (!ValidationUtils.IsValidDogId(id))
            {
                throw new ArgumentException("Invalid dog ID provided.");
            }

            Dog dog = dogList.FirstOrDefault(d => d.Id == id);
            if (dog != null)
            {
                // Check if dog has been adopted
                bool isAdopted = Program.applications.Any(app => app.Dog.Id == id);
                if (isAdopted)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"⚠️  Warning: Dog {dog.Name} has been adopted. Are you sure you want to remove it from the system?");
                    Console.ResetColor();
                    Console.Write("Type 'YES' to confirm: ");
                    string confirmation = Console.ReadLine();

                    if (!confirmation.Equals("YES", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Removal cancelled.");
                        Console.WriteLine("Press Enter to continue...");
                        Console.ReadLine();
                        return;
                    }
                }

                dogList.Remove(dog);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✓ Dog {dog.Name} removed successfully!");
                Console.ResetColor();

                StaffActionPerformed?.Invoke("REMOVE_DOG", $"Removed dog: {dog.Name} (ID: {id})");
                EventManager.TriggerNotification($"🗑️ Dog removed from system: {dog.Name}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Dog with ID {id} not found.");
                Console.ResetColor();
                StaffActionPerformed?.Invoke("REMOVE_DOG_FAILED", $"Failed to remove dog with ID: {id}");
            }
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        // Update an existing dog's details with validation
        public void UpdateDog(int id, List<Dog> dogList)
        {
            if (!ValidationUtils.IsValidDogId(id))
            {
                throw new ArgumentException("Invalid dog ID provided.");
            }

            Dog dog = dogList.FirstOrDefault(d => d.Id == id);
            if (dog != null)
            {
                Console.Clear();
                TerminalArt.Header($"Update Dog: {dog.Name}");
                Console.WriteLine($"Current details: {dog.GetDogDetails()}");
                Console.WriteLine("\nEnter new values (press Enter to keep current value):");

                // Update name
                Console.Write($"Name [{dog.Name}]: ");
                string nameInput = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(nameInput))
                {
                    if (ValidationUtils.IsValidName(nameInput))
                    {
                        dog.Name = nameInput;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid name format. Name not updated.");
                        Console.ResetColor();
                    }
                }

                // Update breed
                Console.Write($"Breed [{dog.Breed}]: ");
                string breedInput = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(breedInput))
                {
                    if (ValidationUtils.IsValidBreed(breedInput))
                    {
                        dog.Breed = breedInput;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid breed format. Breed not updated.");
                        Console.ResetColor();
                    }
                }

                // Update age
                Console.Write($"Age [{dog.Age}]: ");
                string ageInput = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(ageInput))
                {
                    if (int.TryParse(ageInput, out int age) && ValidationUtils.IsValidAge(age))
                    {
                        dog.Age = age;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid age. Age must be between 0 and 20 years. Age not updated.");
                        Console.ResetColor();
                    }
                }

                // Update size
                Console.WriteLine($"Available sizes: Small, Medium, Large, Extra Large");
                Console.Write($"Size [{dog.Size}]: ");
                string sizeInput = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(sizeInput))
                {
                    if (ValidationUtils.IsValidSize(sizeInput))
                    {
                        dog.Size = sizeInput;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid size. Size not updated.");
                        Console.ResetColor();
                    }
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("✓ Dog updated successfully!");
                Console.ResetColor();
                Console.WriteLine($"New details: {dog.GetDogDetails()}");

                StaffActionPerformed?.Invoke("UPDATE_DOG", $"Updated dog ID {id}: {dog.Name}");
                EventManager.TriggerNotification($"✏️ Dog information updated: {dog.Name}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Dog with ID {id} not found.");
                Console.ResetColor();
                StaffActionPerformed?.Invoke("UPDATE_DOG_FAILED", $"Failed to update dog with ID: {id}");
            }
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        // --- Helper menus for input with validation ---

        // Menu to add a new dog
        private void AddDogMenu()
        {
            Dog newDog = Dog.CreateDogWithValidation(Program.availableDogs);
            if (newDog != null)
            {
                AddDog(newDog, Program.availableDogs);
            }
            else
            {
                Console.WriteLine("Dog creation cancelled or failed. Press Enter to continue...");
                Console.ReadLine();
            }
        }

        // Menu to remove a dog with validation
        private void RemoveDogMenu()
        {
            Console.Clear();
            TerminalArt.Header("Remove Dog");

            if (Program.availableDogs.Count == 0)
            {
                Console.WriteLine("No dogs in the system to remove.");
                Console.WriteLine("Press Enter to return...");
                Console.ReadLine();
                return;
            }

            // Show current dogs
            Console.WriteLine("Current dogs in system:");
            foreach (var dog in Program.availableDogs.OrderBy(d => d.Id))
            {
                Console.WriteLine(dog.GetDogDetails());
            }

            int? id = ValidationUtils.GetValidatedIntInput(
                "\nEnter Dog ID to remove: ",
                dogId => ValidationUtils.IsValidDogId(dogId),
                "Please enter a valid positive dog ID."
            );

            if (id.HasValue)
            {
                RemoveDog(id.Value, Program.availableDogs);
            }
        }

        // Menu to update a dog with validation
        private void UpdateDogMenu()
        {
            Console.Clear();
            TerminalArt.Header("Update Dog");

            if (Program.availableDogs.Count == 0)
            {
                Console.WriteLine("No dogs in the system to update.");
                Console.WriteLine("Press Enter to return...");
                Console.ReadLine();
                return;
            }

            // Show current dogs
            Console.WriteLine("Current dogs in system:");
            foreach (var dog in Program.availableDogs.OrderBy(d => d.Id))
            {
                Console.WriteLine(dog.GetDogDetails());
            }

            int? id = ValidationUtils.GetValidatedIntInput(
                "\nEnter Dog ID to update: ",
                dogId => ValidationUtils.IsValidDogId(dogId) && Program.availableDogs.Any(d => d.Id == dogId),
                "Please enter a valid ID of an existing dog."
            );

            if (id.HasValue)
            {
                UpdateDog(id.Value, Program.availableDogs);
            }
        }

        // View all dogs as staff (including adopted ones)
        private void ViewAllDogsAsStaff()
        {
            Console.Clear();
            TerminalArt.Header("All Dogs (Staff View)");

            if (Program.availableDogs.Count == 0)
            {
                Console.WriteLine("No dogs in the system.");
            }
            else
            {
                var sortedDogs = Program.availableDogs.OrderBy(d => d.Id).ToList();
                Console.WriteLine($"Total dogs in system: {sortedDogs.Count}");
                Console.WriteLine($"Available: {sortedDogs.Count(d => d.IsAvailable)}");
                Console.WriteLine($"Adopted: {sortedDogs.Count(d => !d.IsAvailable)}\n");

                foreach (var dog in sortedDogs)
                {
                    Console.WriteLine(dog.GetDogDetails());
                }
            }

            EventManager.TriggerLog($"Staff viewed all dogs ({Program.availableDogs.Count} total)");
            Console.WriteLine("\nPress Enter to return...");
            Console.ReadLine();
        }
    }
}