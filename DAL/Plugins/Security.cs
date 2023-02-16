using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Plugins
{
    public static class Security
    {
        public static string Encode(this string x)
        {
            return En(En(En(x)));
        }
        public static string Decode(string x)
        {
            return De(De(De(x)));
        }
        private static string En(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }
        private static string De(string encodedData)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encodedData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }
        public static bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
        public static bool isEmpty(this string? str)
        {
            if (str == null || str.Length < 1)
            {
                return true;
            }
            return false;
        }
        public static bool isEmpty(this string? str, int length)
        {
            if (str == null || str.Length > length)
            {
                return true;
            }
            return false;
        }
        public static bool isPhoneNumber(this string? str)
        {
            if (str == null || !str.Any(char.IsDigit))
            {
                return false;
            }
            return true;
        }
        public static bool isPhoneNumber(this string? str, int length)
        {
            if (str == null || !str.All(char.IsDigit) || str.Length != length)
            {
                return false;
            }
            return true;
        }
        public static string GenerateAuthKey()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 32)
                .Select(s => s[random.Next(s.Length)]).ToArray()) + Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
