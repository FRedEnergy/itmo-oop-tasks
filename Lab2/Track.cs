using System;

namespace Lab2
{
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
}