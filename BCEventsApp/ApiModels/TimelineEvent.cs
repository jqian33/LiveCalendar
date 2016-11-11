using System;

namespace BCEventsApp.ApiModels
{
    public class TimelineEvent
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string OwnerName { get; set; }

        public DateTime Start { get; set; }

        public int AttendeesCount { get; set; }
    }
}