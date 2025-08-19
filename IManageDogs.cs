using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogAdoption
{
    // Interface for basic dog management actions
    public interface IManageDogs
    {
        // Add a new dog to the list
        void AddDog(Dog dog, List<Dog> dogList);

        // Remove a dog by ID
        void RemoveDog(int id, List<Dog> dogList);

        // Update a dog's details by ID
        void UpdateDog(int id, List<Dog> dogList);
    }
}
