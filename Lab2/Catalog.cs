using System.Collections.Generic;
using System.Linq;

namespace Lab2
{
    public static class Catalog
    {
        private static readonly List<Artist> Artists = new List<Artist>();
        private static readonly List<Playlist> Playlists = new List<Playlist>();

        public static readonly Genre Rock = new Genre("Rock");
        public static readonly Genre AltRock = new SubGenre("Alt Rock", Rock);
        public static readonly Genre HardRock = new SubGenre("Hard Rock", Rock);
        
        static Catalog()
        {
            var placebo = new Artist("Placebo", AltRock);
            var battleForTheSun = new Album("Battle For The Sun", 2009);
            battleForTheSun.CreateTrack(placebo, "Kitty Litter");
            battleForTheSun.CreateTrack(placebo, "Ashtray Heart");
            battleForTheSun.CreateTrack(placebo, "Battle For The Sun");
            battleForTheSun.CreateTrack(placebo, "For What It's Worth");
            battleForTheSun.CreateTrack(placebo, "Devil in The Details");
            battleForTheSun.CreateTrack(placebo, "Bright Lights");
            battleForTheSun.CreateTrack(placebo, "Speak In Tongues");
            battleForTheSun.CreateTrack(placebo, "The Never-Ending Why");
            battleForTheSun.CreateTrack(placebo, "Julien");
            battleForTheSun.CreateTrack(placebo, "Happy You're Gone");
            battleForTheSun.CreateTrack(placebo, "Breath Underwater");
            battleForTheSun.CreateTrack(placebo, "Come Undone");
            battleForTheSun.CreateTrack(placebo, "Kings Of Medicine");
            placebo.AddAlbum(battleForTheSun);
            
            var ledZepelin = new Artist("Led Zepelin", HardRock);
            var coda = new Album("Coda", 1982);
            coda.CreateTrack(ledZepelin, "We're Gonna Groove");
            coda.CreateTrack(ledZepelin, "Poor Tom");
            coda.CreateTrack(ledZepelin, "I Can't Quit You Baby");
            coda.CreateTrack(ledZepelin, "Walter's Walk");
            coda.CreateTrack(ledZepelin, "Ozone Baby");
            coda.CreateTrack(ledZepelin, "Darlene");
            coda.CreateTrack(ledZepelin, "Bonzo's Montreux");
            coda.CreateTrack(ledZepelin, "Wearing and Tearing");
            ledZepelin.AddAlbum(coda);
            
            AddArtist(placebo);
            AddArtist(ledZepelin);
        }
        
        private static IEnumerable<Album> Albums
        {
            get { return Artists.SelectMany(it => it.Albums); }
        }

        private static IEnumerable<Track> Tracks
        {
            get { return Artists.SelectMany(it => it.Tracks); }
        }

        public static void AddArtist(Artist artist)
        {
            Artists.Add(artist);
        }

        public static void AddPlaylist(Playlist playlist)
        {
            Playlists.Add(playlist);
        }

        public static SearchResult Query(SearchQuery query)
        {
            var result = new SearchResult();

            result.Tracks.AddRange(Tracks.Where(it => it.Matches(query)));
            result.Artists.AddRange(Artists.Where(it => it.Matches(query)));
            result.Playlists.AddRange(Albums.Where(it => it.Matches(query)));
            result.Playlists.AddRange(Playlists.Where(it => it.Matches(query)));

            return result;
        }


    }
}