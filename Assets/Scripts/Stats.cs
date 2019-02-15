using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Timer))]
public class Stats : MonoBehaviour {
	public Timer timer { get; private set; }

	[SerializeField] private Text timerText = null;
	public int hits { get; private set; }
	[SerializeField] private Text hitsText = null;

	private void Start() {
		timer = GetComponent<Timer>();
		hits = 0;
	}

	private void Update() {
		// update Timer text in UI
		if (timer.isRunning) {
			timerText.text = timer.GetDuration();
		}
	}

	public void Reset() {
		Reset(0, 0);
	}

	public void Reset(float checkpointTime, int hits) {
		timer.ResetTimer(checkpointTime);
		timerText.text = timer.GetDuration();

		// reset hits
		this.hits = hits;
		hitsText.text = hits.ToString();
	}

	public void PlayerShot() {
		// start Timer when player makes first hit
		if (!timer.isRunning) {
			timer.StartTimer();
		}

		hits++;
		hitsText.text = hits.ToString();
	}
}
