using System;

namespace Lab3
{
    public class IniFormatException : Exception
    {
        public IniFormatException(string message) : base(message)
        {
        }

        public IniFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}