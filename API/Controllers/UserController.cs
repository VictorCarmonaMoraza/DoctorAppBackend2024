using Data.DbContext_Conection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Entidades;

namespace API.Controllers
{
    public class UserController : BaseApiController
    {

        private readonly ApplicationDbContext? _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = _context != null ? await _context.Users.ToListAsync() : null;
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = _context !=null ? await _context!.Users.FindAsync(id) : null;

            if (user == null)
            {
                return NotFound("Usuario no encontrado con ese id en la base de datos");
            }

            return Ok(user);
        }
    }
}