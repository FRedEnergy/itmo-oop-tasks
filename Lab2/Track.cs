using System;

namespace Lab2
{
    public class Track: ISearchable
    {
        public readonly Artist Artist;
        public readonly Album Album;
        public readonly String Name;

        public int Year => Album.Year;

        internal Track(Artist artist, Album album, string name)
        {
            if(album == null || artist == null)
                throw new InvalidOperationException("Tried to create track without artist or album");
            
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

        public override string ToString()
        {
            return $"{Name} by {Artist.Name} ({Album.Name}, {Year})";
        }
    }
}