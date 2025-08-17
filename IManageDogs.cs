using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogAdoption
{
    public interface IManageDogs
    {
        void AddDog(Dog dog, List<Dog> dogList);
        void RemoveDog(int id, List<Dog> dogList);
        void UpdateDog(int id, List<Dog> dogList);
    }
}
