using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RightNowGames.Basic
{
	/// <summary>
	/// Basic persistent singleton functionality to inherit.<br/><br/>
	/// Transforms our basic singleton into a persistent one,<br/>
	/// ensuring not only that the original instance isn't overriden, but also that it persists through scene transitions.
	/// </summary>
	/// <typeparam name="T">The class that extends this class.</typeparam>
	public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour
    {
		protected bool _autoUnparentOnAwake = true;

		/// <summary>
		/// If you don't override and call InitializeInstance() in Awake(),<br/>
		/// make sure to call base.Awake() in override - BEFORE ANY new code in Awake().
		/// </summary>
		protected override void Awake()
		{
			InitializeInstance();
		}

		/// <summary>
		/// Further extends the basic implementation of assigning Instance as itself.<br/>
		/// Now it both checks if Instance is already assigned to avoid overriding it,<br/>
		/// and sets itself to DontDestroyOnLoad to persist through scene changes.<br/><br/>
		/// Make sure to call base.InitializeInstnace() in override if you need to override.
		/// </summary>
		protected override void InitializeInstance()
		{
			// DontDestroyOnLoad ONLY works on gameObjects at the root level (without a parent object).
			// BEFORE ANYTHING ELSE - ensure this object isn't a child object:
			// Check if can initialize & configured to unparent on awake, if so - set parent to null.
			if (CanInitialize() && _autoUnparentOnAwake) transform.SetParent(null);

			// Sets itself as DontDestroyOnLoad -> to persist through scene changes.
			DontDestroyOnLoad(gameObject);

			// Performs the basic InitializeInstance() functionality it inherits:
			// Checks if Instance is already assigned -> if it is - destroys itself, else - assigns itself as Instance.
			base.InitializeInstance();
		}
	}
}