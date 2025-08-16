using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogAdoption
{
    public class StaffMember : Person
    {
        public StaffMember(string name, string contactInfo) : base(name, contactInfo)
        {
        }

        public override string GetDetails()
        {
            return $"Staff Name: {Name}, Contact: {ContactInfo}";
        }

        public void ManageDogs()
        {
            Console.WriteLine("Managing dogs...");
        }
    }
}
