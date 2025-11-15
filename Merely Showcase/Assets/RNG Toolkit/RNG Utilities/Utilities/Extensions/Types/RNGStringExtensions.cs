using System;
using System.Text;
using System.Security.Cryptography;
using System.ComponentModel;
using UnityEngine;
using RightNowGames.Enums;

namespace RightNowGames.Utilities
{
	public static class RNGStringExtensions
	{
		#region Internal

		/// <summary>
		/// Computes the hash of the signature input, based on the signature hashType, as a byte[].
		/// </summary>
		/// <param name="input"></param>
		/// <param name="hashType"></param>
		/// <returns>The computed byte[] hash.</returns>
		private static byte[] GetHash(string input, eHashType hashType)
		{
			byte[] inputBytes = Encoding.ASCII.GetBytes(input);

			return hashType switch
			{
				eHashType.HMAC => HMAC.Create().ComputeHash(inputBytes),
				eHashType.HMACMD5 => HMACMD5.Create().ComputeHash(inputBytes),
				eHashType.HMACSHA1 => HMACSHA1.Create().ComputeHash(inputBytes),
				eHashType.HMACSHA256 => HMACSHA256.Create().ComputeHash(inputBytes),
				eHashType.HMACSHA384 => HMACSHA384.Create().ComputeHash(inputBytes),
				eHashType.HMACSHA512 => HMACSHA512.Create().ComputeHash(inputBytes),
				eHashType.MD5 => MD5.Create().ComputeHash(inputBytes),
				eHashType.SHA1 => SHA1.Create().ComputeHash(inputBytes),
				eHashType.SHA256 => SHA256.Create().ComputeHash(inputBytes),
				eHashType.SHA384 => SHA384.Create().ComputeHash(inputBytes),
				eHashType.SHA512 => SHA512.Create().ComputeHash(inputBytes),
				_ => inputBytes,
			};
		}

		#endregion

		#region Types

		/// <summary>
		/// Converts the extended string to an object of the specified type.
		/// </summary>
		/// <typeparam name="T">The type to convert the string to.</typeparam>
		/// <param name="value">The string to be converted.</param>
		/// <returns>The converted object.</returns>
		/// <exception>Throws an exception if the conversion process encounters an error.</exception>
		public static T Parse<T>(this string value)
		{
			// Get the default value for the type,
			// ensuring that if the string is empty:
			// we can return the default value.
			T result = default(T);

			// Check if the string is functionally empty.
			if (!value.IsNullEmptyOrWhitespace())
			{
				// If it isn't, try to convert the string to an object of the specified type:
				try
				{
					TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(T));
					result = (T)typeConverter.ConvertFrom(value);
				}
				catch
				{
					throw new Exception($"An Exception has occured while trying to convert the string value: {value}, to an object of type: {result.GetType()}.");
				}
			}

			return result;
		}

		/// <summary>
		/// Checks if the extended string is null, empty or contains only white spaces.<br/><br/>
		/// When a string is - null, empty or contains only white spaces,<br/>
		/// it is functionally empty and the check will return true.
		/// </summary>
		/// <param name="input">The extended string to check.</param>
		/// <returns>The result of the check.</returns>
		public static bool IsNullEmptyOrWhitespace(this string input)
		{
			// Check if the string is null or empty.
			if (!string.IsNullOrEmpty(input))
			{
				// If the string isnt null or empty:
				// Cycle throughthe string and look for a char that isn't a withe space.
				for (int i = 0; i < input.Length; i++)
				{
					// If such a char is found return false.
					if (!char.IsWhiteSpace(input[i])) return false;
					// Else - the default return true line will be called,
					// since the string is functionally empty (contains only white spaces).
				}
			}

			// Default return true.
			// When a string is null or empty - this line will be called.
			// When a string isn't null or empty but contains only white spaces - this line will be called.
			return true;
		}

		/// <summary>
		/// Compares the length of the extended string to the signature length.<br/><br/>
		/// Will return true if the extended string's length is AT LEAST EQUAL to the signature length (equal or greater than).
		/// </summary>
		/// <param name="input">The string who's length is being checked.</param>
		/// <param name="length">The minimum required length of the extended string for the check result to be true (default of 2).<br/>
		/// MUST BE A POSITIVE NUMBER, otherwise: AN ERROR WILL BE THROWN.</param>
		/// <returns>The result of the comparison.</returns>
		/// <exception>Throws ArgumentOutOfRangeException if the signature length is a negative number (lesser than 0).</exception>
		public static bool IsLengthAtLeast(this string input, int length = 2)
		{
			if (input.IsNullEmptyOrWhitespace()) return false;

            if (length < 0) throw new ArgumentOutOfRangeException("length", length, "The signature length was less than 0");

			return input.Length >= length;
        }

		/// <summary>
		/// Checks if the extended string can be converted to a DateTime instance.
		/// </summary>
		/// <param name="input">The extended string to check.</param>
		/// <returns>The result of the check.</returns>
		public static bool IsDateTime(this string input)
		{
			// Check if the string isn't functionally empty.
			if (!input.IsNullEmptyOrWhitespace())
			{
				// If the string isn't functionally empty, TryParse a DateTime and return the result.
				return DateTime.TryParse(input, out DateTime dt);
			}
			// Else return false.
			return false;
		}

		/// <summary>
		/// Computes the FNV-1a hash for the extended string.<br/><br/>
		/// The FNV-1a hash is a non-cryptographic hash function known for its speed and good distribution properties.<br/>
		/// Useful for creating Dictionary keys instead of using strings.
		/// https://en.wikipedia.org/wiki/Fowler–Noll–Vo_hash_function
		/// </summary>
		/// <param name="input">The extended string to hash.</param>
		/// <returns>An integer representing the FNV-1a hash of the extended string.</returns>
		public static int ComputeFNV1aHash(this string input)
		{
			uint hash = 2166136261;
			foreach (char c in input)
			{
				hash = (hash ^ c) * 16777619;
			}
			return unchecked((int)hash);
		}

		/// <summary>
		/// Computes the hash of the extended string using a specified hash algorithm.
		/// </summary>
		/// <param name="input">The string to hash.</param>
		/// <param name="hashType">The hash algorithm to use.</param>
		/// <returns>The resulting hash or an empty string on error.</returns>
		public static string ComputeCryptographicHash(this string input, eHashType hashType)
		{
			try
			{
				byte[] hash = GetHash(input, hashType);
				StringBuilder ret = new();

				for (int i = 0; i < hash.Length; i++) 
					ret.Append(hash[i].ToString("x2"));

				return ret.ToString();
			}
			catch 
			{
				return string.Empty;
			}
		}

		/// <summary>
		/// Truncates the extended string to a specified length and replaces the truncated parts with (...).
		/// </summary>
		/// <param name="input">The string to be truncated.</param>
		/// <param name="maxLength">The length to maintain from the original string before appending the suffix (...).<br/><br/>
		/// maxLength must be a positive number and longer than the extended string for the string to be truncated.</param>
		/// <returns>The truncated string.</returns>
		public static string Truncate(this string input, int maxLength)
		{
			const string SUFFIX = "...";
			string truncatedString = input;
			int stringLength = maxLength - SUFFIX.Length;

			// Ensure the string can be appropriately truncated.
			if (maxLength <= 0 || stringLength <= 0
				|| input.IsNullEmptyOrWhitespace() || input.Length <= maxLength)
			{
				// If the string isn't suitable for truncating, return it unchanged.
				return truncatedString;
			}

			// Truncate the string, trim the end and append the suffix, then return the truncated string.
			truncatedString = input[..stringLength];
			truncatedString = truncatedString.TrimEnd();
			truncatedString += SUFFIX;
			return truncatedString;
		}

		#region Case-Convention String Formatting

		/// <summary>
		/// Converts the extended string to camelCase.
		/// </summary>
		/// <param name="input">The string to be converted.</param>
		/// <returns>The converted string.</returns>
		public static string ToCamelCase(this string input)
		{
			// Check if the string isn't valid to be converted (functionally null or shorter than 2 chars).
			// If the string isn't valid for conversion:
			if (input.IsNullEmptyOrWhitespace() || input.Length < 2)
			{
				// Check if it's length is greater than 0,
				// if it is - return it in lower case (first word of cameCase is entirely lower case).
				if (input.Length > 0) return input.ToLower();
				// else return it completely unedited.
				return input;
			}

			// If the string is valid for conversion:

			// Split the string to separate strings(words), remove empty spaces.
			string[] words = input.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);

			// Add the first word in the array to the result, in lower case.
			string result = words[0].ToLower();
			// Add subsequent words in the array to the result, ensuring the first char in each word is upper case.
			for (int i = 1; i < words.Length; i++)
			{
				result += words[i][..1].ToUpper() + words[i][1..];
				// The above line uses substring abbreviation (range operator) and is equivalent to the following line:
				//result += words[i].Substring(0, 1).ToUpper() + words[i].Substring(1);
			}

			return result;
		}

		/// <summary>
		/// Converts the extended string to kebab-case.
		/// </summary>
		/// <param name="input">The string to be converted.</param>
		/// <returns>The converted string.</returns>
		public static string ToKebabCase(this string input)
		{
			// Check if the string isn't valid to be converted (functionally null or shorter than 2 chars).
			// If the string isn't valid for conversion - return it as it is.
			if (input.IsNullEmptyOrWhitespace() || input.Length < 2) return input;

			// If the string is valid for conversion:

			// Split the string to separate strings(words), remove empty spaces.
			string[] words = input.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);

			// Add the first word in the array to the result.
			string result = words[0].ToLower();
			// Add subsequent words in the array to the result, ensuring each word is separated by an underscore(_).
			for (int i = 1; i < words.Length; i++)
				result += "-" + words[i].ToLower();

			return result;
		}

		/// <summary>
		/// Converts the extended string to KEBAB-CASE.<br/>
		/// Same as the other kebab case conversion, only ALL CAPS.
		/// </summary>
		/// <param name="input">The string to be converted.</param>
		/// <returns>The converted string.</returns>
		public static string ToKebabCaseCaps(this string input)
		{
			return input.ToKebabCase().ToUpper();
		}

		/// <summary>
		/// Converts the extended string to PascalCase.
		/// </summary>
		/// <param name="input">The string to be converted.</param>
		/// <returns>The converted string.</returns>
		public static string ToPascalCase(this string input)
		{
			// Check if the string isn't valid to be converted (functionally null or shorter than 2 chars).
			// If the string isn't valid for conversion:
			if (input.IsNullEmptyOrWhitespace() || input.Length < 2)
			{
				// Check if it's length is greater than 0,
				// if it is - return it in lower case (first word of cameCase is entirely lower case).
				if (input.Length > 0) return input.ToUpper();
				// else return it completely unedited.
				return input;
			}

			// If the string is valid for conversion:

			// Split the string to separate strings(words), remove empty spaces.
			string[] words = input.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);

			// Add the first word in the array to the result, in lower case.
			string result = words[0][..1].ToUpper() + words[0][1..];
			// The above line uses substring abbreviation (range operator) and is equivalent to the following line:
			// result = words[0].Substring(0, 1).ToUpper() + words[0].Substring(1);

			// Add subsequent words in the array to the result, ensuring the first char in each word is upper case.
			for (int i = 1; i < words.Length; i++)
			{
				result += words[i][..1].ToUpper() + words[i][1..];
				// The above line uses substring abbreviation (range operator) and is equivalent to the following line:
				//result += words[i].Substring(0, 1).ToUpper() + words[i].Substring(1);
			}

			return result;
		}

		/// <summary>
		/// Converts the extended string to snake_case.
		/// </summary>
		/// <param name="input">The string to be converted.</param>
		/// <returns>The converted string.</returns>
		public static string ToSnakeCase(this string input)
		{
			// Check if the string isn't valid to be converted (functionally null or shorter than 2 chars).
			// If the string isn't valid for conversion - return it as it is.
			if (input.IsNullEmptyOrWhitespace() || input.Length < 2) return input;

			// If the string is valid for conversion:

			// Split the string to separate strings(words), remove empty spaces.
			string[] words = input.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);

			// Add the first word in the array to the result.
			string result = words[0].ToLower();
			// Add subsequent words in the array to the result, ensuring each word is separated by an underscore(_).
			for (int i = 1; i < words.Length; i++)
				result += "_" + words[i].ToLower();

			return result;
		}

		/// <summary>
		/// Converts the extended string to SNAKE_CASE.<br/>
		/// Same as the other snake case conversion, only ALL CAPS.
		/// </summary>
		/// <param name="input">The string to be converted.</param>
		/// <returns>The converted string.</returns>
		public static string ToSnakeCaseCaps(this string input)
		{
			return input.ToSnakeCase().ToUpper();
		}

		#endregion

		#endregion

		#region Objects

		/// <summary>
		/// Calculates a color value, using the extended string as a hex color format FF00FFAA.
		/// </summary>
		/// <param name="input"></param>
		/// <returns>The calculated value.</returns>
		public static Color GetColorFromString(this string input)
		{
			// Convert the string into the separate color float values (1:red, 2:green, 3:blue, 4:alpha)
			float red = RNGUtilities.Hex_to_Dec01(input.Substring(0, 2));
			float green = RNGUtilities.Hex_to_Dec01(input.Substring(2, 2));
			float blue = RNGUtilities.Hex_to_Dec01(input.Substring(4, 2));
			float alpha = 1f;
			if (input.Length >= 8)
			{
				// Color string contains alpha
				alpha = RNGUtilities.Hex_to_Dec01(input.Substring(6, 2));
			}
			// Return the converted color.
			return new Color(red, green, blue, alpha);
		}

		#endregion
	}
}