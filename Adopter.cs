using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogAdoption
{
    public class Adopter : Person
    {
        private string email;
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
       
        public Adopter(string name, string contactInfo, string email) : base(name, contactInfo)
        {
            this.email = email;
        }

        public override string GetDetails()
        {
            return $"Adopter Name: {Name}, Contact Info (Phone number): {ContactInfo}, Email: {email}";
        }
    }
}
