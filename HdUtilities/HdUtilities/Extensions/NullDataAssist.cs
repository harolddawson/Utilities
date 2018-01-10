namespace HdUtilities.Extensions
{
    public static class NullDataAssist
    {
        public static string SafeToUpper(string text)
        {
            return text.Safe()
                       .ToUpper();
        }

        public static bool SafeBool(bool? nullableFlag)
        {
            return nullableFlag.GetValueOrDefault();
        }

        public static string Safe(this string inputString)
        {
            var retInputString = inputString ?? string.Empty;
            return retInputString.Squeeze();
        }

        public static bool HasValueAndIsTrue(this bool? input)
        {
            return SafeBool(input);
        }
    }
}