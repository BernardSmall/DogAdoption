using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogAdoption
{
    // Represents an adoption application made by an adopter for a dog
    public class AdoptionApplication
    {
        // Dog being adopted
        public Dog Dog { get; set; }

        // Adopter applying for the dog
        public Adopter Adopter { get; set; }

        // Date when the application was created
        public DateTime ApplicationDate { get; set; }

        // Constructor sets the dog, adopter, and current date
        public AdoptionApplication(Dog dog, Adopter adopter)
        {
            Dog = dog;
            Adopter = adopter;
            ApplicationDate = DateTime.Now;
        }

        // Returns details of the adoption application
        public string GetApplicationDetails()
        {
            return $"{Adopter.Name} applied for {Dog.Name} on {ApplicationDate}";
        }
    }
}
