using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab2
{
    public class Genre
    {
        public readonly String Name;
        private readonly List<Genre> _childGenres = new List<Genre>();

        public Genre(String name)
        {
            this.Name = name;
        }

        public Genre(String name, params SubGenre[] subGenres): this(name)
        {
            foreach (var genre in subGenres)
                AddChildren(genre);
        }

        public void AddChildren(SubGenre genre)
        {
            this._childGenres.Add(genre);
            genre.AddParent(this);
        }

        public bool IsInstanceOrChildren(Genre genre)
        {
            return genre == this || _childGenres.Any(it => it.IsInstanceOrChildren(genre));
        }
    }
}