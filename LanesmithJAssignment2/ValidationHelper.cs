using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LanesmithJAssignment2
{
    internal class ValidationHelper
    {
        public static string Capitalize(string value)
        {
            //Trim string and return empty string if null or empty
            value = value.Trim();
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            //Convert string to char array and loop through making all letters lower case
            char[] chars = value.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] > 64 && chars[i] < 91 ) 
                {
                    chars[i] = (char)(chars[i] + 32);
                }
            }

            //Rejoin char array into a string and then split into string array to isolate each word
            value = new string(chars);
            string[] words = value.Split(' ');

            //Loop through string array and capitalize the first letter in each word while ignoring single letter words that aren't the first word
            for (int i = 0; i < words.Length; i++)
            {
                //Convert each word into a char array to isolate the first char
                char[] word = words[i].ToCharArray();

                if ((word.Length > 1 || i == 0) && word[0] > 96 && word[0] < 123)
                {
                    word[0] = (char)(word[0] - 32);
                }

                //Rejoin the char array into string as part of the string array
                words[i] = new string(word);
            }

            //Rejoin string array into a single string with each word separated by a space and return string
            value = String.Join(' ', words);
            return value;
        }

        public static bool IsValidPostalCode(string postalCode)
        {   
            // Set regex for postal code and return if postal code is match or not
            Regex postalCodeRe = new Regex(@"^([a-zA-Z]\d[a-zA-Z])\s?(\d[a-zA-Z]\d)$");
            return postalCodeRe.IsMatch(postalCode);
        }

        public static bool IsValidProvinceCode(string province)
        {
            // Delclare list of valid province codes and return if province matches any of the valid codes or not
            string[] provinceCodes = {"AB", "BC", "MB", "NB", "NL", "NS", "NT", "NU", "ON", "PE", "QC", "SK", "YT"};
            return provinceCodes.Contains(province.ToUpper());
        }

        public static bool IsValidPhoneNumber(string phone)
        {
            // Set regex for phone numbers and return if postal code is match or not
            Regex phoneNumberRe = new Regex(@"^(\d{3})[\s-]?(\d{3})[\s-]?(\d{4})$");
            return phoneNumberRe.IsMatch(phone);
        }

        public static string FormatPhoneNumber(string phoneNum)
        {
            //Format phone number string to ###-###-####
            //Replace spaces with '-'
            phoneNum = phoneNum.Replace(' ', '-');

            //Insert a '-' as the fourth character in string if it isn't currently
            if (char.IsDigit(phoneNum[3]))
            {
                phoneNum = phoneNum.Insert(3, "-");
            }

            //Insert a '-' as the eighth character in string if it isn't currently
            if (char.IsDigit(phoneNum[7]))
            {
                phoneNum = phoneNum.Insert(7, "-");
            }

            // Return valid phone number
            return phoneNum;
        }
    }
}
