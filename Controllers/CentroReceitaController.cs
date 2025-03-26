using info;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace SyncOS_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CentroReceitaController : ControllerBase
    {

        private readonly APPDbContext _ctx;

        public CentroReceitaController(APPDbContext context)
        {
            _ctx = context;
        }


        [HttpGet]
        public async Task<ActionResult<List<CentroReceita>>> GetAll()
        {
            try
            {
                var centros = await _ctx.Revendas.ToListAsync();
                var centroReceitas = new List<CentroReceita>();
                foreach (var item in centros)
                {
                    centroReceitas.Add(new CentroReceita
                    {
                        Id = item.Id,
                        Descricao = item.Descricao
                    });
                }

                return Ok(centroReceitas);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
