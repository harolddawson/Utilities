using System;
using System.Linq;
using System.Reflection;
using HdUtilities.Attributes.Enum;
using HdUtilities.Helpers;

namespace HdUtilities.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Returns an array of attributes of the specified type applied to the given enum value.
        /// </summary>
        public static T[] GetAttributes<T>(this IComparable input) where T : Attribute
        {
            var attributes = (T[])input.GetType()
                                        .GetTypeInfo()
                                        .GetDeclaredField(input.ToString())
                                        .GetCustomAttributes(typeof(T),false);
            return attributes;
        }

        /// <summary>
        /// Returns the string constant associated with the specified enum value, or the value's
        /// .ToString() result if no string constant attribute is applied.
        /// </summary>
        public static string ToStringConstant(this IComparable input)
        {
            var attribute = input.GetAttributes<StringConstantAttribute>().FirstOrDefault();
            return (attribute != null)
                       ? attribute.GetStringConstant()
                       : input.ToString();
        }

        /// <summary>
        /// Returns the description associated with the specified enum value, or the value's
        /// .ToString() result if no string constant attribute is applied.
        /// </summary>
        public static string ToDescription(this IComparable input)
        {
            var attribute = input.GetAttributes<DescriptionAttribute>().FirstOrDefault();
            return (attribute != null)
                       ? attribute.GetDescription()
                       : input.ToString();
        }

        public static bool IsEmpty<T>(this string value)
        {
            var members =
                typeof (T).GetTypeInfo()
                    .DeclaredFields.FirstOrDefault(
                        f =>
                            f.GetCustomAttributes(typeof (StringConstantAttribute), false)
                                .Any(a => ((StringConstantAttribute) a)
                                    .GetStringConstant() == value));
            return members == null;
        }

        public static bool IsNotEmpty<T>(this string value)
        {
            return !value.IsEmpty<T>();
        }

        public static bool DoesMatchAny<T>(this T input, params T[] values)
        {
            return values.Any(e => e.Equals(input));
        }

        public static bool DoesNotMatchAny<T>(
            this T input,
            params T[] values)
        {
            return !values.Any(e => e.Equals(input));
        }

        public static T FromStringConstant<T>(this string value) where T : struct
        {
            var option = typeof(T).GetTypeInfo()
                                   .DeclaredFields
                                   .FirstOrDefault(
                                       f => f.GetCustomAttributes(
                                           typeof(StringConstantAttribute),
                                           false)
                                             .Any(a => ((StringConstantAttribute)a).GetStringConstant() == value));
            return option == null
                       ? EnumHelpers.Default<T>()
                       : Enum<T>.Parse(option.Name);
        }

        public static bool DoesMatchAny<TEnum>(
            this string input,
            params TEnum[] values)
            where TEnum : struct
        {
            return FromStringConstant<TEnum>(input)
                .DoesMatchAny(values);
        }

        public static bool DoesNotMatchAny<TEnum>(
            this string input,
            params TEnum[] values)
            where TEnum : struct
        {
            return !input.DoesMatchAny(values);
        }

        public static bool Matches<TEnum>(
            this string input,
            TEnum value)
            where TEnum : struct
        {
            return FromStringConstant<TEnum>(input)
                .DoesMatchAny(value);
        }


    }
}