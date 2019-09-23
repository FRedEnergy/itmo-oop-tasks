using System;
using System.Collections.Generic;

namespace Lab2
{
    public class SubGenre: Genre
    {
        private readonly List<Genre> _parentGenres = new List<Genre>();

        public SubGenre(String name, params Genre[] parentGenres) : base(name)
        {
            foreach (var genre in parentGenres)
                genre.AddChildren(this);
            
        }

        public void AddParent(Genre genre)
        {
            this._parentGenres.Add(genre);
        }
    }
}