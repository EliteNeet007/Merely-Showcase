using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Object = UnityEngine.Object;

namespace RightNowGames.Utilities
{
	public static class RNGTransformExtensions
	{
		/// <summary>
		/// Defines an IEnumerable of all the children of the extended parent Transform.
		/// </summary>
		/// <param name="parent"></param>
		/// <returns>The defined IEnumerable</returns>
		public static IEnumerable<Transform> GetChildren(this Transform parent)
		{
			foreach (Transform child in parent) yield return child;
		}

		/// <summary>
		/// Destroy all child objects of this transform.
		/// </summary>
		/// <param name="parent"></param>
		public static void DestroyAllChildren(this Transform parent)
		{
			parent.PerformActionOnChildren(child => Object.Destroy(child.gameObject));
		}

		/// <summary>
		/// Enable all child objects of this transform.
		/// </summary>
		/// <param name="parent"></param>
		public static void EnableChildren(this Transform parent)
		{
			parent.PerformActionOnChildren(child => child.gameObject.SetActive(true));
		}

		/// <summary>
		/// Disable all child objects of this transform.
		/// </summary>
		/// <param name="parent"></param>
		public static void DisableChildren(this Transform parent)
		{
			parent.PerformActionOnChildren(child => child.gameObject.SetActive(false));
		}

		/// <summary>
		/// Performs the signature action on ALL of the extended parent's children.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="action"></param>
		private static void PerformActionOnChildren(this Transform parent, Action<Transform> action)
		{
			for (int i = parent.childCount - 1; i >= 0; i--) action(parent.GetChild(i));
		}

		/// <summary>
		/// Reset the transform's parameters.<br/>
		/// Allows control of specific parameters to reset.
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="resetPosition"></param>
		/// <param name="resetRotation"></param>
		/// <param name="resetScale"></param>
		public static void ResetTransform(this Transform transform, bool resetPosition = true, bool resetRotation = true, bool resetScale = true)
		{
			// Check if reset is required for both position and rotation.
			// If it is, use SetPositionRotation to save on resources and set both together.
			if (resetPosition && resetRotation) transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
			// Else, individually check and reset according to flags.
			else
			{
				// Position check and reset.
				if (resetPosition) transform.position = Vector3.zero;
				// Rotation check and reset.
				if (resetRotation) transform.rotation = Quaternion.identity;
			}
			// Check if the scale needs to be reset, and reset if it is.
			if (resetScale) transform.localScale = Vector3.one;
		}


	}
}