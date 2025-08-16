using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogAdoption
{
    public class Adopter : Person
    {
        public Adopter(string name, string contactInfo) : base(name, contactInfo)
        {
        }

        public override string GetDetails()
        {
            return $"Adopter Name: {Name}, Contact: {ContactInfo}";
        }
    }
}
