using AMMCrawler.Core.Helpers;

namespace AMMCrawler.Core.Extensions
{
    public static class SelectorBuilderExtensions
    {
        public static LinkSelectorBuilder NotEtc(this LinkSelectorBuilder builder)
        {
            return builder
                .NotHrefEndsWith(".doc")
                .NotHrefEndsWith(".docx")
                .NotHrefEndsWith(".png")
                .NotHrefEndsWith(".flv")
                .NotHrefEndsWith(".tex")
                .NotHrefEndsWith(".zip")
                .NotHrefEndsWith(".jpg")
                .NotHrefEndsWith(".tif")
                .NotHrefEndsWith(".xls")
                .NotHrefEndsWith(".xlsx")
                .NotHrefEndsWith(".pdf")
                .NotHrefStartsWith("mailto:")
                .NotHrefAny(".php?")
                .NotHrefEndsWith(".php")
                .NotHrefAny(".asp?")
                .NotHrefEndsWith(".asp")
                .NotHrefAny(".aspx?")
                .NotHrefEndsWith(".aspx")
                .NotHrefAny(".jsp?")
                .NotHrefEndsWith(".jsp")
                .NotOnClickStartsWith("return false");
        }

        public static LinkSelectorBuilder IsEtc(this LinkSelectorBuilder builder)
        {
            return builder
                .HrefStartsWith("#")
                .OrHrefStartsWith("mailto:")
                .OrHrefEndsWith(".doc")
                .OrHrefEndsWith(".flv")
                .OrHrefEndsWith(".tex")
                .OrHrefEndsWith(".docx")
                .OrHrefEndsWith(".tif")
                .OrHrefEndsWith(".zip")
                .OrHrefEndsWith(".png")
                .OrHrefEndsWith(".jpg")
                .OrHrefEndsWith(".xls")
                .OrHrefEndsWith(".xlsx")
                .OrHrefEndsWith(".pdf")
                .OrHrefAny(".php?")
                .OrHrefEndsWith(".php")
                .OrHrefAny(".asp?")
                .OrHrefEndsWith(".asp")
                .OrHrefAny(".aspx?")
                .OrHrefEndsWith(".aspx")
                .OrHrefAny(".jsp?")
                .OrHrefEndsWith(".jsp")
                .OrHrefStartsWith("/")
                .OnClickStartsWith("return false");
        }
    }
}
