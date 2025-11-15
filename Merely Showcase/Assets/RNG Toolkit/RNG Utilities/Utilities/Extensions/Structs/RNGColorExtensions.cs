using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RightNowGames.Utilities
{
	public static class RNGColorExtensions
	{
		/// <summary>
		/// Computes the extended color's luminance value.
		/// </summary>
		/// <param name="color"></param>
		/// <returns>The computed value.</returns>
		public static float Luminance(this Color color)
		{
			return 0.2126f * color.r + 0.7152f * color.g + 0.0722f * color.b;
		}

		/// <summary>
		/// Computes a mean value between r, g and b.
		/// </summary>
		/// <param name="color"></param>
		/// <returns>The computed value.</returns>
		public static float MeanRGB(this Color color)
		{
			return (color.r + color.g + color.b) / 3f;
		}

		/// <summary>
		/// Computes hex color format FF00FF, from Color value.
		/// </summary>
		/// <param name="color"></param>
		/// <returns>The computed string.</returns>
		public static string GetStringFromColor(this Color color)
		{
			string red = RNGUtilities.Dec01_to_Hex(color.r);
			string green = RNGUtilities.Dec01_to_Hex(color.g);
			string blue = RNGUtilities.Dec01_to_Hex(color.b);
			return red + green + blue;
		}

		/// <summary>
		/// Computes hex color format FF00FF + alpha value - AA, from Color value.
		/// </summary>
		/// <param name="color"></param>
		/// <returns>The computed string.</returns>
		public static string GetStringFromColorWithAlpha(this Color color)
		{
			string alpha = RNGUtilities.Dec01_to_Hex(color.a);
			return color.GetStringFromColor() + alpha;
		}


	}
}