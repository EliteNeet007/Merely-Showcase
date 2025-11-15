using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Object = UnityEngine.Object;

namespace RightNowGames.Utilities
{
	public static class RNGGameObjectExtensions
	{
		/// <summary>
		/// Checks if the extended gameObject has an instance of the specified component attached.<br/><br/>
		/// If the gameObject does not have the specified component attached, add the component and then return it.<br/>
		/// Outputs a boolean, indicating if a component had to be added -> just in case.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="gameObject"></param>
		/// <param name="hadToAddComponent"></param>
		/// <returns>The specified component.</returns>
		public static T GetOrAdd<T>(this GameObject gameObject, out bool hadToAddComponent) where T : Component
		{
			// Regular GetComponent() method.
			T component = gameObject.GetComponent<T>();
			hadToAddComponent = false;

			// component null check:
			// If true, the gameObject does not have the required component attached.
			if (!component)
			{
				// Add the component to the gameobject and flip the indicating boolean.
				component = gameObject.AddComponent<T>();
				hadToAddComponent = true;
			}

			// Return the component.
			return component;
		}

		/// <summary>
		/// Checks if the extended object is null.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <returns>The result of the check.</returns>
		public static T OrNull<T>(this T obj) where T : Object => obj ? obj : null;


	}
}