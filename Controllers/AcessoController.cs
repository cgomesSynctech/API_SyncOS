using info;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
using Services;

namespace SyncOS_API.Controllers
{

    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class AcessoController : ControllerBase
    {
        private readonly APPDbContext _ctx;

        public AcessoController(APPDbContext context)
        {
            _ctx = context;
        }

        [HttpGet("acessomega/{cnpj}")]
        public async Task<string> AcessoMega(string cnpj)
        {
            try
            {
                var cliente = await _ctx.Clientes.FirstOrDefaultAsync(c => c.CPF_CNPJ == cnpj) ?? new Cliente();

                var acessoMega = string.Empty;

                if (cliente.ValorContratoBackup == 0)
                {
                    acessoMega = await System.IO.File.ReadAllTextAsync("./AcessoMegaSemContrato.Json");
                }
                else
                {
                    acessoMega = await System.IO.File.ReadAllTextAsync("./AcessoMega.Json");
                }


                return acessoMega;
            }
            catch (Exception)
            {
                return await System.IO.File.ReadAllTextAsync("./AcessoMegaSemContrato.Json");
            }
        }



        [HttpGet("Atualizacao/{ultima}/{cnpj}/{tipo}")]
        public async Task<ActionResult<Atualizacao>> Get(int ultima, string cnpj, int tipo)
        {
            try
            {
                var atualizacoes = new List<Atualizacao>();
                var lista = await _ctx.Atualizacoes
                    .Where(x => x.Ativo == "S" 
                        && (x.CNPJ == cnpj || x.CNPJ == "99999999999999") 
                        && x.TipoAtualizacaoId == tipo 
                        && x.Id > ultima)
                    .OrderBy(x => x.Id).ToListAsync();
                foreach (var item in lista)
                {
                    var atu = new Atualizacao
                    {
                        Id = item.Id,
                        Versao = item.Versao,
                        DataVersao = item.DataVersao,
                        CNPJ = item.CNPJ,
                        LeiaMe = item.LeiaMe,
                        Script = item.Script,
                        Pacote = item.Pacote,
                        PathDescompactar = item.PathDescompactar,
                        TipoAtualizacaoId = item.TipoAtualizacaoId,
                        Backup = item.Backup,
                        Ativo = item.Ativo
                    };
                    atualizacoes.Add(atu);
                }
                return Ok(atualizacoes);
            }
            catch (Exception e)
            {
                var erro = e.Message;
                return BadRequest(new Atualizacao());
            }
        }



        [HttpGet("Bloqueio/{cnpj}")]
        public async Task<ActionResult<bool>> Get(string cnpj)
        {
            try
            {
                var clienteBloqueado = new Cliente();
                clienteBloqueado.Bloqueado = true;
                var cliente = await _ctx.Clientes.FirstOrDefaultAsync(f => f.CPF_CNPJ == cnpj) ?? clienteBloqueado;
                return Ok(cliente.Bloqueado);
            }
            catch (Exception e)
            {
                Logger.Erro(e.Message);
                return BadRequest(true);
            }
        }

        [HttpPost()]
        public async Task<ActionResult<bool>> Post([FromBody] Cliente cliente)
        {
            try
            {
                var obj = await _ctx.Clientes.FindAsync(cliente.Id) ?? new Cliente();
                if (obj.Id <= 0)
                {
                    obj = cliente;
                    _ctx.Add(obj);
                }
                else
                {
                    obj = cliente;
                    _ctx.Entry(obj).State = EntityState.Modified;
                }
                await _ctx.SaveChangesAsync();
                new EspelharCliente(obj).Salvar();
                return Ok(true);
            }
            catch (Exception e)
            {
                Logger.Erro(e.Message);
                return BadRequest(false);
            }
        }
    }
}
