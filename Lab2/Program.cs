using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Lab2
{

    public class Genre
    {
        public readonly String Name;
        public readonly List<Genre> ChildGenres = new List<Genre>();
        
        public Genre(String name)
        {
            this.Name = name;
        }

        public Genre(String name, params SubGenre[] subGenres): this(name)
        {
            foreach (SubGenre genre in subGenres)
                AddChildren(genre);
        }

        public void AddChildren(SubGenre genre)
        {
            this.ChildGenres.Add(genre);
            genre.AddParent(this);
        }
        
        public bool IsInstanceOrChildren(Genre genre)
        {
            return genre == this || ChildGenres.Any(it => it.IsInstanceOrChildren(genre));
        }
    }

    public class SubGenre: Genre
    {
        
        public readonly List<Genre> ParentGenres = new List<Genre>();

        public SubGenre(String name, params Genre[] parentGenres) : base(name)
        {
            foreach (var genre in parentGenres)
                genre.AddChildren(this);
            
        }

        public void AddParent(Genre genre)
        {
            this.ParentGenres.Add(genre);
        }
    }
    
    
    public class Track: ISearchable
    {
        public readonly Artist Artist;
        public readonly Album Album;
        public readonly String Name;

        public int Year
        {
            get { return Album.Year; }
        }

        public Track(Artist artist, Album album, string name)
        {
            Artist = artist;
            Album = album;
            Name = name;
        }

        public bool Matches(SearchQuery query)
        {
            return query.IncludeTracks
                   && query.NameMatches(this.Name)
                   && query.GenresMatch(this.Album.Genres)
                   && query.YearMatches(this.Year);
        }
    }

    public class TrackGroup
    {
        public readonly String Name;
        public readonly List<Track> Tracks = new List<Track>();

        public IEnumerable<Artist> Artists
        {
            get { return Tracks.Select(it => it.Artist).Distinct(); }
        }

        public IEnumerable<Genre> Genres
        {
            get { return Tracks.SelectMany(it => it.Artist.ArtistGenres); }
        }

        public TrackGroup(string name)
        {
            Name = name;
        }
    }

    public class Album: TrackGroup, ISearchable
    {

        public readonly int Year;
        
        public Artist Artist
        {
            get { return Artists.First(); }
        }
        
        public Album(String name, int year): base(name)
        {
            this.Year = year;
        }

        public void AddTracks(params Track[] tracks)
        {
            this.Tracks.AddRange(tracks);
        }

        public bool Matches(SearchQuery query)
        {
            return query.IncludeAlbums
                   && query.YearMatches(Year)
                   && query.NameMatches(Name)
                   && query.GenresMatch(Genres);
        }
    }
    
    public class Artist: ISearchable
    {
        public readonly String Name;

        public IEnumerable<Track> Tracks
        {
            get { return Albums.SelectMany(it => it.Tracks); }
        }
        public readonly List<Album> Albums = new List<Album>();
        public readonly List<Genre> ArtistGenres = new List<Genre>();

        public Artist(string name, params Genre[] genres)
        {
            Name = name;
            this.ArtistGenres.AddRange(genres);
        }

        public void AddAlbum(Album album)
        {
            this.Albums.Add(album);
        }
        
        public bool Matches(SearchQuery query)
        {
            return query.IncludeArtists
                   && query.NameMatches(Name)
                   && query.GenresMatch(ArtistGenres);
        }
    }

    public interface ISearchable
    {
        bool Matches(SearchQuery query);
    }

    public class SearchQuery
    {
        public bool IncludeTracks = false, IncludeAlbums = false, IncludeArtists = false;
        public List<Genre> TargetGenres = new List<Genre>();
        public String NameQuery = "";
        public int TargetYear = -1;

        public SearchQuery AddTracks()
        {
            this.IncludeTracks = true;
            return this;
        }
        
        public SearchQuery AddAlbums()
        {
            this.IncludeAlbums = true;
            return this;
        }
        
        public SearchQuery AddArtists()
        {
            this.IncludeArtists = true;
            return this;
        }

        public SearchQuery Matching(String query)
        {
            this.NameQuery = query;
            return this;
        }

        public SearchQuery WithGenres(params Genre[] genres)
        {
            this.TargetGenres.AddRange(genres);
            return this;
        }

        public SearchQuery AtYear(int year)
        {
            this.TargetYear = year;
            return this;
        }

        public bool YearMatches(int year)
        {
            return this.TargetYear == -1 || this.TargetYear == year;
        }

        public bool NameMatches(String objectName)
        {
            return this.NameQuery.Length == 0 || objectName.Contains(this.NameQuery);
        }

        public bool GenresMatch(IEnumerable<Genre> genres)
        {
            if (this.TargetGenres.Count == 0)
                return true;
            
            foreach (var targetGenre in TargetGenres)
                foreach (var genre in genres)
                    if (genre.IsInstanceOrChildren(targetGenre))
                        return true;
                
            return false;
        }
    }

    public class SearchResult
    {
        public List<Track> Tracks = new List<Track>();
        public List<Album> Albums = new List<Album>();
        public List<Artist> Artists = new List<Artist>();

        public void Print()
        {
            Console.WriteLine("Search result with (" + (Tracks.Count 
                                                     + Albums.Count + Artists.Count) + ") entries");
            var artists = Artists.Select(it => it.Name + " (" + String.Join(", ", it.ArtistGenres.Select(that => that.Name)) + ")");
            Console.WriteLine("Artists (" + Artists.Count + "): " + String.Join(", ", artists));
            
            var albums = Albums.Select(it => it.Name + " by " + String.Join(", ", it.Artists.Select(that => that.Name)) + " (" + it.Year + ")");
            Console.WriteLine("Albums (" + Albums.Count + "): " + String.Join(", ", albums));
            
            var tracks = Tracks.Select(it => it.Name + " by " + it.Artist.Name + " (" + it.Album.Name + ", " + it.Year + ")");
            Console.WriteLine("Tracks (" + Tracks.Count + "): " + String.Join(", ", tracks));
        }
    }
    
    public class Catalog
    {
        
        public List<Artist> Artists = new List<Artist>();

        public IEnumerable<Album> Albums
        {
            get { return Artists.SelectMany(it => it.Albums); }
        }

        public IEnumerable<Track> Tracks
        {
            get { return Artists.SelectMany(it => it.Tracks); }
        }

        public void AddArtist(Artist artist)
        {
            this.Artists.Add(artist);
        }

        public SearchResult Query(SearchQuery query)
        {
            var result = new SearchResult();
            
            result.Tracks.AddRange(Tracks.Where(it => it.Matches(query)));
            result.Artists.AddRange(Artists.Where(it => it.Matches(query)));
            result.Albums.AddRange(Albums.Where(it => it.Matches(query)));

            return result;
        }
        
        
    }
    
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