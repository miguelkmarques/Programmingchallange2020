using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GenresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/genres
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return _context.Genres.AsNoTracking()
                .Select(s => s.Genre);
        }
    }
}
