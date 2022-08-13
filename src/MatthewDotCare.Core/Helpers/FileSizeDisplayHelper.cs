namespace MatthewDotCare.Core.Helpers
{
    public static class FileSizeDisplayHelper
    {
        /// <summary>
        /// Formats a byte count into a readable format
        /// </summary>
        /// <remarks>
        /// Default string formatting is "0.## suffix"
        /// </remarks>
        /// <param name="bytes">The number of bytes</param>
        /// <param name="format">The format to display. {0} for the size and {1} for the suffix</param>
        /// <returns>The number of bytes in a readable format such as 2.5 GB</returns>
        public static string FromBytes(long bytes, string format = null)
        {
            var absoluteBytes = bytes < 0 ? -bytes : bytes;
            string suffix;
            double readable;

            if (absoluteBytes >= 0x1000000000000000)
            {
                suffix = "EB";
                readable = bytes >> 50;
            }
            else if (absoluteBytes >= 0x4000000000000)
            {
                suffix = "PB";
                readable = bytes >> 40;
            }
            else if (absoluteBytes >= 0x10000000000)
            {
                suffix = "TB";
                readable = bytes >> 30;
            }
            else if (absoluteBytes >= 0x40000000)
            {
                suffix = "GB";
                readable = bytes >> 20;
            }
            else if (absoluteBytes >= 0x100000)
            {
                suffix = "MB";
                readable = bytes >> 10;
            }
            else if (absoluteBytes >= 0x400)
            {
                suffix = "KB";
                readable = bytes;
            }
            else
            {
                return string.IsNullOrEmpty(format) ? bytes.ToString("0 B") : string.Format(format, bytes, "B");
            }

            // Divide by 1024 to get a fractional value
            readable /= 1024;

            return string.IsNullOrEmpty(format) ? readable.ToString("0.## ") + suffix : string.Format(format, readable, suffix);
        }
    }
}