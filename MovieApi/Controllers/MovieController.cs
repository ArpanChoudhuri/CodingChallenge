using CodingChallenge.DataAccess;
using CodingChallenge.DataAccess.Interfaces;
using CodingChallenge.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        public ILibraryService LibraryService { get; private set; }

        private readonly ILogger<MovieController> _logger;

        public MovieController(ILibraryService libraryService,ILogger<MovieController> logger)
        {
            LibraryService = libraryService;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Movie> GetMovies()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new Movie
            {
                
            })
            .ToArray();
        }

        [HttpPost]
        public async Task<List<Movie>> SearchMovies([FromBody] MovieSearch movie)
        {
            List<Movie> movies = new List<Movie>();
            movies = LibraryService.SearchMoviesForApi(movie).ToList();

            return  movies;
        }
    }
}
