using System;
using System.Collections.Generic;
using System.Linq;

namespace HdUtilities.Extensions
{
    public static class CollectionExtensions
    {
        public static void RemoveAllMatching<T>(
            this ICollection<T> collection,
            IEnumerable<T> matchesToRemove)
        {
            var itemsToRemove = collection.Where(matchesToRemove.Contains).ToList();

            foreach (var item in itemsToRemove)
            {
                collection.Remove(item);
            }

        }

        public static void AddAll<T>(
            this ICollection<T> collection,
            IEnumerable<T> itemsToAdd)
        {
            foreach (var item in itemsToAdd)
            {
                collection.Add(item);
            }
        }

        public static void RemoveAllMatching<T>(this ICollection<T> collection,
                                             Func<T, bool> matchesToRemove)
        {
            var itemsToRemove = collection.Where(matchesToRemove).ToList();

            foreach (var item in itemsToRemove)
            {
                collection.Remove(item);
            }
        }

        public static void AddDistinctRange<T>(
            this ICollection<T> list,
            IEnumerable<T> itemsToAdd)
        {
            foreach (var item in itemsToAdd.Where(i => !list.Contains((T) i)))
            {
                list.Add(item);
            }
        }
    }
}