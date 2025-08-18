using System;
using System.Linq;
using System.Threading;

namespace DogAdoption
{
    public static class TerminalArt
    {
        private static readonly string Banner = @"
   ____  ____   ____    _        _           _             
  / ___||  _ \ / ___|  / \   ___| |__   __ _| |_ ___  _ __ 
  \___ \| |_) | |     / _ \ / __| '_ \ / _` | __/ _ \| '__|
   ___) |  __/| |___ / ___ \ (__| | | | (_| | || (_) | |   
  |____/|_|    \____/_/   \_\___|_| |_|\__,_|\__\___/|_|   
";

        // Renamed to avoid clash with your Dog class
        private static readonly string DogArt = @"
  /^ ^\
 / 0 0 \
 V\ Y /V
  / - \
 |     \
 || (__\";

        public static void PrintBanner()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(Banner);
            Console.ResetColor();
        }

        public static void PrintDogWithFrameAndWoof()
        {
            // Split safely on Windows or Unix newlines
            var lines = DogArt.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            int width = lines.Max(l => l.Length);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("┌" + new string('─', width + 2) + "┐");
            foreach (var line in lines)
                Console.WriteLine("│ " + line.PadRight(width) + " │");
            Console.WriteLine("└" + new string('─', width + 2) + "┘");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("     Woof Woof!");
            Console.ResetColor();
        }

        public static void TypeWriter(string text, int delayMs = 10)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delayMs);
            }
        }

        // Optional helper if you want a simple header in menus
        public static void Header(string title)
        {
            int w = Math.Max(40, title?.Length + 8 ?? 40);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(new string('=', w));
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(title);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(new string('=', w));
            Console.ResetColor();
        }

        // Optional splash screen you can call once at startup
        public static void Splash()
        {
            Console.Clear();
            PrintBanner();
            PrintDogWithFrameAndWoof();
            TypeWriter("\nWelcome to the SPCA Adoption System!", 8);
            Console.WriteLine("\n\nPress any key to continue...");
            Console.ReadKey(true);
        }
    }
}
