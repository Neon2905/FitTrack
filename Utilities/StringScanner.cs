namespace FitTrack.Utilities
{
    /// <summary>
    /// Provides functionality to scan a string for various character types.
    /// </summary>
    public class StringScanner
    {
        /// <summary>
        /// Gets a value indicating whether the string contains at least one uppercase letter.
        /// </summary>
        public bool HasUpperCaseLetter {  get; private set; }

        /// <summary>
        /// Gets a value indicating whether the string contains at least one lowercase letter.
        /// </summary>
        public bool HasLowerCaseLetter { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the string contains at least one number.
        /// </summary>
        public bool HasNumber { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the string contains at least one letter (uppercase or lowercase).
        /// </summary>
        public bool HasLetter { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the string contains at least one special character or space.
        /// </summary>
        public bool HasSpecialCharacterAndSpace { get; private set; }

        /// <summary>
        /// Scans the specified string and determines the presence of various character types.
        /// </summary>
        /// <param name="input">The string to scan.</param>
        /// <returns>A <see cref="StringScanner"/> object with properties set based on the input string.</returns>
        public static StringScanner Scan(string input)
        {
            StringScanner scanner = new StringScanner();
            foreach (char c in input)
            {
                if (char.IsUpper(c))
                    scanner.HasUpperCaseLetter = true;

                if (char.IsLower(c))
                    scanner.HasLowerCaseLetter = true;

                if (char.IsDigit(c))
                    scanner.HasNumber = true;

                if (char.IsLetter(c))
                    scanner.HasLetter = true;

                if (!char.IsLetterOrDigit(c))
                    scanner.HasSpecialCharacterAndSpace = true;

                //Break loop if all the flags are detected
                if (scanner.HasUpperCaseLetter &&
                    scanner.HasLowerCaseLetter &&
                    scanner.HasNumber &&
                    scanner.HasLetter &&
                    scanner.HasSpecialCharacterAndSpace)
                    break;
            }
            return scanner;
        }
    }
}
