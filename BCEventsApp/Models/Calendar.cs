using System.Collections.Generic;

namespace BCEventsApp.Models
{

    public class Calendar
    {
        // Calendar ID 
        public int Id { get; set; }

        // Calendar title
        public string Title { get; set; }

        // Calendar description
        public string Description { get; set; }

        // List of Events associated to the calendar
        public Dictionary<int, Event> Events { get; set; }

        // List of associated tags
        public List<Tag> Tags { get; set; } 

        // Owner of the calendar 
        public User Owner { get; set; } 

        // Calendar privacy
        public bool Public { get; set; }

    }
}