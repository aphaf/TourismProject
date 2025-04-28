using LibraryTourismApp;
using Microsoft.EntityFrameworkCore;
using MvcTourismApp.Data;

namespace MvcTourismApp.Models
{
    public class SavedListRepo : ISavedListRepo
    {
        private ApplicationDbContext _database;

        public SavedListRepo(ApplicationDbContext database)
        {
            _database = database;
        }

        public List<SavedList> GetAllSavedListsForUser(string userId)
        {
            List<SavedList> allSavedLists = _database.SavedList.Where(sl => sl.Tourist.Id == userId).Include(sl => sl.Tourist).Include(sl => sl.AttractionList).ThenInclude(al => al.Attraction).ThenInclude(a => a.Reviews).ToList();
            return allSavedLists;
        }

        public SavedList FindSavedList(int savedListId)
        {
            SavedList savedList = _database.SavedList.Where(sl => sl.SavedListId == savedListId).Include(sl => sl.Tourist).Include(sl => sl.AttractionList).ThenInclude(al => al.Attraction).ThenInclude(a => a.Reviews).FirstOrDefault();
            return savedList;
        }

        public void UpdateSavedList(SavedList savedList)
        {
            _database.SavedList.Update(savedList);
            _database.SaveChanges();
        }

        public int AddSavedList(SavedList savedList)
        {
            int savedListId = 0;
            _database.SavedList.Add(savedList);

            try
            {
                _database.SaveChanges();
                savedListId = savedList.SavedListId;
            }
            catch (DbUpdateException dbUpdateException)
            {
                savedListId = -1;
                string errorMessage = dbUpdateException.Message;
            }

            return savedListId;
        }

        public int AddAttractionToSavedList(AttractionSavedList attractionSavedList)
        {
            int attractionSavedListId = 0;

            try
            {
                _database.AttractionSavedList.Add(attractionSavedList);
                _database.SaveChanges();
                attractionSavedListId = attractionSavedList.SavedListId;
            }
            catch (DbUpdateException dbUpdateException)
            {
                attractionSavedListId = -1;
                string errorMessage = dbUpdateException.Message;
            }
            catch (InvalidOperationException invalidOpEx)
            {
                attractionSavedListId = -2;
                string errorMessage = invalidOpEx.Message;
            }
            catch (Exception ex)
            {
                attractionSavedListId = -3;
                string errorMessage = ex.Message;
            }

            return attractionSavedListId;
        }

        public AttractionSavedList FindAttractionSavedList(int attractionSavedListId)
        {
            AttractionSavedList attractionSavedList = _database.AttractionSavedList.Where(asl => asl.AttractionSavedListId == attractionSavedListId).Include(asl => asl.SavedList).Include(asl => asl.Attraction).FirstOrDefault();
            return attractionSavedList;
        }

        public void DeleteAttractionOnSavedList(AttractionSavedList attractionSavedList)
        {
            _database.AttractionSavedList.Remove(attractionSavedList);
            _database.SaveChanges();
        }

        public void DeleteSavedList(SavedList savedList)
        {
            _database.SavedList.Remove(savedList);
            _database.SaveChanges();
        }
    }
}
