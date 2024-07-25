namespace Coroutine
{
    public class Debug
    {
        public static void Log(string source, string format = "", params object[] args)
        {
            var f = string.Format("[{0}]\t[{1}] : {2}", DateTime.Now.ToString("HH:mm:ss fff"), source, format);
            Console.WriteLine(f, args);
        }
    }
}
