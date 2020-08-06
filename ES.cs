using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MoviesTracker
{
	class ES
	{
        private static string pathMovies;
        public static void Init()
        {
            string fullPathWithAppName      = System.Reflection.Assembly.GetEntryAssembly().Location;
            string appName                  = System.AppDomain.CurrentDomain.FriendlyName;
            string fullPathWithoutAppName   = fullPathWithAppName.Replace(appName, string.Empty);
            // Path para pasta dos filmes
            pathMovies = string.Format("{0}{1}", fullPathWithoutAppName, "movies");
            // Cria a pasta movies
            if (!Directory.Exists(pathMovies))
            {
                Directory.CreateDirectory(pathMovies);
            }

        }
		public static void DownloadMovie(Movie movie, Torrent torrent)
		{
            string movieTitle = RemoveIlegalCharater(movie.Title);

            string pathMovie = string.Format("{0}\\{1} - {2}", pathMovies, movie.Year, movieTitle);

            try
            {
                // Verifica se existe a pasta de filmes
                if (!Directory.Exists(pathMovie))
                {
                    Directory.CreateDirectory(pathMovie);
                }
                // Download dos ficherios torrent
                using (var client = new WebClient())
                {
                    // Nome do ficheiro
                    string fileName = string.Format("{0}_{1}.torrent", torrent.Quality, torrent.Hash);
                    //
                    Console.WriteLine("Ficheiro: {0}", fileName);
                    //
                    Console.WriteLine("Caminho: {0}", pathMovie);
                    //
                    client.DownloadFile(torrent.Url, string.Format("{0}\\{1}", pathMovie, fileName));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private static string RemoveIlegalCharater(string text)
        {
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            foreach (char c in invalid)
            {
                text = text.Replace(c.ToString(), "");
            }

            return text;
        }
	}
}
