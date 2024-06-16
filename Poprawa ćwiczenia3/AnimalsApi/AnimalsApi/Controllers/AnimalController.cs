using AnimalsApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AnimalsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AnimalsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: api/animals
        [HttpGet]
        public IActionResult GetAnimals([FromQuery] string orderBy = "name")
        {
            var validColumns = new List<string> { "name", "description", "category", "area" };
            if (!validColumns.Contains(orderBy.ToLower()))
            {
                return BadRequest("Invalid sort column");
            }

            var animals = new List<Animal>();
            var query = $"SELECT * FROM Animals ORDER BY {orderBy} ASC";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        animals.Add(new Animal
                        {
                            IdAnimal = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            Category = reader.GetString(3),
                            Area = reader.GetString(4)
                        });
                    }
                }
            }

            return Ok(animals);
        }

        // POST: api/animals
        [HttpPost]
        public IActionResult AddAnimal([FromBody] Animal newAnimal)
        {
            var query = "INSERT INTO Animals (Name, Description, Category, Area) VALUES (@Name, @Description, @Category, @Area)";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", newAnimal.Name);
                    command.Parameters.AddWithValue("@Description", newAnimal.Description);
                    command.Parameters.AddWithValue("@Category", newAnimal.Category);
                    command.Parameters.AddWithValue("@Area", newAnimal.Area);
                    command.ExecuteNonQuery();
                }
            }

            return CreatedAtAction(nameof(GetAnimals), new { id = newAnimal.IdAnimal }, newAnimal);
        }

        // PUT: api/animals/{idAnimal}
        [HttpPut("{idAnimal}")]
        public IActionResult UpdateAnimal(int idAnimal, [FromBody] Animal updatedAnimal)
        {
            var query = "UPDATE Animals SET Name = @Name, Description = @Description, Category = @Category, Area = @Area WHERE IdAnimal = @IdAnimal";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdAnimal", idAnimal);
                    command.Parameters.AddWithValue("@Name", updatedAnimal.Name);
                    command.Parameters.AddWithValue("@Description", updatedAnimal.Description);
                    command.Parameters.AddWithValue("@Category", updatedAnimal.Category);
                    command.Parameters.AddWithValue("@Area", updatedAnimal.Area);
                    var rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        return NotFound();
                    }
                }
            }

            return NoContent();
        }

        // DELETE: api/animals/{idAnimal}
        [HttpDelete("{idAnimal}")]
        public IActionResult DeleteAnimal(int idAnimal)
        {
            var query = "DELETE FROM Animals WHERE IdAnimal = @IdAnimal";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdAnimal", idAnimal);
                    var rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        return NotFound();
                    }
                }
            }

            return NoContent();
        }
    }
}
