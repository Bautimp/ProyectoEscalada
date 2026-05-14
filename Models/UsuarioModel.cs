using System;
namespace ProyectoEscalada.Models
{
    public class UsuarioModel{
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime FechaRegistro { get; set; }

        //String para conexión a la BD
        private static string ConnectionString => "your_connection_string_here";

        public static List<UsuarioModel> GetAll(){
            var usuarios = new List<UsuarioModel>();

            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand("SELECT Id, Nombre, Email, PasswordHash, FechaRegistro FROM Usuarios", conn)){
                //Abre la conexión con la bd
                conn.Open();
                using (var reader = cmd.ExecuteReader()){
                    while (reader.Read()){
                        usuarios.Add(new UsuarioModel{
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Email = reader.GetString(2),
                            PasswordHash = reader.GetString(3),
                            FechaRegistro = reader.GetDateTime(4)
                        });
                    }
                }
            }
            return usuarios;
        }
        //Método para recuperar usuarios por ID
        public static UsuarioModel GetById(int id){
            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand("SELECT Id, Nombre, Email, PasswordHash, FechaRegistro FROM Usuarios WHERE Id = @Id", conn)){
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();

                using (var reader = cmd.ExecuteReader()){
                    if (reader.Read()){
                        return new UsuarioModel{
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Email = reader.GetString(2),
                            PasswordHash = reader.GetString(3),
                            FechaRegistro = reader.GetDateTime(4)
                        };
                    }
                }
            }
            return null;
        }
        public bool Insert(){
            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand("INSERT INTO Usuarios (Nombre, Email, PasswordHash, FechaRegistro) VALUES (@Nombre, @Email, @PasswordHash, @FechaRegistro); SELECT CAST(SCOPE_IDENTITY() AS INT)", conn)){
                cmd.Parameters.AddWithValue("@Nombre", Nombre);
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@PasswordHash", PasswordHash);
                cmd.Parameters.AddWithValue("@FechaRegistro", FechaRegistro);

                conn.Open();
                Id = (int)cmd.ExecuteScalar();
            }
            return Id > 0;
        }
        public bool Update() {
            if (Id <= 0){
                return false;
            }

            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand("UPDATE Usuarios SET Nombre = @Nombre, Email = @Email, PasswordHash = @PasswordHash WHERE Id = @Id", conn)){
                cmd.Parameters.AddWithValue("@Nombre", Nombre);
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@PasswordHash", PasswordHash);
                cmd.Parameters.AddWithValue("@Id", Id);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        public bool Delete(){
            if (Id <= 0){
                return false;
            }

            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand("DELETE FROM Usuarios WHERE Id = @Id", conn)){
                cmd.Parameters.AddWithValue("@Id", Id);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        public static string HashPassword(string password){
            if (string.IsNullOrEmpty(password)){
                throw new ArgumentException("La contraseña no puede estar vacía.", nameof(password));
            }

            using (var deriveBytes = new Rfc2898DeriveBytes(password, 16, 10000, HashAlgorithmName.SHA256)){
                var salt = deriveBytes.Salt;
                var key = deriveBytes.GetBytes(32);
                var result = new byte[49];
                Buffer.BlockCopy(salt, 0, result, 1, 16);
                result[0] = 0;
                Buffer.BlockCopy(key, 0, result, 17, 32);
                return Convert.ToBase64String(result);
            }
        }
        public static bool VerifyPassword(string password, string hashedPassword){
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hashedPassword)){
                return false;
            }

            var decoded = Convert.FromBase64String(hashedPassword);
            if (decoded.Length != 49 || decoded[0] != 0){
                return false;
            }

            var salt = new byte[16];
            Buffer.BlockCopy(decoded, 1, salt, 0, 16);
            var storedKey = new byte[32];
            Buffer.BlockCopy(decoded, 17, storedKey, 0, 32);

            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256)){
                var computedKey = deriveBytes.GetBytes(32);
                return CryptographicOperations.FixedTimeEquals(storedKey, computedKey);
            }
        }
    }
}
