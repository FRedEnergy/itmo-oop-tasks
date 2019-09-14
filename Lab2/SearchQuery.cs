using System;
using System.Collections.Generic;

namespace Lab2
{
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
}