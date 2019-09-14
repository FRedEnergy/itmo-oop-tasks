using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab2
{
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
}