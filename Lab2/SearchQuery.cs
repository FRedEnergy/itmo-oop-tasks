using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab2
{
    public class SearchQuery
    {
        public bool IncludeTracks { get; private set; } = false;
        public bool IncludePlaylists { get; private set; } = false;
        public bool IncludeArtists { get; private set; } = false;
        private readonly List<Genre> _targetGenres = new List<Genre>();
        private String _nameQuery = "";
        private int _targetYear = -1;

        public SearchQuery AddTracks()
        {
            this.IncludeTracks = true;
            return this;
        }

        public SearchQuery AddAlbums()
        {
            this.IncludePlaylists = true;
            return this;
        }

        public SearchQuery AddArtists()
        {
            this.IncludeArtists = true;
            return this;
        }

        public SearchQuery Matching(String query)
        {
            this._nameQuery = query;
            return this;
        }

        public SearchQuery WithGenres(params Genre[] genres)
        {
            this._targetGenres.AddRange(genres);
            return this;
        }

        public SearchQuery AtYear(int year)
        {
            this._targetYear = year;
            return this;
        }

        public bool YearMatches(int year)
        {
            return this._targetYear == -1 || this._targetYear == year;
        }

        public bool NameMatches(String objectName)
        {
            return this._nameQuery.Length == 0 || objectName.Contains(this._nameQuery);
        }

        public bool GenresMatch(IEnumerable<Genre> genres)
        {
            if (this._targetGenres.Count == 0)
                return true;

            return (from targetGenre in _targetGenres
                from genre in genres
                where genre.IsInstanceOrChildren(targetGenre)
                select targetGenre).Any();
        }
    }
}