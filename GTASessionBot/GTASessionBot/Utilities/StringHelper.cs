using System;
using System.Linq;

namespace GTASessionBot.Utilities {

    public static class StringHelper {

        public static string FirstCharToUpper(string input) {
            if (String.IsNullOrEmpty(input)) {
                throw new ArgumentException("Null input string.");
            }

            return input.First().ToString().ToUpper() + input.Substring(1);
        }

        public static string MultipleOrNot(string input, ulong amount)
            => amount == 1 ? $"{input}" : $"{input}s";
    }
}
