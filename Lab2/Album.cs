using System;
using System.Linq;

namespace Lab2
{
    public class Album: Playlist, ISearchable
    {

        public readonly int Year;

        public Artist Artist => _tracks.Count == 0 ? null : Artists.First();

        public Album(String name, int year): base(name)
        {
            this.Year = year;
        }

        public Track CreateTrack(Artist artist, String name)
        {
            if(Artist != null && artist != Artist)
                throw new InvalidOperationException("One album can contain tracks only from one album");
           
            var track = new Track(artist, this, name);
            this._tracks.Add(track);
            return track;
        }

        public new bool Matches(SearchQuery query)
        {
            return query.IncludePlaylists
                   && query.YearMatches(Year)
                   && query.NameMatches(Name)
                   && query.GenresMatch(Genres);
        }

        public override string ToString()
        {
            return $"{Name} by {String.Join(", ", Artists.Select(that => that.Name))} ({Year})";
        }
    }
}