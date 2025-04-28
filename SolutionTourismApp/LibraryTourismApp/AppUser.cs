using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryTourismApp
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public DateTime DateOfBirth { get; set; }


        public AppUser(string firstName, string lastName, DateTime dob, string email, string phoneNumber, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dob;
            this.Email = email;
            this.UserName = email;
            this.PhoneNumber = phoneNumber;
            PasswordHasher<AppUser> passwordHasher = new PasswordHasher<AppUser>();
            this.PasswordHash = passwordHasher.HashPassword(this, password);
        }


        public AppUser() { }
    }

}