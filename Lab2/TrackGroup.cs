using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab2
{
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
}