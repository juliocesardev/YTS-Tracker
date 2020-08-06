using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesTracker
{
	class Movie
	{
		public string Title { get; set; }
		public string Year { get; set; }
		public string Cover { get; set; }
		public List<Torrent> Torrents { get; set; }
	}
}
