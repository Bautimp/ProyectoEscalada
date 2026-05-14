using Microsoft.EntityFrameworkCore;
using ProyectoEscalada.Data;

var builder = WebApplication.CreateBuilder(args);

//Conexión a la base de datos
// Agregar MVC al contenedor
builder.Services.AddControllersWithViews();

// --- AQUÍ REGISTRAS LA BASE DE DATOS ---
builder.Services.AddDbContext<EscaladaContext>(opciones =>
    opciones.UseSqlite(builder.Configuration.GetConnectionString("ConexionSQL"))
);

var app = builder.Build();

// --- INICIO DEL SEED DE DATOS ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<EscaladaContext>();
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error al inicializar la base de datos.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()){
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
