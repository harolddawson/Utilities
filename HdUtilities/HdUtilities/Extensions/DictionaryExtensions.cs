using System.Collections.Generic;
using System.Linq;

namespace HdUtilities.Extensions
{
    public static class DictionaryExtensions
    {
        public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(
                    key,
                    value);
            }
        }

        public static void AddOrUpdateRange<TKey, TValue>(
            this Dictionary<TKey, TValue> dictionary,
            Dictionary<TKey, TValue> dictionaryToAppend)
        {
            foreach (var entry in dictionaryToAppend)
            {
                dictionary.AddOrUpdate(entry.Key,entry.Value);
            }
        }

        public static string GetValue(
            this Dictionary<string, string> dictionary, 
            string key,
            string defaultValue = null)
        {

            if (key.IsNullOrEmpty()) return defaultValue ?? string.Empty;

            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }

            return defaultValue ?? string.Empty;
        }

        public static TValue GetValue<TKey, TValue>(
            this Dictionary<TKey, TValue> dictionary,
            TKey key,
            TValue defaultValue = default(TValue))
        {
            if (Equals(
                key,
                default(TKey)))
            {
                return defaultValue;
            }

            return dictionary.ContainsKey(key)
                       ? dictionary[key]
                       : defaultValue;
        }

        public static Dictionary<TKey, TValue> GetDifferences<TKey, TValue>(
            this Dictionary<TKey, TValue> start,
            Dictionary<TKey, TValue> ending)
        {
            var modifiedAttributes = new Dictionary<TKey, TValue>();
            if (start == null || !start.Any())
            {
                foreach (var keyValuePair in ending.Where(kvp => !Equals(kvp.Value, default(TValue))))
                {
                    modifiedAttributes.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
            else
            {
                foreach (var keyValuePair in ending)
                {
                    TValue baseValue;
                    start.TryGetValue(keyValuePair.Key, out baseValue);

                    if (!Equals(keyValuePair.Value,baseValue))
                    {
                        modifiedAttributes.Add(
                            keyValuePair.Key,
                            keyValuePair.Value);
                    }
                }
            }

            return modifiedAttributes;
        } 

        public static Dictionary<string, string> GetDifferences(this Dictionary<string, string> start,
            Dictionary<string, string> ending)
        {
            var modifiedAttributes = new Dictionary<string, string>();
            if (start == null || !start.Any())
            {
                foreach (var keyValuePair in ending.Where(kvp => !string.IsNullOrWhiteSpace(kvp.Value)))
                {
                    modifiedAttributes.Add(
                        keyValuePair.Key,
                        keyValuePair.Value);
                }
            }
            else
            {
                foreach (var keyValuePair in ending)
                {
                    string baseValue;
                    var baseHasValue = start.TryGetValue(keyValuePair.Key, out baseValue);

                    if ((!baseHasValue && !string.IsNullOrWhiteSpace(keyValuePair.Value))
                        || !baseValue.SqueezeEquals(keyValuePair.Value))
                    {
                        modifiedAttributes.Add(
                            keyValuePair.Key,
                            keyValuePair.Value);
                    }
                }
            }
            return modifiedAttributes;
        }

    }
}