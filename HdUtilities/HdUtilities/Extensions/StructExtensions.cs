using System;
using System.Linq;

namespace HdUtilities.Extensions
{
    public static class StructExtensions
    {
        /// <summary>
        /// This extension will cast an integer value to the enum passed as <seealso cref="TEnum"/>
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="input"></param>
        /// <exception cref="InvalidCastException">Thrown when TEnum is not an enum</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the integer value passed does not fall within the range of values for <seealso cref="TEnum"/></exception>
        /// <returns></returns>
        public static TEnum CastToEnum<TEnum>(this int input) where TEnum : struct, IComparable
        {
            if (!typeof (TEnum).IsEnum)
            {
                throw new InvalidCastException($"{typeof(TEnum)} is not valid to be cast to an enum.");
            }
            if (input.IsContainedWithin<TEnum>())
            {
                return (TEnum)(object)input;
            }
            throw new ArgumentOutOfRangeException($"{input} is out of range to cast to {typeof(TEnum)}");
        }
       
        public static T MaxValue<T>() where T : struct, IComparable
        {
            if (!typeof(T).IsEnum)
            {
                throw new InvalidCastException($"{typeof(T)} is not valid to be cast to an enum.");
            }
            return System.Enum.GetValues(typeof(T)).Cast<T>().Max();
        }
        public static T MinValue<T>() where T : struct, IComparable
        {
            if (!typeof(T).IsEnum)
            {
                throw new InvalidCastException($"{typeof(T)} is not valid to be cast to an enum.");
            }
            return System.Enum.GetValues(typeof(T)).Cast<T>().Min();
        }

        public static bool IsContainedWithin<T>(this int number) where T : struct, IComparable
        {
            if (!typeof(T).IsEnum)
            {
                throw new InvalidCastException($"{typeof(T)} is not valid to be cast to an enum.");
            }
            var min = MinValue<T>().IntegerValue();
            var max = MaxValue<T>().IntegerValue();
            return number.IsBetween(min, max, true);
        }

        /// <summary>
        /// Converts the enum into its respective integer value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputEnum"></param>
        /// <returns></returns>
        public static int IntegerValue<T>(this T inputEnum) where T : struct, IComparable
        {
            if (!typeof(T).IsEnum)
            {
                throw new InvalidCastException($"{typeof(T)} is not valid to be cast to an enum.");
            }
            return (int)System.Enum.Parse(typeof(T), inputEnum.ToString());
        }
        
        public static bool IsEven(this int number)
        {
            return number % 2 == 0;
        }

        public static bool IsOdd(this int number)
        {
            //If it ain't even, it's odd!
            return !number.IsEven();
        }

        public static bool IsPositive(this double number)
        {
            return number > 0;
        }

        public static bool IsNegative(this double number)
        {
            return number < 0;
        }

        public static bool IsBetween(this int number, int min, int max, bool inclusive)
        {
            return (number > min && number < max) 
                || (inclusive && (number == min || number == max));
        }

        public static bool IsBetween(this double number, double min, double max, bool inclusive)
        {
            if (inclusive)
            {
                return number >= min && number <= max;
            }

            return number > min && number < max;
        }
    }
}