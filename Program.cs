using System;
using TypingMaster.NET;

namespace TypingMasterDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // The legendary Mustafar confrontation speech
            string sampleText = "you were my brother anakin i loved you you were supposed to destroy the sith not join them bring balance to the force not leave it in darkness";

            var engine = new TypingEngine(sampleText);
            var result = engine.RunTest();

            Console.Clear();
            Console.WriteLine("\n=== TypingMaster.NET Results ===\n");

            if (!result.Completed)
            {
                Console.WriteLine("Test aborted by user.");
                return;
            }

            Console.WriteLine($" Speed (WPM): {result.Wpm:F1} words per minute");
            Console.WriteLine($" Accuracy:   {result.Accuracy:F1}%");
            Console.WriteLine($" Time Taken: {result.ElapsedTime.TotalSeconds:F2} seconds");
            Console.WriteLine($" Total Keys: {result.TotalTyped} keystrokes");

            Console.WriteLine("\n--- System Evaluation ---");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\"{result.GetSarcasticEvaluation()}\"");
            Console.ResetColor();

            Console.WriteLine("\nPress any key to exit TypingMaster.NET...");
            Console.ReadKey(true);
        }
    }
}