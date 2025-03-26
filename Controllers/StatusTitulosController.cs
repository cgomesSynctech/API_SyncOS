using info;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace SyncOS_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusTitulosController : ControllerBase
    {
        private readonly APPDbContext _ctx;

        public StatusTitulosController(APPDbContext context)
        {
            _ctx = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<StatusTitulo>>> GetAll()
        {
            try
            {
                return Ok(await _ctx.StatusTitulo
                    .ToListAsync());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StatusTitulo>> PorId(int id)
        {
            try
            {
                var status = await _ctx.StatusTitulo
                    .FirstOrDefaultAsync(f => f.Id == id);
                return Ok(status);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}