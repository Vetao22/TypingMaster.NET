using System;
using System.Diagnostics;
using System.Threading;

namespace TypingMaster.NET
{
    /// <summary>
    /// High-performance terminal typing speed test engine with real-time metrics and sarcastic evaluations.
    /// </summary>
    public sealed class TypingEngine
    {
        private readonly string _targetText;

        public TypingEngine(string targetText)
        {
            _targetText = targetText ?? throw new ArgumentNullException(nameof(targetText));
        }

        /// <summary>
        /// Runs the interactive typing test session.
        /// </summary>
        public TypingResult RunTest()
        {
            Console.Clear();
            Console.CursorVisible = true;

            Console.WriteLine("=== TypingMaster.NET Speed Test ===");
            Console.WriteLine("Type the text below as fast and accurately as you can:\n");
            Console.WriteLine($"\"{_targetText}\"\n");
            Console.WriteLine(new string('-', 50));
            Console.WriteLine("Your input: ");

            int cursorLeft = Console.CursorLeft;
            int cursorTop = Console.CursorTop;

            var stopwatch = new Stopwatch();
            int correctChars = 0;
            int totalTyped = 0;
            bool testStarted = false;

            char[] typedChars = new char[_targetText.Length];
            int currentIndex = 0;

            while (currentIndex < _targetText.Length)
            {
                if (!Console.KeyAvailable)
                {
                    Thread.Sleep(10);
                    continue;
                }

                var keyInfo = Console.ReadKey(true);

                // Handle ESC to abort test
                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    return new TypingResult(0, 0, 0, TimeSpan.Zero, false);
                }

                // Handle Backspace
                if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (currentIndex > 0)
                    {
                        currentIndex--;
                        Console.SetCursorPosition(cursorLeft + currentIndex, cursorTop);
                        Console.Write(' ');
                        Console.SetCursorPosition(cursorLeft + currentIndex, cursorTop);
                    }
                    continue;
                }

                // Ignore control characters/functional keys
                if (keyInfo.Char == '\0' || char.IsControl(keyInfo.KeyChar))
                {
                    continue;
                }

                // Start timer on first valid keystroke
                if (!testStarted)
                {
                    stopwatch.Start();
                    testStarted = true;
                }

                char typedChar = keyInfo.KeyChar;
                typedChars[currentIndex] = typedChar;
                totalTyped++;

                // Provide instant color feedback on terminal
                if (typedChar == _targetText[currentIndex])
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    correctChars++;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }

                Console.Write(typedChar);
                Console.ResetColor();

                currentIndex++;
            }

            stopwatch.Stop();

            // Calculate metrics
            double minutes = stopwatch.Elapsed.TotalMinutes;
            double wpm = minutes > 0 ? (_targetText.Length / 5.0) / minutes : 0;
            double accuracy = totalTyped > 0 ? ((double)correctChars / totalTyped) * 100 : 0;

            return new TypingResult(wpm, accuracy, totalTyped, stopwatch.Elapsed, true);
        }
    }

    /// <summary>
    /// Holds the final metrics and provides sarcastic evaluations of a typing session.
    /// </summary>
    public sealed class TypingResult
    {
        public double Wpm { get; }
        public double Accuracy { get; }
        public int TotalTyped { get; }
        public TimeSpan ElapsedTime { get; }
        public bool Completed { get; }

        public TypingResult(double wpm, double accuracy, int totalTyped, TimeSpan elapsedTime, bool completed)
        {
            Wpm = wpm;
            Accuracy = accuracy;
            TotalTyped = totalTyped;
            ElapsedTime = elapsedTime;
            completed = completed;
            Completed = completed;
        }

        /// <summary>
        /// Generates a humorous or sarcastic remark based on the user's WPM and accuracy performance.
        /// </summary>
        public string GetSarcasticEvaluation()
        {
            if (!Completed) return "Aborted. Cowardly retreat detected.";

            if (Accuracy < 70)
                return "Absolute keyboard massacre! Were you typing with your elbows?";
            
            if (Wpm < 30)
                return "Are you writing code or chiseling stone tablets? A turtle types faster.";
            
            if (Wpm >= 30 && Wpm < 60)
                return "Not terrible, but your keyboard is definitely judging your slow pace.";
            
            if (Wpm >= 60 && Wpm < 90)
                return "Solid speed! You could probably reply to angry Slack messages efficiently.";
            
            if (Wpm >= 90 && Wpm < 120)
                return "Impressive fingers! Are you a hacker in an 90s movie?";
            
            return "Inhuman speed detected! Please slow down before you melt the CPU.";
        }
    }
}