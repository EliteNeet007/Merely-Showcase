using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RightNowGames.Timers
{
	public class FrameTimer : Timer
	{
		public int CurrentFrameCount {  get; protected set; }
		public override bool IsFinished => CurrentFrameCount <= 0;

		protected int _initialFrameCount;

		public FrameTimer(int frameCount)
		{
			_initialFrameCount = frameCount;
		}

		public FrameTimer(int frameCount, Action onTimerStarted = null, Action onTimerFinished = null)
		{
			_initialFrameCount = frameCount;
			if (onTimerStarted != null) OnTimerStarted += onTimerStarted;
			if (onTimerFinished != null) OnTimerFinished += onTimerFinished;
		}

		public override void Start()
		{
			CurrentFrameCount = _initialFrameCount;

			if (!IsRunning)
			{
				IsRunning = true;
				TimerManager.RegisterTimer(this);
				OnTimerStarted.Invoke();
			}
		}

		public void Extend(int additionalFrames)
		{
			_initialFrameCount += additionalFrames;
			CurrentFrameCount += additionalFrames;
		}

		public override void Reset() => CurrentFrameCount -= _initialFrameCount;

		public void Reset(int newFrameCount)
		{
			_initialFrameCount = newFrameCount;
			Reset();
		}

		public override void Tick()
		{
			if (IsRunning)
			{
				if (CurrentFrameCount > 0) CurrentFrameCount--;

				if (CurrentFrameCount <= 0) Finish();
			}
		}
	}

}
