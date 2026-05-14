// Data/EscaladaContext.cs
using Microsoft.EntityFrameworkCore;
using ProyectoEscalada.Models;

namespace ProyectoEscalada.Data
{
    public class EscaladaContext : DbContext
    {
        // El constructor recibe las opciones (como la cadena de conexión)
        public EscaladaContext(DbContextOptions<EscaladaContext> options) : base(options)
        {
        }

        // DbSet representa la tabla en la base de datos
        public DbSet<RutaEscaladaModel> Rutas { get; set; }
    }
}