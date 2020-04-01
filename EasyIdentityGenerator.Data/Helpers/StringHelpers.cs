using System.Text.RegularExpressions;

namespace EasyIdentityGenerator.Data.Helpers
{
    public static class StringHelpers
    {
        /// <summary>
        /// Uses regex '\b' as suggested in https://stackoverflow.com/questions/6143642/way-to-have-string-replace-only-hit-whole-words
        /// </summary>
        /// <param name="original"></param>
        /// <param name="wordToFind"></param>
        /// <param name="replacement"></param>
        /// <param name="regexOptions"></param>
        /// <returns></returns>
        public static string ReplaceWholeWord(this string original, string wordToFind, string replacement, RegexOptions regexOptions = RegexOptions.None)
        {
            var pattern = $@"\b{wordToFind}\b";
            var ret = Regex.Replace(original, pattern, replacement, regexOptions);
            return ret;
        }
    }
}