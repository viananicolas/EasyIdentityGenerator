using System;
using System.Security.Cryptography;
using EasyIdentityGenerator.Data.Services.Interfaces;

namespace EasyIdentityGenerator.Data.Services.Implementation
{
    public class PasswordHasherMvc : IPasswordHasherMvc
    {
        private const int SaltSize = 0x10;
        private const int Iterations = 0x3e8;
        private const int ByteCount = 0x20;
        private const int ByteSize = 0x31;
        private const int BufferDestinationArrayOffset = 0x11;
        private const int SaltDestinationArrayOffset = 1;
        private const int ArrayOffset = 0;

        public string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));
            using var bytes = new Rfc2898DeriveBytes(password, SaltSize, Iterations);
            var salt = bytes.Salt;
            var sourceArrayBuffer = bytes.GetBytes(ByteCount);
            var dst = new byte[ByteSize];
            Buffer.BlockCopy(salt, ArrayOffset, dst, SaltDestinationArrayOffset, SaltSize);
            Buffer.BlockCopy(sourceArrayBuffer, ArrayOffset, dst, BufferDestinationArrayOffset, ByteCount);
            return Convert.ToBase64String(dst);
        }
    }
}
