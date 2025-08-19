using DogAdoption;
using System;
using System.Collections.Generic;
using System.Linq;

public delegate void DogStatusChangedHandler(Dog dog, string oldStatus, string newStatus);

public class Dog : IComparable<Dog>
{
    private int id;
    private string name;
    private string breed;
    private int age;
    private string size;

    public int Id
    {
        get { return id; }
        set
        {
            if (!ValidationUtils.IsValidDogId(value))
            {
                throw new ArgumentException("Dog ID must be a positive integer.");
            }
            id = value;
        }
    }
    public string Name { get { return name; } 
        set
        {
            if (!ValidationUtils.IsValidName(value))
            {
                throw new ArgumentException("Dog name must be 2-50 characters and contain only letters, spaces, hyphens, and apostrophes.");
            }
            name = value.Trim();
        }
    }
    public string Breed { get { return breed; }
        set{
            if (!ValidationUtils.IsValidBreed(value))
            {
                throw new ArgumentException("Breed must be 3-30 characters and contain only letters and spaces.");
            }
            breed = value.Trim();
        }
    }
    public int Age { get { return age; }
        set{
            if (!ValidationUtils.IsValidAge(value))
            {
                throw new ArgumentException("Dog age must be between 0 and 20 years.");
            }
            age = value;
        }
    }
    public string Size { get { return size; }
        set
        {
            if (!ValidationUtils.IsValidSize(value))
            {
                throw new ArgumentException("Size must be one of: Small, Medium, Large, Extra Large.");
            }
            size = value.Trim();
        }
    }
    public bool IsAvailable { get; set; } = true;

    public static event DogStatusChangedHandler StatusChanged;

    public Dog(int id, string name, string breed, int age, string size)
    {
        Id = id;
        Name = name;
        Breed = breed;
        Age = age;
        Size = size;
    }
    // Static method to create dog with input validation
    public static Dog CreateDogWithValidation(List<Dog> existingDogs)
    {
        Console.Clear();
        TerminalArt.Header("Add New Dog");

        // Get validated ID
        int? id = ValidationUtils.GetValidatedIntInput(
            "Enter Dog ID: ",
            dogId => ValidationUtils.IsValidDogId(dogId) && !existingDogs.Any(d => d.Id == dogId),
            "Dog ID must be a positive integer and unique."
        );
        if (id == null) return null;

        // Get validated name
        string name = ValidationUtils.GetValidatedInput(
            "Enter Dog Name: ",
            ValidationUtils.IsValidName,
            "Dog name must be 2-50 characters and contain only letters, spaces, hyphens, and apostrophes."
        );
        if (name == null) return null;

        // Get validated breed
        string breed = ValidationUtils.GetValidatedInput(
            "Enter Breed: ",
            ValidationUtils.IsValidBreed,
            "Breed must be 3-30 characters and contain only letters and spaces."
        );
        if (breed == null) return null;

        // Get validated age
        int? age = ValidationUtils.GetValidatedIntInput(
            "Enter Age: ",
            ValidationUtils.IsValidAge,
            "Dog age must be between 0 and 20 years."
        );
        if (age == null) return null;

        // Get validated size
        Console.WriteLine("Available sizes: Small, Medium, Large, Extra Large");
        string size = ValidationUtils.GetValidatedInput(
            "Enter Size: ",
            ValidationUtils.IsValidSize,
            "Size must be one of: Small, Medium, Large, Extra Large."
        );
        if (size == null) return null;

        try
        {
            return new Dog(id.Value, name, breed, age.Value, size);
        }
        catch (ArgumentException ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error creating dog: {ex.Message}");
            Console.ResetColor();
            return null;
        }
    }

    public void ChangeAvailabilityStatus(bool newStatus)
    {
        string oldStatus = IsAvailable ? "Available" : "Adopted";
        IsAvailable = newStatus;
        string newStatusText = IsAvailable ? "Available" : "Adopted";

        // TRIGGER EVENT: Fire the event when status changes
        StatusChanged?.Invoke(this, oldStatus, newStatusText);
    }

    public int CompareTo(Dog other)
    {
        if (other == null) return 1;

        // Sort by Age first, then by Name
        int ageComparison = this.Age.CompareTo(other.Age);
        if (ageComparison != 0)
            return ageComparison;

        return string.Compare(this.Name, other.Name, StringComparison.OrdinalIgnoreCase);
    }
    public string GetDogDetails()
    {
        return $"ID: {Id}, Name: {Name}, Breed: {Breed}, Age: {Age}, Size: {Size}, Available: {IsAvailable}";
    }

    // Validate dog data
    public bool IsValid()
    {
        return ValidationUtils.IsValidDogId(Id) &&
               ValidationUtils.IsValidName(Name) &&
               ValidationUtils.IsValidBreed(Breed) &&
               ValidationUtils.IsValidAge(Age) &&
               ValidationUtils.IsValidSize(Size);
    }

    // Seed method populate list with dummy data
    public static void SeedDogs(List<Dog> dogList)
    {
        if (dogList.Count == 0)
        {
            try
            {
                dogList.Add(new Dog(1, "Buddy", "Labrador", 3, "Medium"));
                dogList.Add(new Dog(2, "Milo", "Beagle", 2, "Small"));
                dogList.Add(new Dog(3, "Daisy", "German Shepherd", 4, "Large"));
                dogList.Add(new Dog(4, "Rocky", "Bulldog", 5, "Medium"));
                dogList.Add(new Dog(5, "Luna", "Poodle", 1, "Small"));
                dogList.Add(new Dog(6, "Charlie", "Golden Retriever", 6, "Large"));
                dogList.Add(new Dog(7, "Bella", "Cocker Spaniel", 3, "Medium"));
                dogList.Add(new Dog(8, "Max", "Rottweiler", 4, "Large"));
                dogList.Add(new Dog(9, "Coco", "Chihuahua", 2, "Small"));
                dogList.Add(new Dog(10, "Oscar", "Boxer", 5, "Medium"));
                dogList.Add(new Dog(11, "Ruby", "Shih Tzu", 1, "Small"));
                dogList.Add(new Dog(12, "Sam", "Border Collie", 3, "Medium"));
                dogList.Add(new Dog(13, "Nala", "Doberman", 4, "Large"));
                dogList.Add(new Dog(14, "Jack", "Dalmatian", 2, "Medium"));
                dogList.Add(new Dog(15, "Molly", "Yorkshire Terrier", 6, "Small"));
                dogList.Add(new Dog(16, "Leo", "Great Dane", 5, "Large"));
                dogList.Add(new Dog(17, "Rosie", "Siberian Husky", 3, "Large"));
                dogList.Add(new Dog(18, "Toby", "Cavalier King Charles", 4, "Small"));
                dogList.Add(new Dog(19, "Zoe", "Maltese", 2, "Small"));
                dogList.Add(new Dog(20, "Finn", "Australian Shepherd", 3, "Medium"));
            }
            catch (ArgumentException ex)
            {
                EventManager.TriggerLog($"Error seeding dogs: {ex.Message}");
            }
        } 
    }

         // Static method to display all available dogs
    public static void ViewAvailableDogs(List<Dog> dogList)
    {
        Console.Clear();
        TerminalArt.Header("Available Dogs");

        var available = dogList.Where(d => d.IsAvailable).ToList();
        if (available.Count == 0)
        {
            Console.WriteLine("No dogs are currently available.");
        }
        else
        {
            foreach (var dog in available)
            {
                Console.WriteLine(dog.GetDogDetails());
            }
        }

        Console.WriteLine("Press Enter to return.");
        Console.ReadLine();
    }

    // Static method to search dogs by name or ID
    public static void SearchDogs(List<Dog> dogList)
    {
        Console.Clear();
        TerminalArt.Header("Search Dogs");

        string input = ValidationUtils.GetValidatedInput(
            "Enter Dog Name or ID: ",
            searchTerm => ValidationUtils.IsValidStringInput(searchTerm, 1, 50),
            "Search term must be 1-50 characters long."
        );

        if (input == null) return;

        var results = dogList.Where(d =>
            d.Name.Equals(input, StringComparison.OrdinalIgnoreCase) ||
            d.Id.ToString() == input).ToList();

        Console.WriteLine("\nSearch Results:");
        if (results.Count == 0)
        {
            Console.WriteLine("No dogs found matching that criteria.");
        }
        else
        {
            foreach (var dog in results)
            {
                Console.WriteLine(dog.GetDogDetails());
            }
        }

        Console.WriteLine("Press Enter to return.");
        Console.ReadLine();

    }
}



