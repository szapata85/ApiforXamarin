using DB.Data;
using DB.Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace APIHolaMundo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private DatabaseContext _context;

        public ClientsController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Client> GetClient() => _context.Clients.ToList();
    }
}
