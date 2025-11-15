using UnityEngine;

namespace RightNowGames.Basic
{
	/// <summary>
	/// Basic singleton functionality to inherit.<br/><br/>
	/// Transforms our basic static instance into a singleton,<br/>
	/// and ensures any new instances are destroyed -> instead of replacing the original.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
    {
		/// <summary>
		/// If you don't override and call InitializeInstance() in Awake(),<br/>
		/// make sure to call base.Awake() in override - BEFORE ANY new code in Awake().
		/// </summary>
		protected override void Awake()
		{
			InitializeInstance();
		}

		/// <summary>
		/// Returns true if the application is playing.<br/><br/>
		/// Awake() can sometimes be called when in edit mode,<br/>
		/// use this method to ensure you only initialize when the game is running.
		/// </summary>
		/// <returns></returns>
		protected virtual bool CanInitialize() => Application.isPlaying;

		/// <summary>
		/// Extends the basic implementation of assigning instance as itself.<br/>
		/// Checking if instance is null, and only assigning itself as instance if it is, else destroys itself.<br/><br/>
		/// Make sure to call base.InitializeInstnace() in override if you need to override.
		/// </summary>
		protected override void InitializeInstance()
		{
			// Sometimes Awake() can be called while in edite mode,
			// meaning the game isn't running but Awake() is still called.
			// Check if the game is running, if it isn't - return.
			if (!CanInitialize()) return;

			// Checks if an instance is already assigned:
			// If so, destroys itself to avoid overriding the existing instance.
			if (Instance != null)
			{
				Debug.LogWarning($"Destroyed a singleton instance: {gameObject.name}, check if this is an intended outcome and correct the situation if not.");
				Destroy(gameObject);
			}
			// Else -> performs the basic awake functionality (assigns self as instance).
			base.InitializeInstance();
		}
	}
}