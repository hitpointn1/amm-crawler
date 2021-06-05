namespace AMMCrawler.Core.Helpers
{
    public static class StringExtensions
    {
        public static string Remove(this string main, string toRemove)
        {
            if (main is null)
                return main;

            int index = main.IndexOf(toRemove);

            if (index == -1)
                return main;

            string result = main.Substring(index + toRemove.Length);

            return result.Length > 0 ? result : main;
        }

        public static string ReturnBeforeFirstSlash(this string main)
        {
            if (main is null)
                return main;

            int index = main.IndexOf('/');

            if (index == -1)
                return main;

            string result = main.Substring(0, index);

            return result.Length > 0 ? result : main;
        }
    }
}
