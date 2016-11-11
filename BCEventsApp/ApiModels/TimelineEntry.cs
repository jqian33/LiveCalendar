using System;
using System.Collections.Generic;

namespace BCEventsApp.ApiModels
{
    public class TimelineEntry
    {
        public DateTime Date { get; set; }

        public List<TimelineEvent> EventList { get; set; } 
    }
}