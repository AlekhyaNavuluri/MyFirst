using System;

namespace SeatBooking
{
    public class FileContentsInValidException : Exception
    {
        public FileContentsInValidException(Exception inner)
            : base(null, inner)
        {
        }
    }
}