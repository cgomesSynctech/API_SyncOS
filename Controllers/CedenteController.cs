using FluentAssertions.Equivalency;
using info;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
using System.Runtime.ConstrainedExecution;

namespace SyncOS_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CedenteController : ControllerBase
    {
        private readonly APPDbContext _ctx;

        public CedenteController(APPDbContext context)
        {
            _ctx = context;
        }

        [HttpGet("cnpj/{cnpj}")]
        public async Task<ActionResult<Cedente>> GetCNPJ(string cnpj)
        {
            try
            {
                return Ok(await _ctx.Cedentes.FirstOrDefaultAsync(f => f.CpfCnpj == cnpj));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("PorDescricaoCarteira/{descricao}")]
        public ActionResult<Cedente> PorDescricaoCarteira(string descricao)
        {
            try
            {
                var cedente = from b in _ctx.Cedentes
                          join c in _ctx.Carteiras on b.Id equals c.CedenteId
                          where c.Descricao.Contains(descricao) 
                          select new Cedente
                          {
                                Id = b.Id,
                                CpfCnpj = b.CpfCnpj,
                                Codigo = b.Codigo,
                                Nome = b.Nome,
                                Logradouro = b.Logradouro,
                                Numero = b.Numero,
                                Complemento = b.Complemento,
                                Bairro = b.Bairro,
                                Cidade = b.Cidade,
                                CEP = b.CEP,
                                UF = b.UF,
                                LogoCedente = b.LogoCedente
                          };
                return Ok(cedente);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
