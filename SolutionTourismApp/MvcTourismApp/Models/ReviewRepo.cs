using LibraryTourismApp;
using Microsoft.EntityFrameworkCore;
using MvcTourismApp.Data;

namespace MvcTourismApp.Models
{
    public class ReviewRepo : IReviewRepo
    {
        private ApplicationDbContext _database;

        public ReviewRepo(ApplicationDbContext database)
        {
            _database = database;
        }

        public Review FindReview(int reviewId)
        {
            Review review = _database.Review.Include(ar => ar.Attraction).Include(ar => ar.Tourist).Where(ar => ar.ReviewId == reviewId).First();
            return review;
        }

        public void UpdateReview(Review review)
        {
            _database.Review.Update(review);
            _database.SaveChanges();
        }

        public List<Review> GetAllReviews()
        {
            List<Review> allReviews = _database.Review.Include(ar => ar.Attraction).Include(ar => ar.Tourist).ToList();
            return allReviews;
        }
        public List<Review> GetAllReviewsForAttraction(int attractionId)
        {
            List<Review> allReviewsForAttraction = GetAllReviews().Where(r => r.Attraction.AttractionId == attractionId).ToList();
            return allReviewsForAttraction;
        }

        public List<Review> GetAllReviewsForTourist(string loggedInUserId)
        {
            List<Review> allReviewsForAttraction = GetAllReviews().Where(r => r.Tourist.Id == loggedInUserId).ToList();
            return allReviewsForAttraction;
        }

        public int AddReview(Review ar)
        {
            int reviewId = 0;
            _database.Review.Add(ar);

            try
            {
                _database.SaveChanges();
                reviewId = ar.ReviewId;
            }
            catch (DbUpdateException dbUpdateException)
            {
                reviewId = -1;
                string errorMessage = dbUpdateException.Message;
            }

            return reviewId;
        }

        public void DeleteReview(Review review)
        {
            _database.Review.Remove(review);
            _database.SaveChanges();
        }

        
    }
}
