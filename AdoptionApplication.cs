using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogAdoption
{
    public class AdoptionApplication
    {
        public Dog Dog { get; set; }
        public Adopter Adopter { get; set; }
        public DateTime ApplicationDate { get; set; }

        public AdoptionApplication(Dog dog, Adopter adopter)
        {
            Dog = dog;
            Adopter = adopter;
            ApplicationDate = DateTime.Now;
        }

        public string GetApplicationDetails()
        {
            return $"{Adopter.Name} applied for {Dog.Name} on {ApplicationDate}";
        }
    }
}
