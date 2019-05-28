using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;


namespace wizardscode.extension
{
    public static class StringExtension
    {
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
    }
}
