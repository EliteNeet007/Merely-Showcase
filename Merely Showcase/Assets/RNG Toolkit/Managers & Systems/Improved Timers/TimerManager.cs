using System.Collections.Generic;

namespace RightNowGames.Timers
{
	/// <summary>
	/// The core of the RightNowGames.Timers system.<br/><br/>
	/// Manages the timers handled by the system.
	/// </summary>
	public static class TimerManager
	{
		private static readonly List<Timer> timers = new();
		/// <summary>
		/// Is the manager running?<br/>
		/// Returns true if the manager's list of timers is currently not empty.
		/// </summary>
		public static bool IsRunning => timers.Count > 0;

		/// <summary>
		/// Adds the signature timer to the list of timers to manage.
		/// </summary>
		/// <param name="timer">The timer to add to the list.</param>
		public static void RegisterTimer(Timer timer)
		{
			timers.Add(timer);
			timer.OnTimerFinished += () => DeregisterTimer(timer);
		}
		/// <summary>
		/// Removes the timer from the list of timers to manage.
		/// </summary>
		/// <param name="timer">The timer to remove from the list.</param>
		public static void DeregisterTimer(Timer timer) => timers.Remove(timer);
		/// <summary>
		/// Clears the list of timers the manager currently manages.
		/// </summary>
		public static void ClearTimers() => timers.Clear();

		/// <summary>
		/// Cycles through the list of timers the manager currently manages.<br/>
		/// Calls the Tick() method for each of the timers it manages.
		/// </summary>
		public static void UpdateTimers()
		{
			//foreach (Timer timer in timers) timer.Tick();

			for (int i = 0; i < timers.Count; i++) timers[i].Tick();
		}

		/// <summary>
		/// Cycles through the list of timers the manager currently manages and pauses them.
		/// </summary>
		public static void PauseTimers()
		{
			foreach(Timer timer in timers) timer.Pause();
		}

		/// <summary>
		/// Cycles through the list of timers the manager currently manages and resumes them.
		/// </summary>
		public static void ResumeTimers()
		{
			foreach (Timer timer in timers) timer.Resume();
		}


	}
}