namespace SimplzQuestionnaire.Common
{
    public class Utilities
    {
        public static string Base64Encode(string plainText)
        {
            return System.Net.WebUtility.UrlEncode(System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(plainText)));
        }
        public static string Base64Decode(string base64EncodedData)
        {
            return System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(System.Net.WebUtility.UrlDecode(base64EncodedData)));
        }
    }
}
