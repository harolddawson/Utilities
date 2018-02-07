using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HdUtilities.Extensions
{
    public static class EnumerableExtensions
    {
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> enumerable)
        {
            return new HashSet<T>(enumerable);
        }

        public static bool IsEmptyOrNull(this IEnumerable enumerable)
        {
            return enumerable.Cast<object>().IsEmptyOrNull();
        }

        public static bool IsEmptyOrNull<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                return true;
            }
            return !enumerable.Any();
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action.Invoke(item);
            }
        }

        public static int MaxIndex<T>(this IList<T> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (!list.Any())
            {
                return -1;
            }

            return list.IndexOf(list.Last());
        }

        public static int MaxIndex<T>(this T[] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (!array.Any())
            {
                return -1;
            }

            return Array.IndexOf(array, array.Last());
        }

        public static List<T> ToDistinctList<T>(this IEnumerable<T> list)
        {
            return list.Distinct().ToList();
        }


        public static List<TOut> IterateAndReportWithOutput<TIn, TOut>(this IEnumerable<TIn> enumerable, Func<TIn, TOut> action,
            Action<string> reportProgress)
        {
            var result = new ConcurrentBag<TOut>();
            enumerable.IterateAndReport(u => result.Add(action.Invoke(u)), reportProgress);
            return result.ToList();
        }

        public static void IterateAndReport<T>(this IEnumerable<T> enumerable, Action<T> action, Action<string> reportProgress, int maxDegreeOfParallelism = 3)
        {
            var collection = enumerable.ToList();
            var reportIncrement = 0;
            if (collection.Count.IsBetween(0, 10, true))
            {
                reportIncrement = 1;
            }
            else if (collection.Count.IsBetween(10, 200, true))
            {
                reportIncrement = 10;
            }
            else if (collection.Count.IsBetween(200, 1000, true))
            {
                reportIncrement = 50;
            }
            else if (collection.Count.IsBetween(1000, 25000, true))
            {
                reportIncrement = 100;
            }
            else
            {
                reportIncrement = 1000;
            }


            int uCount = 0;
            var loopCounterLock = new object();
            Parallel.ForEach(collection,
                new ParallelOptions() {MaxDegreeOfParallelism = maxDegreeOfParallelism},
                u =>
                {
                    action.Invoke(u);
                    lock (loopCounterLock)
                    {
                        uCount++;
                        if (uCount%reportIncrement == 0)
                        {
                            reportProgress?.Invoke($"{uCount} completed out of {collection.Count}");
                        }
                    }
                }
                );
        }

        public static bool None<TSource>(this IEnumerable<TSource> source)
        {
            var iCollection = source as ICollection<TSource>;
            if (iCollection != null)
            {
                return iCollection.Count == 0;
            }
            return !source.Any();
        }

        public static bool None<TSource>(this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            return !source.Any(predicate);
        }
    }
}