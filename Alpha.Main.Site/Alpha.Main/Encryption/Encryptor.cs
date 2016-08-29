namespace Alpha.Main.Encryption
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Encryptor
    {
        public const string HexDigit = "0123456789abcdef";

        /// <summary>
        /// Hexes the decode.
        /// </summary>
        /// <param name="data">The data string to decode.</param>
        /// <returns>The hex string.</returns>
        public static string HexDecode(string data)
        {
            string b16_digits = HexDigit;
            char[] b16_map = new char[256];
            for (int i = 0; i < 256; i++)
            {
                b16_map[Convert.ToInt32(string.Concat(b16_digits[i >> 4], b16_digits[i & 15]), 16)] = Convert.ToChar(i);
            }

            int c = data.Length;
            if (c % 2 != 0)
            {
                data = '0' + data;
            }

            List<char> result = new List<char>();
            for (int i = 0; i < c; i += 2)
            {
                result.Add(b16_map[Convert.ToInt32(data.Substring(i, 2), 16)]);
            }

            return new string(result.ToArray());
        }

        /// <summary>
        /// Encrypt with RC4 algorithm.
        /// </summary>
        /// <param name="key">The key to encrypt.</param>
        /// <param name="value">The value to encrypt.</param>
        /// <returns>The RC4 string.</returns>
        public static string RC4(string key, string value)
        {
            char[] ks = key.ToCharArray();
            char[] vs = value.ToCharArray();
            int c = key.Length;
            int d = value.Length;
            int[] s = new int[256];
            int i = 0;
            for (i = 0; i < 256; i++)
            {
                s[i] = i;
            }

            int j = 0;
            int x;
            for (i = 0; i < 256; i++)
            {
                j = (j + s[i] + (int)ks[i % c]) % 256;
                x = s[i];
                s[i] = s[j];
                s[j] = x;
            }

            i = 0;
            j = 0;
            string ct = string.Empty;
            for (int y = 0; y < d; y++)
            {
                i = (i + 1) % 256;
                j = (j + s[i]) % 256;
                x = s[i];
                s[i] = s[j];
                s[j] = x;
                ct += Convert.ToChar((int)vs[y] ^ s[(s[i] + s[j]) % 256]);
            }

            return ct;
        }
    }
}
