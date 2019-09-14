using System.Collections.Generic;
using System.Linq;

namespace Lab2
{
    public class Catalog
    {
        
        public List<Artist> Artists = new List<Artist>();

        public IEnumerable<Album> Albums
        {
            get { return Artists.SelectMany(it => it.Albums); }
        }

        public IEnumerable<Track> Tracks
        {
            get { return Artists.SelectMany(it => it.Tracks); }
        }

        public void AddArtist(Artist artist)
        {
            this.Artists.Add(artist);
        }

        public SearchResult Query(SearchQuery query)
        {
            var result = new SearchResult();
            
            result.Tracks.AddRange(Tracks.Where(it => it.Matches(query)));
            result.Artists.AddRange(Artists.Where(it => it.Matches(query)));
            result.Albums.AddRange(Albums.Where(it => it.Matches(query)));

            return result;
        }
        
        
    }
}