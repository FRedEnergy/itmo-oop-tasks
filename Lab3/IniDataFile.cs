using System;
using System.Collections.Generic;
using System.IO;

namespace Lab3
{
    public class IniDataFile
    {
        public readonly Dictionary<string, IniDataSection> Sections = new Dictionary<string, IniDataSection>();

        public IniDataSection this[string key]
        {
            get => Sections.GetValueOrDefault(key, null) ?? throw new IniUnknownSectionException(key);
            private set => Sections[key] = value;
        }

        public static IniDataFile Parse(string fileName)
        {
            IEnumerable<string> lines = File.ReadLines(fileName);

            var result = new IniDataFile();

            IniDataSection section = null;

            var i = 0;
            foreach (var line in lines)
            {
                try
                {
                    var trimmed = line.Trim();
                    if (trimmed.Length == 0)
                        continue;

                    var tokens = trimmed.Split(";");
                    var data = tokens[0].Trim();
                    if(data.Length == 0)
                        continue;

                    if (data.StartsWith("[") && data.EndsWith("]"))
                    {
                        section = new IniDataSection(data.Substring(1, data.Length - 2));
                        result[section.Name] = section;
                        continue;
                    }

                    if (section == null)
                        throw new IniFormatException("Specify section before specifying variables");

                    var exp = data.Split("=", 2);
                    if(exp.Length < 2)
                        throw new IniFormatException("Invalid expression on line " 
                                                     + (i + 1) + ", Use 'KEY = VALUE' format ");

                    var key = exp[0].Trim();
                    var value = exp[1].Trim();

                    section.Put(key, value);
                }
                catch (Exception e)
                {
                    throw new IniFormatException("Error processing line " + (i + 1) + ": " + line, e);
                }

                i++;
            }

            return result;
        }
    }
}