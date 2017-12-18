using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HdUtilities.Extensions
{
    public static class StringExtensions
    {
        public static bool MatchesAny(this string input, params string[] strings)
        {
            return strings.Any(s => (s.Safe().Equals(input.Safe())));
        }

        public static bool MatchesAnyIgnoreCase(this string stringToCheck, params string[] stringsToMatch)
        {
            return stringToCheck.Safe().ToLower().MatchesAny(stringsToMatch.Select(s => s.ToLower()).ToArray());
        }

        public static bool ContainsAnyIgnoreCase(this string stringToCheck, params string[] stringsToMatch)
        {
            if (string.IsNullOrWhiteSpace(stringToCheck))
            {
                return stringsToMatch?.Any(string.IsNullOrWhiteSpace) ?? true;
            }

            return stringsToMatch.Any(s => stringToCheck.ToLower().Contains(s.ToLower()));
        }

        public static bool ContainsAll(this string stringToCheck, bool ignoreCase, params string[] stringsToMatch)
        {
            if (string.IsNullOrWhiteSpace(stringToCheck))
            {
                return stringsToMatch?.Any(string.IsNullOrWhiteSpace) ?? true;
            }
            if (ignoreCase)
            {
                return stringsToMatch.Where(s => !string.IsNullOrWhiteSpace(s)).All(s => stringToCheck.ToLower().Contains(s.ToLower()));
            }
            return stringsToMatch.Where(s => !string.IsNullOrWhiteSpace(s)).All(stringToCheck.Contains);
        }

        public static bool IsNullOrWhitespace(this string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }

        public static bool IsNotNullOrWhitespace(this string input)
        {
            return !string.IsNullOrWhiteSpace(input);
        }

        public static bool IsNullOrEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }

        public static string Squeeze(this string input)
        {
            if (input.IsNullOrWhitespace())
            {
                return string.Empty;
            }
            var stringSections = input.Split(' ');

            return stringSections.Aggregate((a, s) => a.Trim() + " " + s.Trim());
        }

        public static int? ToIntOrNull(this string input)
        {
            int result;

            if (int.TryParse(input, out result))
            {
                return result;
            }
            return null;
        }

        public static int ToIntOrDefault(this string input)
        {
            int result;

            if (int.TryParse(input, out result))
            {
                return result;
            }
            return default(int);
        }

        public static long ToLongOrDefault(this string input)
        {
            long result;

            if (long.TryParse(input, out result))
            {
                return result;
            }
            return default(long);
        }

        public static long? ToLongOrNull(this string input)
        {
            long result;

            if (long.TryParse(input, out result))
            {
                return result;
            }
            return null;
        }

        public static string GetMad97CheckSum(this string barcode)
        {
            var checksum = 0;
            var asciiBytes = Encoding.ASCII.GetBytes(barcode);
            for (var i = 0; i < barcode.Length; i++)
            {
                if (i == 0)
                {
                    checksum = asciiBytes[i] * 1000;
                    continue;
                }

                checksum = ((checksum + asciiBytes[i]) % 97);

                if (i != (barcode.Length - 1))
                {
                    checksum *= 1000;
                }
            }
            return checksum.ToString().PadLeft(2, '0');
        }
        public static bool SqueezeEquals(this string input, string other)
        {
            return input.Safe().Squeeze().Equals(other.Safe().Squeeze());
        }

        public static string JoinStrings(this IEnumerable<string> input,string delimiter = " ")
        {
            if (input.IsEmptyOrNull())
            {
                return string.Empty;
            }
            return string.Join(delimiter,input.Select(s => s.Safe())).Squeeze();
        }

        public static string Concatenate(this string input,params string[] strings)
        {
            if (strings.IsEmptyOrNull())
            {
                return input.Safe();
            }
            return string.Join(string.Empty, input.Safe(), string.Concat(strings));
        }
    }
}