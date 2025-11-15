using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RightNowGames.Timers
{
	public class ElapsedTimer : Timer
	{
		public ElapsedTimer()
		{
			_initialTime = Time.time;
		}

		public override bool IsFinished => false;

		public override void Tick()
		{
			if (IsRunning) CurrentTime += Time.deltaTime;
		}
	}
}