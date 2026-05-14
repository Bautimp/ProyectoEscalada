using System;
using System.Security.Cryptography; // Necesario para el Hash

namespace ProyectoEscalada.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        // Mantenemos tus excelentes métodos de seguridad, ya que no dependen de la base de datos
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("La contraseña no puede estar vacía.", nameof(password));
            }

            var salt = new byte[16];
            RandomNumberGenerator.Fill(salt);
            var key = Rfc2898DeriveBytes.Pbkdf2(password, salt, 10000, HashAlgorithmName.SHA256, 32);
            var result = new byte[49];
            Buffer.BlockCopy(salt, 0, result, 1, 16);
            result[0] = 0;
            Buffer.BlockCopy(key, 0, result, 17, 32);
            return Convert.ToBase64String(result);
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

            var computedKey = Rfc2898DeriveBytes.Pbkdf2(password, salt, 10000, HashAlgorithmName.SHA256, 32);
            return CryptographicOperations.FixedTimeEquals(storedKey, computedKey);
        }
    }
}