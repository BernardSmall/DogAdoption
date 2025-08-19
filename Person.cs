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
        // Backing fields
        private string name;
        private string contactInfo;

        // Common properties
        public string Name { 
            get { return Name; }
            set
            {
                if (!ValidationUtils.IsValidName(value))
                {
                    throw new ArgumentException("Name must be 2-50 characters and contain only letters, spaces, hyphens, and apostrophes.");
                }
                name = value.Trim();
            }
        }
        public string ContactInfo { 
            get { return contactInfo; }
            set{
                if (!ValidationUtils.IsValidPhoneNumber(value))
                {
                    throw new ArgumentException("Contact info must be a valid phone number (10-15 digits).");
                }
                contactInfo = value.Trim();
            }
        }

        // Constructor to set name and contact info
        public Person(string name, string contactInfo)
        {
            // Validate inputs before setting
            if (!ValidationUtils.IsValidName(name))
            {
                throw new ArgumentException("Name must be 2-50 characters and contain only letters, spaces, hyphens, and apostrophes.");
            }

            if (!ValidationUtils.IsValidPhoneNumber(contactInfo))
            {
                throw new ArgumentException("Contact info must be a valid phone number (10-15 digits).");
            }

            this.name = name.Trim();
            this.contactInfo = contactInfo.Trim();
        }

        // Force subclasses to provide their own details
        public abstract string GetDetails();


        // Validate person data
        public virtual bool IsValid()
        {
            return ValidationUtils.IsValidName(name) && ValidationUtils.IsValidPhoneNumber(contactInfo);
        }
    }
}
