using System.Text.RegularExpressions;

namespace BlogSimple.Web.Services;

public static class UtilityMethods
{
    public static int GetWordCount(string html)
    {
        // Remove HTML tags using regular expression
        string plainText = Regex.Replace(html, "<.*?>", "");

        // Remove punctuation and get the array of words
        string[] words = plainText.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                                  .Select(word => new string(word.Where(c => !char.IsPunctuation(c)).ToArray()))
                                  .Where(word => !string.IsNullOrEmpty(word))
                                  .ToArray();

        // Count the number of words
        int wordCount = words.Length;

        return wordCount;
    }
}
