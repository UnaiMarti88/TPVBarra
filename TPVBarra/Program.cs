namespace TPVBarra
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var app = new System.Windows.Application();
            app.Run(new LoginaWpf());
        }
    }
}