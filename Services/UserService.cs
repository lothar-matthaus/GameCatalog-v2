using System;
using System.Security.Cryptography;
using System.Text;
using GameCatalog.Services.Interfaces;

namespace GameCatalog.Services
{
    public class UserService : IUserService
    {
        public string Encrypt(string salt)
        {
            byte[] saltByte;
            byte[] saltByteHashed;

            try
            {
                SHA256Managed sHA256 = new SHA256Managed();

                saltByte = Encoding.UTF8.GetBytes(salt);
                saltByteHashed = sHA256.ComputeHash(saltByte);

                return Convert.ToBase64String(saltByteHashed);
            }
            catch
            {
                throw new Exception("Erro ao encriptar o SALT.");
            }
        }

        public string Encrypt(string salt, string password, int interations)
        {
            byte[] saltByte = Encoding.UTF8.GetBytes(salt);
            byte[] passwordByte = Encoding.UTF8.GetBytes(Encrypt(password));

            try
            {
                Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(passwordByte, saltByte, interations);

                return Convert.ToBase64String(rfc.GetBytes(interations));
            }
            catch
            {
                throw new Exception("Erro ao encriptar a senha.");
            }
        }
    }
}