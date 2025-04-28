using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace LibraryTourismApp
{
    public class Attraction
    {
        [Key]
        public int AttractionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public AttractionTypes TypeOfAttraction { get; set; }
        public AttractionStatus Status { get; set; }

        public string Street { get; set; }
        public string County { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string FullAddress { get { return $"{Street}, {City}, {State} {ZipCode}"; } }
        public string? Website { get; set; }
        

        public Tourist? TouristWhoAdded { get; set; }
        public Moderator? ModeratorWhoMadeDecision { get; set; }
        public DateTime? DateDecisionMade { get; set; }

        public List<AttractionTheme> Themes { get; set; } = new List<AttractionTheme>();
        public List<Review> Reviews { get; set; } = new List<Review>();
        public List<AttractionSavedList> SavedLists { get; set; } = new List<AttractionSavedList>();


        public Attraction(string name, string description, AttractionTypes typeOfAttraction, string street, string city, string zip, string state, string county, AttractionStatus status,
            Tourist? touristWhoAdded = null, 
            string? website = null)
        {
            Name = name;
            Description = description;
            TypeOfAttraction = typeOfAttraction;
            Street = street;
            City = city;
            ZipCode = zip;
            State = state;
            County = county;
            Status = status;

            TouristWhoAdded = touristWhoAdded;
            Website = website;
        }

        public Attraction(string name, string description, AttractionTypes typeOfAttraction, string street, string city, string zip, string state, string county, AttractionStatus status,
            Tourist? touristWhoAdded = null, Moderator? moderatorWhoDecidied = null, DateTime? dateDecisionMade = null,
            string? website = null)
        {
            Name = name;
            Description = description;
            TypeOfAttraction = typeOfAttraction;
            Street = street;
            City = city;
            ZipCode = zip;
            State = state;
            County = county;
            Status = status;

            TouristWhoAdded = touristWhoAdded;
            ModeratorWhoMadeDecision = moderatorWhoDecidied;
            DateDecisionMade = dateDecisionMade;

            Website = website;
        }

        public Attraction() { }

        public List<Review> FindMostRecentReviews()
        {
            List<Review> recentReviews = new List<Review>();

            if (Reviews.Any())
            {
                recentReviews = Reviews.OrderByDescending(ar => ar.DateCreated).ToList();
            }

            return recentReviews;
        }

        public double FindAverageReviewRating()
        {
            int total = 0;
            double avgRating = 0;

            if (Reviews.Any())
            {
                foreach (Review eachReview in Reviews)
                {
                    total += eachReview.Rating;
                }

                avgRating = (double)total / Reviews.Count;
            }

            //a 0 average means the attraction has no reviews
            return avgRating;
        }

        public string FindAvgStarRatingIcons()
        {
            //used to get the display of avg rating in stars
            //full star : <i class="fa fa-star fa_custom"></i>
            //half star : <i class="fa fa-star-half-o fa_custom"></i>
            //empty star : <i class="fa fa-star-o fa_custom"></i>

            StringBuilder sb = new StringBuilder();
            if (Reviews.Any())
            {
                int maxStars = 5;

                double avgRating = FindAverageReviewRating();
                //getting the average rating for display

                int fullStars = (int)avgRating;
                //storing all the full stars (whole numbers) from the average

                int halfStars = avgRating % 1 >= 0.5 ? 1 : 0;
                //storing the half stars (not whole number) from the average -> if the remainder is more than 0.5 it's a half star (1), otherwise it's not (0)

                int emptyStars = maxStars - fullStars - halfStars;
                //5 is the max star amount, subtracting the amount of stars and half stars to find the count of empty stars to display

                for (int i = 0; i < maxStars; i++)
                {
                    //populate the stars using the full star count, then half stars, then empty stars
                    if (i < fullStars)
                        sb.Append("<i class=\"fa fa-star fa_custom\"></i>");
                    else if (i < fullStars + halfStars)
                        sb.Append("<i class=\"fa fa-star-half-o fa_custom\"></i>");
                    else
                        sb.Append("<i class=\"fa fa-star-o fa_custom\"></i>");
                }
            }

            string starIcons = sb.ToString();

            return starIcons;
        }

        public static List<Attraction> SearchAttractions(List<Attraction> inputAttractions,
           int? inputId, string? inputCity, string? inputCounty,
           int? inputThemeId, AttractionTypes? inputType
           )
        {
            List<Attraction> allAttractions = inputAttractions;

            if (inputId != null)
            {
                allAttractions = allAttractions.Where(a => a.AttractionId == inputId).ToList();
            }

            if (!string.IsNullOrWhiteSpace(inputCity))
            {
                allAttractions = allAttractions.Where(a => a.City.ToLower().Contains(inputCity.ToLower())).ToList();
            }

            if (!string.IsNullOrWhiteSpace(inputCounty))
            {
                allAttractions = allAttractions.Where(a => a.County.ToLower().Contains(inputCounty.ToLower())).ToList();
            }

            if (inputThemeId != null)
            {
                allAttractions = allAttractions.Where(a => a.Themes.Any(at => at.Theme.ThemeId == inputThemeId)).ToList();
            }

            if (inputType != null)
            {
                allAttractions = allAttractions.Where(a => a.TypeOfAttraction == inputType).ToList();
            }

            allAttractions = allAttractions.Where(a => a.Status == AttractionStatus.Approved).ToList();

            return allAttractions;

        }

    }

    public enum AttractionTypes
    {
        Lodging, Restaurant, Activity
    }

    public enum AttractionStatus
    {
        Pending, Approved, Denied
    }
}