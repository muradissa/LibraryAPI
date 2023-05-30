using LibraryAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        
        private readonly IConfiguration _configuration;

        public BookController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<Book> books = new List<Book>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Books", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Book book = new Book
                    {
                        BookID = Convert.ToInt32(reader["BookID"]),
                        Name = reader["Name"].ToString(),
                        Author = reader["Author"].ToString(),
                        Year = Convert.ToInt32(reader["Year"]),
                        IsAvailable = Convert.ToBoolean(reader["IsAvailable"]),
                        ClientID = Convert.ToInt32(reader["ClientID"])
                    };

                    books.Add(book);
                }
            }

            return Ok(books);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Book book = null;

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Books WHERE BookID = @ID", connection);
                command.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    book = new Book
                    {
                        BookID = Convert.ToInt32(reader["BookID"]),
                        Name = reader["Name"].ToString(),
                        Author = reader["Author"].ToString(),
                        Year = Convert.ToInt32(reader["Year"]),
                        IsAvailable = Convert.ToBoolean(reader["IsAvailable"]),
                        ClientID = Convert.ToInt32(reader["ClientID"])
                    };
                }
            }

            if (book == null)
                return NotFound();

            return Ok(book);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Book book)
        {
            if (book == null)
                return BadRequest();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO Books (Name, Author, Year, IsAvailable, ClientID) VALUES (@Name, @Author, @Year, @IsAvailable, @ClientID); SELECT SCOPE_IDENTITY();", connection);
                command.Parameters.AddWithValue("@Name", book.Name);
                command.Parameters.AddWithValue("@Author", book.Author);
                command.Parameters.AddWithValue("@Year", book.Year);
                command.Parameters.AddWithValue("@IsAvailable", book.IsAvailable);
                command.Parameters.AddWithValue("@ClientID", book.ClientID);
                int insertedId = Convert.ToInt32(command.ExecuteScalar());

                book.BookID = insertedId;
            }

            return Ok(book);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Book book)
        {
            if (book == null || book.BookID != id)
                return BadRequest();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("UPDATE Books SET Name = @Name, Author = @Author, Year = @Year, IsAvailable = @IsAvailable, ClientID = @ClientID WHERE BookID = @ID", connection);
                command.Parameters.AddWithValue("@ID", id);
                command.Parameters.AddWithValue("@Name", book.Name);
                command.Parameters.AddWithValue("@Author", book.Author);
                command.Parameters.AddWithValue("@Year", book.Year);
                command.Parameters.AddWithValue("@IsAvailable", book.IsAvailable);
                command.Parameters.AddWithValue("@ClientID", book.ClientID);
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                    return NotFound();
            }

            return Ok(book);
        }

        [HttpPut("takebooks/{id}")]
        public IActionResult TakeBooks(int clientID, [FromBody] List<int> listOfBooks)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                connection.Open();

                // Update the IsAvailable field for matching ClientID and BookID rows
                foreach (int bookID in listOfBooks)
                {
                    SqlCommand command = new SqlCommand("UPDATE Books SET IsAvailable = 0 WHERE ClientID = @ClientID AND BookID = @BookID", connection);
                    command.Parameters.AddWithValue("@ClientID", clientID);
                    command.Parameters.AddWithValue("@BookID", bookID);
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        // If no rows were updated, return a not found response
                        return NotFound();
                    }
                }
            }

            return NoContent();
        }


        [HttpPut("returnbooks/{id}")]
        public IActionResult ReturnBooks(int clientID, [FromBody] List<int> listOfBooks)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                connection.Open();

                // Update the IsAvailable field for matching ClientID and BookID rows
                foreach (int bookID in listOfBooks)
                {
                    SqlCommand command = new SqlCommand("UPDATE Books SET IsAvailable = 1 WHERE ClientID = @ClientID AND BookID = @BookID", connection);
                    command.Parameters.AddWithValue("@ClientID", clientID);
                    command.Parameters.AddWithValue("@BookID", bookID);
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        // If no rows were updated, return a not found response
                        return NotFound();
                    }
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Book book = null;

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Books WHERE BookID = @ID", connection);
                command.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    book = new Book
                    {
                        BookID = Convert.ToInt32(reader["BookID"]),
                        Name = reader["Name"].ToString(),
                        Author = reader["Author"].ToString(),
                        Year = Convert.ToInt32(reader["Year"]),
                        IsAvailable = Convert.ToBoolean(reader["IsAvailable"]),
                        ClientID = Convert.ToInt32(reader["ClientID"])
                    };
                }
            }

            if (book == null)
                return NotFound();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("DELETE FROM Books WHERE BookID = @ID", connection);
                command.Parameters.AddWithValue("@ID", id);
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                    return NotFound();
            }

            return Ok(book);
        }
    }

}
