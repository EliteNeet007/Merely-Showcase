using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RightNowGames.Timers
{
	/// <summary>
	/// The base timer class. All timers in this system are deriving from this class.<br/><br/>
	/// This class implements the IDisposable interface.
	/// </summary>
	public abstract class Timer : IDisposable
	{
		public float CurrentTime { get; protected set; }
		public bool IsRunning { get; protected set; }

		protected float _initialTime;

		/// <summary>
		/// Calculates the current progress of the timer as a value between 0 and 1.<br/>
		/// 0 at the start, 1 when the timer has finished running.
		/// </summary>
		/// <returns>The calculated value.</returns>
		public virtual float Progress0To1 => 1 - RemainingDuration0To1;
		/// <summary>
		/// Calculates the remaining duration of the timer as a value between 0 and 1.<br/>
		/// 1 at the start, 0 when the timer has finished running.
		/// </summary>
		/// <returns>The calculated value.</returns>
		public virtual float RemainingDuration0To1 => Mathf.Clamp(CurrentTime / _initialTime, 0f, 1f);
		
		/// <summary>
		/// Calculates the current progress of the timer in seconds.
		/// </summary>
		/// <returns>Returns the calculated progress.</returns>
		public virtual float ProgressInSeconds => _initialTime - CurrentTime;
		/// <summary>
		/// Calculates the remaining duration of the timer in seconds.
		/// </summary>
		/// <returns>The calculated duration.</returns>
		public virtual float RemainingDurationInSeconds => _initialTime - ProgressInSeconds;

		public Action OnTimerStarted = delegate { };
		public Action OnTimerFinished = delegate { };

		protected Timer() { }

		protected Timer(float initialTime)
		{
			_initialTime = initialTime;
		}

		protected Timer(float initialTime, Action onTimerStarted, Action onTimerFinished)
		{
			_initialTime = initialTime;
			if (onTimerStarted != null) OnTimerStarted += onTimerStarted;
			if (onTimerFinished != null) OnTimerFinished += onTimerFinished;
		}

		/// <summary>
		/// Starts the timer.<br/><br/>
		/// Sets CurrentTime to initialTime, registers the timer to the manager, and calls the start event.
		/// </summary>
		public virtual void Start()
		{
			CurrentTime = _initialTime;

			if (!IsRunning)
			{
				IsRunning = true;
				TimerManager.RegisterTimer(this);
				OnTimerStarted.Invoke();
			}
		}

		/// <summary>
		/// Stops the timer.<br/><br/>
		/// Deregisters the timer from the manager and calls the stop event.
		/// </summary>
		public void Finish()
		{
			if (IsRunning)
			{
				IsRunning = false;
				TimerManager.DeregisterTimer(this);
				OnTimerFinished.Invoke();
			}
		}

		/// <summary>
		/// Pauses the timer.
		/// </summary>
		public void Pause() => IsRunning = false;
		/// <summary>
		/// Reusmes the timer.
		/// </summary>
		public void Resume() => IsRunning = true;

		/// <summary>
		/// Extends the timer's duration by signature additionalTime.
		/// </summary>
		/// <param name="additionalTime"></param>
		public void Extend(float additionalTime)
		{
			_initialTime += additionalTime;
			CurrentTime += additionalTime;
		}

		/// <summary>
		/// Resets the timer to the original duration.
		/// </summary>
		public virtual void Reset() => CurrentTime = _initialTime;
		/// <summary>
		/// Resets the timer to a new duration, based on signature newTime.
		/// </summary>
		/// <param name="newTime"></param>
		public void Reset(float newTime)
		{
			_initialTime = newTime;
			Reset();
		}

		public abstract void Tick();
		public abstract bool IsFinished {  get; }

		#region IDisposable Implementation

		bool disposed;

		~Timer()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			// DON'T DISPOSE MORE THAN ONCE!
			if (disposed) return;

			// Called through the Dispose() method.
			if (disposing)
			{
				// Deregister timer.
				TimerManager.DeregisterTimer(this);
			}

			disposed = true;
		}

		#endregion

	}
}