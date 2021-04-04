using AMMCrawler.Enums;

namespace AMMCrawler.Helpers
{
    public static class EnumExtensions
    {
        public static string GetSelectorType(this SelectorType selectorType)
        {
            switch (selectorType)
            {
                case SelectorType.Default:
                    return string.Empty;
                case SelectorType.Any:
                    return "*";
                case SelectorType.EndsWith:
                    return "$";
                case SelectorType.StartsWith:
                    return "^";
                default:
                    return string.Empty;
            }
        }
    }
}
