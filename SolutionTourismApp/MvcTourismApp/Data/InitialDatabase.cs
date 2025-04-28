using LibraryTourismApp;
using Microsoft.AspNetCore.Identity;

namespace MvcTourismApp.Data
{
    public class InitialDatabase
    {
        public static void SeedDatabase(IServiceProvider services)
        {
            //1. database service
            ApplicationDbContext database = services.GetRequiredService<ApplicationDbContext>();

            //2. app user service
            UserManager<AppUser> userManager = services.GetRequiredService<UserManager<AppUser>>();

            //3. roles service
            RoleManager<IdentityRole> roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            string touristRole = "Tourist";
            string moderatorRole = "Moderator";

            if (!database.Roles.Any())
            {
                IdentityRole role = new IdentityRole(touristRole);
                roleManager.CreateAsync(role).Wait();

                role = new IdentityRole(moderatorRole);
                roleManager.CreateAsync(role).Wait();
            }

            if (!database.AppUser.Any())
            {
                List<string> allRoles = new List<string> { touristRole, moderatorRole };

                //Admin user
                AppUser appUser = new AppUser("Test", "Admin 1", DateTime.Now.AddYears(-25), "Test.Admin1@test.com", "3045550001", "Test.Admin1");
                appUser.EmailConfirmed = true;
                userManager.CreateAsync(appUser).Wait();
                userManager.AddToRolesAsync(appUser, allRoles).Wait();

                //No roles user
                appUser = new AppUser("Test", "User 1", DateTime.Now.AddYears(-20), "Test.User1@test.com", "3045550002", "Test.User1");
                appUser.EmailConfirmed = true;
                userManager.CreateAsync(appUser).Wait();
                userManager.AddToRolesAsync(appUser, allRoles).Wait();
            }

            if (!database.Tourist.Any())
            {
                Tourist tourist = new Tourist("Test" ,"Tourist 1", DateTime.Now.AddYears(-22), "Test.Tourist1@test.com", "3045550003", "Test.Tourist1");
                tourist.EmailConfirmed = true;
                userManager.CreateAsync(tourist).Wait();
                userManager.AddToRoleAsync(tourist, touristRole).Wait();

                tourist = new Tourist("Test", "Tourist 2", DateTime.Now.AddYears(-30), "Test.Tourist2@test.com", "3045550004", "Test.Tourist2");
                tourist.EmailConfirmed = true;
                userManager.CreateAsync(tourist).Wait();
                userManager.AddToRoleAsync(tourist, touristRole).Wait();
            }

            if (!database.Moderator.Any())
            {
                Moderator moderator = new Moderator("Test", "Moderator 1", DateTime.Now.AddYears(-21), "Test.Moderator1@test.com", "3045550005", "Test.Moderator1");
                moderator.EmailConfirmed = true;
                userManager.CreateAsync(moderator).Wait();
                userManager.AddToRoleAsync(moderator, moderatorRole).Wait();

                moderator = new Moderator("Test", "Moderator 2", DateTime.Now.AddYears(-30), "Test.Moderator2@test.com", "3045550006", "Test.Moderator2");
                moderator.EmailConfirmed = true;
                userManager.CreateAsync(moderator).Wait();
                userManager.AddToRoleAsync(moderator, moderatorRole).Wait();
            }
                        
            if (!database.Attraction.Any())
            {
                Attraction attraction = new Attraction("The Ritz-Carlton, Los Angeles", "A luxury hotel offering world-class amenities and stunning city views.", AttractionTypes.Lodging, "900 W Olympic Blvd", "Los Angeles", "90015", "California", "Los Angeles County", AttractionStatus.Approved, null, "https://www.ritzcarlton.com/en/hotels/los-angeles");
                database.Attraction.Add(attraction);
                database.SaveChanges();

                attraction = new Attraction("Alinea", "A world-renowned three-Michelin-star restaurant known for its innovative dining experience.", AttractionTypes.Restaurant, "1723 N Halsted St", "Chicago", "60614", "Illinois", "Cook County", AttractionStatus.Approved, null, "https://www.alinearestaurant.com");
                database.Attraction.Add(attraction);
                database.SaveChanges();

                attraction = new Attraction("Taste of Texas", "A famous steakhouse known for its premium cuts and Texas-sized portions.", AttractionTypes.Restaurant, "10505 Katy Fwy", "Houston", "77024", "Texas", "Harris County", AttractionStatus.Approved, null, "https://www.tasteoftexas.com");
                database.Attraction.Add(attraction);
                database.SaveChanges();

                attraction = new Attraction("Desert Botanical Garden", "A beautiful garden showcasing desert plants.", AttractionTypes.Activity, "1201 N Galvin Pkwy", "Phoenix", "85008", "Arizona", "Maricopa County", AttractionStatus.Approved, null, "https://dbg.org");
                database.Attraction.Add(attraction);
                database.SaveChanges();

                attraction = new Attraction("San Diego Zoo", "A world-renowned zoo with thousands of animals.", AttractionTypes.Activity, "2920 Zoo Dr", "San Diego", "92101", "California", "San Diego County", AttractionStatus.Approved, null, "https://zoo.sandiegozoo.org");
                database.Attraction.Add(attraction);
                database.SaveChanges();

                attraction = new Attraction("Pike Place Market", "A historic market with shops, seafood, and the original Starbucks.", AttractionTypes.Activity, "85 Pike St", "Seattle", "98101", "Washington", "King County", AttractionStatus.Denied, null, "https://pikeplacemarket.org");
                database.Attraction.Add(attraction);
                database.SaveChanges();

                attraction = new Attraction("Miami Beach Boardwalk", "A scenic boardwalk along the beach.", AttractionTypes.Activity, "Miami Beach", "Miami", "33140", "Florida", "Miami-Dade County", AttractionStatus.Pending, null, "https://www.miamibeachfl.gov");
                database.Attraction.Add(attraction);
                database.SaveChanges();

                attraction = new Attraction("The Strip", "A vibrant street with casinos and entertainment.", AttractionTypes.Activity, "Las Vegas Blvd", "Las Vegas", "89109", "Nevada", "Clark County", AttractionStatus.Approved, null, "https://www.visitlasvegas.com");
                database.Attraction.Add(attraction);
                database.SaveChanges();

                attraction = new Attraction("The Adolphus Hotel", "A historic luxury hotel in downtown Dallas known for its elegant architecture and upscale accommodations.", AttractionTypes.Lodging, "1321 Commerce St", "Dallas", "75202", "Texas", "Dallas County", AttractionStatus.Approved, null, "https://www.adolphus.com");
                database.Attraction.Add(attraction);
                database.SaveChanges();

                attraction = new Attraction("Liberty Bell", "A historic symbol of American independence.", AttractionTypes.Activity, "526 Market St", "Philadelphia", "19106", "Pennsylvania", "Philadelphia County", AttractionStatus.Pending, null, "https://www.nps.gov/inde/learn/historyculture/stories-libertybell.htm");
                database.Attraction.Add(attraction);
                database.SaveChanges();

                attraction = new Attraction("West Virginia State Museum", "A museum showcasing West Virginia's history.", AttractionTypes.Activity, "1900 Kanawha Blvd E", "Charleston", "25305", "West Virginia", "Kanawha County", AttractionStatus.Approved, null, "https://wvculture.org");
                database.Attraction.Add(attraction);
                database.SaveChanges();

                attraction = new Attraction("Mountaineer Field", "A stadium home to West Virginia University football.", AttractionTypes.Activity, "1 Ira Errett Rodgers Dr", "Morgantown", "26505", "West Virginia", "Monongalia County", AttractionStatus.Approved, null, "https://wvusports.com");
                database.Attraction.Add(attraction);
                database.SaveChanges();

                attraction = new Attraction("Camden Park", "An amusement park with classic rides and attractions.", AttractionTypes.Activity, "5000 Waverly Rd", "Huntington", "25704", "West Virginia", "Cabell County", AttractionStatus.Denied, null, "https://camdenpark.com");
                database.Attraction.Add(attraction);
                database.SaveChanges();
            }

            if (!database.SavedList.Any())
            {
                Tourist tourist = database.Tourist.FirstOrDefault();
                SavedList savedList = new SavedList(tourist, "Restaurants", new DateTime(2025, 1, 1));
                database.SavedList.Add(savedList);
                database.SaveChanges();

                savedList = new SavedList(tourist, "Some things I want to see", new DateTime(2025, 2, 1));
                database.SavedList.Add(savedList);
                database.SaveChanges();
            }

            if (!database.AttractionSavedList.Any())
            {
                List<SavedList> savedLists = database.SavedList.ToList();
                List<Attraction> restaurants = database.Attraction.Where(a => a.TypeOfAttraction == AttractionTypes.Restaurant).ToList();

                AttractionSavedList attractionSavedList = new AttractionSavedList(restaurants[0], savedLists[0], new DateTime(2025, 1, 1));
                database.AttractionSavedList.Add(attractionSavedList);
                database.SaveChanges();

                attractionSavedList = new AttractionSavedList(restaurants[1], savedLists[0], new DateTime(2025, 1, 1));
                database.AttractionSavedList.Add(attractionSavedList);
                database.SaveChanges();


                List<Attraction> attractions = database.Attraction.ToList();

                attractionSavedList = new AttractionSavedList(attractions[0], savedLists[1], new DateTime(2025, 2, 1));
                database.AttractionSavedList.Add(attractionSavedList);
                database.SaveChanges();

                attractionSavedList = new AttractionSavedList(attractions[10], savedLists[1], new DateTime(2025, 2, 3));
                database.AttractionSavedList.Add(attractionSavedList);
                database.SaveChanges();

                attractionSavedList = new AttractionSavedList(attractions[11], savedLists[1], new DateTime(2025, 2, 2));
                database.AttractionSavedList.Add(attractionSavedList);
                database.SaveChanges();
            }

            if (!database.Theme.Any())
            {
                Theme theme = new Theme("Cultural Heritage", "Attractions that showcase the rich cultural history and heritage of an area.");
                database.Theme.Add(theme);
                database.SaveChanges();

                theme = new Theme("Luxury Lodging", "High-end hotels and resorts offering exceptional services and experiences.");
                database.Theme.Add(theme);
                database.SaveChanges();

                theme = new Theme("Dining Experience", "Fine dining and renowned restaurants offering unique and exceptional culinary experiences.");
                database.Theme.Add(theme);
                database.SaveChanges();

                theme = new Theme("Family Fun", "Attractions that are perfect for family-friendly activities, entertainment, and adventure.");
                database.Theme.Add(theme);
                database.SaveChanges();

                theme = new Theme("Nature and Outdoors", "Attractions offering access to beautiful natural landscapes and outdoor adventures.");
                database.Theme.Add(theme);
                database.SaveChanges();

                theme = new Theme("Historic Landmarks", "Attractions with significant historical importance or those that have shaped the local culture.");
                database.Theme.Add(theme);
                database.SaveChanges();

                theme = new Theme("Art and Museums", "Attractions focused on art, history, and education through museum collections and exhibitions.");
                database.Theme.Add(theme);
                database.SaveChanges();
            }

            if (!database.AttractionTheme.Any())
            {
                List<Attraction> attractions = database.Attraction.ToList();
                List<Theme> themes = database.Theme.ToList();

                AttractionTheme attractionTheme = new AttractionTheme(attractions[0], themes[0]);  // The Beverly Hills Hotel -> Cultural Heritage
                database.AttractionTheme.Add(attractionTheme);
                database.SaveChanges();

                attractionTheme = new AttractionTheme(attractions[0], themes[1]);  // The Beverly Hills Hotel -> luxury lodging
                database.AttractionTheme.Add(attractionTheme);
                database.SaveChanges();

                attractionTheme = new AttractionTheme(attractions[1], themes[5]);  // Alinea -> Dining Experience
                database.AttractionTheme.Add(attractionTheme);
                database.SaveChanges();

                attractionTheme = new AttractionTheme(attractions[2], themes[5]);  // Taste of Texas -> Dining Experience
                database.AttractionTheme.Add(attractionTheme);
                database.SaveChanges();

                attractionTheme = new AttractionTheme(attractions[3], themes[3]);  // Desert Botanical Garden -> Family Fun
                database.AttractionTheme.Add(attractionTheme);
                database.SaveChanges();

                attractionTheme = new AttractionTheme(attractions[4], themes[4]);  // San Diego Zoo -> Nature and Outdoors
                database.AttractionTheme.Add(attractionTheme);
                database.SaveChanges();

                attractionTheme = new AttractionTheme(attractions[4], themes[3]);  // San Diego Zoo -> family fun
                database.AttractionTheme.Add(attractionTheme);
                database.SaveChanges();

                attractionTheme = new AttractionTheme(attractions[5], themes[3]);  // Pike Place Market -> Family Fun
                database.AttractionTheme.Add(attractionTheme);
                database.SaveChanges();

                attractionTheme = new AttractionTheme(attractions[6], themes[4]);  // Miami Beach Boardwalk -> Nature and Outdoors
                database.AttractionTheme.Add(attractionTheme);
                database.SaveChanges();

                attractionTheme = new AttractionTheme(attractions[7], themes[2]);  // The Strip -> Dining Experience
                database.AttractionTheme.Add(attractionTheme);
                database.SaveChanges();

                attractionTheme = new AttractionTheme(attractions[8], themes[5]);  // The Adolphus Hotel -> Luxury Lodging
                database.AttractionTheme.Add(attractionTheme);
                database.SaveChanges();

                attractionTheme = new AttractionTheme(attractions[9], themes[5]);  // Liberty Bell -> Historic Landmarks
                database.AttractionTheme.Add(attractionTheme);
                database.SaveChanges();

                attractionTheme = new AttractionTheme(attractions[10], themes[0]);  // Mountaineer Field -> Cultural Heritage
                database.AttractionTheme.Add(attractionTheme);
                database.SaveChanges();

                attractionTheme = new AttractionTheme(attractions[11], themes[4]);  // Coopers Rock State Forest -> Nature and Outdoors
                database.AttractionTheme.Add(attractionTheme);
                database.SaveChanges();

                attractionTheme = new AttractionTheme(attractions[12], themes[3]);  // Camden Park -> Family Fun
                database.AttractionTheme.Add(attractionTheme);
                database.SaveChanges();

            }

            if (!database.Review.Any())
            {
                List<Tourist> tourists = database.Tourist.ToList();
                List<Attraction> attractions = database.Attraction.ToList();

                Review review = new Review(tourists[0], attractions[0], "Amazing Stay!", 5, new DateTime(2025, 2, 2), "The Beverly Hills Hotel is luxurious and the staff was fantastic!", new DateTime(2025, 2, 1), null);
                database.Review.Add(review);
                database.SaveChanges();

                review = new Review(tourists[0], attractions[1], "Unforgettable Dining Experience", 5, new DateTime(2025, 3, 2), "Alinea offers a dining experience like no other. Each course is a work of art!", new DateTime(2025, 3, 1), null);
                database.Review.Add(review);
                database.SaveChanges();


                review = new Review(tourists[0], attractions[2], "A Great Meal", 4, new DateTime(2025, 1, 31), "Taste of Texas served up some of the best steaks I've ever had. The portions were huge!", new DateTime(2025, 1, 30), null);
                database.Review.Add(review);
                database.SaveChanges();

                review = new Review(tourists[1], attractions[3], "Perfect Family Outing", 4, new DateTime(2025, 3, 15), "The Desert Botanical Garden was a peaceful retreat, and my kids loved exploring the trails!", null, null);
                database.Review.Add(review);
                database.SaveChanges();


                review = new Review(tourists[1], attractions[4], "Great Day Out", 5, new DateTime(2025, 1, 1), "The San Diego Zoo is incredibly well-maintained. We loved seeing the pandas and the giraffes!", null, new DateTime(2025, 1, 5));
                database.Review.Add(review);
                database.SaveChanges();


                review = new Review(tourists[0], attractions[10], "A Thrilling Game Day!", 5, new DateTime(2025, 1, 1), "The atmosphere at Mountaineer Field is unmatched. The fans are passionate, and the game was a blast!", null, null);
                database.Review.Add(review);
                database.SaveChanges();

                review = new Review(tourists[1], attractions[10], "I don't like the field", 1, new DateTime(2024, 12, 2), null, new DateTime(2025, 1, 1), new DateTime(2025, 1, 2));
                database.Review.Add(review);
                database.SaveChanges();
            }
            
        }

    }
}
