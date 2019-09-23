using System;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Lab2
{
    internal class Program
    {
        public static void Main(string[] args)
        {
          
            Console.WriteLine("Searching for Artists by Genre");
            Catalog.Query(new SearchQuery().AddArtists().WithGenres(Catalog.AltRock)).Print();
            Console.WriteLine("");
         
            Console.WriteLine("Searching for Artists by Name");
            Catalog.Query(new SearchQuery().AddArtists().Matching("Zepelin")).Print();
            Console.WriteLine("");
            
            Console.WriteLine("Searching for everything by genre");
            Catalog.Query(new SearchQuery()
                    .AddAlbums().AddArtists().AddTracks()
                    .WithGenres(Catalog.AltRock)).Print();
            Console.WriteLine("");
            
            Console.WriteLine("Searching for tracks by year and genre");
            Catalog.Query(new SearchQuery().AddTracks().WithGenres(Catalog.HardRock).AtYear(1982)).Print();
            Console.WriteLine("");
            
            Console.WriteLine("Printing full catalog");
            Catalog.Query(new SearchQuery().AddTracks().AddAlbums().AddArtists()).Print();
            Console.WriteLine("");

        }
    }
}