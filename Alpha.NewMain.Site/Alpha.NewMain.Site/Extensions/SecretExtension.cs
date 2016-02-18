namespace Alpha.NewMain.Site.Extensions
{
    using Microsoft.VisualBasic;

    public static class SecretExtension
    {
        private static string CfsCode(string nword)
        {
            string number = string.Empty;
            for (int i = 1; i <= nword.Length; i++)
            {
                number = number + Strings.Asc(Strings.Mid(nword, i, 1));
            }

            return Conversion.Hex(number);
        }

        public static string CfsEnCode(string encryptValue)
        {
            if (string.IsNullOrEmpty(encryptValue))
            {
                return string.Empty;
            }

            int num = 30;
            int num2 = num - encryptValue.Length;
            if (num2 > 1)
            {
                for (int m = 1; m <= num2; m++)
                {
                    encryptValue = encryptValue + Strings.Chr(0x15);
                }
            }

            double num3 = 1.0;
            for (int i = 1; i <= num; i++)
            {
                int num5 = num + (Strings.Asc(Strings.Mid(encryptValue, i, 1)) * i);
                num3 *= num5;
            }

            encryptValue = num3.ToString();
            string str = string.Empty;
            for (int j = 1; j <= encryptValue.Length; j++)
            {
                str = str + CfsCode(Strings.Mid(encryptValue, j, 3));
            }

            string str2 = string.Empty;
            for (int k = 20; k <= (str.ToString().Length - 0x12); k += 2)
            {
                str2 = str2 + Strings.Mid(str, k, 1);
            }

            return str2;
        }

        public static string CfsEnCode(string encryptValue, int number)
        {
            int num = number < 6 ? 6 : number;
            int num2 = num - encryptValue.Length;
            if (num2 > 1)
            {
                for (int m = 1; m <= num2; m++)
                {
                    encryptValue = encryptValue + Strings.Chr(0x15);
                }
            }

            double num3 = 1.0;
            for (int i = 1; i <= num; i++)
            {
                int num5 = num + (Strings.Asc(Strings.Mid(encryptValue, i, 1)) * i);
                num3 *= num5;
            }

            encryptValue = num3.ToString();
            string str = string.Empty;
            for (int j = 1; j <= encryptValue.Length; j++)
            {
                str = str + CfsCode(Strings.Mid(encryptValue, j, 3));
            }

            string str2 = string.Empty;
            for (int k = 20; k <= (str.ToString().Length - 0x12); k += 2)
            {
                str2 = str2 + Strings.Mid(str, k, 1);
            }

            return str2;
        }
    }
}