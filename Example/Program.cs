using CookieReader;

namespace Example
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CookieReaderUtil.CheckFileExists();
            var cookie = CookieReaderUtil.GetCookies(p => p.Name == "_U")?.FirstOrDefault();
            if (cookie != null)
            {               
                Console.WriteLine(cookie.Value);
            }
            Console.WriteLine("Hello, World!");           
        }
    }
}