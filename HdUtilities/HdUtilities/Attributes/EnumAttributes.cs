using System;

namespace HdUtilities.Attributes
{
    namespace Enum
    {
        /// <summary>
        /// Declares that a class instance maps to a specific string constant, and defines
        /// a method for getting that constant. This is commonly used by attribute classes
        /// that are used with Enums to associate an enum instance with a string representation
        /// that is stored in a database.
        /// </summary>
        public interface IStringConstant
        {
            string GetStringConstant();
        }

        /// <summary>
        /// Maps an enum to a string constant.
        /// </summary>
        [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
        public class StringConstantAttribute
            : Attribute,
              IStringConstant
        {
            private readonly string value;

            public StringConstantAttribute(string constant)
            {
                value = constant;
            }

            public string GetStringConstant()
            {
                return value;
            }
        }

        [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
        public class DefaultAttribute : Attribute
        {
        }

        /// <summary>
        /// Maps an enum to a string constant.
        /// </summary>
        [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
        public class DescriptionAttribute : Attribute
        {
            private readonly string value;

            public DescriptionAttribute(string description)
            {
                value = description;
            }

            public string GetDescription()
            {
                return value;
            }
        }
    }
}