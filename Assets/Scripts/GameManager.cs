using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

[RequireComponent(typeof(Stats))]
public class GameManager : MonoBehaviour {
	public static GameManager instance;

	public float fixedDeltaTimeStart { get; private set; }
	public PlayerScriptableObject playerSettings;
	public PostProcessVolume slowMotionPostProcessingVolume;
	public Stats stats { get; private set; }

	[SerializeField] private GameObject player;
	private Vector3 playerSpawnPoint;
	private GameObject currentCheckpoint;

	public string saveFilename;
	public GameObject EndscreenUI;
	public GameObject[] highscoreEntries;
	public HighscoreSettingsScriptableObject highscoreSettings;

	// use GameManager as a Singleton
	void Start() {
		if (instance == null) {
			instance = this;
			// unparent GameManager
			transform.parent = null;
			DontDestroyOnLoad(gameObject);

			Setup();
		} else {
			Destroy(gameObject);
		}
	}

	private void Setup() {
		fixedDeltaTimeStart = Time.fixedDeltaTime;
		stats = GetComponent<Stats>();
		playerSpawnPoint = player.transform.position;
	}

	public void Restart() {
		EndscreenUI.SetActive(false);

		// reset all checkpoints
		foreach(CheckPoint check in FindObjectsOfType<CheckPoint>()) {
			check.activated = false;
		}

		// reset player to level start
		player.GetComponent<Rigidbody>().useGravity = true;
		player.GetComponent<PlayerController>().enabled = true;
		Respawn(playerSpawnPoint, 0, 0);
	}

	/// <summary>
	/// Respawn the player at the last Checkpoint.
	/// </summary>
	public void Respawn() {
		Respawn(currentCheckpoint.transform.position, currentCheckpoint.GetComponent<CheckPoint>().currentTime, currentCheckpoint.GetComponent<CheckPoint>().currentHits);
	}

	/// <summary>
	/// Respawn player at the given position.
	/// </summary>
	/// <param name="position"> where the player should respawn </param>
	public void Respawn(Vector3 position, float time, int strokes) {
		player.transform.position = position;
		player.GetComponent<Rigidbody>().velocity = Vector3.zero;

		stats.Reset(time, strokes);
	}

	public void Pause() {
		player.GetComponent<Rigidbody>().velocity = Vector3.zero;
		player.GetComponent<Rigidbody>().useGravity = false;
		player.GetComponent<PlayerController>().enabled = false;
		stats.timer.PauseTimer();
	}

	// reset Time.fixedDeltaTime when game is closed
	private void OnDisable() {
		Time.fixedDeltaTime = fixedDeltaTimeStart;
	}

	public void SetCurrentCheckpoint(GameObject checkpoint) {
		if (checkpoint.GetComponent<CheckPoint>() != null) {
			currentCheckpoint = checkpoint;
		} else {
			Debug.LogError(checkpoint.name + " is not a checkpoint!");
		}
	}

	public void PlayerEnteredGoal() {
		// stop the game
		Pause();
		EndscreenUI.SetActive(true);

		// create highscore entry from current run
		HighscoreEntry entry = new HighscoreEntry("Player", stats.timer.currentTime, stats.hits);

		// add highscore entry to highscore
		Highscore highscore = WriteJSON.LoadHighscore(saveFilename);
		highscore.AddEntry(entry);

		// save new highscore entry to save file
		WriteJSON.SaveHighscore(saveFilename, highscore);

		// clear score board
		for (int i = 0; i < highscoreEntries.Length; i++) {
			highscoreEntries[i].transform.GetChild(0).GetComponent<Text>().text = "";
			highscoreEntries[i].transform.GetChild(1).GetComponent<Text>().text = "";
			highscoreEntries[i].transform.GetChild(2).GetComponent<Text>().text = "";
			highscoreEntries[i].transform.GetChild(3).GetComponent<Text>().text = "";
		}

		// get smallest needed max index to draw either all highscore entries (entries < first n entries) or to draw the first n entries
		int maxIndex = (highscore.GetLength() < highscoreEntries.Length) ? highscore.GetLength() : highscoreEntries.Length;

		highscore.Sort();

		// show highscore
		for(int i = 0; i < maxIndex; i++) {
			HighscoreEntry highscoreEntry = highscore.GetEntry(i);
			highscoreEntries[i].transform.GetChild(0).GetComponent<Text>().text = highscoreEntry.name;
			highscoreEntries[i].transform.GetChild(1).GetComponent<Text>().text = highscoreEntry.time.ToString("0.00") + " sec";
			highscoreEntries[i].transform.GetChild(2).GetComponent<Text>().text = highscoreEntry.strokes.ToString();
			highscoreEntries[i].transform.GetChild(3).GetComponent<Text>().text = highscoreEntry.points.ToString("0.00");
		}
	}
}
