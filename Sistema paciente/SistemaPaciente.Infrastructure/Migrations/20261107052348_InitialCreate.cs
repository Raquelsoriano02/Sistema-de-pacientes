using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SistemaPaciente.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Consultorios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Ubicacion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultorios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Especialidades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Activa = table.Column<bool>(type: "bit", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Especialidades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Medicamentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PrincipioActivo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Presentacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicamentos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Seguros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PorcentajeCobertura = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seguros", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Medicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Licencia = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    EspecialidadId = table.Column<int>(type: "int", nullable: false),
                    CostoConsulta = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medicos_Especialidades_EspecialidadId",
                        column: x => x.EspecialidadId,
                        principalTable: "Especialidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pacientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cedula = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FechaNacimiento = table.Column<DateOnly>(type: "date", nullable: false),
                    Sexo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TipoSangre = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    SeguroId = table.Column<int>(type: "int", nullable: true),
                    NumeroAfiliado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pacientes_Seguros_SeguroId",
                        column: x => x.SeguroId,
                        principalTable: "Seguros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Horarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicoId = table.Column<int>(type: "int", nullable: false),
                    DiaSemana = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    HoraInicio = table.Column<TimeOnly>(type: "time", nullable: false),
                    HoraFin = table.Column<TimeOnly>(type: "time", nullable: false),
                    Disponible = table.Column<bool>(type: "bit", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Horarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Horarios_Medicos_MedicoId",
                        column: x => x.MedicoId,
                        principalTable: "Medicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Alergias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PacienteId = table.Column<int>(type: "int", nullable: false),
                    Sustancia = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Severidad = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Observacion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alergias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alergias_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Citas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PacienteId = table.Column<int>(type: "int", nullable: false),
                    MedicoId = table.Column<int>(type: "int", nullable: false),
                    ConsultorioId = table.Column<int>(type: "int", nullable: false),
                    FechaHoraInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaHoraFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Costo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cobertura = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Motivo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Citas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Citas_Consultorios_ConsultorioId",
                        column: x => x.ConsultorioId,
                        principalTable: "Consultorios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Citas_Medicos_MedicoId",
                        column: x => x.MedicoId,
                        principalTable: "Medicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Citas_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Consultas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CitaId = table.Column<int>(type: "int", nullable: false),
                    PesoKg = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TemperaturaC = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PresionArterial = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Sintomas = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Diagnostico = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Tratamiento = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Consultas_Citas_CitaId",
                        column: x => x.CitaId,
                        principalTable: "Citas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pagos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CitaId = table.Column<int>(type: "int", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MetodoPago = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FechaPago = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Referencia = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pagos_Citas_CitaId",
                        column: x => x.CitaId,
                        principalTable: "Citas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Recetas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConsultaId = table.Column<int>(type: "int", nullable: false),
                    MedicamentoId = table.Column<int>(type: "int", nullable: false),
                    Dosis = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Frecuencia = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DuracionDias = table.Column<int>(type: "int", nullable: false),
                    Indicaciones = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recetas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recetas_Consultas_ConsultaId",
                        column: x => x.ConsultaId,
                        principalTable: "Consultas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Recetas_Medicamentos_MedicamentoId",
                        column: x => x.MedicamentoId,
                        principalTable: "Medicamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Consultorios",
                columns: new[] { "Id", "Activo", "Codigo", "FechaRegistro", "Ubicacion" },
                values: new object[,]
                {
                    { 1, true, "C-101", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Primer piso, ala norte" },
                    { 2, true, "C-102", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Primer piso, ala sur" },
                    { 3, true, "C-201", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Segundo piso" }
                });

            migrationBuilder.InsertData(
                table: "Especialidades",
                columns: new[] { "Id", "Activa", "Descripcion", "FechaRegistro", "Nombre" },
                values: new object[,]
                {
                    { 1, true, "Atención primaria y chequeos de rutina", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Medicina General" },
                    { 2, true, "Salud de niños y adolescentes", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pediatría" },
                    { 3, true, "Corazón y sistema circulatorio", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cardiología" },
                    { 4, true, "Piel, cabello y uñas", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dermatología" }
                });

            migrationBuilder.InsertData(
                table: "Medicamentos",
                columns: new[] { "Id", "Activo", "FechaRegistro", "Nombre", "Presentacion", "PrincipioActivo" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Acetaminofén 500 mg", "Tabletas", "Paracetamol" },
                    { 2, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ibuprofeno 400 mg", "Tabletas", "Ibuprofeno" },
                    { 3, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Amoxicilina 500 mg", "Cápsulas", "Amoxicilina" },
                    { 4, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Loratadina 10 mg", "Tabletas", "Loratadina" },
                    { 5, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Omeprazol 20 mg", "Cápsulas", "Omeprazol" }
                });

            migrationBuilder.InsertData(
                table: "Pacientes",
                columns: new[] { "Id", "Apellido", "Cedula", "Direccion", "Email", "FechaNacimiento", "FechaRegistro", "Nombre", "NumeroAfiliado", "SeguroId", "Sexo", "Telefono", "TipoSangre" },
                values: new object[] { 3, "Castillo", "001-0000003-3", null, null, new DateOnly(2015, 7, 22), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sofía", null, null, "Femenino", "829-555-0303", "BNegativo" });

            migrationBuilder.InsertData(
                table: "Seguros",
                columns: new[] { "Id", "Activo", "FechaRegistro", "Nombre", "PorcentajeCobertura", "Telefono" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Salud Total", 80m, "809-555-0101" },
                    { 2, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vida Plena", 60m, "809-555-0102" },
                    { 3, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Protección Familiar", 50m, "809-555-0103" }
                });

            migrationBuilder.InsertData(
                table: "Medicos",
                columns: new[] { "Id", "Activo", "Apellido", "CostoConsulta", "Email", "EspecialidadId", "FechaRegistro", "Licencia", "Nombre", "Telefono" },
                values: new object[,]
                {
                    { 1, true, "Méndez", 1500m, "laura.mendez@example.com", 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "LIC-10234", "Laura", "809-555-0201" },
                    { 2, true, "Rosario", 2000m, "carlos.rosario@example.com", 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "LIC-20871", "Carlos", "809-555-0202" },
                    { 3, true, "Vargas", 3000m, "elena.vargas@example.com", 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "LIC-35510", "Elena", "809-555-0203" }
                });

            migrationBuilder.InsertData(
                table: "Pacientes",
                columns: new[] { "Id", "Apellido", "Cedula", "Direccion", "Email", "FechaNacimiento", "FechaRegistro", "Nombre", "NumeroAfiliado", "SeguroId", "Sexo", "Telefono", "TipoSangre" },
                values: new object[,]
                {
                    { 1, "Pérez", "001-0000001-1", "Calle Primera 12", "ana.perez@example.com", new DateOnly(1990, 3, 14), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ana", "ST-44821", 1, "Femenino", "829-555-0301", "OPositivo" },
                    { 2, "Santos", "001-0000002-2", "Avenida Central 45", "miguel.santos@example.com", new DateOnly(1978, 11, 2), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Miguel", "VP-10977", 2, "Masculino", "829-555-0302", "APositivo" }
                });

            migrationBuilder.InsertData(
                table: "Alergias",
                columns: new[] { "Id", "FechaRegistro", "Observacion", "PacienteId", "Severidad", "Sustancia" },
                values: new object[] { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Erupción cutánea en la infancia", 2, "Severa", "Amoxicilina" });

            migrationBuilder.InsertData(
                table: "Horarios",
                columns: new[] { "Id", "DiaSemana", "Disponible", "FechaRegistro", "HoraFin", "HoraInicio", "MedicoId" },
                values: new object[,]
                {
                    { 1, "Lunes", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(12, 0, 0), new TimeOnly(8, 0, 0), 1 },
                    { 2, "Lunes", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(17, 0, 0), new TimeOnly(14, 0, 0), 1 },
                    { 3, "Martes", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(12, 0, 0), new TimeOnly(8, 0, 0), 1 },
                    { 4, "Martes", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(17, 0, 0), new TimeOnly(14, 0, 0), 1 },
                    { 5, "Miercoles", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(12, 0, 0), new TimeOnly(8, 0, 0), 1 },
                    { 6, "Miercoles", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(17, 0, 0), new TimeOnly(14, 0, 0), 1 },
                    { 7, "Jueves", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(12, 0, 0), new TimeOnly(8, 0, 0), 1 },
                    { 8, "Jueves", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(17, 0, 0), new TimeOnly(14, 0, 0), 1 },
                    { 9, "Viernes", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(12, 0, 0), new TimeOnly(8, 0, 0), 1 },
                    { 10, "Viernes", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(17, 0, 0), new TimeOnly(14, 0, 0), 1 },
                    { 11, "Lunes", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(12, 0, 0), new TimeOnly(8, 0, 0), 2 },
                    { 12, "Lunes", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(17, 0, 0), new TimeOnly(14, 0, 0), 2 },
                    { 13, "Martes", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(12, 0, 0), new TimeOnly(8, 0, 0), 2 },
                    { 14, "Martes", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(17, 0, 0), new TimeOnly(14, 0, 0), 2 },
                    { 15, "Miercoles", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(12, 0, 0), new TimeOnly(8, 0, 0), 2 },
                    { 16, "Miercoles", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(17, 0, 0), new TimeOnly(14, 0, 0), 2 },
                    { 17, "Jueves", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(12, 0, 0), new TimeOnly(8, 0, 0), 2 },
                    { 18, "Jueves", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(17, 0, 0), new TimeOnly(14, 0, 0), 2 },
                    { 19, "Viernes", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(12, 0, 0), new TimeOnly(8, 0, 0), 2 },
                    { 20, "Viernes", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(17, 0, 0), new TimeOnly(14, 0, 0), 2 },
                    { 21, "Lunes", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(12, 0, 0), new TimeOnly(8, 0, 0), 3 },
                    { 22, "Lunes", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(17, 0, 0), new TimeOnly(14, 0, 0), 3 },
                    { 23, "Martes", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(12, 0, 0), new TimeOnly(8, 0, 0), 3 },
                    { 24, "Martes", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(17, 0, 0), new TimeOnly(14, 0, 0), 3 },
                    { 25, "Miercoles", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(12, 0, 0), new TimeOnly(8, 0, 0), 3 },
                    { 26, "Miercoles", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(17, 0, 0), new TimeOnly(14, 0, 0), 3 },
                    { 27, "Jueves", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(12, 0, 0), new TimeOnly(8, 0, 0), 3 },
                    { 28, "Jueves", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(17, 0, 0), new TimeOnly(14, 0, 0), 3 },
                    { 29, "Viernes", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(12, 0, 0), new TimeOnly(8, 0, 0), 3 },
                    { 30, "Viernes", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(17, 0, 0), new TimeOnly(14, 0, 0), 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alergias_PacienteId_Sustancia",
                table: "Alergias",
                columns: new[] { "PacienteId", "Sustancia" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Citas_ConsultorioId_FechaHoraInicio",
                table: "Citas",
                columns: new[] { "ConsultorioId", "FechaHoraInicio" });

            migrationBuilder.CreateIndex(
                name: "IX_Citas_MedicoId_FechaHoraInicio",
                table: "Citas",
                columns: new[] { "MedicoId", "FechaHoraInicio" });

            migrationBuilder.CreateIndex(
                name: "IX_Citas_PacienteId_FechaHoraInicio",
                table: "Citas",
                columns: new[] { "PacienteId", "FechaHoraInicio" });

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_CitaId",
                table: "Consultas",
                column: "CitaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Consultorios_Codigo",
                table: "Consultorios",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Especialidades_Nombre",
                table: "Especialidades",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Horarios_MedicoId_DiaSemana",
                table: "Horarios",
                columns: new[] { "MedicoId", "DiaSemana" });

            migrationBuilder.CreateIndex(
                name: "IX_Medicamentos_Nombre",
                table: "Medicamentos",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medicos_Email",
                table: "Medicos",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Medicos_EspecialidadId",
                table: "Medicos",
                column: "EspecialidadId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicos_Licencia",
                table: "Medicos",
                column: "Licencia",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_Cedula",
                table: "Pacientes",
                column: "Cedula",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_Email",
                table: "Pacientes",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_SeguroId",
                table: "Pacientes",
                column: "SeguroId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_CitaId",
                table: "Pagos",
                column: "CitaId");

            migrationBuilder.CreateIndex(
                name: "IX_Recetas_ConsultaId_MedicamentoId",
                table: "Recetas",
                columns: new[] { "ConsultaId", "MedicamentoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recetas_MedicamentoId",
                table: "Recetas",
                column: "MedicamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Seguros_Nombre",
                table: "Seguros",
                column: "Nombre",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alergias");

            migrationBuilder.DropTable(
                name: "Horarios");

            migrationBuilder.DropTable(
                name: "Pagos");

            migrationBuilder.DropTable(
                name: "Recetas");

            migrationBuilder.DropTable(
                name: "Consultas");

            migrationBuilder.DropTable(
                name: "Medicamentos");

            migrationBuilder.DropTable(
                name: "Citas");

            migrationBuilder.DropTable(
                name: "Consultorios");

            migrationBuilder.DropTable(
                name: "Medicos");

            migrationBuilder.DropTable(
                name: "Pacientes");

            migrationBuilder.DropTable(
                name: "Especialidades");

            migrationBuilder.DropTable(
                name: "Seguros");
        }
    }
}
