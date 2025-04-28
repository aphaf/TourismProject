using LibraryTourismApp;

namespace MvcTourismApp.Models
{
    public interface IReviewRepo
    {
        public Review FindReview(int reviewId);
        public List<Review> GetAllReviews();
        public List<Review> GetAllReviewsForAttraction(int attractionId);
        public List<Review> GetAllReviewsForTourist(string loggedInUserId);
        public int AddReview(Review review);
        public void UpdateReview(Review review);
        public void DeleteReview(Review review);
    }
}
