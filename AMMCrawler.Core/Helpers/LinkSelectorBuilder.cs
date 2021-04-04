using AMMCrawler.Core.Enums;
using AMMCrawler.Core.Extensions;
using System;
using System.Text;

namespace AMMCrawler.Core.Helpers
{
    public class LinkSelectorBuilder
    {
        private readonly StringBuilder _builder;
        private const string HREF_ATTR = "href";
        private const string ONCLICK_ATTR = "onclick";
        public LinkSelectorBuilder()
        {
            _builder = new StringBuilder();
            _builder.Append("'a");
        }

        public LinkSelectorBuilder NotHrefStartsWith(string selector)
        {
            return Not(HrefStartsWith, selector);
        }

        public LinkSelectorBuilder NotHrefEndsWith(string selector)
        {
            return Not(HrefEndsWith, selector);
        }

        public LinkSelectorBuilder NotHrefAny(string selector)
        {
            return Not(HrefAny, selector);
        }

        public LinkSelectorBuilder OrHrefStartsWith(string selector)
        {
            return Or().HrefStartsWith(selector);
        }

        public LinkSelectorBuilder OrHrefEndsWith(string selector)
        {
            return Or().HrefEndsWith(selector);
        }

        public LinkSelectorBuilder OrHrefAny(string selector)
        {
            return Or().HrefAny(selector);
        }

        public LinkSelectorBuilder HrefEndsWith(string selector)
        {
            return AddHrefAttribute(SelectorType.EndsWith, selector);
        }

        public LinkSelectorBuilder HrefAny(string selector)
        {
            return AddHrefAttribute(SelectorType.Any, selector);
        }

        public LinkSelectorBuilder HrefStartsWith(string selector)
        {
            return AddHrefAttribute(SelectorType.StartsWith, selector);
        }

        private LinkSelectorBuilder AddHrefAttribute(SelectorType type, string selector)
        {
            return AddAttribute(HREF_ATTR, type, selector);
        }

        public LinkSelectorBuilder NotOnClickStartsWith(string selector)
        {
            return Not(OnClickStartsWith, selector);
        }

        public LinkSelectorBuilder OnClickStartsWith(string selector)
        {
            return AddOnClickAttribute(SelectorType.StartsWith, selector);
        }

        private LinkSelectorBuilder AddOnClickAttribute(SelectorType type, string selector)
        {
            return AddAttribute(ONCLICK_ATTR, type, selector);
        }

        private LinkSelectorBuilder AddAttribute(string attr, SelectorType type, string selector)
        {
            _builder.AppendFormat("[{0}{1}=\"{2}\"]", attr, type.GetSelectorType(), selector);
            return this;
        }

        private LinkSelectorBuilder Or()
        {
            _builder.Append(",a");
            return this;
        }

        private LinkSelectorBuilder Not(Func<string, LinkSelectorBuilder> inner, string selector)
        {
            _builder.Append(":not(");
            inner(selector);
            _builder.Append(")");
            return this;
        }

        public string Build()
        {
            _builder.Append('\'');
            return _builder.ToString();
        }

        public void Clear()
        {
            _builder.Clear();
            _builder.Append("'a");
        }
    }
}
