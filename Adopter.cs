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
            set {
                if (!ValidationUtils.IsValidEmail(value))
                {
                    throw new ArgumentException("Invalid email format provided.");
                }
                email = value.Trim();
            }
        }

        // Constructor takes name, contact info, and email
        public Adopter(string name, string contactInfo, string email) : base(name, contactInfo)
        {
            // Validate email before setting
            if (!ValidationUtils.IsValidEmail(email))
            {
                throw new ArgumentException("Invalid email format provided.");
            }

            this.email = email.Trim();
        }

        // Static method to create adopter with input validation
        public static Adopter CreateAdopterWithValidation()
        {
            Console.Clear();
            TerminalArt.Header("Adopter Registration");

            // Get validated name
            string name = ValidationUtils.GetValidatedInput(
                "Enter your full name: ",
                ValidationUtils.IsValidName,
                "Name must be 2-50 characters and contain only letters, spaces, hyphens, and apostrophes."
            );
            if (name == null) return null;

            // Get validated phone number
            string contact = ValidationUtils.GetValidatedInput(
                "Enter your phone number: ",
                ValidationUtils.IsValidPhoneNumber,
                "Phone number must be 10-15 digits and can include spaces, hyphens, or parentheses."
            );
            if (contact == null) return null;

            // Get validated email
            string email = ValidationUtils.GetValidatedInput(
                "Enter your email address: ",
                ValidationUtils.IsValidEmail,
                "Please enter a valid email address (e.g., john@example.com)."
            );
            if (email == null) return null;

            try
            {
                return new Adopter(name, contact, email);
            }
            catch (ArgumentException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error creating adopter: {ex.Message}");
                Console.ResetColor();
                return null;
            }
        }


        // Returns a formatted string with adopter details
        public override string GetDetails()
        {
            return $"Adopter Name: {Name}, Contact Info (Phone number): {ContactInfo}, Email: {email}";
        }
      
        // Validate adopter data
        public bool IsValid()
        {
            return ValidationUtils.IsValidName(Name) &&
                   ValidationUtils.IsValidPhoneNumber(ContactInfo) &&
                   ValidationUtils.IsValidEmail(email);
        }
    }
}
