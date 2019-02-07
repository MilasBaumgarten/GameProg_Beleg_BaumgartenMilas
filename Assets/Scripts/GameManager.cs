using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GameManager : MonoBehaviour {
	public static GameManager instance;

	public float fixedDeltaTimeStart;

	public PlayerScriptableObject playerSettings;
	public PostProcessVolume slowMotionPostProcessingVolume;


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
	}
}
