using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;


namespace wizardscode.extension
{
    public static class StringExtension
    {
        /// <summary>
        /// Remove all leading and spaces.
        /// </summary>
        public static string TrimAndReduce(this string str)
        {
            return ConvertWhitespacesToSingleSpaces(str).Trim();
        }

        public static string ConvertWhitespacesToSingleSpaces(this string value)
        {
            return Regex.Replace(value, @"\s+", " ");
        }

        /// <summary>
        /// Break a CamelCase string into individual words, separated by spaces.
        /// </summary>
        /// <returns></returns>
        public static string BreakCamelCase(this string value)
        {
            return Regex.Replace(value, "(\\B[A-Z])", " $1");
        }

        /// <summary>
        /// Take a string with underscores and camel case and prettify it for display by breaking into words.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Prettify(this string value)
        {
            string result = value.Replace("_", " ");
            result.BreakCamelCase();
            result.TrimAndReduce();
            return result;
        }
    }
}
