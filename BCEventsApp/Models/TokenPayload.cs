using System;

namespace BCEventsApp.Models
{
    public class TokenPayload
    {
        public string UserId { get; set; }

        public DateTime ExpireTime { get; set; }
    }
}