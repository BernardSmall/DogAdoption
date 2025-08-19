using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogAdoption
{
    // Adopter inherits from Person and adds an Email field
    public class Adopter : Person
    {
        // Backing field for email
        private string email;

        // Property for accessing and setting email
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        // Constructor takes name, contact info, and email
        public Adopter(string name, string contactInfo, string email) : base(name, contactInfo)
        {
            this.email = email;
        }

        // Returns a formatted string with adopter details
        public override string GetDetails()
        {
            return $"Adopter Name: {Name}, Contact Info (Phone number): {ContactInfo}, Email: {email}";
        }
    }
}
