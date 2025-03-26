using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyncOS_API.Migrations
{
    public partial class CedenteCarteira : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Atualizacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataVersao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Versao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CNPJ = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LeiaMe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Script = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pacote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PathDescompactar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoAtualizacaoId = table.Column<int>(type: "int", nullable: false),
                    Backup = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ativo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atualizacoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cedentes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CpfCnpj = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Logradouro = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Numero = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Complemento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bairro = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cidade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CEP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UF = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogoCedente = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cedentes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CentroReceitas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CentroReceitas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    RevendaId = table.Column<int>(type: "int", nullable: false),
                    Razao = table.Column<string>(type: "varchar(255)", nullable: true),
                    Fantasia = table.Column<string>(type: "varchar(50)", nullable: true),
                    CPF_CNPJ = table.Column<string>(type: "varchar(14)", nullable: true),
                    Endereco = table.Column<string>(type: "varchar(255)", nullable: true),
                    Bairro = table.Column<string>(type: "varchar(255)", nullable: true),
                    CEP = table.Column<string>(type: "varchar(10)", nullable: true),
                    UFId = table.Column<string>(type: "Char(2)", nullable: true),
                    Numero = table.Column<string>(type: "varchar(10)", nullable: true),
                    Bloqueado = table.Column<bool>(type: "bit", nullable: false),
                    Desativado = table.Column<bool>(type: "bit", nullable: false),
                    ValorContrato = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ValorContratoBackup = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    UltimaAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataInclusao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MunicipioId = table.Column<int>(type: "int", nullable: false),
                    LatitudeLongitude = table.Column<string>(type: "varchar(100)", nullable: true),
                    InscricaoEstadual = table.Column<string>(type: "varchar(20)", nullable: true),
                    ChaveFlexDocs = table.Column<string>(type: "varchar(255)", nullable: true),
                    ChaveCSC = table.Column<string>(type: "varchar(255)", nullable: true),
                    ChaveManifesto = table.Column<string>(type: "varchar(255)", nullable: true),
                    WhatsApp = table.Column<string>(type: "varchar(20)", nullable: true),
                    Email = table.Column<string>(type: "varchar(255)", nullable: true),
                    Resp_Nome = table.Column<string>(type: "varchar(255)", nullable: true),
                    Resp_Endereco = table.Column<string>(type: "varchar(255)", nullable: true),
                    Resp_Numero = table.Column<string>(type: "varchar(10)", nullable: true),
                    Resp_UF = table.Column<string>(type: "Char(2)", nullable: true),
                    Resp_Cidade = table.Column<string>(type: "varchar(255)", nullable: true),
                    Resp_CEP = table.Column<string>(type: "varchar(10)", nullable: true),
                    Resp_CPF = table.Column<string>(type: "varchar(11)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Generators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UltimoID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Generators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Retornos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroDocumento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataPagamento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataOcorrencia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Baixa = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CodigoOcorrencia = table.Column<int>(type: "int", nullable: false),
                    BoletoId = table.Column<int>(type: "int", nullable: false),
                    SacadoId = table.Column<int>(type: "int", nullable: false),
                    ValorPago = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Retornos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Revendas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "varchar(255)", nullable: false),
                    CNPJ = table.Column<string>(type: "varchar(14)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Revendas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sacados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fantasia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CpfCnpj = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Logradouro = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Numero = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Complemento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bairro = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cidade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CEP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UF = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsBloqueado = table.Column<bool>(type: "bit", nullable: true),
                    IsDesativado = table.Column<bool>(type: "bit", nullable: true),
                    CentroReceitaId = table.Column<int>(type: "int", nullable: true),
                    ValorContrato = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sacados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatusTitulo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusTitulo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TiposAPP",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposAPP", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UFs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "Char(2)", nullable: false),
                    Descricao = table.Column<string>(type: "varchar(255)", nullable: false),
                    Codigo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UFs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoginName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoginPass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PerfilId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Carteiras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DVConvenio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoCarteira = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Variacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Agencia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DVAgencia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Banco = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DVBanco = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Conta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DVConta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CedenteId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carteiras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carteiras_Cedentes_CedenteId",
                        column: x => x.CedenteId,
                        principalTable: "Cedentes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Municipios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    UFId = table.Column<string>(type: "Char(2)", nullable: true),
                    Descricao = table.Column<string>(type: "varchar(255)", nullable: true),
                    Ordem = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Municipios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Municipios_UFs_UFId",
                        column: x => x.UFId,
                        principalTable: "UFs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Boletos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LinhaDigitavel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroDocumento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodigoBarras = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NossoNumero = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Emissao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Vencimento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Baixa = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Pagamento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Valor = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PercentualMulta = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    MoraDia = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ValorPago = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    InstrucoesLinha1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstrucoesLinha2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Doc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cancelado = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    Base64Pdf = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    SacadoId = table.Column<int>(type: "int", nullable: false),
                    StatusTituloId = table.Column<int>(type: "int", nullable: false),
                    CarteiraId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    RemessaId = table.Column<int>(type: "int", nullable: false),
                    Pagador_Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status_Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Carteira_Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boletos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Boletos_Carteiras_CarteiraId",
                        column: x => x.CarteiraId,
                        principalTable: "Carteiras",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Boletos_Sacados_SacadoId",
                        column: x => x.SacadoId,
                        principalTable: "Sacados",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Boletos_StatusTitulo_StatusTituloId",
                        column: x => x.StatusTituloId,
                        principalTable: "StatusTitulo",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Boletos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Boletos_CarteiraId",
                table: "Boletos",
                column: "CarteiraId");

            migrationBuilder.CreateIndex(
                name: "IX_Boletos_SacadoId",
                table: "Boletos",
                column: "SacadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Boletos_StatusTituloId",
                table: "Boletos",
                column: "StatusTituloId");

            migrationBuilder.CreateIndex(
                name: "IX_Boletos_UsuarioId",
                table: "Boletos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Carteiras_CedenteId",
                table: "Carteiras",
                column: "CedenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Municipios_UFId",
                table: "Municipios",
                column: "UFId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Atualizacoes");

            migrationBuilder.DropTable(
                name: "Boletos");

            migrationBuilder.DropTable(
                name: "CentroReceitas");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Generators");

            migrationBuilder.DropTable(
                name: "Municipios");

            migrationBuilder.DropTable(
                name: "Retornos");

            migrationBuilder.DropTable(
                name: "Revendas");

            migrationBuilder.DropTable(
                name: "TiposAPP");

            migrationBuilder.DropTable(
                name: "Carteiras");

            migrationBuilder.DropTable(
                name: "Sacados");

            migrationBuilder.DropTable(
                name: "StatusTitulo");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "UFs");

            migrationBuilder.DropTable(
                name: "Cedentes");
        }
    }
}
