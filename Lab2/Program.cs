using System;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Lab2
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var rock = new Genre("Rock");
            var altRock = new SubGenre("Alt Rock", rock);
            var hardRock = new SubGenre("Hard Rock", rock);

            var placebo = new Artist("Placebo", altRock);
            var battleForTheSun = new Album("Battle For The Sun", 2009);
            battleForTheSun.AddTracks(
                new Track(placebo, battleForTheSun, "Kitty Litter"),
                new Track(placebo, battleForTheSun, "Ashtray Heart"),
                new Track(placebo, battleForTheSun, "Battle For The Sun"),
                new Track(placebo, battleForTheSun, "For What It's Worth"),
                new Track(placebo, battleForTheSun, "Devil in The Details"),
                new Track(placebo, battleForTheSun, "Bright Lights"),
                new Track(placebo, battleForTheSun, "Speak In Tongues"),
                new Track(placebo, battleForTheSun, "The Never-Ending Why"),
                new Track(placebo, battleForTheSun, "Julien"),
                new Track(placebo, battleForTheSun, "Happy You're Gone"),
                new Track(placebo, battleForTheSun, "Breath Underwater"),
                new Track(placebo, battleForTheSun, "Come Undone"),
                new Track(placebo, battleForTheSun, "Kings Of Medicine")
            );
            placebo.AddAlbum(battleForTheSun);
            
            var ledZepelin = new Artist("Led Zepelin", hardRock);
            var coda = new Album("Coda", 1982);
            coda.AddTracks(
                new Track(ledZepelin, coda, "We're Gonna Groove"),
                new Track(ledZepelin, coda, "Poor Tom"),
                new Track(ledZepelin, coda, "I Can't Quit You Baby"),
                new Track(ledZepelin, coda, "Walter's Walk"),
                new Track(ledZepelin, coda, "Ozone Baby"),
                new Track(ledZepelin, coda, "Darlene"),
                new Track(ledZepelin, coda, "Bonzo's Montreux"),
                new Track(ledZepelin, coda, "Wearing and Tearing")
            );
            ledZepelin.AddAlbum(coda);
            
            Catalog catalog = new Catalog();
            catalog.AddArtist(placebo);
            catalog.AddArtist(ledZepelin);
            
            Console.WriteLine("Searching for Artists by Genre");
            catalog.Query(new SearchQuery().AddArtists().WithGenres(altRock)).Print();
            Console.WriteLine("");
         
            Console.WriteLine("Searching for Artists by Name");
            catalog.Query(new SearchQuery().AddArtists().Matching("Zepelin")).Print();
            Console.WriteLine("");
            
            Console.WriteLine("Searching for everything by genre");
            catalog.Query(new SearchQuery()
                    .AddAlbums().AddArtists().AddTracks()
                    .WithGenres(altRock)).Print();
            Console.WriteLine("");
            
            Console.WriteLine("Searching for tracks by year and genre");
            catalog.Query(new SearchQuery().AddTracks().WithGenres(hardRock).AtYear(1982)).Print();
            Console.WriteLine("");
            
            Console.WriteLine("Printing full catalog");
            catalog.Query(new SearchQuery().AddTracks().AddAlbums().AddArtists()).Print();
            Console.WriteLine("");

        }
    }
}