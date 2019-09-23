using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab2
{
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
        
        public override string ToString()
        {
            return $"{Name} ({String.Join(", ", ArtistGenres.Select(that => that.Name))})";
        }
    }
}