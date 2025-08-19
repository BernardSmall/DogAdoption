using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogAdoption
{
    // Represents a dog in the adoption system
    public class Dog
    {
        // Unique ID for the dog
        public int Id { get; set; }

        // Dog's name
        public string Name { get; set; }

        // Dog's breed
        public string Breed { get; set; }

        // Dog's age in years
        public int Age { get; set; }

        // Dog's size (e.g., Small, Medium, Large)
        public string Size { get; set; }

        // Availability status of the dog
        public bool IsAvailable { get; set; }

        // Constructor initializes dog details and sets availability to true
        public Dog(int id, string name, string breed, int age, string size)
        {
            Id = id;
            Name = name;
            Breed = breed;
            Age = age;
            Size = size;
            IsAvailable = true;
        }

        // Returns a formatted string with dog details and adoption status
        public string GetDogDetails()
        {
            string status = IsAvailable ? "Available" : "Adopted";
            return $"ID: {Id}, Name: {Name}, Breed: {Breed}, Age: {Age}, Size: {Size}, Status: {status}";
        }
    }
}
