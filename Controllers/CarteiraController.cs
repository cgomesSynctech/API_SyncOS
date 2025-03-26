using info;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace SyncOS_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarteiraController : ControllerBase
    {
        private readonly APPDbContext _ctx;

        public CarteiraController(APPDbContext context)
        {
            _ctx = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Carteira>>> GetAll()
        {
            try
            {
                return Ok(await _ctx.Carteiras
                    .Include(i => i.Cedente)
                    .ToListAsync());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Carteira>> PorId(int id)
        {
            try
            {
                var carteira = await _ctx.Carteiras
                    .Include(i => i.Cedente)
                    .FirstOrDefaultAsync(f => f.Id == id); 
                return Ok(carteira);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


    }
}
