using info;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modelos;

namespace SyncOS_API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class GeneratorController : ControllerBase
    {
        private readonly APPDbContext _ctx;

        public GeneratorController(APPDbContext context)
        {
            _ctx = context;
        }

        [HttpGet("{nome}")]
        public async Task<int> GetNextId(string nome)
        {
            try
            {
                var generator = new GeradorID();
                if (!_ctx.Generators.Any(a => a.Nome == nome))
                {
                    generator.Nome = nome;
                    generator.UltimoID = 0;
                    _ctx.Add(generator);
                    await _ctx.SaveChangesAsync();
                }
                generator = _ctx.Generators.FirstOrDefault(f => f.Nome == nome);
                generator.UltimoID++;
                _ctx.Update(generator);
                await _ctx.SaveChangesAsync();
                return generator.UltimoID;
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
