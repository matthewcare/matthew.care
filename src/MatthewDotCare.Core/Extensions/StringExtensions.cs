namespace MatthewDotCare.Core.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Removes whitespace from a string, including newlines and tabs.
        /// </summary>
        /// <param name="value">The string to remove whitespace from</param>
        /// <returns>The input string with whitespace removed</returns>
        public static string RemoveWhiteSpace(this string value) => value == null ? null : string.Concat(value.Split());
    }
}
