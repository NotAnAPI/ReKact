using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using NotAnAPI.ReKact.Core;
using NotAnAPI.ReKact.Core.Enums;
using NotAnAPI.ReKact.Core.EventArgs;

namespace NotAnAPI.ReKact
{
    class Program
    {
        public static CancellationTokenSource Cts;

        static void Main(string[] args)
        {
            Console.Title = "NotAnAPI.ReKact - v1.0.0";
            Console.WriteLine($"NotAnAPI.ReKact - v1.0.0");
            Console.WriteLine(@"Akamai puts kact on top of mact when it comes to hierarchy. ReKact helps you see how your kact actually looks.{0}", Environment.NewLine + Environment.NewLine);
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.CursorVisible = false;

            var readResult = Read.ReadKact(args);

            Kact kact = readResult.Kact;
            if (kact?.IsValid is not true)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Error: Invalid Kact.");
                Console.ResetColor();
                Console.WriteLine("Press ENTER to exit...");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Kact from {0}", readResult.Source);

            kact.OnKeyDown += KactOnOnKeyDown;
            kact.OnKeyPress += KactOnOnKeyPress;
            kact.OnKeyUp += KactOnOnKeyUp;
            kact.OnWait += KactOnOnWait;

            Console.ForegroundColor = ConsoleColor.DarkGray;
            kact.Print();
            Console.ResetColor();
            Console.WriteLine(kact.GetSummary());
            Console.WriteLine(@"NOTE:
- Akamai's kact does not store the exact keycodes in order to protect users' privacy.
- As such, this tool also cannot enact the exact key presses that are done in a specific kact.
- However, in the event you press and hold any of the SHIFT, CTRL, META or ALT keys, kact will store the exact keycode and the tool will also be precise in those cases.
- To act more like a keyboard, the tool uses a random keycode for modified keycodes. For example, the keycode for -3 represents any keycode >= 33 and <= 47 in kact and the tool uses a random key in that range instead of -3.");
            
            Console.Write("- The keys in ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("Cyan");
            Console.ResetColor();
            Console.WriteLine(" are Key Down events.");

            Console.Write("- The keys in ");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("Blue");
            Console.ResetColor();
            Console.WriteLine(" are Key Down events that have the exact keycode and are not modified.");

            Console.Write("- The keys in ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("White");
            Console.ResetColor();
            Console.WriteLine(" are Key Press events.");

            Console.Write("- The keys in ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Dark Yellow");
            Console.ResetColor();
            Console.WriteLine(" are Key Up events.");

            Console.WriteLine("- Warnings cannot always be accurate and one example that can result in a wrong warning is when you do CTRL+F or any other keyboard shortcuts. These will not make a Key Up event and result in a warning.{0}", Environment.NewLine + Environment.NewLine);

            for (int countDown = 5; countDown >= 0; countDown--)
            {
                Console.CursorLeft = 0;
                if (countDown == 0)
                {
                    Console.Write(new string(' ', Console.BufferWidth));
                }
                else
                {
                    Console.Write("ReKact starts in {0} seconds", countDown);
                }
                System.Threading.Thread.Sleep(1000);
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            kact.Play();
            stopwatch.Stop();
            Console.ResetColor();

            Console.WriteLine("{0}Playing the Kact took {1} ms.", Environment.NewLine + Environment.NewLine + Environment.NewLine, stopwatch.ElapsedMilliseconds);

            kact.PrintWarnings();

            Console.WriteLine("Press ENTER to exit...");
            Console.ReadLine();
        }

        private static void KactOnOnWait(object sender, WaitEventArgs e)
        {
            var timeToWait = e.Time - 100;
            if (timeToWait < 1) return;
            Cts = new CancellationTokenSource(timeToWait);
            Task.Factory.StartNew(() =>
            {
                for (int i = timeToWait - 1; i >= 0; i -= 100)
                {
                    TimeSpan ts = TimeSpan.FromMilliseconds(i);
                    Console.Title = $"Next Act in {(ts.Minutes > 0 ? ts.Minutes + " minutes and " : "")}{ts.Seconds} seconds...";
                    Thread.Sleep(100);
                }
            }, Cts.Token);
        }

        private static void KactOnOnKeyDown(object sender, KeyDownEventArgs e)
        { 
            Cts.Cancel();
            PlayWrite(e.KeyAct);
        }

        private static void KactOnOnKeyPress(object sender, KeyPressEventArgs e)
        {
            Cts.Cancel();
            PlayWrite(e.KeyAct);
        }

        private static void KactOnOnKeyUp(object sender, KeyUpEventArgs e)
        {
            Cts.Cancel();
            PlayWrite(e.KeyAct);
        }

        private static void PlayWrite(KeyAct keyAct)
        {
            string keyByKeyCode = keyAct.Char;
            if (keyAct.IsDescribed)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
            }

            string fixedKey = keyAct.IsUnknown ? keyByKeyCode.Substring(1, keyByKeyCode.Length - 2) : keyByKeyCode;

            if (keyAct.Type.Equals(ActTypes.KeyDown))
            {
                Console.ForegroundColor = keyAct.KeyCode > 0 ? ConsoleColor.DarkBlue : ConsoleColor.DarkCyan;
            } else if (keyAct.Type.Equals(ActTypes.KeyPress))
            {
                Console.ForegroundColor = ConsoleColor.White;
            } else if (keyAct.Type.Equals(ActTypes.KeyUp))
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
            }

            if (keyAct.IsUnknown)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
            }

            lock (Console.Out)
            {
                Console.Write(fixedKey);
            }
            if (Console.ForegroundColor != ConsoleColor.White) Console.ResetColor();
        }
    }

    public static class KactHelper
    {
        public static void Print(this Kact kact)
        {
            ConsoleColor startingConsoleColor = Console.ForegroundColor;
            foreach (var keyAct in kact.Keys)
            {
                keyAct.Print();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write(';');
                Console.ForegroundColor = startingConsoleColor;
            }

            Console.WriteLine();
        }

        public static void Print(this KeyAct keyAct)
        {
            int[] keyActValues = new int[keyAct.NotTrusted != -1 ? 8 : 7];
            keyActValues[(int) KactValues.Index] = keyAct.Index;
            keyActValues[(int) KactValues.Type] = (int) keyAct.Type;
            keyActValues[(int) KactValues.Time] = keyAct.Time;
            keyActValues[(int) KactValues.KeyCode] = keyAct.KeyCode;
            keyActValues[(int) KactValues.L] = keyAct.L;
            keyActValues[(int) KactValues.Modifier] = keyAct.Modifier;
            keyActValues[(int) KactValues.FormElement] = keyAct.FormElement;
            if (keyAct.NotTrusted != -1) keyActValues[(int) KactValues.NotTrusted] = keyAct.NotTrusted;
            ConsoleColor startingForegroundColor = Console.ForegroundColor;
            for (int i = 0; i < keyActValues.Length; i++)
            {
                if (i == (int) KactValues.KeyCode) Console.ForegroundColor = ConsoleColor.White;
                Console.Write(keyActValues[i]);
                if (i == (int) KactValues.KeyCode) Console.ForegroundColor = startingForegroundColor;
                if (i != keyActValues.Length - 1)
                {
                    Console.Write(',');
                }
            }
        }

        public static void PrintWarnings(this Kact kact)
        {
            var leftovers = kact.GetLeftoverKeyEvents();
            var unknowns = kact.GetUnknowns();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            if (leftovers.KeyPress > 0) Console.WriteLine("WARNING: {0} processed but no Keypress event happened for {1}.", leftovers.KeyPress == 1 ? "A Key down" : leftovers.KeyPress + " KeyUp", leftovers.KeyPress == 1 ? "it" : "any of them");
            if (leftovers.KeyUp > 0) Console.WriteLine("WARNING: {0} processed but no Keyup event happened for {1}.", leftovers.KeyUp == 1 ? "A Key down" : leftovers.KeyUp + " KeyDown", leftovers.KeyUp == 1 ? "it" : "any of them");
            if (unknowns.Count > 0)
            {
                foreach (KeyAct unknownAct in unknowns)
                {
                    Console.WriteLine("WARNING: A Keyact exists with keycode \"{0}\" while it's not known which key is that", unknownAct.KeyCode);   
                }
            }
            Console.ResetColor();
        }
    }
}
