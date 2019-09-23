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
            var fileName = "./../../../sample.i" +
                           "ni";
            var dataFile = File.Exists(fileName) ? IniDataFile.Parse(fileName) : null;
            if (dataFile == null)
            {
                Console.WriteLine(fileName + " not found");
                return;
            }

            var commonSection = dataFile["COMMON"];
            var statisterTimeMs = commonSection.get<int>("StatisterTimeMs");
            var diskCachePath = commonSection.get<string>("DiskCachePath");

            Console.WriteLine("COMMON.StatisterTimeMS = " + statisterTimeMs);
            Console.WriteLine("COMMON.DiskCachePath = " + diskCachePath);


            Console.WriteLine("NCMD.SampleRate + 0.5 = " + dataFile["NCMD"].get<double>("SampleRate") + 0.5);

            Console.WriteLine("ADC_DEV.Driver = " + dataFile["ADC_DEV"].get<string>("Driver"));
        }
    }
}