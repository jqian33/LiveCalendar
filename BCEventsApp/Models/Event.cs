using System;
using System.Collections.Generic;

namespace BCEventsApp.Models
{

    public class Event
    {
        // Unique event identifier
        public int Id { get; set; }

        // Event title
        public string Title { get; set; }

        // Event description
        public string Description { get; set; }

        // Event location
        public string Location { get; set; }

        // Start date and time
        public DateTime Start { get; set; }

        // Event duration
        public int Duration { get; set; }

        // End date and time
        public DateTime End { get; set; }

        // Recurance rule
        public string RRule { get; set; }

        // List of associated tags
        public List<Tag> Tags { get; set; }

        // Owner of event
        public User Owner { get; set; }

        // Parent calendar ID
        public int CalendarId { get; set; }

        // Event privacy
        public bool Public { get; set; }

        // List of attendees
        public List<User> Attendees { get; set; }

    }
}