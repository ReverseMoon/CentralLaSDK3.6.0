using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Kittenji
{
    public static class TMDBApi
    {
        // cec4c3b69a9fd8d96643d9ce9909f124
        const string PosterEndpoint92 = "https://image.tmdb.org/t/p/w92";

        const string EditorApiKeyName = "TMDBApiKey";
        public static string ApiKey
        {
            get => EditorPrefs.GetString("TMDBApiKey", null);
            set => EditorPrefs.SetString(EditorApiKeyName, value);
        }

        public static Texture2D GetThumbnailFromResult(Result result)
        {
            string url = PosterEndpoint92 + result.poster_path;
            Debug.Log("Requesting Texture Poster: " + url);

            Texture2D texture = null;
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                request.SendWebRequest();

                while (!request.isDone && !request.isNetworkError && !request.isHttpError) { }

                if (request.isHttpError || request.isNetworkError || !string.IsNullOrWhiteSpace(request.error))
                    Debug.LogError("TMDB Error: " + request.error);
                else {
                    Debug.Log("Request is done...");
                    texture = new Texture2D(2, 2);
                    texture.LoadImage(request.downloadHandler.data);
                }
            }
            texture.name = result.poster_path.Substring(1);

            return texture;
        }

        public static Genre[] GetGenreList(bool isMovie)
        {
            string tag = (isMovie ? "movie" : "tv");
            string url = $"https://api.themoviedb.org/3/genre/{tag}/list?api_key={ApiKey}&language=es-MX";

            Genre[] genres = null;

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                request.SendWebRequest();

                while (!request.isDone && !request.isNetworkError && !request.isHttpError)
                { }

                EditorUtility.ClearProgressBar();
                if (request.isHttpError || request.isNetworkError || !string.IsNullOrWhiteSpace(request.error))
                {
                    Debug.LogError("TMDB Error: " + request.error);
                }
                else
                {
                    string json = request.downloadHandler.text;
                    Debug.Log("Request is done:\n " + json);
                    GenreList genreList = Newtonsoft.Json.JsonConvert.DeserializeObject<GenreList>(json);
                    if (genreList != null && genreList.genres != null && genreList.genres.Length > 0)
                        genres = genreList.genres;
                }
            }

            return genres;
        }

        public static Result[] SearchMovie(string query, bool isMovie, string year = null)
        {
            string q = UnityWebRequest.EscapeURL(query);
            string tag = (isMovie ? "movie" : "tv");
            string url = $"https://api.themoviedb.org/3/search/{tag}?api_key={ApiKey}&language=es-MX&query={q}&page=1&include_adult=false";
            if (!string.IsNullOrEmpty(year)) url += $"&year={year}";

            Result[] results = null;
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                request.SendWebRequest();

                while (!request.isDone && !request.isNetworkError && !request.isHttpError)
                {
                    EditorUtility.DisplayProgressBar("TMDB", "Buscando: " + query, request.downloadProgress);
                }
                EditorUtility.ClearProgressBar();
                if (request.isHttpError || request.isNetworkError || !string.IsNullOrWhiteSpace(request.error))
                {
                    Debug.LogError("TMDB Error: " + request.error);
                    EditorUtility.DisplayDialog("TMDB", "Se ha producido un error buscando resultados para:\n" + query, "Ok");
                }
                else
                {
                    string json = request.downloadHandler.text;
                    Debug.Log("Request is done:\n " + json);
                    Search searchObject = JsonUtility.FromJson<Search>(json);
                    if (searchObject == null || searchObject.results == null || searchObject.results.Length == 0)
                        EditorUtility.DisplayDialog("TMDB", "No se han encontrado resultados para:\n" + query, "Ok");
                    else
                    {
                        results = searchObject.results;
                        foreach (var res in results) res.isMovie = isMovie;
                    }
                }
            }

            return results;
        }

        [Serializable]
        public class Search
        {
            public int page;
            public Result[] results;
            public int total_results;
            public int total_pages;
        }

        [Serializable]
        public class GenreList
        {
            public Genre[] genres;
        }

        [Serializable]
        public struct Genre
        {
            public int id;
            public string name;
        }

        [Serializable]
        public class Result
        {
            public bool isMovie;

            public string poster_path;
            public bool adult;
            public string overview;
            public string release_date;
            public string first_air_date;
            public int[] genre_ids;
            public int id;
            public string original_title;
            public string original_language;
            public string title;
            public string name;
            public string backdrop_path;
            public float popularity;
            public int vote_count;
            public bool video;
            public float vote_average;

            public string GetName() => isMovie ? this.title : this.name;
            public string GetDate() => isMovie ? this.release_date : this.first_air_date;
            public string GetYear() => GetDate().Substring(0, 4);
        }
    }
}
