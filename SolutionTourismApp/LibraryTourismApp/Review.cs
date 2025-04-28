using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace LibraryTourismApp
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        public Tourist Tourist { get; set; }
        public Attraction Attraction { get; set; }
        public string Title { get; set; }
        public int Rating { get; set; }
        public DateTime DateCreated { get; set; }
        public string? Description { get; set; }
        public DateTime? DateVisited { get; set; }
        public DateTime? LastEditedDate { get; set; }

        public Review(Tourist tourist, Attraction attraction, string title, int rating,
            string? description = null, DateTime? dateVisited = null)
        {
            Tourist = tourist;
            Attraction = attraction;
            Title = title;
            Rating = rating;
            DateCreated = DateTime.Now;
            Description = string.IsNullOrWhiteSpace(description) ? null : description;
            DateVisited = dateVisited;
        }

        public Review(Tourist tourist, Attraction attraction, string title, int rating, DateTime dateCreated,
            string? description = null, DateTime? dateVisited = null, DateTime? lastEditedDate = null)
        {
            Tourist = tourist;
            Attraction = attraction;
            Title = title;
            Rating = rating;
            DateCreated = dateCreated;
            Description = string.IsNullOrWhiteSpace(description) ? null : description;
            DateVisited = dateVisited;
            LastEditedDate = lastEditedDate;
        }

        public Review() { }

        
        public string FindStarRatingIcons()
        {
            //used to get the display of rating in stars
            //full star : <i class="fa fa-star fa_custom"></i>
            //empty star : <i class="fa fa-star-o fa_custom"></i>

            StringBuilder sb = new StringBuilder();

            int maxStars = 5;

            int fullStars = Rating;
            //storing all the full stars (whole numbers) from the rating

            int emptyStars = maxStars - fullStars;
            //5 is the max star amount, subtracting the amount of stars to find the count of empty stars to display

            for (int i = 0; i < maxStars; i++)
            {
                //populate the stars using the full star count, then half stars, then empty stars
                if (i < fullStars)
                    sb.Append("<i class=\"fa fa-star fa_custom\"></i>");
                else
                    sb.Append("<i class=\"fa fa-star-o fa_custom\"></i>");
            }

            string starIcons = sb.ToString();

            return starIcons;
        }

        public static List<Review> SearchReviews(List<Review> inputReviews,
            int? attractionId, string? inputTouristId, int? inputRating,
            DateTime? inputStartCreationDate, DateTime? inputEndCreationDate
            )
        {
            List<Review> allReviews = inputReviews;

            if (attractionId != null)
            {
                allReviews = allReviews.Where(a => a.Attraction.AttractionId == attractionId).ToList();
            }

            if (!string.IsNullOrWhiteSpace(inputTouristId))
            {
                allReviews = allReviews.Where(a => a.Tourist.Id == inputTouristId).ToList();
            }

            if (inputRating != null)//searching for ratings greater than or equal to input
            {
                allReviews = allReviews.Where(a => a.Rating >= inputRating).ToList();
            }

            if (inputStartCreationDate != null)
            {
                allReviews = allReviews.Where(a => a.DateCreated.Date >= inputStartCreationDate.Value.Date).ToList();
            }

            if (inputEndCreationDate != null)
            {
                allReviews = allReviews.Where(a => a.DateCreated.Date <= inputEndCreationDate.Value.Date).ToList();
            }

            return allReviews;
        }
    }
}