using System;

namespace BCEventsApp.Exceptions
{
    public class CustomException : ApplicationException
    {
        private readonly string message;

        public CustomException(string msg)
        {
            message = msg;
        }

        public static Exception BadTokenException()
        {
            return new CustomException("Bad Token.");
        }

        public static Exception UserExistException()
        {
            return new CustomException("Username already exist.");
        }

        public static Exception InvalidUserIdException()
        {
            return new CustomException("Invalid userId.");
        }

        public static Exception CalendarSubscriptionException()
        {
            return new CustomException("Error subscribing to calendar, duplicate or invalid userId and calendarId.");
        }

        public static Exception EventSubscriptionException()
        {
            return new CustomException("Error subscribing to event, duplicate or invalid userId and eventId.");
        }

        public static Exception AttendEventException()
        {
            return new CustomException("Error attending event, duplicate or invalid userId and eventId.");
        }

        public static Exception UserNotFoundException()
        {
            return new CustomException("User not found.");
        }

        public string GetMessage()
        {
            return message;
        }
    }
}