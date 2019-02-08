using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GameManager : MonoBehaviour {
	public static GameManager instance;

	public float fixedDeltaTimeStart;

	public PlayerScriptableObject playerSettings;
	public PostProcessVolume slowMotionPostProcessingVolume;

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

	public void Respawn() {
		player.transform.position = lastactivatedSpawn.transform.position;
		player.GetComponent<Rigidbody>().velocity = Vector3.zero;

	}

	// reset Time.fixedDeltaTime when game is closed
	private void OnDisable() {
		Debug.Log("Game was closed");
		Time.fixedDeltaTime = fixedDeltaTimeStart;
	}
}
