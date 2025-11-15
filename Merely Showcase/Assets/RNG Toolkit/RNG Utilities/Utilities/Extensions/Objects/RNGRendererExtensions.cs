using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RightNowGames.Utilities
{
	public static class RNGRendererExtensions
	{
		/// <summary>
		/// Checks if the extended renderer is visible from the signature camera
		/// </summary>
		/// <param name="renderer"></param>
		/// <param name="camera"></param>
		/// <returns>The result of the check.</returns>
		public static bool IsVisibleFromCamera(this Renderer renderer, Camera camera)
		{
			Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(camera);
			return GeometryUtility.TestPlanesAABB(frustumPlanes, renderer.bounds);
		}


	}
}