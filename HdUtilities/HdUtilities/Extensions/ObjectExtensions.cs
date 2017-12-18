using System;
using System.Collections.Generic;
using System.Linq;

namespace HdUtilities.Extensions
{
    public static class ObjectExtensions
    {
        public static T[] ToArrayOfOne<T>(this T thing)
        {
            return new[] {thing};
        }

        public static List<T> ToListOfOne<T>(this T thing)
        {
            return new List<T>{ thing };
        }

        public static object GetDefaultValue(this Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }
        /// <summary>
        /// This method creates a new object of type TOut. 
        /// Then it copies values of any properties that have the same names between the objects 
        /// if the types match, can be parsed, or converted to the destination type. 
        /// The destination type MUST have a parameterless constructor.
        /// </summary>
        /// <typeparam name="TOut">Type to be created. Must have parameterless constructor</typeparam>
        /// <typeparam name="TIn">Type of source object</typeparam>
        /// <param name="inputObj"></param>
        /// <returns></returns>
        public static TOut CreateNewObjWithCopiedValues<TOut, TIn>(this TIn inputObj) where TOut : class, new()
        {
            var outputObj = new TOut();
            CopyValuesToObjectFromMatchingProperies(inputObj, outputObj);
            return outputObj;
        }

        /// <summary>
        /// This method walks through the source object and copies the value to the properties on the destination object with the same name, 
        /// attempting to cast values if possible. If the destination type is <seealso cref="System.string"/> 
        /// and the source is not, the copied value will be the result of obj.ToString().
        /// </summary>
        /// <typeparam name="TDest"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="inputObj"></param>
        /// <param name="destinationObj"></param>
        public static void CopyValuesToObjectFromMatchingProperies<TDest, TSource>(TSource inputObj, TDest destinationObj)
        {
            if (inputObj == null) return;
            if (destinationObj == null)
            {
                throw new ArgumentNullException(nameof(destinationObj));
            }

            var inputProps = typeof(TSource).GetProperties();
            var outputProps = typeof(TDest).GetProperties();

            foreach (var sourceProp in inputProps)
            {
                var destinationProp =
                    outputProps.FirstOrDefault(p => string.Equals(p.Name, sourceProp.Name, StringComparison.CurrentCultureIgnoreCase));
                if (destinationProp == null)
                {
                    continue;
                }

                //Same type is easy, just copy it
                if (destinationProp.PropertyType == sourceProp.PropertyType)
                {
                    destinationProp.SetValue(destinationObj, sourceProp.GetValue(inputObj));
                }
                //Destination type is string, but source type is not. Copy the source value.ToString() to the destination
                else if (destinationProp.PropertyType == typeof(string))
                {
                    destinationProp.SetValue(destinationObj, sourceProp.GetValue(inputObj)?.ToString());
                }
                //Destination is a nullable variation of source, just copy from the source to destination
                else if (destinationProp.PropertyType.IsOfTypeOrNullableType(sourceProp.PropertyType))
                {
                    destinationProp.SetValue(destinationObj, sourceProp.GetValue(inputObj));
                }
                //Source is nullable variation of destination
                else if (sourceProp.PropertyType.IsOfTypeOrNullableType(destinationProp.PropertyType))
                {
                    //Copy the source value, unless it's null. Then use the default of the value type
                    destinationProp.SetValue(destinationObj,
                        sourceProp.GetValue(inputObj) ?? destinationProp.PropertyType.GetDefaultValue());
                }
                else if (sourceProp.PropertyType.IsEnum && destinationProp.PropertyType.IsOfTypeOrNullableType<int>())
                {
                    //What do I do with this?
                }
                //This means both are Enums, but different types of enums
                else if (destinationProp.PropertyType.IsEnum && sourceProp.PropertyType.IsEnum)
                {
                    //What do I do with this?
                }
                else 
                {
                    //Stringify the value and attempt to cast it based on the destination property type
                    var stringValue = sourceProp.GetValue(inputObj)?.ToString();
                    //Attempt to cast numbers
                    if (destinationProp.PropertyType.IsOfTypeOrNullableType<float>())
                    {
                        float f;
                        if (float.TryParse(stringValue, out f))
                        {
                            destinationProp.SetValue(destinationObj, f);
                        }
                    }
                    else if (destinationProp.PropertyType.IsOfTypeOrNullableType<int>())
                    {
                        int i;
                        if (int.TryParse(stringValue, out i))
                        {
                            destinationProp.SetValue(destinationObj, i);
                        }
                    }
                    else if (destinationProp.PropertyType.IsOfTypeOrNullableType<double>())
                    {
                        double dou;
                        if (double.TryParse(stringValue, out dou))
                        {
                            destinationProp.SetValue(destinationObj, dou);
                        }
                    }
                    else if (destinationProp.PropertyType.IsOfTypeOrNullableType<decimal>())
                    {
                        decimal dec;
                        if (decimal.TryParse(stringValue, out dec))
                        {
                            destinationProp.SetValue(destinationObj, dec);
                        }
                    }
                    //If we get here, it means that the property types don't match, the destination isn't a string, and I can't figure out how to cast it.
                    else
                    {
                        var message =
                            $"Unable to cast Source Type: {sourceProp.PropertyType} to  Destination Type: {destinationProp.PropertyType}";
                        throw new InvalidCastException(message);
                        //What do we do now?
                    }
                }
            }
        }

        public static bool IsEnumOrNullableEnum<T>()
        {
            return IsEnumOrNullableEnum(typeof (T));
        }

        public static bool IsEnumOrNullableEnum(this Type type)
        {
            return (Nullable.GetUnderlyingType(type) ?? type).IsEnum;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsOfTypeOrNullableType<T>(this Type type) where T : struct
        {
            return type.IsOfTypeOrNullableType(typeof(T));
        }

        /// <summary>
        /// This compares the input type to the parameter and determines if they are the same type or if the input type is a nullable variation of the comparison type. 
        /// If the comparison type is nullable, the input type must also be a nullable of the same type to return true.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public static bool IsOfTypeOrNullableType(this Type type, Type comparison)
        {
            if (type == null || comparison == null)
            {
                return false;
            }
            
            return type == comparison || (Nullable.GetUnderlyingType(type) ?? type) == comparison;
        }
    }
}