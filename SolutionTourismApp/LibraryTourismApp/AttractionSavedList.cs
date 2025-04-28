using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace LibraryTourismApp
{
    public class AttractionSavedList
    {
        [Key]
        public int AttractionSavedListId { get; set; }

        public int AttractionId { get; set; }
        [ForeignKey(nameof(AttractionId))]
        public Attraction Attraction { get; set; }

        public int SavedListId { get; set; }
        [ForeignKey(nameof(SavedListId))]
        public SavedList SavedList { get; set; }
        public DateTime DateAddedToList { get; set; }

        public AttractionSavedList(Attraction attraction, SavedList savedList)
        {
            Attraction = attraction;
            SavedList = savedList;
            DateAddedToList = DateTime.Now;
        }

        public AttractionSavedList(Attraction attraction, SavedList savedList, DateTime dateAdded)
        {
            Attraction = attraction;
            SavedList = savedList;
            DateAddedToList = dateAdded;
        }

        public AttractionSavedList() { }
    }
}