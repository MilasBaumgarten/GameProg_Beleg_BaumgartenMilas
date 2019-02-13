using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

[RequireComponent(typeof(Stats))]
public class GameManager : MonoBehaviour {
	public static GameManager instance;

	// for access in other files
	public float fixedDeltaTimeStart { get; private set; }
	public PlayerScriptableObject playerSettings;
	public PostProcessVolume slowMotionPostProcessingVolume;
	public Stats stats { get; private set; }

	private GameObject player;
	private GameObject currentCheckpoint;

	public string saveFilename;
	public GameObject EndscreenUI;
	public GameObject[] highscoreEntries;

	// use GameManager as a Singleton
	void Start() {
		if (instance == null) {
			instance = this;
			// unparent object
			transform.parent = null;
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
		stats = GetComponent<Stats>();
	}

	public void Restart() {
		EndscreenUI.SetActive(false);

		player.GetComponent<Rigidbody>().useGravity = true;
		player.GetComponent<PlayerController>().enabled = true;
		Respawn();
		// teleport player to level start
	}

	public void Respawn() {
		player.transform.position = currentCheckpoint.transform.position;
		player.GetComponent<Rigidbody>().velocity = Vector3.zero;

		stats.Reset(currentCheckpoint.GetComponent<CheckPoint>().GetCheckpointTime(), currentCheckpoint.GetComponent<CheckPoint>().GetCheckpointHits());
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
