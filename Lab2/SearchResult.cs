using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab2
{
    public class SearchResult
    {
        public List<Track> Tracks = new List<Track>();
        public List<Playlist> Playlists = new List<Playlist>();
        public List<Artist> Artists = new List<Artist>();

        public void Print()
        {
            Console.WriteLine("Search result with (" + (Tracks.Count
                                                        + Playlists.Count + Artists.Count) + ") entries");
            Console.WriteLine("Artists (" + Artists.Count + "): "
                              + string.Join(", ", Artists.Select(it => it.ToString())));

            Console.WriteLine("Playlists (" + Playlists.Count + "): " 
                              + string.Join(", ", Playlists.Select(it => it.ToString())));

            Console.WriteLine("Tracks (" + Tracks.Count + "): " 
                              + string.Join(", ", Tracks.Select(it => it.ToString())));
        }
    }
}