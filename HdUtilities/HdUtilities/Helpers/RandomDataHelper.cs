using System;
using System.Text.RegularExpressions;

namespace HdUtilities.Helpers
{
    public static class RandomDataHelper
    {
        private static readonly object randomLock = new object();
        private static readonly Random Random = new Random();
        /// <summary>
        /// Returns a random string of characters that is the same length as the input string provided. 
        /// If the input is null, then null is returned.
        /// If the input string is empty, then an empty string is returned.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string RandomizeIfHasValue(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }
            return RandomString(input.Length);
        }

        /// <summary>
        /// Returns a random string consisting of "ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz-0123456789" (including whitespace) that is the provided length
        /// If no length is provided, the length will be random between 1 and 35 characters
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RandomString(int? length = null)
        {
            var stringLength = length ?? RandomInteger(1, 35);
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz-0123456789";
            var stringChars = new char[stringLength];

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[RandomInteger(0, chars.Length)];
            }

            return new string(stringChars);
        }

        /// <summary>
        /// Returns a random string consisting of only digits (0-9) of the provided length
        /// </summary>
        /// <param name="length" ></param>
        /// <param name="allowLeadingWithZero">FALSE prevents 0 from being the first digit</param>
        /// <returns></returns>
        public static string RandomStringDigitsOnly(int length, bool allowLeadingWithZero = false)
        {
            var stringChars = new char[length];
//            var max = 9;
            for (int i = 0; i < stringChars.Length; i++)
            {
                var min = (i > 0 || allowLeadingWithZero) ? 0 : 1;
                stringChars[i] = RandomInteger(min, 9).ToString()[0];
            }

            return new string(stringChars);
        }

        /// <summary>
        /// Returns a random 0 or 1 value
        /// </summary>
        /// <returns></returns>
        public static string RandomBinary()
        {
            return RandomInteger(0,1).ToString();
        }

        /// <summary>
        /// Returns a boolean value calculated on the odds (0 to 100) of returning true
        /// If no value is provided, the default odds are 50/50
        /// </summary>
        /// <returns></returns>
        public static bool RandomBool(int oddsOfTrue = 50)
        {
            if (oddsOfTrue <= 0)
            {
                return false;
            }
            if (oddsOfTrue >= 100)
            {
                return true;
            }

            var number = RandomInteger(0, 1000);
            return number <= oddsOfTrue * 10;
        }

        /// <summary>
        /// Returns a random integer between the minimum and the maximum
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <returns>A random integer between the minimum and the maximum</returns>
        public static int RandomInteger(int minimum = 0, int maximum = int.MaxValue)
        {
            lock (randomLock)
            {
                var min = Math.Min(minimum, maximum);
                var max = Math.Max(minimum, maximum);
                return Random.Next(min, max);
            }
        }

        /// <summary>
        /// Returns a random double between the minimum and the maximum
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <returns>A random double between the minimum and the maximum</returns>
        public static double RandomDouble(double minimum = 0, double maximum = double.MaxValue)
        {
            lock (randomLock)
            {
                var min = Math.Min(minimum, maximum);
                var max = Math.Max(minimum, maximum);
                return Random.NextDouble()*(max - min) + min;
            }
        }

        /// <summary>
        /// Returns a random sequence of numbers that pass Phone Number validation
        /// </summary>
        /// <returns></returns>
        public static string RandomPhoneNumber()
        {
            string number = string.Empty;
            while (!IsPhoneNumber(number))
            {
                number = RandomStringDigitsOnly(10);
            }
            return number;
        }

        /// <summary>
        /// Returns true/false based on if the provided phone number meets the valid phone number criteria
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}").Success;
        }
    }
}