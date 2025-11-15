using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RightNowGames.Timers
{
    public class CountdownTimer : Timer
    {
		public CountdownTimer(float length) : base(length) { }
        public CountdownTimer(float length, Action onTimerStarted = null, Action onTimerFinished = null) : base(length, onTimerStarted, onTimerFinished) { }

		public override void Tick()
		{
			if (IsRunning)
			{
				if (CurrentTime > 0) CurrentTime -= Time.deltaTime;

				if (CurrentTime <= 0) Finish();
			}
		}

		public override bool IsFinished => CurrentTime <= 0;
	}
}