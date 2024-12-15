using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RPBDIS_LibraryDB_lab6.Controllers;
using RPBDIS_LibraryDB_lab6.Data;
using RPBDIS_LibraryDB_lab6.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test_lab6
{
    public class BooksControllerTests
    {
        private LibraryDbContext GetTestDbContext()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // ”никальное им€ базы данных
                .Options;

            return new LibraryDbContext(options);
        }
            
        [Fact]
        public async Task GetBook_ReturnsBook_WhenBookExists()
        {
            // Arrange
            var context = GetTestDbContext();
            var book = new Book { BookId = 1, Title = "Book 1", Author = "Test Author 1" };
            context.Books.Add(book);
            await context.SaveChangesAsync();

            var controller = new BooksController(context);

            // Act
            var result = await controller.GetBook(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(book.BookId, result.Value.BookId);
        }

        [Fact]
        public async Task GetBook_ReturnsNotFound_WhenBookDoesNotExist()
        {
            // Arrange
            var context = GetTestDbContext();
            var controller = new BooksController(context);

            // Act
            var result = await controller.GetBook(999);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostBook_CreatesNewBook()
        {
            // Arrange
            var context = GetTestDbContext();
            var controller = new BooksController(context);
            var newBook = new Book { Title = "New Book", Author = "Test Author 1" };

            // Act
            var result = await controller.PostBook(newBook);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedBook = Assert.IsType<Book>(createdResult.Value);
            Assert.Equal("GetBook", createdResult.ActionName);
            Assert.Equal(newBook.Title, returnedBook.Title);
            Assert.Equal(1, context.Books.Count());
        }

        [Fact]
        public async Task PutBook_ReturnsBadRequest_WhenIdsDoNotMatch()
        {
            // Arrange
            var context = GetTestDbContext();
            var controller = new BooksController(context);
            var updatedBook = new Book { BookId = 2, Title = "Updated Title", Author = "Test Author 2" };

            // Act
            var result = await controller.PutBook(1, updatedBook);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteBook_RemovesBook_WhenBookExists()
        {
            // Arrange
            var context = GetTestDbContext();
            var book = new Book { BookId = 1, Title = "Book to Delete", Author = "Test Author 1" };
            context.Books.Add(book);
            await context.SaveChangesAsync();

            var controller = new BooksController(context);

            // Act
            var result = await controller.DeleteBook(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(0, context.Books.Count());
        }

        [Fact]
        public async Task DeleteBook_ReturnsNotFound_WhenBookDoesNotExist()
        {
            // Arrange
            var context = GetTestDbContext();
            var controller = new BooksController(context);

            // Act
            var result = await controller.DeleteBook(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}