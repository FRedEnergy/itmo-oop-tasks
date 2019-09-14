using System;
using System.Linq;

namespace Lab2
{
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
}