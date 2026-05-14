using ProyectoEscalada.Models;

namespace ProyectoEscalada.Data
{
    public static class DbInitializer
    {
        public static void Initialize(EscaladaContext context)
        {
            // Esto asegura que la BD exista y esté actualizada con la última migración
            context.Database.EnsureCreated();

            // Si ya hay usuarios, significa que la base de datos ya tiene datos, no hacemos nada.
            if (context.Usuarios.Any())
            {
                return; 
            }

            // 1. Crear base de Usuarios de prueba
            var usuarios = new UsuarioModel[]
            {
                new UsuarioModel { Nombre = "Bautista", Email = "bautista@test.com", PasswordHash = UsuarioModel.HashPassword("123456"), FechaRegistro = DateTime.UtcNow },
                new UsuarioModel { Nombre = "Adam Ondra", Email = "adam@silence.com", PasswordHash = UsuarioModel.HashPassword("123456"), FechaRegistro = DateTime.UtcNow },
                new UsuarioModel { Nombre = "Janja Garnbret", Email = "janja@boulder.com", PasswordHash = UsuarioModel.HashPassword("123456"), FechaRegistro = DateTime.UtcNow }
            };

            // Agregarlos todos de golpe
            context.Usuarios.AddRange(usuarios);
            context.SaveChanges();

            // 2. Crear base de Rutas y Boulders de prueba
            var rutas = new RutaEscaladaModel[]
            {
                new RutaEscaladaModel { Nombre = "Burden of Dreams", GradoDificultad = "V17", TipoAgarre = "Micro regletas", EsBoulder = true },
                new RutaEscaladaModel { Nombre = "Silence", GradoDificultad = "9c", TipoAgarre = "Invertidos y empotres", EsBoulder = false },
                new RutaEscaladaModel { Nombre = "Monkey Magic", GradoDificultad = "V10", TipoAgarre = "Dinámicos y romos", EsBoulder = true },
                new RutaEscaladaModel { Nombre = "La Dura Dura", GradoDificultad = "9b+", TipoAgarre = "Monodedos y bidedos", EsBoulder = false },
                new RutaEscaladaModel { Nombre = "Ruta de Calentamiento", GradoDificultad = "6a", TipoAgarre = "Cazos (Jugs)", EsBoulder = false }
            };

            context.Rutas.AddRange(rutas);
            context.SaveChanges();
        }
    }
}