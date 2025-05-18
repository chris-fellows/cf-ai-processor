using System.Drawing;

namespace CFAIProcessor.UI.Utilities
{
    public static class HTMLUtilities
    {
        public static string GetHTMLColor(string color)
        {
            if (color.StartsWith("#")) return color;

            if (Int32.TryParse(color, out var colorValue))
            {
                return ColorTranslator.ToHtml(Color.FromArgb(colorValue));
            }
            return color;
        }
    }
}
