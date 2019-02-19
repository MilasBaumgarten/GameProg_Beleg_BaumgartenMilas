using UnityEngine;

public class Essentials : MonoBehaviour {
	public static Essentials instance;

	void Start() {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);
		} else {
			Destroy(gameObject);
		}
	}
}
