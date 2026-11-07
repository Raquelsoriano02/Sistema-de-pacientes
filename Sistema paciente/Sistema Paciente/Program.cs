using System.Text.Json.Serialization;
using SistemaPaciente.Application.Interfaces;
using SistemaPaciente.Application.Service;
using SistemaPaciente.Infrastructure.Context;
using SistemaPaciente.Infrastructure.Interfaces;
using SistemaPaciente.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Sistema_Paciente
{
    public class Program
    {
        private const string PoliticaFrontend = "Frontend";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<SistemaPacienteContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<ISeguroRepository, SeguroRepository>();
            builder.Services.AddScoped<IEspecialidadRepository, EspecialidadRepository>();
            builder.Services.AddScoped<IConsultorioRepository, ConsultorioRepository>();
            builder.Services.AddScoped<IMedicamentoRepository, MedicamentoRepository>();
            builder.Services.AddScoped<IMedicoRepository, MedicoRepository>();
            builder.Services.AddScoped<IHorarioRepository, HorarioRepository>();
            builder.Services.AddScoped<IPacienteRepository, PacienteRepository>();
            builder.Services.AddScoped<IAlergiaRepository, AlergiaRepository>();
            builder.Services.AddScoped<ICitaRepository, CitaRepository>();
            builder.Services.AddScoped<IConsultaRepository, ConsultaRepository>();
            builder.Services.AddScoped<IRecetaRepository, RecetaRepository>();
            builder.Services.AddScoped<IPagoRepository, PagoRepository>();

            builder.Services.AddScoped<ISeguroService, SeguroService>();
            builder.Services.AddScoped<IEspecialidadService, EspecialidadService>();
            builder.Services.AddScoped<IConsultorioService, ConsultorioService>();
            builder.Services.AddScoped<IMedicamentoService, MedicamentoService>();
            builder.Services.AddScoped<IMedicoService, MedicoService>();
            builder.Services.AddScoped<IHorarioService, HorarioService>();
            builder.Services.AddScoped<IPacienteService, PacienteService>();
            builder.Services.AddScoped<IAlergiaService, AlergiaService>();
            builder.Services.AddScoped<ICitaService, CitaService>();
            builder.Services.AddScoped<IConsultaService, ConsultaService>();
            builder.Services.AddScoped<IRecetaService, RecetaService>();
            builder.Services.AddScoped<IPagoService, PagoService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(PoliticaFrontend, policy =>
                {
                    var origenes = builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? [];
                    policy.WithOrigins(origenes).AllowAnyHeader().AllowAnyMethod();
                });
            });

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: false));
                });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseHttpsRedirection();
            }

            app.UseCors(PoliticaFrontend);

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
