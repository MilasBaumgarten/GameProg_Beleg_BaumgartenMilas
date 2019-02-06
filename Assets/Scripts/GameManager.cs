using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public static GameManager instance;

	public PlayerScriptableObject playerSettings;

	void Start() {
		if(instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);
		} else {
			Destroy(gameObject);
		}
	}
}
