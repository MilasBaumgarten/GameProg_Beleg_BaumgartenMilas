using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

[RequireComponent(typeof(Timer))]
public class GameManager : MonoBehaviour {
	public static GameManager instance;

	private float fixedDeltaTimeStart;

	public PlayerScriptableObject playerSettings;
	public PostProcessVolume slowMotionPostProcessingVolume;

	private Timer timer;
	[SerializeField] private Text timerText = null;
	private int hits = 0;
	[SerializeField] private Text hitsText = null;

	private GameObject player;
	private GameObject currentCheckpoint;

	void Start() {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);

			Setup();
		} else {
			Destroy(gameObject);
		}
	}

	private void Setup() {
		fixedDeltaTimeStart = Time.fixedDeltaTime;
		// find the player
		player = GameObject.FindGameObjectWithTag("Player");

		timer = GetComponent<Timer>();
	}

	private void Update() {
		// update Timer text in UI
		if (timer.TimerIsRunning()) {
			timerText.text = timer.GetDuration();
		}
	}

	public void Respawn() {
		player.transform.position = currentCheckpoint.transform.position;
		player.GetComponent<Rigidbody>().velocity = Vector3.zero;

		// reset timer
		//timer.Reset(Time.time - currentCheckpoint.GetComponent<CheckPoint>().GetCheckpointTime());
		timer.ResetTimer(currentCheckpoint.GetComponent<CheckPoint>().GetCheckpointTime());
		timerText.text = timer.GetDuration();

		// reset hits
		hits = currentCheckpoint.GetComponent<CheckPoint>().GetCheckpointHits();
		hitsText.text = hits.ToString();
	}

	// reset Time.fixedDeltaTime when game is closed
	private void OnDisable() {
		Debug.Log("Game was closed");
		Time.fixedDeltaTime = fixedDeltaTimeStart;
	}

	public float GetFixedDeltaTimeBaseValue() {
		return fixedDeltaTimeStart;
	}

	public void PlayerShot() {
		// start Timer when player makes first hit
		if (!timer.TimerIsRunning()) {
			timer.StartTimer();
		}

		hits ++;
		hitsText.text = hits.ToString();
	}

	public void SetCurrentCheckpoint(GameObject checkpoint) {
		if (checkpoint.GetComponent<CheckPoint>() != null) {
			currentCheckpoint = checkpoint;
		} else {
			Debug.LogError(checkpoint.name + " is not a checkpoint!");
		}
	}

	public int GetCurrentHits() {
		return hits;
	}

	public float GetCurrentTimerTime() {
		return timer.GetCurrentTime();
	}
}
