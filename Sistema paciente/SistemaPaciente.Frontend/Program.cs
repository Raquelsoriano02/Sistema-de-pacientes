// Solo sirve los archivos de wwwroot (HTML, CSS y JS) para poder iniciarlo desde Visual Studio.
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles(new StaticFileOptions
{
    // El navegador revalida cada archivo para que los cambios se vean al recargar.
    OnPrepareResponse = context => context.Context.Response.Headers.CacheControl = "no-cache"
});

app.Run();
