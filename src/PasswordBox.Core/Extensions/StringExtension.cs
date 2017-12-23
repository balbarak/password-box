using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PasswordBox.Core.Extensions
{
    public static class StringExtension
    {
        public static string Brief(this string word, int maxSize)
        {
            if (String.IsNullOrWhiteSpace(word))
                return "";


            if (word.Length > maxSize)
                word = word.Substring(0, maxSize) + " ...";

            return word;
        }

        public static string StripHtml(this string text)
        {
            string result = Regex.Replace(text, "<(.|\\n)*?>", string.Empty);
            result = result.Replace("&nbsp;", " ");
            return result;
        }

    }
}
