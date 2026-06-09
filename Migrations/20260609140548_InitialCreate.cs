using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GlobalSolution.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_LOCALIZACAO",
                columns: table => new
                {
                    ID_LOCALIZACAO = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NM_CIDADE = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    SG_ESTADO = table.Column<string>(type: "NVARCHAR2(2)", maxLength: 2, nullable: false),
                    NM_PAIS = table.Column<string>(type: "NVARCHAR2(80)", maxLength: 80, nullable: false),
                    NR_LATITUDE = table.Column<decimal>(type: "FLOAT(38)", nullable: true),
                    NR_LONGITUDE = table.Column<decimal>(type: "FLOAT(38)", nullable: true),
                    NR_CEP = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_LOCALIZACAO", x => x.ID_LOCALIZACAO);
                });

            migrationBuilder.CreateTable(
                name: "TB_USUARIO",
                columns: table => new
                {
                    ID_USUARIO = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NM_USUARIO = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    DS_EMAIL = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    DS_SENHA = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    ST_ATIVO = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    DT_CRIACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_USUARIO", x => x.ID_USUARIO);
                });

            migrationBuilder.CreateTable(
                name: "TB_PROPRIEDADE",
                columns: table => new
                {
                    ID_PROPRIEDADE = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NM_PROPRIEDADE = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    NR_AREA_TOTAL = table.Column<decimal>(type: "FLOAT(38)", nullable: false),
                    TP_SOLO = table.Column<string>(type: "NVARCHAR2(80)", maxLength: 80, nullable: true),
                    ID_USUARIO = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    ID_LOCALIZACAO = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PROPRIEDADE", x => x.ID_PROPRIEDADE);
                    table.ForeignKey(
                        name: "FK_TB_PROPRIEDADE_TB_LOCALIZACAO_ID_LOCALIZACAO",
                        column: x => x.ID_LOCALIZACAO,
                        principalTable: "TB_LOCALIZACAO",
                        principalColumn: "ID_LOCALIZACAO",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TB_PROPRIEDADE_TB_USUARIO_ID_USUARIO",
                        column: x => x.ID_USUARIO,
                        principalTable: "TB_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TB_PLANTACAO",
                columns: table => new
                {
                    ID_PLANTACAO = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NM_PLANTACAO = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    DS_CULTURA = table.Column<string>(type: "NVARCHAR2(80)", maxLength: 80, nullable: false),
                    NR_AREA_HECTARES = table.Column<decimal>(type: "FLOAT(38)", nullable: false),
                    DS_STATUS = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    ID_PROPRIEDADE = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PLANTACAO", x => x.ID_PLANTACAO);
                    table.ForeignKey(
                        name: "FK_TB_PLANTACAO_TB_PROPRIEDADE_ID_PROPRIEDADE",
                        column: x => x.ID_PROPRIEDADE,
                        principalTable: "TB_PROPRIEDADE",
                        principalColumn: "ID_PROPRIEDADE",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TB_SENSOR",
                columns: table => new
                {
                    ID_SENSOR = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NM_SENSOR = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    TP_SENSOR = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    ST_ATIVO = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    DT_INSTALACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    ID_PLANTACAO = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_SENSOR", x => x.ID_SENSOR);
                    table.ForeignKey(
                        name: "FK_TB_SENSOR_TB_PLANTACAO_ID_PLANTACAO",
                        column: x => x.ID_PLANTACAO,
                        principalTable: "TB_PLANTACAO",
                        principalColumn: "ID_PLANTACAO",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TB_LEITURA",
                columns: table => new
                {
                    ID_LEITURA = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NR_TEMPERATURA = table.Column<decimal>(type: "FLOAT(38)", nullable: true),
                    NR_UMIDADE = table.Column<decimal>(type: "FLOAT(38)", nullable: true),
                    NR_PRECIPITACAO = table.Column<decimal>(type: "FLOAT(38)", nullable: true),
                    NR_NDVI = table.Column<decimal>(type: "FLOAT(38)", nullable: true),
                    DT_LEITURA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    ID_SENSOR = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_LEITURA", x => x.ID_LEITURA);
                    table.ForeignKey(
                        name: "FK_TB_LEITURA_TB_SENSOR_ID_SENSOR",
                        column: x => x.ID_SENSOR,
                        principalTable: "TB_SENSOR",
                        principalColumn: "ID_SENSOR",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TB_ALERTA",
                columns: table => new
                {
                    ID_ALERTA = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TP_ALERTA = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    DS_SEVERIDADE = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    DS_MENSAGEM = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    DS_STATUS = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    DT_CRIACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    ID_LEITURA = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    ID_PLANTACAO = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ALERTA", x => x.ID_ALERTA);
                    table.ForeignKey(
                        name: "FK_TB_ALERTA_TB_LEITURA_ID_LEITURA",
                        column: x => x.ID_LEITURA,
                        principalTable: "TB_LEITURA",
                        principalColumn: "ID_LEITURA",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TB_ALERTA_TB_PLANTACAO_ID_PLANTACAO",
                        column: x => x.ID_PLANTACAO,
                        principalTable: "TB_PLANTACAO",
                        principalColumn: "ID_PLANTACAO",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_ALERTA_ID_LEITURA",
                table: "TB_ALERTA",
                column: "ID_LEITURA");

            migrationBuilder.CreateIndex(
                name: "IX_TB_ALERTA_ID_PLANTACAO",
                table: "TB_ALERTA",
                column: "ID_PLANTACAO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_LEITURA_ID_SENSOR",
                table: "TB_LEITURA",
                column: "ID_SENSOR");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PLANTACAO_ID_PROPRIEDADE",
                table: "TB_PLANTACAO",
                column: "ID_PROPRIEDADE");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PROPRIEDADE_ID_LOCALIZACAO",
                table: "TB_PROPRIEDADE",
                column: "ID_LOCALIZACAO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PROPRIEDADE_ID_USUARIO",
                table: "TB_PROPRIEDADE",
                column: "ID_USUARIO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_SENSOR_ID_PLANTACAO",
                table: "TB_SENSOR",
                column: "ID_PLANTACAO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_USUARIO_DS_EMAIL",
                table: "TB_USUARIO",
                column: "DS_EMAIL",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_ALERTA");

            migrationBuilder.DropTable(
                name: "TB_LEITURA");

            migrationBuilder.DropTable(
                name: "TB_SENSOR");

            migrationBuilder.DropTable(
                name: "TB_PLANTACAO");

            migrationBuilder.DropTable(
                name: "TB_PROPRIEDADE");

            migrationBuilder.DropTable(
                name: "TB_LOCALIZACAO");

            migrationBuilder.DropTable(
                name: "TB_USUARIO");
        }
    }
}
