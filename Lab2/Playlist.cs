using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Lab2
{
    public class Playlist: ISearchable
    {
        public readonly String Name;
     
        protected readonly List<Track> _tracks = new List<Track>();
        public ReadOnlyCollection<Track> Tracks => _tracks.AsReadOnly();
        
        public IEnumerable<Artist> Artists
        {
            get { return _tracks.Select(it => it.Artist).Distinct(); }
        }

        public IEnumerable<Genre> Genres
        {
            get { return _tracks.SelectMany(it => it.Artist.ArtistGenres); }
        }

        public Playlist(string name, params Track[] tracks)
        {
            Name = name;
            this._tracks.AddRange(tracks);
        }

        public bool Matches(SearchQuery query)
        {
            return query.IncludePlaylists && query.NameMatches(Name);
        }
        
        public override string ToString()
        {
            return $"{Name} by {String.Join(", ", Artists.Select(that => that.Name))}";
        }
    }
}