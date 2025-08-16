using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogAdoption
{
    public class Dog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Breed { get; set; }
        public int Age { get; set; }
        public string Size { get; set; }
        public bool IsAvailable { get; set; }

        public Dog(int id, string name, string breed, int age, string size)
        {
            Id = id;
            Name = name;
            Breed = breed;
            Age = age;
            Size = size;
            IsAvailable = true;
        }

        public string GetDogDetails()
        {
            string status = IsAvailable ? "Available" : "Adopted";
            return $"ID: {Id}, Name: {Name}, Breed: {Breed}, Age: {Age}, Size: {Size}, Status: {status}";
        }
    }
}