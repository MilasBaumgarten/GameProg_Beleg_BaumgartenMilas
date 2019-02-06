using UnityEngine;

public class GameManager : MonoBehaviour {
	public static GameManager instance;

	public float fixedDeltaTimeStart;

	public PlayerScriptableObject playerSettings;

	void Start() {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);

			GetValues();
		} else {
			Destroy(gameObject);
		}
	}

	private void GetValues() {
		fixedDeltaTimeStart = Time.fixedDeltaTime;
	}
}
