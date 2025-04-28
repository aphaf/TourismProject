using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace LibraryTourismApp
{
    public class Theme
    {
        [Key]
        public int ThemeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<AttractionTheme> AttractionList { get; set; } = new List<AttractionTheme>();


        public Theme(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public Theme() { }
    }
}