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

        //Application processed event
        public static event Action<AdoptionApplication> ApplicationProcessed;

        // Constructor sets the dog, adopter, and current date
        public AdoptionApplication(Dog dog, Adopter adopter)
        {
            Dog = dog;
            Adopter = adopter;
            ApplicationDate = DateTime.Now;

            ApplicationProcessed?.Invoke(this);
        }

        // Returns details of the adoption application
        public string GetApplicationDetails()
        {
            return $"{Adopter.Name} applied for {Dog.Name} on {ApplicationDate}";
        }
        // INBUILT INTERFACE
        public int CompareTo(AdoptionApplication other)
        {
            if (other == null) return 1;

            // Sort by ApplicationDate (most recent first)
            return other.ApplicationDate.CompareTo(this.ApplicationDate);
        }
    }
}
