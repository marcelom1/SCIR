using System;
using System.Security.Cryptography;
using System.IO;

namespace SCIR.Utils
{
    public class CriptoAES 
    {

        private byte[] Key;
        private byte[] IV = { 0x50, 0x08, 0xF1, 0xDD, 0xDE, 0x3C, 0xF2, 0x18, 0x44, 0x74, 0x19, 0x2C, 0x53, 0x49, 0xAB, 0xBC };
        private byte[] salt = { 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x5, 0x4, 0x3, 0x2, 0x1, 0x0 };

        public CriptoAES()
        {
            Key = Convert.FromBase64String("Q3JpcHRvZ3JhZmlhcyBjb20gUmluamRhZWwgLyBBRVM=");
        }

        public CriptoAES(string _Key)
        {
            Rfc2898DeriveBytes chave = new Rfc2898DeriveBytes(_Key, salt);

            Key = chave.GetBytes(16);
        }

        public string Encrypt(string plainText)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");
            byte[] encrypted;

            byte[] bKey = Key;

            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = bKey;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encrypted);

        }

        public string Decrypt(string plainText)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");

            byte[] cipherText = Convert.FromBase64String(plainText);

            byte[] bKey = Key;

            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = bKey;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plainText = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plainText;

        }
        //private RijndaelManaged Engine;
        //private SHA256CryptoServiceProvider HashProvider;
        //private byte[] HashBytes;

        //private string _Password;

        ///// <summary>
        ///// Retorna ou modifica a senha usada por esta instância para opções de criptografia e descriptografia
        ///// </summary>
        //public string Password
        //{
        //    get { return _Password; }
        //    set
        //    {
        //        _Password = value;
        //        HashBytes = HashProvider.ComputeHash(Encoding.UTF8.GetBytes(value));
        //        Engine.Key = HashBytes;
        //    }
        //}

        ///// <summary>
        ///// Cria uma nova instância
        ///// </summary>
        ///// <param name="Password">Senha utilizada para criptografar e descriptografar</param>
        //public CriptoAES (string Password)
        //{
        //    _Password = Password;
        //    Engine = new RijndaelManaged();
        //    HashProvider = new SHA256CryptoServiceProvider();
        //    HashBytes = HashProvider.ComputeHash(Encoding.UTF8.GetBytes(Password));
        //    Engine.Mode = CipherMode.CBC;
        //    Engine.Key = HashBytes;
        //}

        ///// <summary>
        ///// Criptografa um buffer de bytes
        ///// </summary>
        ///// <param name="Buffer"></param>
        ///// <returns></returns>
        //public byte[] Encrypt(byte[] Buffer)
        //{
        //    using (ICryptoTransform Encryptor = Engine.CreateEncryptor())
        //        return Encryptor.TransformFinalBlock(Buffer, 0, Buffer.Length);

        //}

        ///// <summary>
        ///// Descriptografa um buffer de bytes
        ///// </summary>
        ///// <param name="Buffer"></param>
        ///// <returns></returns>
        //public byte[] Decrypt(byte[] Buffer)
        //{
        //    using (ICryptoTransform Decryptor = Engine.CreateDecryptor())
        //        return Decryptor.TransformFinalBlock(Buffer, 0, Buffer.Length);
        //}

        ///// <summary>
        ///// Criptografa um texto
        ///// </summary>
        ///// <param name="Str"></param>
        ///// <returns></returns>
        //public string Encrypt(string Str)
        //{
        //    return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(Str)));
        //}

        ///// <summary>
        ///// Descriptografa um texto
        ///// </summary>
        ///// <param name="Str"></param>
        ///// <returns></returns>
        //public string Decrypt(string Str)
        //{
        //    return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(Str)));
        //}

        //public void Dispose()
        //{
        //    Engine.Dispose();
        //    HashProvider.Dispose();
        //    HashBytes = null;
        //    _Password = null;
        //}


    }
}