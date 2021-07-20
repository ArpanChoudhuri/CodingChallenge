using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using CodingChallenge.DataAccess;
using CodingChallenge.DataAccess.Interfaces;
using CodingChallenge.DataAccess.Models;
using CodingChallenge.UI.Models;

namespace CodingChallenge.UI.Controllers
{
    public class DefaultController : Controller
    {
        public ILibraryService LibraryService { get; private set; }

        public DefaultController() { }

        public DefaultController(ILibraryService libraryService)
        {
            LibraryService = libraryService;
        }

        public ActionResult Index([ModelBinder(typeof(GridBinder))] GridOptions options,string search)
        {
            options.TotalItems = LibraryService.SearchMoviesCount("");
            if (options.SortColumn == null)
                options.SortColumn = "ID";
            var model = new MovieListViewModel
            {
                GridOptions = options,
                //change the api signature in accommodate for sortcolumn, sortdirection
                Movies = LibraryService.SearchMovies(search, (options.Page - 1) * options.ItemsPerPage, options.ItemsPerPage, options.SortColumn, options.SortDirection).ToList()
            };
            return View(model);
        }
    }
}
