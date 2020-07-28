using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Dtos;
using Database.Data;
using Database.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    /// <summary>
    /// Endpoint api/movies
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/movies
        /// <summary>
        /// Endpoint HTTP GET api/movies para retornar a lista de Movies do banco de dados
        /// </summary>
        /// <param name="genre">Genre para aplicar filtro</param>
        /// <returns>IEnumerable<MovieDto></returns>
        [HttpGet]
        //Aqui estou limitando o número de resultados para 3000 em um uníca requisição,
        //O recomendável é usar a função no OData de paginação, porém no Web-Client não implementei a função de Paginação
        //Também estou filtrando algumas funções para não ser possível que o client execute queries pesadas no banco de dados
        [EnableQuery(PageSize = 3000, AllowedFunctions = AllowedFunctions.AllFunctions &
            ~AllowedFunctions.All & ~AllowedFunctions.Any & ~AllowedFunctions.AllStringFunctions, AllowedOrderByProperties = "title, averageRating")]
        public IEnumerable<MovieDto> Get(string genre)
        {
            IQueryable<Movies> query = null;

            if (string.IsNullOrWhiteSpace(genre))
            {
                query = _context.Movies.FromSqlRaw("select * from movies");
            }
            else
            {
                //Como o EF Core não conseguiu gerar um script SQL para filtrar o Genre, eu escrevi um script para poder aplicar o Filtro
                //Usei a função FromSqlInterpolated onde ele previne SQL Injection 
                query = _context.Movies.FromSqlInterpolated($"select * from movies m where m.movie_id in (select mg.movie_id from movie_genres mg where genre = {genre})");
            }

            return query.AsNoTracking()
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
