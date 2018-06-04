using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PQSE_GUI
{
    public static class Crypto
    {
        public static readonly byte[] Key = Encoding.UTF8.GetBytes("C7PxX4jPfPQ2SmzB");
        public static readonly byte[] Iv = Encoding.UTF8.GetBytes("nSdhdc3ecDDEM7fA");
        public static readonly byte[] ChecksumKey = Encoding.UTF8.GetBytes("chikuwa-hanpen");
        public static readonly int SaveLength = 0x80000;

        public static byte[] EncryptSave(byte[] save)
        {
            // Recalculate hash
            var hash = new HMACSHA256(ChecksumKey);
            var checksum = hash.ComputeHash(save, 0x38, save.Length - 0x38);
            Array.Copy(checksum, 0, save, 0x14, 0x20);

            // Encrypt head and body chunks
            var encryptedLength = save.Length + 16 & ~0xF;
            var head = Encrypt(BitConverter.GetBytes(encryptedLength), 0, 4);
            var body = Encrypt(save, 0, save.Length);

            // Concat the 2 chunks
            var encrypted = new byte[SaveLength];
            Array.Copy(head, encrypted, 16);
            Array.Copy(body, 0, encrypted, 16, body.Length);
            return encrypted;
        }

        public static byte[] DecryptSave(byte[] saveEnc)
        {
            var length = BitConverter.ToInt32(Decrypt(saveEnc, 0, 16), 0);
            return Decrypt(saveEnc, 16, length);
        }

        public static byte[] Encrypt(byte[] data, int index, int length)
        {
            using (var aes = Aes.Create())
            using (var encryptor = aes.CreateEncryptor(Key, Iv))
                return Transform(data, index, length, encryptor);
        }

        private static byte[] Decrypt(byte[] data, int index, int length)
        {
            using (var aes = Aes.Create())
            using (var decryptor = aes.CreateDecryptor(Key, Iv))
                return Transform(data, index, length, decryptor);
        }

        private static byte[] Transform(byte[] data, int index, int length, ICryptoTransform decryptor)
        {
            using (var ms = new MemoryStream())
            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
            {
                cs.Write(data, index, length);
                cs.FlushFinalBlock();
                return ms.ToArray();
            }
        }

        public static List<string> ByteArrayToHex(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            List<string> tmpHexs = new List<string>();
            foreach (byte b in ba)
            {
                tmpHexs.Add(b.ToString("X2").PadLeft(2));
            }
            return tmpHexs;
        }
    }
}
