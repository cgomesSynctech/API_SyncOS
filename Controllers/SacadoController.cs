using info;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;
using Services;
using System.Net;

namespace SyncOS_API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class SacadoController : ControllerBase
    {
        private readonly APPDbContext _ctx;

        public SacadoController(APPDbContext context)
        {
            _ctx = context;
        }

        [Route("{id}/{cpfCnpj}")]
        [HttpGet]
        public async Task<ActionResult> Filtrar(int id, string cpfCnpj)
        {
            try
            {
                var query = from c in _ctx.Clientes
                            join b in _ctx.Boletos on c.Id equals b.SacadoId into boletos
                            from b in boletos.DefaultIfEmpty()
                            join s in _ctx.StatusTitulo on b != null ? b.StatusId : 0 equals s.Id into statuses
                            from s in statuses.DefaultIfEmpty()
                                // Apply the WHERE conditions
                            where (id == 0 || c.Id == id) &&
                                  (cpfCnpj == "-" || c.CPF_CNPJ == cpfCnpj)
                            // Apply the ORDER BY
                            orderby c.Razao
                            select new
                            {
                                Cliente = c.Razao,
                                CpfCnpj = c.CPF_CNPJ,
                                c.Bloqueado,
                                c.ValorContrato,
                                ValorBoleto = b != null ? b.Valor : 0,
                                Vencimento = b != null ? b.Vencimento : (DateTime?)null,
                                Pagamento = b != null ? b.Pagamento : (DateTime?)null,
                                StatusTitulo = s != null ? s.Descricao : null
                            };
                var sacados = await query.ToListAsync();
                return Ok(sacados);
            }
            catch (Exception e)
            {
                return NoContent();
            }
        }

        [Route("filtrarSacado/{dataInicial}/{dataFinal}/{statusId}/{centroReceitaId}")]
        [HttpGet]
        public async Task<ActionResult<List<Sacado>>> FiltrarSacado(string dataInicial, string dataFinal, int statusId, int centroReceitaId)
        {
            try
            {
                var dataI = DateTime.Parse(dataInicial);
                var dataF = DateTime.Parse(dataFinal);
                var boletos = new List<Boleto>();
                if (statusId == 0)
                {

                    boletos = await  _ctx.Boletos
                        .Where(w => w.Vencimento >= dataI && w.Vencimento <= dataF)
                        .Select(b => new Boleto() {
                            Id = b.Id,
                            SacadoId = b.SacadoId,
                            Vencimento = b.Vencimento,
                            Valor = b.Valor
                        })
                        .AsNoTracking()
                        .ToListAsync();
                }
                else
                {
                    boletos = await _ctx.Boletos
                        .Where(w => w.Vencimento >= dataI && w.Vencimento <= dataF && w.StatusId == statusId)
                        .Select(b => new Boleto()
                        {
                            Id = b.Id,
                            SacadoId = b.SacadoId,
                            Vencimento = b.Vencimento,
                            Valor = b.Valor
                        })
                        .AsNoTracking()
                        .ToListAsync();
                }

                var sacados = new List<Sacado>();
                foreach (var boleto in boletos)
                {
                    var cliente = (centroReceitaId == 0)
                        ? await _ctx.Clientes.FindAsync(boleto.SacadoId)
                        : await _ctx.Clientes.FirstOrDefaultAsync(f => f.Id == boleto.SacadoId && f.RevendaId == centroReceitaId);
                    if (cliente != null)
                    {
                        sacados.Add(new Sacado
                        {
                            Id = cliente.Id,
                            Codigo = cliente.Id.ToString().PadLeft(6, '0'),
                            CentroReceitaId = cliente.RevendaId,
                            Nome = cliente.Razao,
                            Fantasia = cliente.Fantasia,
                            CpfCnpj = cliente.CPF_CNPJ,
                            Logradouro = cliente.Endereco,
                            Bairro = cliente.Bairro,
                            CEP = cliente.CEP,
                            Complemento = "",
                            UF = cliente.UFId,
                            IsBloqueado = cliente.Bloqueado,
                            IsDesativado = cliente.Desativado,
                            Numero = cliente.Numero,
                            ValorContrato = cliente.ValorContrato,
                            NomeAvalista = cliente.Resp_Nome,
                            CpfCnpjAvalista = cliente.Resp_CPF,
                            Valor = boleto.Valor,
                            Vencimento = boleto.Vencimento
                        });
                    }
                }
                return sacados.ToList();
            }
            catch (Exception e)
            {
                return new List<Sacado>();
            }

        }


        [Route("desbloquear")]
        [HttpPut]
        public async Task<ActionResult<bool>> Desbloquear([FromBody] Sacado sacado)
        {
            try
            {
                var cliente = await _ctx.Clientes.SingleOrDefaultAsync(f => f.CPF_CNPJ == sacado.CpfCnpj) ?? new Cliente();
                cliente.Bloqueado = false;
                _ctx.Update(cliente);
                await _ctx.SaveChangesAsync();
                var salvarCliente = new EspelharCliente(cliente).Salvar();
                return Ok(true);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [Route("bloquear")]
        [HttpPut]
        public async Task<ActionResult<bool>> Bloquear([FromBody] Sacado sacado)
        {
            try
            {
                var cliente = await _ctx.Clientes.SingleOrDefaultAsync(f => f.CPF_CNPJ == sacado.CpfCnpj) ?? new Cliente();
                cliente.Bloqueado = true;
                _ctx.Update(cliente);
                await _ctx.SaveChangesAsync();
                var salvarCliente = new EspelharCliente(cliente).Salvar();
                return Ok(true);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("Avisar")]
        public async Task<ActionResult<bool>> Avisar([FromBody] Sacado sacado)
        {
            try
            {
  
                sacado.Nome = "Aviso cliente: " + sacado.Nome;
                var aviso = new Atualizacao
                {
                    Ativo = "S",
                    Backup = "N",
                    Versao = "1.0",
                    CNPJ = sacado.CpfCnpj,
                    LeiaMe = sacado.Fantasia,
                    Script = @"http://187.45.181.115:5555/Avisos/AvisoPendencia.html",
                    TipoAtualizacaoId = 100,
                };
                await _ctx.Atualizacoes.AddAsync(aviso);
                await _ctx.SaveChangesAsync();
                return Ok(true);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }



        [HttpPost("AvisarAutomaticamente/{quantidadeDiasInicial}/{quantidadeDiasFinal}")]
        public async Task<ActionResult<bool>> AvisarAutomaticamente(int quantidadeDiasInicial, int quantidadeDiasFinal)
        {
            try
            {
                var dataInicial = DateTime.Now.AddDays(quantidadeDiasInicial * -1).Date;
                var dataFinal = DateTime.Now.AddDays(quantidadeDiasFinal * -1).Date;

                var boletos = await _ctx.Boletos.Where(w => (w.Vencimento >= dataInicial && w.Vencimento < dataFinal) && w.StatusId == 2 && w.Cancelado == "N").ToListAsync();

                foreach (var boleto in boletos)
                {
                    var ret = await _ctx.Clientes.FindAsync(boleto.SacadoId) ?? new Cliente();

                    if (ret.Bloqueado)
                    {
                        var aviso = new Atualizacao
                        {
                            Ativo = "S",
                            Backup = "N",
                            Versao = "1.0",
                            CNPJ = ret.CPF_CNPJ,
                            DataVersao = dataFinal,
                            LeiaMe = ret.Fantasia,
                            Script = @"http://187.45.181.115:5555/Avisos/AvisoPendencia.html",
                            TipoAtualizacaoId = 100,
                        };
                        await _ctx.Atualizacoes.AddAsync(aviso);
                        await _ctx.SaveChangesAsync();
                    }
                }
                return Ok(true);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }



        [HttpPost("BloquearAutomaticamente/{quantidadeDias}")]
        public async Task<ActionResult<bool>> BloquearAutomaticamente(int quantidadeDias)
        {
            try
            {
                var data = DateTime.Now.AddDays(quantidadeDias * -1).Date;
                var dataBloqueio = DateTime.Now.Date;

                var boletos = await _ctx.Boletos.Where(w => w.Vencimento < data && w.StatusId == 2 && w.Cancelado == "N").ToListAsync();

                foreach (var boleto in boletos)
                {
                    var obj = await _ctx.Clientes.FindAsync(boleto.SacadoId) ?? new Cliente();
                    obj.Bloqueado = true;
                    obj.DataAlteracao = dataBloqueio;
                    _ctx.Update(obj);
                    await _ctx.SaveChangesAsync();
                    new EspelharCliente(obj).Salvar();
                }
                return Ok(true);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpGet("revendaId/{id}")]
        public async Task<ActionResult<List<Sacado>>> GetEmpresaId(int id)
        {
            try
            {
                var clientes = await _ctx.Clientes.Where(f => f.RevendaId == id).ToListAsync();

                var sacados = new List<Sacado>();
                foreach (var cliente in clientes)
                {
                    var sacado = new Sacado()
                    {
                        Id = cliente.Id,
                        Codigo = cliente.Id.ToString().PadLeft(6, '0'),
                        Nome = cliente.Razao,
                        Fantasia = cliente.Fantasia,
                        ValorContrato = cliente.ValorContrato,
                        CpfCnpj = cliente.CPF_CNPJ,
                        Logradouro = cliente.Endereco,
                        Numero = cliente.Numero,
                        Bairro = cliente.Bairro,
                        Cidade = _ctx.Municipios.Find(cliente.MunicipioId)?.Descricao ?? "João Pessoa",
                        CentroReceitaId = cliente.RevendaId,
                        IsBloqueado = cliente.Bloqueado,
                        IsDesativado = cliente.Desativado,
                        UF = cliente.UFId,
                        CEP = cliente.CEP,
                        NomeAvalista = cliente.Resp_Nome,
                        CpfCnpjAvalista = cliente.Resp_CPF
                    };
                    sacados.Add(sacado);
                }
                return Ok(sacados.ToList());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpGet("cnpj/{cnpj}")]
        public async Task<ActionResult<List<Sacado>>> GetCNPJ(string cnpj)
        {
            try
            {
                var sacados = new List<Sacado>();
                var cliente = await _ctx.Clientes.FirstOrDefaultAsync(f => f.CPF_CNPJ!.Contains(cnpj));
                var municipio = await _ctx.Municipios.FindAsync(cliente?.MunicipioId);
                sacados.Add(new Sacado()
                {
                    Id = cliente!.Id,
                    Codigo = cliente.Id.ToString().PadLeft(6, '0'),
                    Nome = cliente.Razao,
                    Fantasia = cliente.Fantasia,
                    ValorContrato = cliente.ValorContrato,
                    CpfCnpj = cliente.CPF_CNPJ,
                    Logradouro = cliente.Endereco,
                    Numero = cliente.Numero,
                    Bairro = cliente.Bairro,
                    Cidade = municipio?.Descricao,
                    CentroReceitaId = cliente.RevendaId,
                    IsBloqueado = cliente.Bloqueado,
                    IsDesativado = cliente.Desativado,
                    UF = cliente.UFId,
                    CEP = cliente.CEP,
                    NomeAvalista = cliente.Resp_Nome,
                    CpfCnpjAvalista = cliente.Resp_CPF
                });
                return Ok(sacados);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }



        [HttpGet("nome/{nome}")]
        public async Task<ActionResult<List<Sacado>>> GetNome(string nome)
        {
            try
            {
                var sacados = new List<Sacado>();
                var clientes = await _ctx.Clientes
                    .AsNoTracking()
                    .Where(f => f.Razao!.Contains(nome))
                    .ToListAsync();
                foreach (var cliente in clientes)
                {
                    var municipio = await _ctx.Municipios.FindAsync(cliente?.MunicipioId);
                    sacados.Add(new Sacado
                    {
                        Id = cliente!.Id,
                        Codigo = cliente.Id.ToString().PadLeft(6, '0'),
                        Nome = cliente.Razao,
                        Fantasia = cliente.Fantasia,
                        ValorContrato = cliente.ValorContrato,
                        CpfCnpj = cliente.CPF_CNPJ,
                        Logradouro = cliente.Endereco,
                        Numero = cliente.Numero,
                        Bairro = cliente.Bairro,
                        Cidade = municipio?.Descricao,
                        CentroReceitaId = cliente.RevendaId,
                        IsBloqueado = cliente.Bloqueado,
                        IsDesativado = cliente.Desativado,
                        UF = cliente.UFId,
                        CEP = cliente.CEP,
                        NomeAvalista = cliente.Resp_Nome,
                        CpfCnpjAvalista = cliente.Resp_CPF
                    });
                }

                return Ok(sacados.ToList());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }



        //[HttpGet("nome/{nome}")]
        //public async Task<ActionResult<List<Sacado>>> GetNome(string nome)
        //{
        //    try
        //    {
        //        var sacados = new List<Sacado>();
        //        var cliente = await _ctx.Clientes.FirstOrDefaultAsync(f => f.Razao.Contains(nome));
        //        var municipio = await _ctx.Municipios.FindAsync(cliente?.MunicipioId);
        //        sacados.Add(new Sacado()
        //        {
        //            Id = cliente!.Id,
        //            Codigo = cliente.Id.ToString().PadLeft(6, '0'),
        //            Nome = cliente.Razao,
        //            Fantasia = cliente.Fantasia,
        //            ValorContrato = cliente.ValorContrato,
        //            CpfCnpj = cliente.CPF_CNPJ,
        //            Logradouro = cliente.Endereco,
        //            Numero = cliente.Numero,
        //            Bairro = cliente.Bairro,
        //            Cidade = municipio?.Descricao,
        //            CentroReceitaId = cliente.RevendaId,
        //            IsBloqueado = cliente.Bloqueado,
        //            IsDesativado = cliente.Desativado,
        //            UF = cliente.UFId,
        //            CEP = cliente.CEP,
        //            NomeAvalista = cliente.Resp_Nome,
        //            CpfCnpjAvalista = cliente.Resp_CPF
        //        });
        //        return Ok(sacados);
        //    }
        //    catch (Exception)
        //    {
        //        return BadRequest();
        //    }
        //}


        [HttpGet("{id}")]
        public async Task<ActionResult<List<Sacado>>> Get(int id)
        {
            try
            {
                var cliente = await _ctx.Clientes.FindAsync(id);
                var municipio = await _ctx.Municipios.FindAsync(cliente?.MunicipioId);
                var sacado = new Sacado()
                {
                    Id = cliente!.Id,
                    Codigo = cliente.Id.ToString().PadLeft(6, '0'),
                    Nome = cliente.Razao,
                    Fantasia = cliente.Fantasia,
                    ValorContrato = cliente.ValorContrato,
                    CpfCnpj = cliente.CPF_CNPJ,
                    Logradouro = cliente.Endereco,
                    Numero = cliente.Numero,
                    Bairro = cliente.Bairro,
                    Cidade = municipio?.Descricao,
                    CentroReceitaId = cliente.RevendaId,
                    IsBloqueado = cliente.Bloqueado,
                    IsDesativado = cliente.Desativado,
                    UF = cliente.UFId,
                    CEP = cliente.CEP,
                    NomeAvalista = cliente.Resp_Nome,
                    CpfCnpjAvalista = cliente.Resp_CPF
    };
                return Ok(sacado);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }



        [HttpGet]
        public async Task<ActionResult<List<Sacado>>> GetAll()
        {
            try
            {
                var sacados = new List<Sacado>();
                var clientes = await _ctx.Clientes
                    .AsNoTracking()
                    .Where(w => w.Desativado.Equals(false))
                    .ToListAsync();
                foreach (var cliente in clientes)
                {
                    var municipio = await _ctx.Municipios.FindAsync(cliente?.MunicipioId);
                    sacados.Add(new Sacado
                    {
                        Id = cliente!.Id,
                        Codigo = cliente.Id.ToString().PadLeft(6, '0'),
                        Nome = cliente.Razao,
                        Fantasia = cliente.Fantasia,
                        ValorContrato = cliente.ValorContrato,
                        CpfCnpj = cliente.CPF_CNPJ,
                        Logradouro = cliente.Endereco,
                        Numero = cliente.Numero,
                        Bairro = cliente.Bairro,
                        Cidade = municipio?.Descricao,
                        CentroReceitaId = cliente.RevendaId,
                        IsBloqueado = cliente.Bloqueado,
                        IsDesativado = cliente.Desativado,
                        UF = cliente.UFId,
                        CEP = cliente.CEP,
                        NomeAvalista = cliente.Resp_Nome,
                        CpfCnpjAvalista = cliente.Resp_CPF
                    });
                }

                return Ok(sacados.ToList());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

    }
}
