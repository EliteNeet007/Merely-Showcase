using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
using UnityEditor;
using RightNowGames.Utilities;

namespace RightNowGames.Timers
{
	internal static class TimerBootstrapper
	{
		static PlayerLoopSystem timerSystem;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
		internal static void Initialize()
		{
			PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();

			if (!InsertTimerManager<Update>(ref currentPlayerLoop, 0))
			{
				Debug.LogWarning("Improved Timers not initialized, unable to register TimerManager into the Update loop.");
				return;
			}
			PlayerLoop.SetPlayerLoop(currentPlayerLoop);
			RNGUtilities.PrintPlayerLoop(currentPlayerLoop);

#if UNITY_EDITOR
			EditorApplication.playModeStateChanged -= OnPlayModeState;
			EditorApplication.playModeStateChanged += OnPlayModeState;
#endif

			static void OnPlayModeState(PlayModeStateChange state)
			{
				if (state == PlayModeStateChange.ExitingPlayMode)
				{
					PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
					RemoveTimerManager<Update>(ref currentPlayerLoop);
					PlayerLoop.SetPlayerLoop(currentPlayerLoop);

					TimerManager.ClearTimers();
				}
			}
		}

		private static bool InsertTimerManager<T>(ref PlayerLoopSystem loop, int index)
		{
			// Create the timer system to insert into the player loop.
			timerSystem = new PlayerLoopSystem()
			{
				type = typeof(TimerManager),
				updateDelegate = TimerManager.UpdateTimers,
				subSystemList = null
			};

			// Insert the system into the player loop and return the result.
			return RNGUtilities.InsertSystem<T>(ref loop, in timerSystem, index);
		}

		private static void RemoveTimerManager<T>(ref PlayerLoopSystem loop)
		{
			RNGUtilities.RemoveSystem<T>(ref loop, in timerSystem);
		}
	}
}