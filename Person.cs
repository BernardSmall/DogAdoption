using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogAdoption
{
    // Base class for all people in the system (adopters, staff, etc.)
    public abstract class Person
    {
        // Common properties
        public string Name { get; set; }
        public string ContactInfo { get; set; }

        // Constructor to set name and contact info
        public Person(string name, string contactInfo)
        {
            Name = name;
            ContactInfo = contactInfo;
        }

        // Force subclasses to provide their own details
        public abstract string GetDetails();
    }
}
