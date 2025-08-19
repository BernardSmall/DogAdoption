using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DogAdoption
{
    internal class ValidationUtils
    {
        // Email validation using regex
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
                return emailRegex.IsMatch(email);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        // Phone number validation (supports various formats)
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            // Remove common separators and spaces
            string cleanPhone = Regex.Replace(phoneNumber, @"[\s\-\(\)]+", "");

            // Check if it contains only digits and is between 10-15 characters
            return Regex.IsMatch(cleanPhone, @"^\d{10,15}$");
        }

        // Name validation
        public static bool IsValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            // Name should be at least 2 characters and contain only letters, spaces, hyphens, and apostrophes
            return name.Trim().Length >= 2 &&
                   Regex.IsMatch(name.Trim(), @"^[a-zA-Z\s\-']+$") &&
                   name.Trim().Length <= 50;
        }

        // Dog breed validation
        public static bool IsValidBreed(string breed)
        {
            if (string.IsNullOrWhiteSpace(breed))
                return false;

            return breed.Trim().Length >= 3 &&
                   breed.Trim().Length <= 30 &&
                   Regex.IsMatch(breed.Trim(), @"^[a-zA-Z\s]+$");
        }

        // Age validation for dogs
        public static bool IsValidAge(int age)
        {
            return age >= 0 && age <= 20; // Reasonable age range for dogs
        }

        // Size validation
        public static bool IsValidSize(string size)
        {
            if (string.IsNullOrWhiteSpace(size))
                return false;

            string[] validSizes = { "Small", "Medium", "Large", "Extra Large" };
            return Array.Exists(validSizes, s => s.Equals(size.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        // Dog ID validation (must be positive and unique)
        public static bool IsValidDogId(int id)
        {
            return id > 0;
        }

        // Password validation (basic requirements)
        public static bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            return password.Length >= 6 && password.Length <= 50;
        }

        // Generic string input validation
        public static bool IsValidStringInput(string input, int minLength = 1, int maxLength = 100)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            return input.Trim().Length >= minLength && input.Trim().Length <= maxLength;
        }

        // Numeric input validation helper
        public static bool TryParsePositiveInt(string input, out int result)
        {
            result = 0;
            if (int.TryParse(input, out result))
            {
                return result > 0;
            }
            return false;
        }

        // Get validated input from user with retry mechanism
        public static string GetValidatedInput(string prompt, Func<string, bool> validator, string errorMessage, int maxAttempts = 3)
        {
            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (validator(input))
                {
                    return input.Trim();
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{errorMessage} (Attempt {attempt}/{maxAttempts})");
                Console.ResetColor();

                if (attempt == maxAttempts)
                {
                    Console.WriteLine("Maximum attempts reached. Returning to previous menu...");
                    return null;
                }
            }
            return null;
        }

        // Get validated integer input
        public static int? GetValidatedIntInput(string prompt, Func<int, bool> validator, string errorMessage, int maxAttempts = 3)
        {
            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (int.TryParse(input, out int result) && validator(result))
                {
                    return result;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{errorMessage} (Attempt {attempt}/{maxAttempts})");
                Console.ResetColor();

                if (attempt == maxAttempts)
                {
                    Console.WriteLine("Maximum attempts reached. Returning to previous menu...");
                    return null;
                }
            }
            return null;
        }
    }
}
