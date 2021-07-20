using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CodingChallenge.DataAccess.Interfaces;
using CodingChallenge.DataAccess.Models;
using CodingChallenge.Utilities;

namespace CodingChallenge.DataAccess
{
    public class LibraryService : ILibraryService
    {
        public LibraryService() { }

        //below method getting called from the mvc razor app
        private IEnumerable<Movie> GetMovies()
        {
            return _movies ?? (_movies = ConfigurationManager.AppSettings["LibraryPath"].FromFileInExecutingDirectory().DeserializeFromXml<Library>().Movies);
        }

        //below method getting called when calling from MovieAPI
        private IEnumerable<Movie> GetMoviesForAPi()
        {
            var x= Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var y = Path.Combine(x, "LibraryFile");
            foreach (var filePath in Directory.EnumerateFiles(y))
            {
                _movies = File.ReadAllText(filePath).DeserializeFromXml<Library>().Movies;

            }
            return _movies;
        }
        private IEnumerable<Movie> _movies { get; set; }

        public int SearchMoviesCount(string title)
        {
            return SearchMovies(title).Count();
        }

        //Data Access method that gets called from mvc razor app
        public IEnumerable<Movie> SearchMovies(string title, int? skip = null, int? take = null, string sortColumn = null, SortDirection sortDirection = SortDirection.Ascending)
        {

            var param = sortColumn;

            IEnumerable<Movie> movies;
            //filter based on the search criteria

            //Title
            title = string.IsNullOrEmpty(title) ? string.Empty : title.ToLowerInvariant();
            movies = GetMovies().Where(s => s.Title.ToLowerInvariant().Contains(title)).Distinct();

            if (!string.IsNullOrEmpty(sortColumn))
            {
                var propertyInfo = typeof(Movie).GetProperty(param);
                //build the regex expression that will be used to remove leading articles
                Regex MyRegEx = new Regex(@"\b(a|an|the)\b", RegexOptions.IgnoreCase);

                //sort it based on the leading articles removed and trimmed, when sorting on title
                //otherwise do the regular sorting
                switch (sortDirection)
                {
                    case SortDirection.Ascending:
                        if (sortColumn.ToLowerInvariant() == "title")
                        {
                            movies = movies.ToList()?
                           .Select(mov => new { mov, leadsremoved = MyRegEx.Replace(mov.Title.ToLowerInvariant(), "", 1) })
                           .OrderBy(x => x.leadsremoved.Trim())
                           .Select(x => x.mov);
                        }
                        else
                        {
                            movies = movies.ToList()?
                            .OrderBy(x => propertyInfo.GetValue(x, null)).ToList();
                        }
                        break;
                    case SortDirection.Descending:
                        if (sortColumn.ToLowerInvariant() == "title")
                        {
                            movies = movies.ToList()?
                          .Select(mov => new { mov, leadsremoved = MyRegEx.Replace(mov.Title.ToLowerInvariant(), "", 1) })
                          .OrderByDescending(x => x.leadsremoved.Trim())
                          .Select(x => x.mov);
                        }
                        else
                        {
                            movies = movies.ToList()?
                            .OrderByDescending(x => propertyInfo.GetValue(x, null)).ToList();
                        }
                        break;
                    default:
                        if (sortColumn.ToLowerInvariant() == "title")
                        {
                            movies = movies.ToList()?
                            .Select(mov => new { mov, leadsremoved = MyRegEx.Replace(mov.Title.ToLowerInvariant(), "", 1) })
                            .OrderByDescending(x => x.leadsremoved.Trim())
                            .Select(x => x.mov);
                        }
                        else
                        {
                            movies = movies.ToList()?
                            .OrderBy(x => propertyInfo.GetValue(x, null)).ToList();
                        }
                        break;

                }
            }



            if (skip.HasValue && take.HasValue)
            {
                movies = movies.Skip(skip.Value).Take(take.Value);
            }

            return movies.ToList();
        }

        //Data Access method that gets called from MovieAPI
        public IEnumerable<Movie> SearchMoviesForApi(MovieSearch movie)
        {

            IEnumerable<Movie> movies = GetMoviesForAPi();

            //filter based on the search criteria

            //Title
            if(!string.IsNullOrEmpty(movie.Title) )
            {
                movies = movies.Where(s => s.Title.ToLowerInvariant().Contains(movie.Title.ToLowerInvariant())).Distinct();
            }

            //Rating
            if(movie.RatingAbove > 0)
            {
                movies = movies.Where(s => s.Rating > movie.RatingAbove).Distinct();
            }
            if (movie.RatingBelow > 0)
            {
                movies = movies.Where(s => s.Rating < movie.RatingBelow).Distinct();
            }

            //date range
            if (movie.StartDate!=null && movie.StartDate != DateTime.MinValue && movie.EndDate != null && movie.EndDate != DateTime.MinValue)
            {
                movies = movies.Where(s => s.Date >= movie.StartDate && s.Date <= movie.EndDate).Distinct();
            }

            //franchise
            if (!string.IsNullOrEmpty(movie.Franchise))
            {
                movies = movies.Where(s => s.Franchise!=null && s.Franchise.ToLowerInvariant().Contains(movie.Franchise.ToLowerInvariant())).Distinct();
            }

            return movies.ToList();
        }
    }
}
