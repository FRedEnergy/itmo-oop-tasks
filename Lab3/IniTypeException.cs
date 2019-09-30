using System;

namespace Lab3
{
    public class IniTypeException: Exception
    {
        public IniTypeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}