using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.DbOperations;
using WebAPI.BookOperations.GetBooks;
using WebAPI.BookOperations.CreateBook;
using WebAPI.BookOperations.UpdateBook;
using WebAPI.BookOperations.GetBookDetail;
using WebAPI.BookOperations.DeleteBook;
using FluentValidation;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]s")]
    public class BookController : ControllerBase
    {

        private readonly BookStoreDbContext _context;

        public BookController(BookStoreDbContext context)
        {

            _context = context;

        }


        [HttpGet]
        public IActionResult GetBooks()
        {
            //old
            //var bookList = _context.Books.OrderBy(x => x.Id).ToList<Book>();
            //return bookList;

            GetBooksQuery query = new GetBooksQuery(_context);
            var result = query.Handle();
            return Ok(result);

            

            

        }


        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            /*old
            var book = _context.Books.Where(book => book.Id == id).SingleOrDefault();
            return book;
            */

            /* 1
            GetBookById getByIdQuery = new GetBookById(_context);
            var result = getByIdQuery.Handle(id);
            return Ok(result);
            */


            BookDetailViewModel result;
            try
            {
                GetBookDetailQuery query = new GetBookDetailQuery(_context);
                GetBookDetailQueryValidator validator = new GetBookDetailQueryValidator();                
                query.BookId = id;
                validator.ValidateAndThrow(query);
                result = query.Handle();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok(result);



        }

        [HttpPost]
        public IActionResult AddBook([FromBody] CreateBookModel newBook)
        {
            /* old
            var book = _context.Books.SingleOrDefault(x => x.Title == newBook.Title);
            if(book is not null)
            {
                return BadRequest();
            }
            _context.Books.Add(newBook);
            _context.SaveChanges();
            return Ok();
            */

            CreateBookCommand command = new CreateBookCommand(_context);
            try
            {
                command.Model = newBook;
                CreateBookCommandValidator validator = new CreateBookCommandValidator();
                validator.ValidateAndThrow(command);
                command.Handle();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();




        }


        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, [FromBody] UpdateBookModel updatedBook )
        {
            /* old
            var book = _context.Books.SingleOrDefault(x => x.Id == id);
            if(book is null)
            {
                return BadRequest();
            }
            book.GenreId = (updatedBook.GenreId != default) ? updatedBook.GenreId : book.GenreId;
            book.PageCount = (updatedBook.PageCount != default) ? updatedBook.PageCount : book.PageCount;
            book.PublishDate = (updatedBook.PublishDate != default) ? updatedBook.PublishDate : book.PublishDate;
            book.Title = (updatedBook.Title != default) ? updatedBook.Title : book.Title;
            _context.SaveChanges();
            return Ok();
            */

            
            try
            {
                UpdateBookCommand command = new UpdateBookCommand(_context);
                command.BookId = id;
                command.Model = updatedBook;
                UpdateBookCommandValidator validator = new UpdateBookCommandValidator();
                validator.ValidateAndThrow(command);
                command.Handle();

            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
    
            }
                    
            return Ok();



        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBoook(int id)
        {
            /* old
            var book = _context.Books.SingleOrDefault(x => x.Id == id);

            if(book is null)
            {
                return BadRequest();
            }

            _context.Books.Remove(book);
            _context.SaveChanges();

            return Ok();
            */

            /* 1
            try
            {
                DeleteBookCommand command = new DeleteBookCommand(_context);
                command.BookId = id;
                command.Handle();

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);

            }

            return Ok();
            */

            try
            {
                DeleteBookCommand command = new DeleteBookCommand(_context);
                command.BookId = id;
                DeleteBookCommandValidator validator = new DeleteBookCommandValidator();
                validator.ValidateAndThrow(command);                
                command.Handle();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();






        }







    }
}
