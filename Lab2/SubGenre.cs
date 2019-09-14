using System;
using System.Collections.Generic;

namespace Lab2
{
    public class SubGenre: Genre
    {
        
        public readonly List<Genre> ParentGenres = new List<Genre>();

        public SubGenre(String name, params Genre[] parentGenres) : base(name)
        {
            foreach (var genre in parentGenres)
                genre.AddChildren(this);
            
        }

        public void AddParent(Genre genre)
        {
            this.ParentGenres.Add(genre);
        }
    }
}