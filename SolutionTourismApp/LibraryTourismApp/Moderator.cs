using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryTourismApp
{
    public class Moderator : AppUser
    {

        public List<Attraction> AttractionsDecisionsMadeOn { get; set; } = new List<Attraction>();

        public Moderator(string firstName, string lastName, DateTime dob, string email, string phoneNumber, string password) : base(firstName, lastName, dob, email, phoneNumber, password)
        {

        }

        public Moderator() : base() { }
    }
}