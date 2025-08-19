using DogAdoption;
using System;
using System.Collections.Generic;
using System.Linq;

public class Dog
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Breed { get; set; }
    public int Age { get; set; }
    public string Size { get; set; }
    public bool IsAvailable { get; set; } = true;

    public Dog(int id, string name, string breed, int age, string size)
    {
        Id = id;
        Name = name;
        Breed = breed;
        Age = age;
        Size = size;
    }

    public string GetDogDetails()
    {
        return $"ID: {Id}, Name: {Name}, Breed: {Breed}, Age: {Age}, Size: {Size}, Available: {IsAvailable}";
    }

    // Seed method populate list with dummy data
    public static void SeedDogs(List<Dog> dogList)
    {
        if (dogList.Count == 0)
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
        } }

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
        Console.Write("Enter Dog Name or ID: ");
        string input = Console.ReadLine();

        var results = dogList.Where(d =>
                        d.Name.Equals(input, StringComparison.OrdinalIgnoreCase)
                        || d.Id.ToString() == input).ToList();

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



