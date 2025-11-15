using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RightNowGames.Utilities
{
	public static class RNGVectorExtensions
	{
		#region Vector2Int

		/// <summary>
		/// Normalizes the extended vector as a Vector2.
		/// </summary>
		/// <param name="vector"></param>
		/// <returns>The normalized vector.</returns>
		public static Vector2 Normalized(this Vector2Int vector)
		{
			float magnitude = vector.magnitude;

			if (magnitude > 0) return new Vector2(vector.x / magnitude, vector.y / magnitude);
			else return Vector2.zero;
		}

		#endregion

		#region Vector2

		/// <summary>
		/// Generates a Vector2 that equals the extended vector + the signature values.<br/><br/>
		/// Only requires you specify the values you want changed, defaults the rest to 0 which will keep them as they are.
		/// </summary>
		/// <param name="vector"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns>The generated vector.</returns>
		public static Vector2 Add(this Vector2 vector, float? x = null, float? y = null)
		{
			return new Vector2(vector.x + (x ?? 0), vector.y + (y ?? 0));
		}

		/// <summary>
		/// Generates a Vector2 that augments the extended vector, ONLY augments the specified values.<br/><br/>
		/// i.e: say you only to change the height of a vector,<br/>
		/// extend it using the With() method and put y:newValue in the signature -> vector.With(y:5),<br/>
		/// will return the extended vector with a height of 5, x will remain unchanged.
		/// </summary>
		/// <param name="vector"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns>The generated augmented vector.</returns>
		public static Vector2 With(this Vector2 vector, float? x = null, float? y = null)
		{
			return new Vector2(x ?? vector.x, y ?? vector.y);
		}

		#endregion

		#region Vector3Int

		/// <summary>
		/// Normalizes the extended vector as a Vector3.
		/// </summary>
		/// <param name="vector"></param>
		/// <returns>The normalized vector.</returns>
		public static Vector3 Normalized(this Vector3Int vector)
		{
			float magnitude = vector.magnitude;

			if (magnitude > 0) return new Vector3(vector.x / magnitude, vector.y / magnitude, vector.z / magnitude);
			else return Vector3.zero;
		}

		#endregion

		#region Vector3

		/// <summary>
		/// Generates a Vector3 that equals the extended vector + the signature values.<br/><br/>
		/// Only requires you specify the values you want changed, defaults the rest to 0 which will keep them as they are.
		/// </summary>
		/// <param name="vector"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <returns>The generated vector.</returns>
		public static Vector3 Add(this Vector3 vector, float? x = null, float? y = null, float? z = null)
		{
			return new Vector3(vector.x + (x ?? 0), vector.y + (y ?? 0), vector.z + (z ?? 0));
		}

		/// <summary>
		/// Generates a Vector3 that augments the vector it extends, ONLY augments the specified values.<br/><br/>
		/// i.e: say you only want to change the height of a vector.<br/>
		/// extend it using the With() method and put y:newValue in the signature -> vector.With(y:5),<br/>
		/// will return the extended vector with a height of 5, x and z will remain unchanged.
		/// </summary>
		/// <param name="vector"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <returns>The generated augmented vector.</returns>
		public static Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null)
		{
			return new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
		}

		#endregion


	}
}