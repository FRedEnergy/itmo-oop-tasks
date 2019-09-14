using System;
using System.Collections.Generic;

namespace Lab3
{
    public class IniDataSection
    {
        public readonly string Name;
        public readonly Dictionary<string, string> Values;

        public IniDataSection(string name, Dictionary<String, String> data)
        {
            this.Name = name;
            this.Values = data;
        }

        public IniDataSection(string name) : this(name, new Dictionary<string, string>()){}

        public void put(string key, string data)
        {
            this.Values[key] = data;
        }

        public T get<T>(string key)
        {
            var v = Values.GetValueOrDefault(key, null);
            if (v == null)
            {
                return default(T);
            }

            try {
                return (T)Convert.ChangeType(v, typeof(T));
            }
            catch (Exception e){
                if(e is FormatException || e is InvalidCastException)
                    throw new IniFormatException("Unable to convert " + v + " to " + typeof(T));
                throw;
            }
        }
    }
}