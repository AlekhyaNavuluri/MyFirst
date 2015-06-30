using System;

namespace SeatBooking
{
    public class CannotReadFileException : Exception
    {
        public CannotReadFileException(Exception inner)
            : base(null, inner)
        {
        }

        public CannotReadFileException(Exception inner, String message)
            : base(message, inner)
        {
        }
    }
}