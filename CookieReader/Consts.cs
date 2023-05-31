namespace CookieReader
{
    public class Consts
    {
        public static string CookieDbPath { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), @"AppData\Local\Microsoft\Edge\User Data\Default\Network\Cookies");
        public static string LocalStatePath { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), @"AppData\Local\Microsoft\Edge\User Data\Local State");
    }
}
