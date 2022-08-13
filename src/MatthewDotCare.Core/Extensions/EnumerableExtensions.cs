using System.Collections;

namespace MatthewDotCare.Core.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Tests to see if an enumerable contains no elements
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="input">An enumerable</param>
        /// <returns></returns>
        public static bool None<TSource>(this IEnumerable<TSource> input) => !input.Any();

        /// <summary>
        /// Tests to see if an enumerable contains no elements with a given predicate
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="input">An enumerable</param>
        /// <param name="predicate"></param>
        public static bool None<TSource>(this IEnumerable<TSource> input, Func<TSource, bool> predicate) =>
            !input.Any(predicate);

        /// <summary>
        /// Tests to see if an enumerable is null or empty
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="input">An enumerable</param>
        public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> input)
        {
            return input == null ||
                   input is ICollection collection && collection.Count == 0 ||
                   input.None();
        }

        /// <summary>
        /// Tests to see if an enumerable is not null or empty
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="input">An enumerable</param>
        public static bool IsNotNullOrEmpty<TSource>(this IEnumerable<TSource> input)
        {
            return !input.IsNullOrEmpty();
        }
    }
}