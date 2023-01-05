using System;
using System.Collections.Generic;
using System.Text;
using TMDB.ExtensionMethods;
using System.Net.Http;
using System.Threading.Tasks;
using TMDB.ResponseModels;

namespace TMDB
{
    class TMDBClient : HttpClient
    {
        private string _key = "<redacted>"; //private key


        public TMDBClient(string key)
        {
            _key = key;
            BaseAddress = new Uri("https://api.themoviedb.org/3/");
        }

        public TMDBClient()
        {
            BaseAddress = new Uri("https://api.themoviedb.org/3/");
        }

        public async Task<ResponseModels.Movie.Root> SearchMovieByTitleAsync(string title)
        {
            var query = $"&query={title.ReplaceSpacesWithPluses()}";
            var requestString = AssembleRequestString("search/movie", query);
            return await SendAPIRequestAsync(requestString);
        }

        public async Task<ResponseModels.Movie.Root> GetTrendingMoviesAsync()
        {
            var requestString = AssembleRequestString("trending/movie/week", "");
            return await SendAPIRequestAsync(requestString);
        }

        private async Task<ResponseModels.Movie.Root> SendAPIRequestAsync(string requestString)
        {
            var response = await GetAsync(requestString);

            if (!response.IsSuccessStatusCode)
                return null;
            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(jsonResponse);
            var movieResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseModels.Movie.Root>(jsonResponse);
            return movieResponse;

        }
        public string SearchShowByTitle(string title)
        {
            var query = $"&query={title.ReplaceSpacesWithPluses()}";

            var requestString = AssembleRequestString("search/show", query);
            Console.WriteLine("requestString");
            return "";
        }




        private string AssembleRequestString(string requestType, List<string> queries)
        {
            var queriesString = "";

            foreach (var q in queries)
            {
                queriesString += q;
            }
            var reqString = $"{requestType}{_key}{queriesString}";

            return reqString;
        }

        //overload to allow single query string
        private string AssembleRequestString(string requestType, string query)
        {
            return AssembleRequestString(requestType, new List<string> { query });
        }





    }
}
