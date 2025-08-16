using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogAdoption
{
    public abstract class Person
    {
        public string Name { get; set; }
        public string ContactInfo { get; set; }

        public Person(string name, string contactInfo)
        {
            Name = name;
            ContactInfo = contactInfo;
        }

        public abstract string GetDetails();
    }
}
