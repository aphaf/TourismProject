using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace LibraryTourismApp
{
    public class AttractionTheme
    {
        [Key]
        public int AttractionThemeId { get; set; }

        public int AttractionId { get; set; }
        [ForeignKey(nameof(AttractionId))]
        public Attraction Attraction { get; set; }
        
        public int ThemeId { get; set; }
        [ForeignKey(nameof(ThemeId))]
        public Theme Theme { get; set; }

        public AttractionTheme(Attraction attraction, Theme theme)
        {
            AttractionId = attraction.AttractionId;
            Attraction = attraction;
            ThemeId = theme.ThemeId;
            Theme = theme;
        }

        public AttractionTheme() { }
    }
}