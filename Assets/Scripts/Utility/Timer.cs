using UnityEngine;

public class Timer : MonoBehaviour {
	private bool isRunning = false;
	private float currentTime = 0;

	public void StartTimer() {
		isRunning = true;
	}

	public void PauseTimer() {
		isRunning = false;
	}

	/// <summary>
	/// Resets the Timer to the given time.
	/// </summary>
	/// <param name="startTime"> time, the Timer should be set to (e.g. last checkpoint time). Set to 0 for full reset. </param>
	public void ResetTimer(float startTime) {
		currentTime = startTime;
		PauseTimer();
	}

	/// <summary>
	/// Get the running time of the timer in the form: "0.00 sec".
	/// </summary>
	/// <returns></returns>
	public string GetDuration() {
		return currentTime.ToString("0.00") + " sec";
	}

	public float GetCurrentTime() {
		return currentTime;
	}

	public bool TimerIsRunning() {
		return isRunning;
	}

	// increase running time of timer when timer is activated
	private void Update() {
		if (isRunning) {
			currentTime += Time.deltaTime;
		}
	}
}
