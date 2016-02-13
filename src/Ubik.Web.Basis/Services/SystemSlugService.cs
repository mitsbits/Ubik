using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ubik.Web.Basis.Contracts;

namespace Ubik.Web.Basis.Services
{
    public class SystemSlugService : ISlugifier
    {
        private readonly IEnumerable<ISlugWordReplacer> _wordReplacers;
        private readonly IEnumerable<ISlugCharOmmiter> _charOmmiters;
        private readonly IEnumerable<IInternationalCharToAsciiProvider> _asciiProviders;

        public SystemSlugService(IEnumerable<ISlugWordReplacer> wordReplacers, IEnumerable<ISlugCharOmmiter> charOmmiters, IEnumerable<IInternationalCharToAsciiProvider> asciiProviders)
        {
            _wordReplacers = wordReplacers;
            _charOmmiters = charOmmiters;
            _asciiProviders = asciiProviders;
        }

        public string SlugifyText(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;

            text = StripPunctuation(WordReplace(text));

            var len = text.Length;
            var prevdash = false;
            var sb = new StringBuilder(len);
            char c;

            for (int i = 0; i < len; i++)
            {
                c = text[i];
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    sb.Append(c);
                    prevdash = false;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    // tricky way to convert to lowercase
                    sb.Append((char)(c | 32));
                    prevdash = false;
                }
                else if (CharOmmit(c))
                {
                    if (!prevdash && sb.Length > 0)
                    {
                        sb.Append('-');
                        prevdash = true;
                    }
                }
                else if (c >= 128)
                {
                    int prevlen = sb.Length;
                    sb.Append(CharToAscii(c));
                    if (prevlen != sb.Length) prevdash = false;
                }
            }

            return prevdash ? sb.ToString().ToLower().Substring(0, sb.Length - 1) : sb.ToString().ToLower();
        }

        public string SlugifyDate(DateTime date)
        {
            return date.Ticks.ToString();
        }

        public DateTime DateFromSlug(string slug)
        {
            return new DateTime(long.Parse(slug));
        }

        private string WordReplace(string source)
        {
            return _wordReplacers.Aggregate(source, (current, slugWordReplacer) => slugWordReplacer.Replace(current));
        }

        private bool CharOmmit(char source)
        {
            return _charOmmiters.Any(slugCharOmmiter => slugCharOmmiter.Ommit(source));
        }

        private char[] CharToAscii(char source)
        {
            var hits = _asciiProviders.Where(x => x.Reference.ContainsKey(source)).ToList();
            return !hits.Any() ? new[] { source } : hits.Last().Remap(source);
        }

        private static string StripPunctuation(string source)
        {
            var sb = new StringBuilder();
            foreach (var c in source)
            {
                if (!char.IsPunctuation(c))
                    sb.Append(c);
            }
            return sb.ToString();
        }
    }
}