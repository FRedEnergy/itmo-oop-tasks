using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab2
{
    public class Genre
    {
        public readonly String Name;
        public readonly List<Genre> ChildGenres = new List<Genre>();

        public Genre(String name)
        {
            this.Name = name;
        }

        public Genre(String name, params SubGenre[] subGenres): this(name)
        {
            foreach (SubGenre genre in subGenres)
                AddChildren(genre);
        }

        public void AddChildren(SubGenre genre)
        {
            this.ChildGenres.Add(genre);
            genre.AddParent(this);
        }

        public bool IsInstanceOrChildren(Genre genre)
        {
            return genre == this || ChildGenres.Any(it => it.IsInstanceOrChildren(genre));
        }
    }
}