using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace LibraryTourismApp
{
    public class SavedList
    {
        [Key]
        public int SavedListId { get; set; }

        public Tourist Tourist { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public string? Description { get; set; }

        public List<AttractionSavedList> AttractionList { get; set; } = new List<AttractionSavedList>();

        public SavedList(Tourist tourist, string name, string? description = null)
        {
            Tourist = tourist;
            Name = name;
            Description = description;
            DateCreated = DateTime.Now;
        }

        public SavedList(Tourist tourist, string name, DateTime dateCreated, string? description = null)
        {
            Tourist = tourist;
            Name = name;
            Description = description;
            DateCreated = dateCreated;
        }

        public SavedList() { }
    }
}