using UnityEngine;

public class Timer {
	private bool isRunning;
	private float startTime;

	public void Start() {
		isRunning = true;
		startTime = Time.time;
	}

	public void Pause() {
		throw new System.NotImplementedException();
	}

	public void Reset() {
		isRunning = false;
	}

	public string GetDuration() {
		return (Time.time - startTime).ToString("0.00") + " sec";
	}

	public bool TimerIsRunning() {
		return isRunning;
	}
}
