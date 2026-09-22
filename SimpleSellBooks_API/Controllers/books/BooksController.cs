using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SimpleSellBooks_API.helper_methods;
using SimpleSellBooks_API.Services;
using SimpleSellBooks_BusinessLayer.books;
using SimpleSellBooks_DataLayer.books;
using System.Net;
using System.Security.Claims;

namespace SimpleSellBooks_API.Controllers.books
{

    [Authorize]
    [Route("api/Books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ISecurityAuditService _auditService;

        public BooksController(ISecurityAuditService auditService)
        {
            _auditService = auditService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("All", Name = "GetAllBooks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<clsBookDTO>> GetAllBooks()
        {
            IEnumerable<clsBookDTO> bookList = clsBookBusiness.GetAllBooks();

            if (!bookList.Any())
            {
                return NotFound("No Books Found!");
            }

            return Ok(bookList);
        }


        [Authorize(Roles = "Admin, Seller")]
        [HttpGet("FindBookById", Name = "GetBookById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<clsBookDTO> GetBookById(int bookId)
        {
            if (bookId < 1)
            {
                return BadRequest($"Not accepted bookId {bookId}");
            }

            clsBookBusiness? book = clsBookBusiness.FindBook(bookId);

            if (book == null)
            {
                return NotFound($"Book with bookId {bookId} not found.");
            }

            return Ok(book.bookDTO);
        }

        [Authorize(Roles = "Admin, Seller")]
        [HttpPost("AddNewBook")]
        [EnableRateLimiting("CreatePolicy")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<clsBookDTO>> AddNewBook(clsBookDTO newBookDTO)
        {
            if (newBookDTO == null || newBookDTO.sellerId < 0 || 
                newBookDTO.categoryId < 0 || string.IsNullOrEmpty(newBookDTO.title) || 
                string.IsNullOrEmpty(newBookDTO.author) || newBookDTO.price < 0 || 
                newBookDTO.stock < 0 || newBookDTO.conditionId < 0 || newBookDTO.statusId < 0)
            {
                return BadRequest("Invalid book data.");
            }

            clsBookBusiness book = new clsBookBusiness();
            book.sellerId = newBookDTO.sellerId;
            book.categoryId = newBookDTO.categoryId;
            book.title = newBookDTO.title;
            book.author = newBookDTO.author;
            book.bookDescription = !string.IsNullOrEmpty(newBookDTO.bookDescription) ? newBookDTO.bookDescription : null;
            book.price = newBookDTO.price;
            book.stock = newBookDTO.stock;
            book.conditionId = newBookDTO.conditionId;
            book.coverImg = !string.IsNullOrEmpty(newBookDTO.coverImg) ? newBookDTO.coverImg : null;
            book.statusId = newBookDTO.statusId;

            if (book.Save())
            {
                await _auditService.LogAsync(
                    eventType: clsHelperMethods.GetCurrentRole(HttpContext),
                    HttpContext,
                    userId: clsHelperMethods.GetCurrentUserId(HttpContext),
                    action: SecurityAction.Delete,
                    statusCode: StatusCodes.Status201Created,
                    targetType: "Book",
                    targetId: book.bookId.ToString(),
                    details: $"Book Added By{clsHelperMethods.GetCurrentRole(HttpContext)}."
                );
                return CreatedAtRoute("GetBookById",
                    new { bookId = book.bookId },
                    book.bookDTO);
            }

            return BadRequest("Failed to add book.");
        }


        [Authorize(Roles = "Admin, Seller")]
        [HttpPut("UpdateBookByID")]
        [EnableRateLimiting("UpdatePolicy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<clsBookDTO>> UpdateBook(int id, clsBookDTO updatedBookDTO)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            if (updatedBookDTO == null ||
                updatedBookDTO.categoryId < 0 || string.IsNullOrEmpty(updatedBookDTO.title) ||
                string.IsNullOrEmpty(updatedBookDTO.author) || updatedBookDTO.price < 0 ||
                updatedBookDTO.stock < 0 || updatedBookDTO.conditionId < 0 || updatedBookDTO.statusId < 0)
            {
                return BadRequest("Invalid book data.");
            }

            clsBookBusiness? book = clsBookBusiness.FindBook(id);

            if (book == null)
            {
                return NotFound($"Book with ID {id} not found.");
            }
            
            book.categoryId = updatedBookDTO.categoryId;
            book.title = updatedBookDTO.title;
            book.author = updatedBookDTO.author;
            book.bookDescription = !string.IsNullOrEmpty(updatedBookDTO.bookDescription) ? updatedBookDTO.bookDescription : null;
            book.price = updatedBookDTO.price;
            book.stock = updatedBookDTO.stock;
            book.conditionId = updatedBookDTO.conditionId;
            book.coverImg = !string.IsNullOrEmpty(updatedBookDTO.coverImg) ? updatedBookDTO.coverImg : null;
            book.statusId = updatedBookDTO.statusId;

            if (book.Save())
            {
                await _auditService.LogAsync(
                    eventType: clsHelperMethods.GetCurrentRole(HttpContext),
                    HttpContext,
                    userId: clsHelperMethods.GetCurrentUserId(HttpContext),
                    action: SecurityAction.Delete,
                    statusCode: StatusCodes.Status200OK,
                    targetType: "Book",
                    targetId: book.bookId.ToString(),
                    details: $"Book Was Updated By {clsHelperMethods.GetCurrentRole(HttpContext)}."
                );
                return Ok(book.bookDTO);
            }

            return BadRequest("Failed to update book.");
        }


        [Authorize(Roles = "Admin, Seller")]
        [HttpDelete("DeleteBookByID")]
        [EnableRateLimiting("DeletePolicy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteBook(int id)
        {
            
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            clsBookBusiness? book = clsBookBusiness.FindBook(id);

            if (book == null)
            {
                return NotFound($"Book with ID {id} not found.");
            }

            if (clsBookBusiness.DeleteBook(id))
            {
                await _auditService.LogAsync(
                    eventType: clsHelperMethods.GetCurrentRole(HttpContext),
                    HttpContext,
                    userId: clsHelperMethods.GetCurrentUserId(HttpContext),
                    action: SecurityAction.Delete,
                    statusCode: StatusCodes.Status200OK,
                    targetType: "Book",
                    targetId: book.bookId.ToString(),
                    details: $"Book Was Deleted By {clsHelperMethods.GetCurrentRole(HttpContext)}."
                );
                return Ok($"Book with ID {id} has been deleted.");
            }

            return BadRequest("Failed to delete book.");
        }
    }
}
