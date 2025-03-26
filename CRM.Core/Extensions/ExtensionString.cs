using System.Text;

namespace CRM.Core.Extensions {
    public static partial class ExtensionString {

        public static bool IsNullOrWhitespace(this string str) => string.IsNullOrWhiteSpace(str);

        public static bool IsNotNullOrWhitespace(this string str) => !string.IsNullOrWhiteSpace(str);

        public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);

        public static bool IsEmptyOrWhiteSpace(this string str) => str == string.Empty && str == " ";

        public static bool IsNotNullOrEmpty(this string str) => !string.IsNullOrEmpty(str);

        public static bool IsNullOrEmptyOrWhiteSpace(this string str) => string.IsNullOrEmpty(str) && string.IsNullOrWhiteSpace(str);

        public static bool IsNotEmptyOrWhiteSpace(this string str) => str != string.Empty && str != " ";

        public static bool IsNotNullOrEmptyOrWhiteSpace(this string str) => !string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str);       

        /// <summary>
        /// String içeriğine göre İngilizce çoğul hali için -s,-es ya da -ies takısı getirir.
        /// </summary>
        /// <returns>String</returns>
        public static string GetPluralName(this string str) {
            if(str.EndsWith('y')) {
                str = str.Remove(str.Length - 1);
                return $"{str}ies";
            }
            else {
                return str.EndsWith('s') ? $"{str}es" : $"{str}s";
            }
        }

        public static string ToUpperFirstLetter(this string str) {            
            return str.Length > 1 ? $"{char.ToUpper(str[0])}{str[1..]}" : str;
        }
    }
}