using info;
using Microsoft.AspNetCore.Mvc;
using Modelos;

namespace SyncOS_API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class RetornoController : ControllerBase
    {

        private readonly APPDbContext _ctx;

        public RetornoController(APPDbContext context)
        {
            _ctx = context;
        }

        [HttpPost]
        public async Task<ActionResult<Retorno>> Post([FromBody] Retorno retorno)
        {
            try
            {
                var ret = await _ctx.Retornos.AddAsync(retorno);
                await _ctx.SaveChangesAsync();
                return Ok(retorno);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
