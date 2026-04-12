using Ganss.Xss;
using System.Text.Encodings.Web;

namespace Ewan.Application.Helpers
{
    public static class TextToHtmlConverter
    {
        private static readonly HtmlSanitizer Sanitizer = BuildSanitizer();

        public static string ConvertPlainTextToHtml(string? content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return string.Empty;

            var normalizedInput = content.Trim();
            var html = LooksLikeHtml(normalizedInput)
                ? normalizedInput
                : ConvertTextToParagraphs(normalizedInput);

            return Sanitizer.Sanitize(html);
        }

        private static string ConvertTextToParagraphs(string content)
        {
            var encoded = HtmlEncoder.Default.Encode(content);
            var normalized = encoded.Replace("\r\n", "\n").Replace("\r", "\n");
            var paragraphs = normalized.Split("\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            if (paragraphs.Length == 0)
                return string.Empty;

            return string.Join(string.Empty, paragraphs.Select(x => $"<p>{x}</p>"));
        }

        private static bool LooksLikeHtml(string content)
            => content.Contains('<') && content.Contains('>');

        private static HtmlSanitizer BuildSanitizer()
        {
            var sanitizer = new HtmlSanitizer();

            sanitizer.AllowedTags.Clear();
            sanitizer.AllowedTags.Add("h1");
            sanitizer.AllowedTags.Add("h2");
            sanitizer.AllowedTags.Add("h3");
            sanitizer.AllowedTags.Add("h4");
            sanitizer.AllowedTags.Add("h5");
            sanitizer.AllowedTags.Add("h6");
            sanitizer.AllowedTags.Add("span");
            sanitizer.AllowedTags.Add("p");

            sanitizer.AllowedAttributes.Clear();
            sanitizer.AllowedCssProperties.Clear();
            sanitizer.AllowedSchemes.Clear();

            return sanitizer;
        }
    }
}
