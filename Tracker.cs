using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace MoviesTracker
{
	class Tracker
	{
		public static List<Movie> moviesList = new List<Movie>();
		public static string website = "https://yts.mx/api/v2/list_movies.json";
		public static int timeDelay = 2000;
		public static double numberPages = 0;
		public static double moviesCounter = 0;
		//
		public static void Init()
		{
			double moviesLimitPerPage = 0;
			//
			try
			{
				using (var webClient = new System.Net.WebClient())
				{
					string url = website;
					string json = webClient.DownloadString(url);
					// Dados do ficheiro json desta URL
					JObject jsonObject = JObject.Parse(json);
					// Dados desta pagina
					JObject data = JObject.Parse(jsonObject["data"].ToString());
					// Numero de filmes desta pagina
					moviesCounter = double.Parse(data["movie_count"].ToString());
					// Limite de filmes por pagina
					moviesLimitPerPage = double.Parse(data["limit"].ToString());
				}
				// Numero de paginas
				numberPages = Math.Ceiling(moviesCounter / moviesLimitPerPage);
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
		public static void GetAllMovies()
		{
			for (int page = 1; page <= numberPages; page++)
			{
				GetMovies(page);
				//
				Console.WriteLine("Aguardar {0} segundo(s)...", (timeDelay / 1000));
				Console.WriteLine("{0} de {1} Filmes", moviesList.Count, moviesCounter);
				// 
				Thread.Sleep(timeDelay);
			}
			//
			Console.Clear();
		}
		public static void GetMovies(int pageNumber = 1)
		{
			try
			{
				using (var webClient = new System.Net.WebClient())
				{
					string url = string.Format("{0}?page={1}", website, pageNumber);
					string json = webClient.DownloadString(url);
					// Dados do ficheiro json desta URL
					JObject jsonObject = JObject.Parse(json);
					// Dados desta pagina
					JObject data = JObject.Parse(jsonObject["data"].ToString());
					// Lista de filmes desta pagina
					JArray movies = JArray.Parse(data["movies"].ToString());
					// Loop dos filmes existentes
					for (int movieIndex = 0; movieIndex < movies.Count; movieIndex++)
					{
						Torrent torrent;
						List<Torrent> torrents = new List<Torrent>();
						Movie movie = new Movie();
						//
						JArray torrentsArray = JArray.Parse(movies[movieIndex]["torrents"].ToString());
						// Loop torrents list
						for (int torrentIndex = 0; torrentIndex < torrentsArray.Count; torrentIndex++)
						{
							//Criar torrent
							torrent = new Torrent()
							{
								Url = torrentsArray[torrentIndex]["url"].ToString(),
								Hash = torrentsArray[torrentIndex]["hash"].ToString(),
								Quality = torrentsArray[torrentIndex]["quality"].ToString()
							};
							// Adicionar torrent a este filme
							torrents.Add(torrent);
						}
						//
						movie.Title		= movies[movieIndex]["title"].ToString();
						movie.Cover		= movies[movieIndex]["large_cover_image"].ToString();
						movie.Year		= movies[movieIndex]["year"].ToString();
						movie.Torrents	= torrents;
						//
						moviesList.Add(movie);
						//
						Console.WriteLine("'{0}' adicionado.", movie.Title);
					}
				}
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
		public static void DownloadFilesTorrent()
		{
			// Loop da lista de filmes
			foreach (Movie movie in moviesList)
			{
				Console.WriteLine("Filme: {0}", movie.Title);

				Console.WriteLine("Ano: {0}", movie.Year);
				//
				Console.WriteLine("");
				Console.WriteLine("Torrents:");
				//
				foreach (Torrent torrent in movie.Torrents)
				{
					Console.WriteLine("");
					Console.WriteLine("------------ {0} ---------------------------", torrent.Hash);

					Console.WriteLine("Qualidade:{0}", torrent.Quality);
					Console.WriteLine("Hash:{0}", torrent.Hash);
					Console.WriteLine("Url:{0}", torrent.Url);

					ES.DownloadMovie(movie, torrent);
				}
				//
				Console.WriteLine("_________________________________________________________________________________");
				//
				Console.WriteLine("Aguardar {0} segundo(s)...", (timeDelay / 1000));
				// 
				Thread.Sleep(timeDelay);
				//
				Console.Clear();
			}
		}
	}
}
