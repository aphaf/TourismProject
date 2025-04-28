using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryTourismApp
{
    public class Tourist : AppUser
    {
        public List<SavedList> SavedList { get; set; } = new List<SavedList>();
        public List<Review> ReviewsCreated { get; set; } = new List<Review>();

        public List<Attraction> AttractionsAdded { get; set; } = new List<Attraction>();

        public Tourist(string firstName, string lastName, DateTime dob, string email, string phoneNumber, string password) : base(firstName, lastName, dob, email, phoneNumber, password)
        {
        }

        public Tourist() : base() { }

        public Review FindMostRecentReview()
        {
            Review? mostRecentReview = null;

            if (ReviewsCreated != null && ReviewsCreated.Any())
            {
                mostRecentReview = ReviewsCreated.OrderByDescending(r => r.DateCreated).FirstOrDefault();
            }

            return mostRecentReview;
        }
    }
}