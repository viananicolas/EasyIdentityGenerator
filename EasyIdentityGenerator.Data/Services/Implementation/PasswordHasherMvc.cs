using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using EasyIdentityGenerator.Data.Services.Interfaces;

namespace EasyIdentityGenerator.Data.Services.Implementation
{
    public class PasswordHasherMvc : IPasswordHasherMvc
    {
        public string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));
            using var bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8);
            var salt = bytes.Salt;
            var buffer2 = bytes.GetBytes(0x20);
            var dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }
    }
}
