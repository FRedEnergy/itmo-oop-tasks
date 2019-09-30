using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace Lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileName = "./../../../sample.ini";

            if (!File.Exists(fileName))
            {
                Console.WriteLine(fileName + " not found");
                return;
            }

            var dataFile = IniDataFile.Parse(fileName);

            var commonSection = dataFile["COMMON"];
            var statisterTimeMs = commonSection.Get<int>("StatisterTimeMs");
            var diskCachePath = commonSection.Get<string>("DiskCachePath");

            Console.WriteLine("COMMON.StatisterTimeMS = " + statisterTimeMs);
            Console.WriteLine("COMMON.DiskCachePath = " + diskCachePath);


            Console.WriteLine("NCMD.SampleRate + 0.5 = " + dataFile["NCMD"].Get<double>("SampleRate") + 0.5);

            Console.WriteLine("ADC_DEV.Driver = " + dataFile["ADC_DEV"].Get<string>("Driver"));
        }
    }
}