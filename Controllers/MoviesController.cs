using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Pets_API_Practice.Models;

//In API controller you may write non-endpoint helper methods 
//Examples of helper methods: 
//Validation - for rare cases SQL and C# can't cover something 
//Sorting or format output - say you only want the movie titles in alphabetical order 
//These helpers exist to be called by endpoints, but can't be called directly by HTTP 
//Helpers need to be private, as any public method in the API controller is treated as an endpoint


//writing an API endpoint that takes a string for title search, and returns all movies with all types.
//[http___("name of endpoint, --> {__}= expecting parameter, assign value of 'title' ")]
//LINQ is your friend with API calls.



namespace Pets_API_Practice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        public readonly MovieContext _context;

        public MoviesController(MovieContext context)
        {
            _context = context;
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }

        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }







        // GET: api/Movies
        [HttpGet] //Get a list of all movies
        public async Task<ActionResult<IEnumerable<Movie>>> GetAllMovies()
        {
            return await _context.Movies.ToListAsync();
        }

        [HttpGet("SearchByGenre/{genre}")] //Get a list of all movies in a specific category
        public async Task<ActionResult<IEnumerable<Movie>>> SearchByGenre(string genre)
        {
            return await _context.Movies.Where(m => m.Genre.Contains(genre)).ToListAsync();
        }

        [HttpGet("GetRandomMovie")] //Get a random movie pick by genre
        public Movie GetRandom()
        {  
            List<Movie> movies = _context.Movies.ToList();

            var rnd = new Random();
            int index = rnd.Next(movies.Count);

            return movies[index];
        }

        [HttpGet("SearchGenreRandomMovie/{genre}")]
        public IEnumerable<Movie> RandomGenreSearch()
        {
            List<Movie> movies = _context.Movies.ToList();

            var rnd = new Random();
            int index = rnd.Next(movies.Count);

            /*return _HERE__*/
        }
        
         /* 
         
        Get a random movie pick from a specific category
            User specifies category as a query parameter


        Get a list of random movie picks
            User specifies quantity as a query paramete*/




        [HttpGet("SearchByTitle/{title}")] //search by keyword
        public async Task<ActionResult<IEnumerable<Movie>>> SearchByTitle(string title)
        {
            return await _context.Movies.Where(m => m.Title.Contains(title)).ToListAsync();
        }


        //no (), as there is no user input needed
        //this function handles the functionality/process
        private List<string> GetTitles() // display all movies by title part1
        {
            List<string> titles = new List<string>();
            List<Movie> movies = _context.Movies.ToList();

            foreach (Movie m in movies)
            {
                titles.Add(m.Title);
            }
            titles.Sort();
            return titles;
        }

        [HttpGet("GetTitles")] // display all movies by title part2
        public async Task<ActionResult<IEnumerable<String>>> GetTitlesAlphabetical()
        {
            return GetTitles();
        }

        [HttpGet("GetDetails/{title}")] //get details about 1 movie by title
        public async Task<ActionResult<IEnumerable<Movie>>> GetDetails(string title)
        {
            return await _context.Movies.Where(m => m.Title.Contains(title)).ToListAsync();

        }
    }
}
