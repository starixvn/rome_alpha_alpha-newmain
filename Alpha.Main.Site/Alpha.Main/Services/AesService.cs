namespace Alpha.Main.Services
{
    public interface AesService
    {
        string Encrypt(string plainText, string secretKey);

        string Decrypt(string cipherText, string secretKey);
    }
}