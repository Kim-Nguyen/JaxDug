// Type: System.Web.HttpUtility
// Assembly: Google.Apis, Version=1.1.4338.25503, Culture=neutral
// Assembly location: C:\Users\Tony\Dropbox\Programming\Librarys\Google Library\Lib\Google.Apis.dll

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace System.Web
{
    public sealed class HttpUtility
    {
        public static void HtmlAttributeEncode(string s, TextWriter output);
        public static string HtmlAttributeEncode(string s);
        public static string UrlDecode(string str);
        public static string UrlDecode(string s, Encoding e);
        public static string UrlDecode(byte[] bytes, Encoding e);
        public static string UrlDecode(byte[] bytes, int offset, int count, Encoding e);
        public static byte[] UrlDecodeToBytes(byte[] bytes);
        public static byte[] UrlDecodeToBytes(string str);
        public static byte[] UrlDecodeToBytes(string str, Encoding e);
        public static byte[] UrlDecodeToBytes(byte[] bytes, int offset, int count);
        public static string UrlEncode(string str);
        public static string UrlEncode(string s, Encoding Enc);
        public static string UrlEncode(byte[] bytes);
        public static string UrlEncode(byte[] bytes, int offset, int count);
        public static byte[] UrlEncodeToBytes(string str);
        public static byte[] UrlEncodeToBytes(string str, Encoding e);
        public static byte[] UrlEncodeToBytes(byte[] bytes);
        public static byte[] UrlEncodeToBytes(byte[] bytes, int offset, int count);
        public static string UrlEncodeUnicode(string str);
        public static byte[] UrlEncodeUnicodeToBytes(string str);
        public static string HtmlDecode(string s);
        public static void HtmlDecode(string s, TextWriter output);
        public static string HtmlEncode(string s);
        public static void HtmlEncode(string s, TextWriter output);
        public static string UrlPathEncode(string s);
        public static Dictionary<string, string> ParseQueryString(string query);
        public static Dictionary<string, string> ParseQueryString(string query, Encoding encoding);
    }
}
