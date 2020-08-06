using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MoviesTracker
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.Title = "YTS Tracker";
			// Inicia o IO
			ES.Init();
			// Inicia o Tracker
			Tracker.Init();
			// Busca os filmes numa certa pagina
			//Tracker.GetMovies(1);
			// Busca todos os filmes
			Tracker.GetAllMovies();
			// Loop da lista de filmes
			Tracker.DownloadFilesTorrent();
			//
			Console.WriteLine("Descarregado {0} Ficheiros.", Tracker.moviesList.Count);
			//
			Console.ReadKey();
		}
	}
}
