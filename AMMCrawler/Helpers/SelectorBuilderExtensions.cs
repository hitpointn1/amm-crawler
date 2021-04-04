namespace AMMCrawler.Helpers
{
    public static class SelectorBuilderExtensions
    {
        public static LinkSelectorBuilder NotEtc(this LinkSelectorBuilder builder)
        {
            return builder
                .NotHrefEndsWith(".doc")
                .NotHrefEndsWith(".docx")
                .NotHrefEndsWith(".png")
                .NotHrefEndsWith(".jpg")
                .NotHrefEndsWith(".xls")
                .NotHrefEndsWith(".xlsx")
                .NotHrefEndsWith(".pdf")
                .NotHrefStartsWith("mailto:")
                .NotHrefAny(".php?")
                .NotHrefEndsWith(".php")
                .NotOnClickStartsWith("return false");
        }

        public static LinkSelectorBuilder IsEtc(this LinkSelectorBuilder builder)
        {
            return builder
                .HrefStartsWith("#")
                .OrHrefStartsWith("mailto:")
                .OrHrefEndsWith(".doc")
                .OrHrefEndsWith(".docx")
                .OrHrefEndsWith(".png")
                .OrHrefEndsWith(".jpg")
                .OrHrefEndsWith(".xls")
                .OrHrefEndsWith(".xlsx")
                .OrHrefEndsWith(".pdf")
                .OrHrefEndsWith(".php")
                .OrHrefAny(".php?")
                .OrHrefStartsWith("/")
                .OnClickStartsWith("return false");
        }
    }
}
