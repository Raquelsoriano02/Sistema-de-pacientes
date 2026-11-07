using SistemaPaciente.Domain.Core;
using SistemaPaciente.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace SistemaPaciente.Infrastructure.Context
{
    public class SistemaPacienteContext : DbContext
    {
        private static readonly DateTime FechaSeed = new DateTime(2026, 1, 1);

        public SistemaPacienteContext(DbContextOptions<SistemaPacienteContext> options)
            : base(options)
        {
        }

        public DbSet<Seguro> Seguros { get; set; }
        public DbSet<Especialidad> Especialidades { get; set; }
        public DbSet<Consultorio> Consultorios { get; set; }
        public DbSet<Medicamento> Medicamentos { get; set; }
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Horario> Horarios { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Alergia> Alergias { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<Consulta> Consultas { get; set; }
        public DbSet<Receta> Recetas { get; set; }
        public DbSet<Pago> Pagos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Seguro>(entity =>
            {
                entity.ToTable("Seguros");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Telefono).HasMaxLength(20);
                entity.Property(e => e.PorcentajeCobertura).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.FechaRegistro).HasDefaultValueSql("GETDATE()");

                entity.HasIndex(e => e.Nombre).IsUnique();
            });

            modelBuilder.Entity<Especialidad>(entity =>
            {
                entity.ToTable("Especialidades");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Descripcion).HasMaxLength(500);
                entity.Property(e => e.FechaRegistro).HasDefaultValueSql("GETDATE()");

                entity.HasIndex(e => e.Nombre).IsUnique();
            });

            modelBuilder.Entity<Consultorio>(entity =>
            {
                entity.ToTable("Consultorios");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Codigo).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Ubicacion).HasMaxLength(150);
                entity.Property(e => e.FechaRegistro).HasDefaultValueSql("GETDATE()");

                entity.HasIndex(e => e.Codigo).IsUnique();
            });

            modelBuilder.Entity<Medicamento>(entity =>
            {
                entity.ToTable("Medicamentos");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(150);
                entity.Property(e => e.PrincipioActivo).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Presentacion).HasMaxLength(100);
                entity.Property(e => e.FechaRegistro).HasDefaultValueSql("GETDATE()");

                entity.HasIndex(e => e.Nombre).IsUnique();
            });

            modelBuilder.Entity<Medico>(entity =>
            {
                entity.ToTable("Medicos");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Apellido).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).HasMaxLength(150);
                entity.Property(e => e.Telefono).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Licencia).IsRequired().HasMaxLength(30);
                entity.Property(e => e.CostoConsulta).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.FechaRegistro).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Especialidad)
                    .WithMany(s => s.Medicos)
                    .HasForeignKey(e => e.EspecialidadId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Licencia).IsUnique();
            });

            modelBuilder.Entity<Horario>(entity =>
            {
                entity.ToTable("Horarios");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.DiaSemana).HasConversion<string>().HasMaxLength(20);
                entity.Property(e => e.FechaRegistro).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Medico)
                    .WithMany(m => m.Horarios)
                    .HasForeignKey(e => e.MedicoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.MedicoId, e.DiaSemana });
            });

            modelBuilder.Entity<Paciente>(entity =>
            {
                entity.ToTable("Pacientes");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Apellido).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).HasMaxLength(150);
                entity.Property(e => e.Telefono).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Cedula).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Sexo).HasConversion<string>().HasMaxLength(20);
                entity.Property(e => e.TipoSangre).HasConversion<string>().HasMaxLength(20);
                entity.Property(e => e.Direccion).HasMaxLength(250);
                entity.Property(e => e.NumeroAfiliado).HasMaxLength(50);
                entity.Property(e => e.FechaRegistro).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Seguro)
                    .WithMany(s => s.Pacientes)
                    .HasForeignKey(e => e.SeguroId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Cedula).IsUnique();
            });

            modelBuilder.Entity<Alergia>(entity =>
            {
                entity.ToTable("Alergias");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Sustancia).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Severidad).HasConversion<string>().HasMaxLength(20);
                entity.Property(e => e.Observacion).HasMaxLength(500);
                entity.Property(e => e.FechaRegistro).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Paciente)
                    .WithMany(p => p.Alergias)
                    .HasForeignKey(e => e.PacienteId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.PacienteId, e.Sustancia }).IsUnique();
            });

            modelBuilder.Entity<Cita>(entity =>
            {
                entity.ToTable("Citas");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Costo).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.Cobertura).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.Estado).HasConversion<string>().HasMaxLength(20);
                entity.Property(e => e.Motivo).IsRequired().HasMaxLength(500);
                entity.Property(e => e.FechaRegistro).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Paciente)
                    .WithMany(p => p.Citas)
                    .HasForeignKey(e => e.PacienteId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Medico)
                    .WithMany(m => m.Citas)
                    .HasForeignKey(e => e.MedicoId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Consultorio)
                    .WithMany(c => c.Citas)
                    .HasForeignKey(e => e.ConsultorioId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => new { e.MedicoId, e.FechaHoraInicio });
                entity.HasIndex(e => new { e.PacienteId, e.FechaHoraInicio });
                entity.HasIndex(e => new { e.ConsultorioId, e.FechaHoraInicio });
            });

            modelBuilder.Entity<Consulta>(entity =>
            {
                entity.ToTable("Consultas");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.PesoKg).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TemperaturaC).HasColumnType("decimal(18,2)");
                entity.Property(e => e.PresionArterial).HasMaxLength(20);
                entity.Property(e => e.Sintomas).HasMaxLength(1000);
                entity.Property(e => e.Diagnostico).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.Tratamiento).HasMaxLength(1000);
                entity.Property(e => e.FechaRegistro).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Cita)
                    .WithOne(c => c.Consulta)
                    .HasForeignKey<Consulta>(e => e.CitaId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Receta>(entity =>
            {
                entity.ToTable("Recetas");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Dosis).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Frecuencia).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Indicaciones).HasMaxLength(500);
                entity.Property(e => e.FechaRegistro).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Consulta)
                    .WithMany(c => c.Recetas)
                    .HasForeignKey(e => e.ConsultaId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Medicamento)
                    .WithMany(m => m.Recetas)
                    .HasForeignKey(e => e.MedicamentoId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => new { e.ConsultaId, e.MedicamentoId }).IsUnique();
            });

            modelBuilder.Entity<Pago>(entity =>
            {
                entity.ToTable("Pagos");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Monto).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.MetodoPago).HasConversion<string>().HasMaxLength(20);
                entity.Property(e => e.FechaPago).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.Referencia).HasMaxLength(100);
                entity.Property(e => e.FechaRegistro).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Cita)
                    .WithMany(c => c.Pagos)
                    .HasForeignKey(e => e.CitaId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            SeedData(modelBuilder);
        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Seguro>().HasData(
                new Seguro { Id = 1, Nombre = "Salud Total", Telefono = "809-555-0101", PorcentajeCobertura = 80, Activo = true, FechaRegistro = FechaSeed },
                new Seguro { Id = 2, Nombre = "Vida Plena", Telefono = "809-555-0102", PorcentajeCobertura = 60, Activo = true, FechaRegistro = FechaSeed },
                new Seguro { Id = 3, Nombre = "Protección Familiar", Telefono = "809-555-0103", PorcentajeCobertura = 50, Activo = true, FechaRegistro = FechaSeed });

            modelBuilder.Entity<Especialidad>().HasData(
                new Especialidad { Id = 1, Nombre = "Medicina General", Descripcion = "Atención primaria y chequeos de rutina", Activa = true, FechaRegistro = FechaSeed },
                new Especialidad { Id = 2, Nombre = "Pediatría", Descripcion = "Salud de niños y adolescentes", Activa = true, FechaRegistro = FechaSeed },
                new Especialidad { Id = 3, Nombre = "Cardiología", Descripcion = "Corazón y sistema circulatorio", Activa = true, FechaRegistro = FechaSeed },
                new Especialidad { Id = 4, Nombre = "Dermatología", Descripcion = "Piel, cabello y uñas", Activa = true, FechaRegistro = FechaSeed });

            modelBuilder.Entity<Consultorio>().HasData(
                new Consultorio { Id = 1, Codigo = "C-101", Ubicacion = "Primer piso, ala norte", Activo = true, FechaRegistro = FechaSeed },
                new Consultorio { Id = 2, Codigo = "C-102", Ubicacion = "Primer piso, ala sur", Activo = true, FechaRegistro = FechaSeed },
                new Consultorio { Id = 3, Codigo = "C-201", Ubicacion = "Segundo piso", Activo = true, FechaRegistro = FechaSeed });

            modelBuilder.Entity<Medicamento>().HasData(
                new Medicamento { Id = 1, Nombre = "Acetaminofén 500 mg", PrincipioActivo = "Paracetamol", Presentacion = "Tabletas", Activo = true, FechaRegistro = FechaSeed },
                new Medicamento { Id = 2, Nombre = "Ibuprofeno 400 mg", PrincipioActivo = "Ibuprofeno", Presentacion = "Tabletas", Activo = true, FechaRegistro = FechaSeed },
                new Medicamento { Id = 3, Nombre = "Amoxicilina 500 mg", PrincipioActivo = "Amoxicilina", Presentacion = "Cápsulas", Activo = true, FechaRegistro = FechaSeed },
                new Medicamento { Id = 4, Nombre = "Loratadina 10 mg", PrincipioActivo = "Loratadina", Presentacion = "Tabletas", Activo = true, FechaRegistro = FechaSeed },
                new Medicamento { Id = 5, Nombre = "Omeprazol 20 mg", PrincipioActivo = "Omeprazol", Presentacion = "Cápsulas", Activo = true, FechaRegistro = FechaSeed });

            modelBuilder.Entity<Medico>().HasData(
                new Medico { Id = 1, Nombre = "Laura", Apellido = "Méndez", Email = "laura.mendez@example.com", Telefono = "809-555-0201", Licencia = "LIC-10234", EspecialidadId = 1, CostoConsulta = 1500, Activo = true, FechaRegistro = FechaSeed },
                new Medico { Id = 2, Nombre = "Carlos", Apellido = "Rosario", Email = "carlos.rosario@example.com", Telefono = "809-555-0202", Licencia = "LIC-20871", EspecialidadId = 2, CostoConsulta = 2000, Activo = true, FechaRegistro = FechaSeed },
                new Medico { Id = 3, Nombre = "Elena", Apellido = "Vargas", Email = "elena.vargas@example.com", Telefono = "809-555-0203", Licencia = "LIC-35510", EspecialidadId = 3, CostoConsulta = 3000, Activo = true, FechaRegistro = FechaSeed });

            var horarios = new List<Horario>();
            var dias = new[] { DiaSemana.Lunes, DiaSemana.Martes, DiaSemana.Miercoles, DiaSemana.Jueves, DiaSemana.Viernes };
            var horarioId = 1;

            foreach (var medicoId in new[] { 1, 2, 3 })
            {
                foreach (var dia in dias)
                {
                    horarios.Add(new Horario { Id = horarioId++, MedicoId = medicoId, DiaSemana = dia, HoraInicio = new TimeOnly(8, 0), HoraFin = new TimeOnly(12, 0), Disponible = true, FechaRegistro = FechaSeed });
                    horarios.Add(new Horario { Id = horarioId++, MedicoId = medicoId, DiaSemana = dia, HoraInicio = new TimeOnly(14, 0), HoraFin = new TimeOnly(17, 0), Disponible = true, FechaRegistro = FechaSeed });
                }
            }

            modelBuilder.Entity<Horario>().HasData(horarios);

            modelBuilder.Entity<Paciente>().HasData(
                new Paciente { Id = 1, Nombre = "Ana", Apellido = "Pérez", Email = "ana.perez@example.com", Telefono = "829-555-0301", Cedula = "001-0000001-1", FechaNacimiento = new DateOnly(1990, 3, 14), Sexo = Sexo.Femenino, TipoSangre = TipoSangre.OPositivo, Direccion = "Calle Primera 12", SeguroId = 1, NumeroAfiliado = "ST-44821", FechaRegistro = FechaSeed },
                new Paciente { Id = 2, Nombre = "Miguel", Apellido = "Santos", Email = "miguel.santos@example.com", Telefono = "829-555-0302", Cedula = "001-0000002-2", FechaNacimiento = new DateOnly(1978, 11, 2), Sexo = Sexo.Masculino, TipoSangre = TipoSangre.APositivo, Direccion = "Avenida Central 45", SeguroId = 2, NumeroAfiliado = "VP-10977", FechaRegistro = FechaSeed },
                new Paciente { Id = 3, Nombre = "Sofía", Apellido = "Castillo", Telefono = "829-555-0303", Cedula = "001-0000003-3", FechaNacimiento = new DateOnly(2015, 7, 22), Sexo = Sexo.Femenino, TipoSangre = TipoSangre.BNegativo, FechaRegistro = FechaSeed });

            modelBuilder.Entity<Alergia>().HasData(
                new Alergia { Id = 1, PacienteId = 2, Sustancia = "Amoxicilina", Severidad = Severidad.Severa, Observacion = "Erupción cutánea en la infancia", FechaRegistro = FechaSeed });
        }
    }
}
