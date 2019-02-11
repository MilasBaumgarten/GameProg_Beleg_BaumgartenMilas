using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public static GameManager instance;

	private float fixedDeltaTimeStart;

	public PlayerScriptableObject playerSettings;
	public PostProcessVolume slowMotionPostProcessingVolume;

	private Timer timer = new Timer();
	[SerializeField] private Text timerText = null;
	private int hits = 0;
	[SerializeField] private Text hitsText = null;

	private GameObject player;
	[HideInInspector] public GameObject lastactivatedSpawn;

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
	}

	private void Update() {
		// update Timer text in UI
		if (timer.TimerIsRunning()) {
			timerText.text = timer.GetDuration();
		}
	}

	public void Respawn() {
		player.transform.position = lastactivatedSpawn.transform.position;
		player.GetComponent<Rigidbody>().velocity = Vector3.zero;

		// reset timer and hits
		// TODO: reset to value at last checkpoint
		timer.Reset();
		timerText.text = "0.00 sec";

		hits = 0;
		hitsText.text = "0";
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
			timer.Start();
		}

		hits ++;
		hitsText.text = hits.ToString();
	}
}
