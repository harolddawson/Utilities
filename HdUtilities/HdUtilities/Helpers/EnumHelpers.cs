using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HdUtilities.Attributes.Enum;

namespace HdUtilities.Helpers
{
    public static class EnumHelpers
    {
        public static IList<string> StringConstants<T>() where T : struct
        {
            return typeof (T).GetTypeInfo()
                             .DeclaredFields
                             .Select(f => f.EnumStringConstantAttribute())
                             .Where(a => a != null)
                             .Select(a => a.GetStringConstant())
                             .ToList();
        }

        public static IList<T> List<T>() where T : struct
        {
            return System.Enum.GetValues(typeof (T))
                       .Cast<T>()
                       .ToList();
        }

        public static T Default<T>() where T : struct
        {
            var option = typeof (T).GetTypeInfo()
                                   .DeclaredFields
                                   .FirstOrDefault(f => f.GetCustomAttributes(typeof (DefaultAttribute),false).Any());
            return Enum<T>.Parse(option.Name);
        }

        private static StringConstantAttribute EnumStringConstantAttribute(this MemberInfo fieldInfo)
        {
            return (StringConstantAttribute) fieldInfo.GetCustomAttributes(typeof (StringConstantAttribute),false).FirstOrDefault();
        }
    }

    public static class Enum<T> where T : struct
    {
        public static T Parse(string value)
        {
            T output;
            if (!System.Enum.TryParse(value,true,out output))
            {
                return EnumHelpers.Default<T>();
            }
            return output; //(T) System.Enum.Parse(typeof (T), value);
        }
    }
}