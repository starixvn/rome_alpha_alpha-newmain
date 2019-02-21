using Fanex.Cryptography.Aes;

namespace Alpha.Main.Services.Impl
{
    public class AesServiceImpl : AesService
    {
        public string Decrypt(string cipherText, string secretKey)
        {
            try
            {
                return AesExtension.Decrypt(cipherText, secretKey);
            }
            catch
            {
                return string.Empty;
            }
        }

        public string Encrypt(string plainText, string secretKey)
        {
            return AesExtension.Encrypt(plainText, secretKey);
        }
    }
}