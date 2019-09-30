using System;

namespace Lab3
{
    public class IniUnknownSectionException: Exception
    {
        public IniUnknownSectionException(String section) : base("Unable to find section with name " + section)
        {
            
        }
    }
}