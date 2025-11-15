using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RightNowGames.Utilities
{
    public static class RNGFloatExtensions
    {
		/// <summary>
		/// Normalizes an angle in degrees.
		/// </summary>
		/// <param name="angleInDegrees"></param>
		/// <returns>The normalized angle.</returns>
		public static float NormalizeAngle(this float angleInDegrees)
		{
			angleInDegrees = angleInDegrees % 360f;
			if (angleInDegrees < 0)
			{
				angleInDegrees += 360f;
			}
			return angleInDegrees;
		}

		/// <summary>
		/// Rounds a float down.
		/// </summary>
		/// <param name="number"></param>
		/// <param name="decimalPlaces"></param>
		/// <returns>The rounded float.</returns>
		public static float RoundDown(this float number, int decimalPlaces)
		{
			return Mathf.Floor(number * Mathf.Pow(10, decimalPlaces)) / Mathf.Pow(10, decimalPlaces);
		}


	}
}