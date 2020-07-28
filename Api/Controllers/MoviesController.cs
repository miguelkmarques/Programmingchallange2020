using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Dtos;
using Database.Data;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/movies
        [HttpGet]
        [EnableQuery(PageSize = 3000, AllowedFunctions = AllowedFunctions.AllFunctions &
            ~AllowedFunctions.All & ~AllowedFunctions.AllStringFunctions, AllowedOrderByProperties = "title, averageRating")]
        public IEnumerable<MovieDto> Get()
        {
            return _context.Movies.AsNoTracking()
                .Select(s => new MovieDto
                {
                    movieId = s.MovieId,
                    title = s.Title,
                    averageRating = s.AverageRating,
                    year = s.Year,
                    genres = s.Genres.Select(sg => sg.Genre).ToList()
                });
        }
    }
}
