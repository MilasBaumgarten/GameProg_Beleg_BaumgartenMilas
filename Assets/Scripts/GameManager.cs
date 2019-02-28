using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Stats))]
public class GameManager : MonoBehaviour {
	public static GameManager instance;

	public float fixedDeltaTimeStart { get; private set; }
	public PlayerScriptableObject playerSettings;
	public PostProcessVolume slowMotionPostProcessingVolume;
	public CinemachineVirtualCamera virtualCamera;
	public Stats stats { get; private set; }

	public GameObject player { get; private set; }
	private SpawnPoint playerSpawnPoint;
	private GameObject currentCheckpoint;
	private ControlsTutorial controlsTutorial;

	public string saveFilename;
	public GameObject endscreenUI;
	public GameObject[] highscoreEntries;
	public HighscoreSettingsScriptableObject highscoreSettings;

	// use GameManager as a Singleton
	void Start() {
		if (instance == null) {
			instance = this;
			// unparent GameManager
			transform.parent = null;
			DontDestroyOnLoad(gameObject);

			// set variables which won't be changed later
			fixedDeltaTimeStart = Time.fixedDeltaTime;
			stats = GetComponent<Stats>();

			Setup();
		} else {
			Destroy(gameObject);
		}
	}

	private void Setup() {
		playerSpawnPoint = FindObjectOfType<SpawnPoint>();
		controlsTutorial = FindObjectOfType<ControlsTutorial>();

		player = playerSpawnPoint.SpawnPlayer();

		virtualCamera.m_Follow = player.transform;
		virtualCamera.m_LookAt = player.transform;

		SceneManager.sceneLoaded += Restart;
	}

	/// <summary>
	/// Reset all checkpoints and respawn the player at the start of the level.
	/// </summary>
	public void Restart() {
		Destroy(player.gameObject);
		Setup();

		controlsTutorial.enabled = true;
		endscreenUI.SetActive(false);

		// reset all checkpoints
		foreach(CheckPoint check in FindObjectsOfType<CheckPoint>()) {
			check.activated = false;
		}

		stats.Reset();
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

	public void StartNextLevel() {
		int buildIndex = SceneManager.GetActiveScene().buildIndex + 1;
		// check Buildindex and reset player to first level after completing the last level
		if (SceneManager.sceneCountInBuildSettings <= buildIndex) {
			SceneManager.LoadScene(0);
		} else {
			// load next Scene in Build Index
			SceneManager.LoadScene(buildIndex);
		}
	}

	private void Restart(Scene scene, LoadSceneMode mode) {
		Restart();
	}

	public void PlayerEnteredGoal() {
		// stop the game
		stats.timer.PauseTimer();
		controlsTutorial.enabled = false;
		endscreenUI.SetActive(true);

		// hide the player (so the camera will still center)
		player.GetComponent<MeshRenderer>().enabled = false;
		player.GetComponent<Rigidbody>().velocity = Vector3.zero;
		player.GetComponent<Rigidbody>().useGravity = false;
		player.GetComponent<PlayerController>().enabled = false;
		player.transform.GetChild(0).gameObject.SetActive(false);

		// create highscore entry from current run
		HighscoreEntry entry = new HighscoreEntry("Player", stats.timer.currentTime, stats.hits);

		// add highscore entry to highscore
		Highscore highscore = WriteJSON.LoadHighscore("/" + saveFilename + SceneManager.GetActiveScene().name + ".json");
		highscore.AddEntry(entry);

		// save new highscore entry to save file
		WriteJSON.SaveHighscore("/" + saveFilename + SceneManager.GetActiveScene().name + ".json", highscore);

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
