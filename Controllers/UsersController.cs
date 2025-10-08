using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Models;

namespace UserManagementAPI.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase {
        private static List<User> users = new();

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAll(int page = 1, int pageSize = 10) {
            try {
                var pagedUsers = users.Skip((page - 1) * pageSize).Take(pageSize);
                return Ok(pagedUsers);
            } catch {
                return StatusCode(500, new { error = "Internal server error." });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetById(int id) {
            try {
                var user = users.FirstOrDefault(u => u.Id == id);
                return user == null ? NotFound() : Ok(user);
            } catch {
                return StatusCode(500, new { error = "Internal server error." });
            }
        }

        [HttpPost]
        public ActionResult<User> Create(User user) {
            try {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                user.Id = users.Count + 1;
                users.Add(user);
                return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
            } catch {
                return StatusCode(500, new { error = "Internal server error." });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, User updatedUser) {
            try {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var user = users.FirstOrDefault(u => u.Id == id);
                if (user == null) return NotFound();
                user.Name = updatedUser.Name;
                user.Email = updatedUser.Email;
                return NoContent();
            } catch {
                return StatusCode(500, new { error = "Internal server error." });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id) {
            try {
                var user = users.FirstOrDefault(u => u.Id == id);
                if (user == null) return NotFound();
                users.Remove(user);
                return NoContent();
            } catch {
                return StatusCode(500, new { error = "Internal server error." });
            }
        }
    }
}