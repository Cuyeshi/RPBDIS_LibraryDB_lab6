using Microsoft.AspNetCore.Mvc;
using RPBDIS_LibraryDB_lab6.Models;
using Microsoft.EntityFrameworkCore;
using RPBDIS_LibraryDB_lab6.Data;

namespace RPBDIS_LibraryDB_lab6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BooksController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: api/<BooksController>
        [HttpGet]
        public IActionResult GetBooks([FromQuery] int page = 1, [FromQuery] int pageSize = 6)
        {
            var totalBooks = _context.Books.Count();
            var totalPages = (int)Math.Ceiling(totalBooks / (double)pageSize);
            var books = _context.Books
                .Include(b => b.Genre)
                .Include(b => b.Publisher)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new
                {
                    b.BookId,
                    b.Title,
                    b.Author,
                    b.PublishYear,
                    b.Price,
                    Genre = b.Genre.Name,
                    Publisher = b.Publisher.Name
                })
                .ToList();
            return Ok(new
            {
                books,
                totalPages
            });
        }


        // GET api/<BooksController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Books.Include(b => b.Genre).Include(b => b.Publisher).FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // POST api/<BooksController>
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook([FromBody] Book bookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var genre = await _context.Genres.FindAsync(bookDto.GenreId);
            var publisher = await _context.Publishers.FindAsync(bookDto.PublisherId);

            if (genre == null || publisher == null)
            {
                return BadRequest("Invalid Genre or Publisher");
            }

            var book = new Book
            {
                Title = bookDto.Title,
                Author = bookDto.Author,
                PublishYear = bookDto.PublishYear,
                Price = bookDto.Price,
                GenreId = bookDto.GenreId,
                PublisherId = bookDto.PublisherId
            };

            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBook), new { id = book.BookId }, book);
        }

        // PUT api/<BooksController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, [FromBody] Book bookDto)
        {
            if (id != bookDto.BookId)
            {
                return BadRequest("Book ID mismatch");
            }

            var existingBook = await _context.Books.FindAsync(id);
            if (existingBook == null)
            {
                return NotFound();
            }

            existingBook.Title = bookDto.Title;
            existingBook.Author = bookDto.Author;
            existingBook.PublishYear = bookDto.PublishYear;
            existingBook.Price = bookDto.Price;
            existingBook.GenreId = bookDto.GenreId;
            existingBook.PublisherId = bookDto.PublisherId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/<BooksController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }

        // GET: api/Books/Genres
        [HttpGet("Genres")]
        public async Task<IActionResult> GetGenres()
        {
            var genres = await _context.Genres.Select(g => new { g.GenreId, g.Name }).ToListAsync();
            return Ok(genres);
        }

        // GET: api/Books/Publishers
        [HttpGet("Publishers")]
        public async Task<IActionResult> GetPublishers()
        {
            var publishers = await _context.Publishers.Select(p => new { p.PublisherId, p.Name }).ToListAsync();
            return Ok(publishers);
        }
    }
}
