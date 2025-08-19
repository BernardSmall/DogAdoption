using System;
using System.Linq;
using System.Threading;

namespace DogAdoption
{
    public static class TerminalArt
    {
        // ASCII banner text that shows the app name in a cool way
        private static readonly string Banner = @"
   ____  ____   ____    _        _           _             
  / ___||  _ \ / ___|  / \   ___| |__   __ _| |_ ___  _ __ 
  \___ \| |_) | |     / _ \ / __| '_ \ / _` | __/ _ \| '__|
   ___) |  __/| |___ / ___ \ (__| | | | (_| | || (_) | |   
  |____/|_|    \____/_/   \_\___|_| |_|\__,_|\__\___/|_|   
";

        // ASCII art of a dog (renamed so it does not clash with Dog class)
        private static readonly string DogArt = @"
  /^ ^\
 / 0 0 \
 V\ Y /V
  / - \
 |     \
 || (__\";

        // Show the banner text in cyan
        public static void PrintBanner()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(Banner);
            Console.ResetColor();
        }

        // Show the dog art inside a box with "Woof Woof!"
        public static void PrintDogWithFrameAndWoof()
        {
            // Split lines of the ASCII dog safely for all systems
            var lines = DogArt.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            int width = lines.Max(l => l.Length);

            // Draw top border of the box
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("┌" + new string('─', width + 2) + "┐");

            // Print each line of the dog inside the box
            foreach (var line in lines)
                Console.WriteLine("│ " + line.PadRight(width) + " │");

            // Draw bottom border of the box
            Console.WriteLine("└" + new string('─', width + 2) + "┘");
            Console.ResetColor();

            // Print "Woof Woof!" under the dog
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("     Woof Woof!");
            Console.ResetColor();
        }

        // Typewriter effect: prints text slowly letter by letter
        public static void TypeWriter(string text, int delayMs = 10)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delayMs); // wait a little between each letter
            }
        }

        // Show a simple header with lines above and below
        public static void Header(string title)
        {
            int w = Math.Max(40, title?.Length + 8 ?? 40);

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(new string('=', w)); // top line
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(title); // the title text
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(new string('=', w)); // bottom line
            Console.ResetColor();
        }

        // Splash screen shown once when the program starts
        public static void Splash()
        {
            Console.Clear();
            PrintBanner();                   // show the banner
            PrintDogWithFrameAndWoof();      // show the dog art
            TypeWriter("\nWelcome to the SPCA Adoption System!", 8);
            Console.WriteLine("\n\nPress any key to continue...");
            Console.ReadKey(true);           // wait for user to press a key
        }
    }
}
