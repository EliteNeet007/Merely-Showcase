using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RightNowGames.Utilities
{
    public static class RNGCameraExtensions
    {
		/// <summary>
		/// Calculates the width of the area displayed by the camera in worldSpace units,<br/>
		/// at the specified depth for perspective cameras, everywhere for orthographic ones.
		/// </summary>
		/// <param name="camera"></param>
		/// <param name="depth"></param>
		/// <returns>The calculated width.</returns>
		public static float GetWorldSpaceWidth(this Camera camera, float depth = 0f)
		{
			if (camera.orthographic)
			{
				return camera.aspect * camera.orthographicSize * 2f;
			}
			else
			{
				float fieldOfView = camera.fieldOfView * Mathf.Deg2Rad;
				return camera.aspect * depth * Mathf.Tan(fieldOfView);
			}
		}

		/// <summary>
		/// Calculates the height of the camera in worldSpace units,<br/>
		/// at the specified depth for perspective cameras, everywhere for orthographic ones.
		/// </summary>
		/// <param name="camera"></param>
		/// <param name="depth"></param>
		/// <returns>The calculated height.</returns>
		public static float GetWorldSpaceHeight(this Camera camera, float depth = 0f)
		{
			if (camera.orthographic)
			{
				return camera.orthographicSize * 2f;
			}
			else
			{
				float fieldOfView = camera.fieldOfView * Mathf.Deg2Rad;
				return depth * Mathf.Tan(fieldOfView);
			}
		}


	}
}