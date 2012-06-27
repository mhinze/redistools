using System.Text;

namespace Client
{
    public static class Extensions
    {
        public static byte[] ToBytes(this string utf)
        {
            return Encoding.UTF8.GetBytes(utf);
        }

        public static string ToUtf8String(this byte[] bytes)
        {
            if (bytes == null) return null;
            return Encoding.UTF8.GetString(bytes);
        }
    }
}