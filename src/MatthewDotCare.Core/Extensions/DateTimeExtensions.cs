namespace MatthewDotCare.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime OrIfMinValue(this DateTime dateTime, DateTime fallbackDateTime)
        {
            return dateTime == DateTime.MinValue ? fallbackDateTime : dateTime;
        }
    }
}