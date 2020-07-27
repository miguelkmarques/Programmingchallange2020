using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public MoviesController(ApplicationDbContext context, ILogger<MoviesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/<MoviesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
