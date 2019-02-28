using UnityEngine;

[RequireComponent(typeof(Timer))]
public class ControlsTutorial : MonoBehaviour {
	private Timer timer;
	[SerializeField] private float minVelocityThreshold = 0.1f;
	[SerializeField] private float startTutorialAfterNSeconds = 5.0f;

	[SerializeField] private GameObject tutorial = null;
	private Camera cam;

	private void Start() {
		timer = GetComponent<Timer>();
		cam = Camera.main;
	}

	void Update() {
		// start countdown when player is not moving
		if (GameManager.instance.player.GetComponent<Rigidbody>().velocity.magnitude <= minVelocityThreshold) {
			if (!timer.isRunning) {
				timer.StartTimer();
			}
		} else {
			timer.ResetTimer(0);
			tutorial.SetActive(false);
		}

		// show tutorial animation after some idle time
		if (timer.currentTime >= startTutorialAfterNSeconds && !tutorial.activeSelf) {
			tutorial.SetActive(true);
			transform.position = GameManager.instance.player.transform.position;
		}
	}
}
