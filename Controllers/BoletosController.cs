//using info;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Modelos;

//namespace SyncOS_API.Controllers
//{
//    [AllowAnonymous]
//    [ApiController]
//    [Route("[controller]")]
//    public class BoletosController : ControllerBase
//    {
//        private readonly APPDbContext _ctx;

//        public BoletosController(APPDbContext context)
//        {
//            _ctx = context;
//        }

//        [HttpGet("evento/{inicio}/{fim}/{carteiraId}/{tipoData}")]
//        public async Task<ActionResult<IList<ListaEventos>>> PorPeriodo(string inicio, string fim, int carteiraId, string tipoData)
//        {
//            try
//            {
//                inicio = inicio.Substring(0, 10) + " 00:00:00";
//                fim = fim.Substring(0, 10) + " 23:59:59";
//            }
//            catch (Exception)
//            {
//            }

//                var query = string.Empty;
//                var where = string.Empty;
//                query = @"select b.Id, b.NumeroDocumento, c.Razao as NomePagador, b.Vencimento, b.Pagamento, b.Valor, b.ValorPago,
//                            CASE WHEN b.ValorPago > 0  THEN b.ValorPago - b.Valor ELSE 0 END as Oscilacao,  b.Doc, cc.Descricao as Carteira,
//                            e.Data, e.Arquivo, t.descricao as StatusEvento, e.TipoEvento
//                            from Boletos b 
//                            inner join  synctech.BoletoEventos e on e.BoletoId = b.Id
//                            inner join StatusTitulos t on t.Id = e.StatusId
//                            inner join Clientes c on c.Id = b.SacadoId
//                            inner join Carteiras cc on cc.Id = b.CarteiraId ";

//                if (tipoData == "E")
//                {
//                    where = @"where b.CarteiraId = @carteiraId and b.Emissao >= @inicio and b.Emissao <= @fim and Cancelado <> 'S'";
//                }
//                else if (tipoData == "V")
//                {
//                    where = @"where b.CarteiraId = @carteiraId and b.vencimento >= @inicio and b.vencimento <= @fim and Cancelado<> 'S'";
//                }
//                else
//                {
//                    where = @"where b.CarteiraId = @carteiraId and e.Data >= @inicio and e.Data <= @fim and Cancelado<> 'S'";
//                }

//                query += where;


//                var eventos =  _ctx.ListaEventos.FromSqlRaw(query,
//                    new
//                    {
//                        CarteiraId = carteiraId,
//                        Inicio = inicio,
//                        Fim = fim
//                    });
//            return await eventos.ToListAsync();
//        }



//        [HttpPost("evento")]
//        public async Task<ActionResult<BoletoEvento>> PostEvento([FromBody] BoletoEvento evento) 
//        {
//            try
//            {
//                var ret = await _ctx.BoletoEventos.AddAsync(evento);
//                await _ctx.SaveChangesAsync();
//                return Ok(evento);
//            }
//            catch (Exception)
//            {
//                return BadRequest();
//            }
//        }

//        [HttpGet("{carteiraId}/{inicio}/{fim}/{centroReceitaId}/{doc}")]
//        public async Task<ActionResult<List<BoletoReport>>> ExtratoMovimentacao(
//            int carteiraId,
//            string inicio,
//            string fim,
//            int centroReceitaId,
//            string doc)
//        {
//            try
//            {
//                var dInicio = DateTime.Parse(inicio);
//                var dFim = DateTime.Parse(fim);

//                IQueryable<BoletoReport> boletos;

//                if (centroReceitaId > 0)
//                {
//                    boletos = from b in _ctx.Boletos
//                              join c in _ctx.Clientes on b.SacadoId equals c.Id
//                              join d in _ctx.Carteiras on b.CarteiraId equals d.Id
//                              where b.CarteiraId == carteiraId
//                                    && b.Cancelado != "S"
//                                    && c.RevendaId == centroReceitaId
//                                    && (b.Pagamento >= dInicio && b.Pagamento <= dFim)
//                              select new BoletoReport
//                              {
//                                  Id = b.Id,
//                                  NumeroDocumento = b.NumeroDocumento,
//                                  NomePagador = c.Razao,
//                                  Vencimento = b.Vencimento,
//                                  Pagamento = b.Pagamento,
//                                  Valor = b.Valor,
//                                  Oscilacao = b.ValorPago - b.Valor,
//                                  ValorPago = b.ValorPago,
//                                  Doc = b.Doc,
//                                  Carteira = d.Descricao,
//                                  ValorSped = 0,
//                                  ValorTef = 0,
//                                  ValorBackup = 0,
//                                  ValorManifesto = 0,
//                                  ValorComissao = 0
//                              };
//                }
//                else
//                {
//                    boletos = from b in _ctx.Boletos
//                              join c in _ctx.Clientes on b.SacadoId equals c.Id
//                              join d in _ctx.Carteiras on b.CarteiraId equals d.Id
//                              where b.CarteiraId == carteiraId
//                                    && b.Cancelado != "S"
//                                    && (b.Pagamento >= dInicio && b.Pagamento <= dFim)
//                              select new BoletoReport
//                              {
//                                  Id = b.Id,
//                                  NumeroDocumento = b.NumeroDocumento,
//                                  NomePagador = c.Razao,
//                                  Vencimento = b.Vencimento,
//                                  Pagamento = b.Pagamento,
//                                  Valor = b.Valor,
//                                  Oscilacao = b.ValorPago - b.Valor,
//                                  ValorPago = b.ValorPago,
//                                  Doc = b.Doc,
//                                  Carteira = d.Descricao,
//                                  ValorSped = 0,
//                                  ValorTef = 0,
//                                  ValorBackup = 0,
//                                  ValorManifesto = 0,
//                                  ValorComissao = 0
//                              };
//                }

//                if (!doc.Equals("-"))
//                {
//                    return Ok(await boletos.Where(w => w.Doc == doc).ToListAsync());
//                }
//                else
//                {
//                    return Ok(await boletos.ToListAsync());
//                }
//            }
//            catch (Exception)
//            {
//                return BadRequest();
//            }
//        }

//        [HttpGet("{carteiraId}/{sacadoId}/{cpfCnpj}/{tipoData}/{inicio}/{fim}/{statusId}/{centroReceitaId}/{numeroDocumento}/{vendedor}")]
//        public async Task<ActionResult<IList<BoletoReport>>> PlanilhaPagamento(int carteiraId, int sacadoId, string cpfCnpj, string tipoData,
//            DateTime inicio, DateTime fim, int statusId, int centroReceitaId, string numeroDocumento, string vendedor,
//            string doc)
//        {
//            try
//            {
//                // Assuming you have DbSet properties for these tables in your DbContext
//                var query = from b in _ctx.Boletos
//                            join c in _ctx.Clientes on b.SacadoId equals c.Id
//                            join cc in _ctx.Carteiras on b.CarteiraId equals cc.Id
//                            join s in _ctx.StatusTitulo on b.StatusId equals s.Id
//                            where b.CarteiraId == carteiraId && b.Cancelado != "S"
//                            select new BoletoReport
//                            {
//                                Id = b.Id,
//                                NumeroDocumento = b.NumeroDocumento,
//                                NomePagador = c.Fantasia,
//                                CpfCnpj = c.CPF_CNPJ,
//                                Emissao = b.Emissao,
//                                Vencimento = b.Vencimento,
//                                Pagamento = b.Pagamento,
//                                Valor = b.Valor,
//                                Oscilacao = b.ValorPago - b.Valor,
//                                ValorPago = b.ValorPago,
//                                StatusId = s.Id,
//                                Vendedor = c.Vendedor,
//                                Status = s.Descricao,
//                                Doc = b.Doc,
//                                Carteira = cc.Descricao,
//                                ValorSped = decimal.Parse(c.ValorSped.ToString()),
//                                ValorTef = decimal.Parse(c.ValorTef.ToString()),
//                                ValorBackup = decimal.Parse(c.ValorContratoBackup.ToString()),
//                                ValorManifesto = decimal.Parse(c.ValorManifesto.ToString()),
//                                ValorComissao = decimal.Parse(c.ValorComissao.ToString()),
//                            };

//                // Apply conditional filters
//                if (sacadoId != 0)
//                    query = query.Where(b => b.Id == sacadoId);

//                if (cpfCnpj != "-")
//                    query = query.Where(b => _ctx.Clientes.Where(c => c.CPF_CNPJ == cpfCnpj).Select(c => c.Id).Contains(b.SacadoId));

//                if (tipoData == "Emissao")
//                {
//                    var fimAdjusted = fim.AddDays(1);
//                    query = query.Where(b => b.Emissao >= inicio && b.Emissao < fimAdjusted);
//                }
//                else if (tipoData == "Vencimento")
//                    query = query.Where(b => b.Vencimento >= inicio && b.Vencimento <= fim);
//                else if (tipoData == "Pagamento")
//                    query = query.Where(b => b.Pagamento >= inicio && b.Pagamento <= fim);

//                if (statusId != 0)
//                    query = query.Where(b => b.StatusId == statusId);

//                if (centroReceitaId != 0)
//                    query = query.Where(b => _ctx.Clientes.Where(c => c.RevendaId == centroReceitaId).Select(c => c.Id).Contains(b.SacadoId));

//                if (numeroDocumento != "-")
//                    query = query.Where(b => b.NumeroDocumento!.Contains(numeroDocumento));

//                if (vendedor != "-")
//                    query = query.Where(b => b.Vendedor == vendedor);

//                if (doc != "-")
//                    query = query.Where(b => b.Doc == doc);

//                return await query.ToListAsync();
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }


//        [HttpGet("{carteiraId}/{sacadoId}/{cpfCnpj}/{tipoData}/{inicio}/{fim}/{statusId}/{centroReceitaId}/{numeroDocumento}")]
//        public async Task<ActionResult<IList<RelacaoBoletos>>> Filtrar(int carteiraId, int sacadoId, string cpfCnpj, string tipoData,
//        DateTime inicio, DateTime fim, int statusId, int centroReceitaId, string numeroDocumento)
//        {
//            try { 
//                var query = @"select 
//                                b.Id
//                                , c.Razao as NomePagador
//                                , c.CPF_CNPJ as CpfCnpj
//                                , b.Emissao
//                                , b.Vencimento
//                                , b.Valor
//                                , b.PercentualMulta
//                                , b.MoraDia
//                                , b.baixa
//                                , b.Pagamento
//                                , b.ValorPago
//                                , b.NumeroDocumento
//                                , b.NossoNumero
//                                , s.Descricao as Status_Descricao
//                                , cc.Descricao as Carteira_Descricao
//                                , b.Doc
//                                , b.SacadoId 
//                                , b.StatusId
//                                , b.CarteiraId 
//                                , c.RevendaId as CentroReceitaId 
//                              from Boletos b 
//                            inner join Clientes c on c.Id = b.SacadoId
//                            inner join Carteiras cc on cc.Id = b.CarteiraId 
//                            inner join StatusTitulo s on s.Id = b.StatusId
//                            where  Cancelado <> 'S' ";
//                if (carteiraId != 0)
//                    query += string.Format(" and cc.Id = {0} ", carteiraId);
//                if (sacadoId != 0)
//                    query += string.Format("and SacadoId = {0} ", sacadoId);
//                if (cpfCnpj != "-")
//                    query += string.Format("and SacadoId in (select Id from Clientes where CPF_CNPJ = '{0}') ", cpfCnpj);
//                if (tipoData == "Emissao")
//                {
//                    fim = fim.AddDays(1);
//                    query += string.Format("and Emissao >= '{0}' and Emissao < '{1}' ", inicio.ToString("yyyy-MM-dd"), fim.ToString("yyyy-MM-dd"));
//                }
//                else if (tipoData == "Vencimento")
//                    query += string.Format("and Vencimento >= '{0}' and Vencimento < '{1}' ", inicio.ToString("yyyy-MM-dd"), fim.ToString("yyyy-MM-dd"));
//                else if (tipoData == "Pagamento")
//                    query += string.Format("and Pagamento >= '{0}' and Pagamento < '{1}' ", inicio.ToString("yyyy-MM-dd"), fim.ToString("yyyy-MM-dd"));
//                if (statusId != 0)
//                    query += string.Format("and StatusId = {0} ", statusId);
//                if (centroReceitaId != 0)
//                    query += string.Format("and SacadoId in (select Id from Clientes where RevendaId = {0}) ", centroReceitaId);
//                if (numeroDocumento != "-")
//                    query += "and NumeroDocumento like @numeroDocumento ";

//                var lista = _ctx.RelacaoBoletos.FromSqlRaw(query);
//                var listaPopulada = await lista.ToListAsync();
//                return listaPopulada; ;
//            }
//            catch (Exception e)
//            {
//                return new List<RelacaoBoletos>();
//            }
//        }


//        [HttpGet]
//        public async Task<ActionResult<List<Boleto>>> GetAll()
//        {
//            try
//            {
//                return  Ok(await _ctx.CentroReceitas.ToListAsync());
//            }
//            catch (Exception)
//            {
//                return BadRequest();
//            }
//        }


//        [HttpGet("{id}")]
//        public async Task<ActionResult<Boleto>> PorId(int id)
//        {
//            try
//            {
//                return Ok(await _ctx.Boletos.FindAsync(id));
//            }
//            catch (Exception e)
//            {
//                return BadRequest();
//            }
//        }

//        [HttpGet("pornumerodocumento/{id}")]
//        public async Task<ActionResult<Boleto>> PorNumeroDoc(string id)
//        {
//            try
//            {
//                return Ok(await _ctx.Boletos.FirstOrDefaultAsync(f => f.NumeroDocumento == id));
//            }
//            catch (Exception e)
//            {
//                return BadRequest();
//            }
//        }




//        [HttpPost]
//        public async Task<ActionResult<Boleto>> Post([FromBody] Boleto boleto)
//        {
//            try
//            {
//                boleto.Cancelado = "N";
//                var ret = await _ctx.Boletos.AddAsync(boleto);
//                await _ctx.SaveChangesAsync();
//                return Ok(boleto);
//            }
//            catch (Exception)
//            {
//                return BadRequest();
//            }
//        }

//        [HttpPut]
//        public async Task<ActionResult<Boleto>> Put([FromBody] Boleto boleto)
//        {
//            try
//            {
//                var bol = await _ctx.Boletos.FindAsync(boleto.Id);
//                if (bol == null)
//                {
//                    return BadRequest();
//                }
//                else
//                {
//                    bol = boleto;
//                    _ctx.Update(bol);
//                    await _ctx.SaveChangesAsync();
//                    return Ok(boleto);
//                }    
//            }
//            catch (Exception)
//            {
//                return BadRequest();
//            }
//        }



//        [HttpPut("cancelar/{id}")]
//        public async Task<ActionResult<bool>> Cancelar(int id)
//        {
//            try
//            {
//                var boleto = await _ctx.Boletos.FindAsync(id);
//                if (boleto == null)
//                {
//                    return BadRequest();
//                }
//                else
//                {
//                    boleto.Cancelado = "S";
//                    _ctx.Update(boleto);
//                   await _ctx.SaveChangesAsync();
//                    return Ok(true);
//                }
//            }
//            catch (Exception)
//            {
//                return BadRequest(false);
//            }
//        }


//        [HttpPut("atualizarstatus/retorno")]
//        public async Task<ActionResult<Boleto>> AtualizarStatus([FromBody] Retorno retorno)
//        {
//            try
//            {
//                var ret = await _ctx.Boletos.FirstOrDefaultAsync(f => f.NumeroDocumento == retorno.NumeroDocumento) ?? new Boleto();
//                ret.ValorPago = retorno.ValorPago;
//                ret.Baixa = retorno.Baixa;
//                ret.StatusId = retorno.CodigoOcorrencia;
//                ret.Pagamento = retorno.DataPagamento;  
//                _ctx.Update(ret);
//                await _ctx.SaveChangesAsync();
//                return Ok(ret);
//            }
//            catch (Exception)
//            {
//                return BadRequest();
//            }
//        }

//        [HttpPut("AtualizarStatus/boleto")]
//        public async Task<ActionResult<Boleto>> AtualizarStatus([FromBody] Boleto boleto)
//        {
//            try
//            {
//                var ret = await _ctx.Boletos.FirstOrDefaultAsync(f => f.NumeroDocumento == boleto.NumeroDocumento) ?? new Boleto();
//                ret.StatusId = boleto.StatusId;
//                _ctx.Update(ret);
//                await _ctx.SaveChangesAsync();
//                return Ok(ret);
//            }
//            catch (Exception)
//            {
//                return BadRequest();
//            }
//        }
//    }
//}

using info;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace SyncOS_API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class BoletosController : ControllerBase
    {
        private readonly APPDbContext _ctx;

        public BoletosController(APPDbContext context)
        {
            _ctx = context;
        }

        [HttpGet("evento/{inicio}/{fim}/{carteiraId}/{tipoData}")]
        public async Task<ActionResult<IList<ListaEventos>>> PorPeriodo(string inicio, string fim, int carteiraId, string tipoData)
        {
            try
            {
                inicio = inicio.Substring(0, 10) + " 00:00:00";
                fim = fim.Substring(0, 10) + " 23:59:59";
            }
            catch (Exception)
            {
            }

            var query = string.Empty;
            var where = string.Empty;
            query = @"select b.Id, b.NumeroDocumento, c.Razao as NomePagador, b.Vencimento, b.Pagamento, b.Valor, b.ValorPago,
                            CASE WHEN b.ValorPago > 0  THEN b.ValorPago - b.Valor ELSE 0 END as Oscilacao,  b.Doc, cc.Descricao as Carteira,
                            e.Data, e.Arquivo, t.descricao as StatusEvento, e.TipoEvento
                            from Boletos b 
                            inner join  synctech.BoletoEventos e on e.BoletoId = b.Id
                            inner join StatusTitulos t on t.Id = e.StatusId
                            inner join Clientes c on c.Id = b.SacadoId
                            inner join Carteiras cc on cc.Id = b.CarteiraId ";

            if (tipoData == "E")
            {
                where = @"where b.CarteiraId = @carteiraId and b.Emissao >= @inicio and b.Emissao <= @fim and Cancelado <> 'S'";
            }
            else if (tipoData == "V")
            {
                where = @"where b.CarteiraId = @carteiraId and b.vencimento >= @inicio and b.vencimento <= @fim and Cancelado<> 'S'";
            }
            else
            {
                where = @"where b.CarteiraId = @carteiraId and e.Data >= @inicio and e.Data <= @fim and Cancelado<> 'S'";
            }

            query += where;


            var eventos = _ctx.ListaEventos.FromSqlRaw(query,
                new
                {
                    CarteiraId = carteiraId,
                    Inicio = inicio,
                    Fim = fim
                });
            return await eventos.ToListAsync();
        }



        [HttpPost("evento")]
        public async Task<ActionResult<BoletoEvento>> PostEvento([FromBody] BoletoEvento evento)
        {
            try
            {
                var ret = await _ctx.BoletoEventos.AddAsync(evento);
                await _ctx.SaveChangesAsync();
                return Ok(evento);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{carteiraId}/{sacadoId}/{cpfCnpj}/{tipoData}/{inicio}/{fim}/{statusId}/{centroReceitaId}")]
        //public async Task<ActionResult<IList<RelacaoBoletos>>> Filtrar(int carteiraId, int sacadoId, string cpfCnpj, string tipoData,
        //DateTime inicio, DateTime fim, int statusId, int centroReceitaId)
        public async Task<ActionResult> Filtrar(int carteiraId, int sacadoId, string cpfCnpj, string tipoData,
        DateTime inicio, DateTime fim, int statusId, int centroReceitaId)
        {
            try
            {
                var query = @"select 
                                b.Id
                                , c.Razao as Pagador_Nome
                                , c.CPF_CNPJ as PagadorCpfCnpj
                                , b.Emissao
                                , b.Vencimento
                                , b.Valor
                                , b.PercentualMulta
                                , b.MoraDia
                                , b.baixa
                                , b.Pagamento
                                , b.ValorPago
                                , b.NumeroDocumento
                                , b.NossoNumero
                                , s.Descricao as Status_Descricao
                                , cc.Descricao as Carteira_Descricao
                                , b.Doc
                                , b.SacadoId 
                                , b.StatusId 
                                , b.CarteiraId 
                                , c.RevendaId as CentroReceitaId 
                            from Boletos b 
                            inner join Clientes c on c.Id = b.SacadoId
                            inner join Carteiras cc on cc.Id = b.CarteiraId 
                            inner join StatusTitulo s on s.Id = b.StatusId
                            where  Cancelado <> 'S' ";
                if (carteiraId != 0)
                    query += string.Format(" and cc.Id = {0} ", carteiraId);
                if (sacadoId != 0)
                    query += string.Format("and SacadoId = {0} ", sacadoId);
                if (cpfCnpj != "-")
                    query += string.Format("and SacadoId in (select Id from Clientes where CPF_CNPJ = '{0}') ", cpfCnpj);
                if (tipoData == "Emissao")
                {
                    fim = fim.AddDays(1);
                    query += string.Format("and Emissao >= '{0}' and Emissao <= '{1}' ", inicio.ToString("yyyy-MM-dd"), fim.ToString("yyyy-MM-dd"));
                }
                else if (tipoData == "Vencimento")
                    query += string.Format("and Vencimento >= '{0}' and Vencimento <= '{1}' ", inicio.ToString("yyyy-MM-dd"), fim.ToString("yyyy-MM-dd"));
                else if (tipoData == "Pagamento")
                    query += string.Format("and Pagamento >= '{0}' and Pagamento <= '{1}' ", inicio.ToString("yyyy-MM-dd"), fim.ToString("yyyy-MM-dd"));
                if (statusId != 0)
                    query += string.Format("and StatusId = {0} ", statusId);
                if (centroReceitaId != 0)
                    query += string.Format("and SacadoId in (select Id from Clientes where RevendaId = {0}) ", centroReceitaId);

                var lista = _ctx.RelacaoBoletos.FromSqlRaw(query);

                return Ok(await lista.ToListAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet]
        public async Task<ActionResult<List<Boleto>>> GetAll()
        {
            try
            {
                return Ok(await _ctx.CentroReceitas.ToListAsync());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Boleto>> PorId(int id)
        {
            try
            {
                return Ok(await _ctx.Boletos.FindAsync(id));
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpGet("pornumerodocumento/{id}")]
        public async Task<ActionResult<Boleto>> PorNumeroDoc(string id)
        {
            try
            {
                return Ok(await _ctx.Boletos.FirstOrDefaultAsync(f => f.NumeroDocumento == id));
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpGet("{carteiraId}/{inicio}/{fim}/{centroReceitaId}")]
        public async Task<ActionResult> ObterBoletos(int carteiraId, DateTime inicio, DateTime fim, int centroReceitaId = 0)
        {
            try
            {
                var resultado = _ctx.Boletos
                    .Join(
                        _ctx.Clientes,
                        boleto => boleto.SacadoId, // Chave de junção em Boletos
                        cliente => cliente.Id,     // Chave de junção em Clientes
                        (boleto, cliente) => new { boleto, cliente } // Resultado intermediário
                    )
                    .Join(
                        _ctx.Revendas,
                        temp => temp.cliente.RevendaId, // Chave de junção em Clientes
                        revenda => revenda.Id,         // Chave de junção em Revendas
                        (temp, revenda) => new { temp.boleto, temp.cliente, revenda } // Resultado final com todas as entidades
                    )
                    .Where(x => x.boleto.CarteiraId == carteiraId &&
                                x.boleto.Vencimento >= inicio &&
                                x.boleto.Vencimento <= fim &&
                                x.boleto.Cancelado != "S" &&
                                x.boleto.StatusId == 2)
                    .OrderBy(x => x.cliente.Razao) // Ordena pelo nome do cliente (Razao)
                    .Select(x => new
                    {
                        Id = x.boleto.Id,
                        NumeroDocumento = x.boleto.NumeroDocumento,
                        Vencimento = x.boleto.Vencimento,
                        Valor = x.boleto.Valor,
                        Doc = x.boleto.Doc,
                        ClienteId = x.cliente.Id,
                        Nome = x.cliente.Razao,
                        CpfCnpj = x.cliente.CPF_CNPJ,
                        RevendaId = x.revenda.Id,
                        Descricao = x.revenda.Descricao
                    })
                    ; // Retorna como uma lista de objetos dinâmicos

                return Ok(await resultado.ToListAsync<dynamic>());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        [HttpGet("extratoMovimentacao/{carteiraId}/{inicio}/{fim}/{centroReceitaId}/{doc}")]
        public async Task<ActionResult<List<BoletoReport>>> ExtratoMovimentacao(
            int carteiraId,
            string inicio,
            string fim,
            int centroReceitaId,
            string doc)
        {
            try
            {
                var dInicio = DateTime.Parse(inicio);
                var dFim = DateTime.Parse(fim);

                IQueryable<BoletoReport> boletos;

                if (centroReceitaId > 0)
                {
                    boletos = from b in _ctx.Boletos
                              join c in _ctx.Clientes on b.SacadoId equals c.Id
                              join d in _ctx.Carteiras on b.CarteiraId equals d.Id
                              where b.CarteiraId == carteiraId
                                    && b.Cancelado != "S"
                                    && c.RevendaId == centroReceitaId
                                    && (b.Pagamento >= dInicio && b.Pagamento <= dFim)
                              select new BoletoReport
                              {
                                  Id = b.Id,
                                  NumeroDocumento = b.NumeroDocumento,
                                  NomePagador = c.Razao,
                                  Vencimento = b.Vencimento,
                                  Pagamento = b.Pagamento,
                                  Valor = b.Valor,
                                  Oscilacao = b.ValorPago - b.Valor,
                                  ValorPago = b.ValorPago,
                                  Doc = b.Doc,
                                  Carteira = d.Descricao
                              };
                }
                else
                {
                    boletos = from b in _ctx.Boletos
                              join c in _ctx.Clientes on b.SacadoId equals c.Id
                              join d in _ctx.Carteiras on b.CarteiraId equals d.Id
                              where b.CarteiraId == carteiraId
                                    && b.Cancelado != "S"
                                    && (b.Pagamento >= dInicio && b.Pagamento <= dFim)
                              select new BoletoReport
                              {
                                  Id = b.Id,
                                  NumeroDocumento = b.NumeroDocumento,
                                  NomePagador = c.Razao,
                                  Vencimento = b.Vencimento,
                                  Pagamento = b.Pagamento,
                                  Valor = b.Valor,
                                  Oscilacao = b.ValorPago - b.Valor,
                                  ValorPago = b.ValorPago,
                                  Doc = b.Doc,
                                  Carteira = d.Descricao
                              };
                }

                boletos = (!string.IsNullOrEmpty(doc) ? boletos.Where(w => w.Doc == doc) : boletos);
                return Ok(await boletos.ToListAsync());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpPost]
        public async Task<ActionResult<Boleto>> Post([FromBody] Boleto boleto)
        {
            try
            {
                boleto.Cancelado = "N";
                var ret = await _ctx.Boletos.AddAsync(boleto);
                await _ctx.SaveChangesAsync();
                return Ok(boleto);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<ActionResult<Boleto>> Put([FromBody] Boleto boleto)
        {
            try
            {
                var bol = await _ctx.Boletos.FindAsync(boleto.Id);
                if (bol == null)
                {
                    return BadRequest();
                }
                else
                {
                    bol = boleto;
                    _ctx.Update(bol);
                    await _ctx.SaveChangesAsync();
                    return Ok(boleto);
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }



        [HttpPut("cancelar/{id}")]
        public async Task<ActionResult<bool>> Cancelar(int id)
        {
            try
            {
                var boleto = await _ctx.Boletos.FindAsync(id);
                if (boleto == null)
                {
                    return BadRequest();
                }
                else
                {
                    boleto.Cancelado = "S";
                    _ctx.Update(boleto);
                    await _ctx.SaveChangesAsync();
                    return Ok(true);
                }
            }
            catch (Exception)
            {
                return BadRequest(false);
            }
        }


        [HttpPut("atualizarstatus/retorno")]
        public async Task<ActionResult<Boleto>> AtualizarStatus([FromBody] Retorno retorno)
        {
            try
            {
                var ret = await _ctx.Boletos.FirstOrDefaultAsync(f => f.NumeroDocumento == retorno.NumeroDocumento) ?? new Boleto();
                ret.ValorPago = retorno.ValorPago;
                ret.Baixa = retorno.Baixa;
                ret.StatusId = retorno.CodigoOcorrencia;
                ret.Pagamento = retorno.DataPagamento;
                _ctx.Update(ret);
                await _ctx.SaveChangesAsync();
                return Ok(ret);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut("AtualizarStatus/boleto")]
        public async Task<ActionResult<Boleto>> AtualizarStatus([FromBody] Boleto boleto)
        {
            try
            {
                var ret = await _ctx.Boletos.FirstOrDefaultAsync(f => f.NumeroDocumento == boleto.NumeroDocumento) ?? new Boleto();
                ret.StatusId = boleto.StatusId;
                _ctx.Update(ret);
                await _ctx.SaveChangesAsync();
                return Ok(ret);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}

