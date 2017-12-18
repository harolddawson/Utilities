using System;
using System.Collections.Generic;
using System.Linq;
using HdUtilities.Extensions;

namespace HdUtilities.Helpers
{
    /// <summary>
    /// This class is designed to support thread-safe interaction with the Console.
    /// </summary>
    public static class ConsoleHelper
    {
        /// <summary>
        /// A collection of valid symbols allowed in passwords
        /// </summary>
        private static readonly char[] PasswordSymbols = "!@#$%^&*".ToCharArray();

        /// <summary>
        ///  The ConsoleLock object is used so that if multiple threads attempt to write to the console simultaneously, 
        /// </summary>
        private static readonly object ConsoleLock = new object();

        /// <summary>
        /// This method calls <code>System.Console.WriteLine()</code> but in a thread-safe manor.
        /// </summary>
        public static void WriteLine()
        {
            lock (ConsoleLock)
            {
                Console.WriteLine();
            }
        }

        /// <summary>
        /// This method calls <code>System.Console.WriteLine()</code> but in a thread-safe manor.
        /// </summary>
        public static void WriteLine(object obj)
        {
            lock (ConsoleLock)
            {
                Console.WriteLine(obj);
            }
        }

        /// <summary>
        /// This method calls <code>System.Console.WriteLine()</code> but in a thread-safe manor.
        /// </summary>
        public static void WriteLine(string mask, params object[] values)
        {
            lock (ConsoleLock)
            {
                Console.WriteLine(mask, values);
            }
        }

        /// <summary>
        /// This method calls <code>System.Console.WriteLine()</code> but in a thread-safe manor.
        /// </summary>
        public static void Write(object obj)
        {
            lock (ConsoleLock)
            {
                Console.Write(obj);
            }
        }

        /// <summary>
        /// This method calls <code>System.Console.ReadLine()</code> but in a thread-safe manor and returns the result.
        /// </summary>
        public static string ReadLine()
        {
            lock (ConsoleLock)
            {
                return Console.ReadLine();
            }
        }

        /// <summary>
        /// This method calls <code>System.Console.ReadKey()</code> but in a thread-safe manor and returns the result.
        /// </summary>
        public static ConsoleKeyInfo ReadKey(bool intercept = false)
        {
            lock (ConsoleLock)
            {
                return Console.ReadKey(intercept);
            }
        }

        /// <summary>
        /// This method calls <code>System.Console.Clear()</code> but in a thread-safe manor.
        /// </summary>
        public static void Clear()
        {
            lock (ConsoleLock)
            {
                Console.Clear();
            }
        }

        /// <summary>
        /// This method returns the row on the Console where the cursor is currently placed.
        /// </summary>
        public static int GetConsoleRow()
        {
            lock (ConsoleLock)
            {
                return Console.CursorTop;
            }
        }

        /// <summary>
        /// This prompts the user to enter a password from the console.<br/>
        /// And then waits for the user to enter text via the keyboard.<br/>
        /// The text is not displayed to the user. When the user presses enter, the text is returned.
        /// </summary>
        /// <returns></returns>
        public static string CollectPassword()
        {
            var pass = "";
            Write("Enter your password: ");
            while (true)
            {
                var key = Console.ReadKey(true);
                // Stops Receving Keys Once Enter is Pressed
                if (key.Key == ConsoleKey.Enter)
                {
                    WriteLine();
                    return pass;
                }
                //Look for valid password characters (letters, numbers, and specific symbols)
                if (char.IsLetterOrDigit(key.KeyChar) || key.KeyChar.DoesMatchAny(PasswordSymbols))
                {
                    pass += key.KeyChar;
                    Write("*");
                }
                //Look for the backspace key
                else if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    pass = pass.Substring(0, (pass.Length - 1));
                    Write("\b \b");
                }
            }
        }
        
        /// <summary>
        /// This method reads one line from the console. If it can be parsed as an integer, it's returned.<br/>
        /// If it cannot be parsed, it is returned as null.
        /// </summary>
        /// <returns></returns>
        public static int? GetIntFromDataEntry()
        {
            var line = ReadLine();
            int i;
            if (int.TryParse(line, out i))
            {
                return i;
            }
            return null;
        }

        /// <summary>
        /// This method goes to the Console and prompts the user to enter a numeric response.<br/>
        /// The textPrompt is one line of text, it then waits for the user to type a response and press enter.<br/>
        /// If it is unable to parse the response into an enum, it will loop indefinitely until a response is parsed correctly.
        /// </summary>
        public static int PromptForIntegerInput(string textPrompt)
        {
            return PromptForIntegerInput(new[] { textPrompt });
        }

        /// <summary>
        /// This method goes to the Console and prompts the user to enter a numeric response.<br/>
        /// The textPrompt is an Enumerable containing one or more lines of text that is displayed to the user in order.<br/>
        /// If it is unable to parse the response into an enum, it will loop indefinitely until a response is parsed correctly.
        /// </summary>
        public static int PromptForIntegerInput(IEnumerable<string> textPrompt)
        {
            while (true)
            {
                textPrompt.ForEach(WriteLine);
                WriteLine();
                var result = GetIntFromDataEntry();
                if (result.HasValue)
                {
                    return result.Value;
                }
            }
        }

        /// <summary>
        /// This method goes to the Console and prompts the user to pick one of the items from the enum passed as the generic type<br/>
        /// The textPrompt is one line of text, it then waits for the user to type a response and press enter.<br/>
        /// If it is unable to parse the response into an enum, it will loop indefinitely until a response is parsed correctly.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="textPrompt"></param>
        /// <returns></returns>
        public static TEnum PromptForEnumInput<TEnum>(string textPrompt) where TEnum : struct, IComparable
        {
            return PromptForEnumInput<TEnum>(new[] { textPrompt });
        }

        /// <summary>
        /// This method goes to the Console and prompts the user to pick one of the items from the enum passed as the generic type.<br/>
        /// The textPrompt is an Enumerable containing one or more lines of text that is displayed to the user in order.<br/>
        /// It then waits for the user to type a response and press enter.<br/>
        /// If it is unable to parse the response into an enum, it will loop indefinitely until a response is parsed correctly.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="textPrompt"></param>
        /// <returns></returns>
        public static TEnum PromptForEnumInput<TEnum>(IEnumerable<string> textPrompt) where TEnum : struct, IComparable
        {
            while (true)
            {
                try
                {
                    var response =
                        PromptForIntegerInput(textPrompt.Union(System.Enum.GetValues(typeof(TEnum)).Cast<TEnum>().Select(e => $"{e.IntegerValue()}. {e.ToDescription()}")));
                    return response.CastToEnum<TEnum>();

                }
                catch (ArgumentOutOfRangeException)
                {
                    WriteLine("Unable to determine selection. Please try again.");
                }
            }
        }

        /// <summary>
        /// This method goes to the Console and prompts the user to enter a response via the keyboard.<br/>
        /// The textPrompt is one line of text that is displayed to the user.<br/>
        /// It then waits for the user to type a response and press enter.
        /// </summary>
        public static string PromptForTextInput(string textPrompt)
        {
            return PromptForTextInput(new[] { textPrompt });
        }

        /// <summary>
        /// This method goes to the Console and prompts the user to enter a response via the keyboard.<br/>
        /// The textPrompt is an Enumerable containing one or more lines of text that is displayed to the user in order.<br/>
        /// It then waits for the user to type a response and press enter.
        /// </summary>
        public static string PromptForTextInput(IEnumerable<string> textPrompt)
        {
            textPrompt.ForEach(WriteLine);
            WriteLine();
            return ReadLine();
        }
    }
}