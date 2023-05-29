using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ClientController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<Client> clients = new List<Client>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Clients", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Client client = new Client
                    {
                        ClientID = Convert.ToInt32(reader["ClientID"]),
                        Name = reader["Name"].ToString(),
                        Email = reader["Email"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Address = reader["Address"].ToString()
                    };

                    clients.Add(client);
                }
            }

            return Ok(clients);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Client client = null;

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Clients WHERE ClientID = @ID", connection);
                command.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    client = new Client
                    {
                        ClientID = Convert.ToInt32(reader["ClientID"]),
                        Name = reader["Name"].ToString(),
                        Email = reader["Email"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Address = reader["Address"].ToString()
                    };
                }
            }

            if (client == null)
                return NotFound();

            return Ok(client);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Client client)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO Clients (Name, Email, Phone, Address) VALUES (@Name, @Email, @Phone, @Address); SELECT CAST(SCOPE_IDENTITY() AS INT);", connection);
                command.Parameters.AddWithValue("@Name", client.Name);
                command.Parameters.AddWithValue("@Email", client.Email);
                command.Parameters.AddWithValue("@Phone", client.Phone);
                command.Parameters.AddWithValue("@Address", client.Address);
                int insertedId = (int)command.ExecuteScalar();
                client.ClientID = insertedId;
            }

            return CreatedAtAction(nameof(Get), new { id = client.ClientID }, client);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Client client)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("UPDATE Clients SET Name = @Name, Email = @Email, Phone = @Phone, Address = @Address WHERE ClientID = @ID", connection);
                command.Parameters.AddWithValue("@Name", client.Name);
                command.Parameters.AddWithValue("@Email", client.Email);
                command.Parameters.AddWithValue("@Phone", client.Phone);
                command.Parameters.AddWithValue("@Address", client.Address);
                command.Parameters.AddWithValue("@ID", id);
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                    return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("DELETE FROM Clients WHERE ClientID = @ID", connection);
                command.Parameters.AddWithValue("@ID", id);
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                    return NotFound();
            }

            return NoContent();
        }
    }

    public class Client
    {
        public int ClientID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
